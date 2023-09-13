using Hl7.Fhir.Model;
using Hl7.Fhir.Utility;
using Io.HcxProtocol.Dto;
using Io.HcxProtocol.Exceptions;
using Io.HcxProtocol.Helper;
using Io.HcxProtocol.Init;
using Io.HcxProtocol.Utils;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog.Filters;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Cms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


namespace Io.HcxProtocol.Service
{
    public class NotificationService
    {
        private NotificationService()
        {

        }

        public static void validateNotificationRequest(NotificationRequest notificationRequest)
        {
            if (string.IsNullOrEmpty(notificationRequest.getTopicCode()))
            {
                throw new ClientException("Topic code cannot be empty");
            }
            else if (string.IsNullOrEmpty(notificationRequest.getMessage()) && notificationRequest.getTemplateParams().Count == 0)
            {
                throw new ClientException("Either the message or the template parameters are mandatory.");
            }
            else if (notificationRequest.getRecipients().Count == 0)
            {
                throw new ClientException("Recipients cannot be empty");
            }
            else if (string.IsNullOrEmpty(notificationRequest.getRecipientType()))
            {
                throw new ClientException("Recipient type cannot be empty");
            }
        }

        public static Dictionary<string, object> CreateNotificationRequest(NotificationRequest notificationRequest, Dictionary<string, object> output, string message)
        {

            Dictionary<string, object> headers = getJWSRequestHeader(notificationRequest);
            Dictionary<string, object> payload = getJWSRequestPayload(notificationRequest, output, message);
            //Dictionary<string, object> payload = new Dictionary<string, object>();
            string privatekey = getPrivateKey(notificationRequest.getConfig());
           string jwsToken = JWSUtils.generate2(headers, payload, privatekey);
            Dictionary<string, object> requestBody = new Dictionary<string, object>();
            //  jwsToken = "eyJhbGciOiJSUzI1NiIsIngtaGN4LW5vdGlmaWNhdGlvbl9oZWFkZXJzIjp7InJlY2lwaWVudF90eXBlIjoicGFydGljaXBhbnRfcm9sZSIsInJlY2lwaWVudHMiOlsicGF5b3IiXSwieC1oY3gtY29ycmVsYXRpb25faWQiOiJjODlmZTliYy03NTViLTQzOWQtYTdmMy03YzQzNDM3ZjQ5MTgiLCJzZW5kZXJfY29kZSI6InRlc3Rwcm92aWRlcjEuYXBvbGxvQHN3YXN0aC1oY3gtZGV2IiwidGltZXN0YW1wIjoxNjk0MTU4MDcyNzIwfX0.eyJ0b3BpY19jb2RlIjoibm90aWYtcGFydGljaXBhbnQtbmV3LXByb3RvY29sLXZlcnNpb24tc3VwcG9ydCIsIm1lc3NhZ2UiOiJ0ZXN0LXByb3ZpZGVyIG5vdyBzdXBwb3J0cyB2MC44IG9uIGl0cyBwbGF0Zm9ybS4gQWxsIHBhcnRpY2lwYW50cyBhcmUgcmVxdWVzdGVkIHRvIHVwZ3JhZGUgdG8gdjAuOCBvciBhYm92ZSB0byB0cmFuc2FjdCB3aXRoIHRlc3QtcHJvdmlkZXIuIn0.XdH4G9wIN0nUOfB8Aoms5I0f1hZ3_67p0LwKlpe_tle51BVSp6eg2fZwzZJ8fn-xRwBsteQDeF-zZIj2qQRb3A7OWnPQBAxScJ9hUHKUjusar14ZaWNDGrP-ntE_I_hb9gNe99YUMCqcxHgyxPUw8iAkdF3R4F6U7ea-xLvXHcZRI_33daTHJ651kB3avp6HZkCgaZZDTIsYGGC7uK3lCynvC9XQA-Hji4Fd0gylUrkgHR8I50K1ZxiWPL30ma09CDL-16MhlFt3z7DQnYBDa9MJwxp568Et7Gc7srrWB1KULCklNubFfUYuLAbRZt7OuFxPBECtVD_iPXJOgzm8Gg";
            // jwsToken = "eyJhbGciOiJSUzI1NiIsIngtaGN4LW5vdGlmaWNhdGlvbl9oZWFkZXJzIjp7InJlY2lwaWVudF90eXBlIjoicGFydGljaXBhbnRfcm9sZSIsInJlY2lwaWVudHMiOlsicGF5b3IiXSwieC1oY3gtY29ycmVsYXRpb25faWQiOiIyOTcxODA1Zi0wYTdkLTRhNzEtYWM2YS0yY2IzZWIyNmQ3Y2UiLCJzZW5kZXJfY29kZSI6InRlc3Rwcm92aWRlcjEuYXBvbGxvQHN3YXN0aC1oY3gtZGV2IiwidGltZXN0YW1wIjoxNjk0NDMwMDU5NTg2fX0.eyJ0b3BpY19jb2RlIjoibm90aWZ5LXBhcnRpY2lwYW50LW5ldy1wcm90b2NvbC12ZXJzaW9uLXN1cHBvcnQiLCJtZXNzYWdlIjoiUGFydGljaXBhbnQgaGFzIHVwZ3JhZGVkIHRvIGxhdGVzdCBwcm90b2NvbCB2ZXJzaW9uIn0.UJKnxYMS6Cq3cNsJ587IcqAN0lw4PV8wOyWUwDWD9j7NRdSBebADRqd9KXfw2u5_wePLkR1-27y_jxQfirqQEK-TMYk8Ti2ePpi0qLqCjkOKot4BeunuS8TyRWXheCpf6Rjbsd0QHV7ctoxgWc2qzwF9RBUFuxRRMEjyIpg4V4EprVRn4Q2E_SKf-Qkq518wKsZcWlAMg6XCIR9Z7W73p8Fv4W7Sf1vtdFhS4Oz56Mqu4f9dGthlHAL5WQA44c_zTMwS3sS-ZihJRfnBuqxgY0ShrH53tHzT-tu8hjNaldW9Lm9byPd9sWRjdhBqlQEyicNRkrXyU6bes8Ex1WYRGA";
           // jwsToken = "eyJhbGciOiJSUzI1NiIsIngtaGN4LW5vdGlmaWNhdGlvbl9oZWFkZXJzIjp7InJlY2lwaWVudF90eXBlIjoicGFydGljaXBhbnRfcm9sZSIsInJlY2lwaWVudHMiOlsicGF5b3IiXSwieC1oY3gtY29ycmVsYXRpb25faWQiOiIyMDI0ODU5Zi1lZWE3LTQ5YTgtOWNjYi0wYmExMGFlOTgxYjIiLCJzZW5kZXJfY29kZSI6InRlc3Rwcm92aWRlcjEuYXBvbGxvQHN3YXN0aC1oY3gtZGV2IiwidGltZXN0YW1wIjoxNjk0NDk3NDA0NjQ1fX0.eyJ0b3BpY19jb2RlIjoibm90aWZ5LXBhcnRpY2lwYW50LW5ldy1wcm90b2NvbC12ZXJzaW9uLXN1cHBvcnQiLCJtZXNzYWdlIjoiUGFydGljaXBhbnQgaGFzIHVwZ3JhZGVkIHRvIGxhdGVzdCBwcm90b2NvbCB2ZXJzaW9uIn0.V8Yk9B230OyZ3SLFql2vKVXE7qGEsj9UoFuB3A_1kZXdn2wH5AxeynRIfkbV-utTLA_ntVo4rmkdfmBKuueHLM7DWRKwgG3Pb-hldqWlWEuYrAL_aQo3UKPJg9UP6YEBTDGya6Y-GdhDXCZm0qfcVmiJpDK6Fo4HtwOHOldOXX2Nr8jtITXOOJU-bHeeH4PJfK4GNs2MGdqdHrh5hYTP8wpMrXTiHC90iJMU4ww24uQVgWI6W42uuxU6bUXXeNSQ1JVgzQArDgDjsecjal3keEawUlQ4fvz0FYxON4bDUTABMHAEmhE8H88ScWj9u-g_w13ft9U-2CY34KEF-ZouKg";
            requestBody.Add("payload", jwsToken);
            return requestBody;
        }

        private static Dictionary<string, object> getJWSRequestHeader(NotificationRequest notificationRequest)
        {
            var guid = Guid.NewGuid().ToString();
            Dictionary<string, object> headersDictionary = new Dictionary<string, object>();
            headersDictionary.Add("x-hcx-correlation_id", string.IsNullOrEmpty(notificationRequest.getCorrelationId()) ? guid : notificationRequest.getCorrelationId());
            headersDictionary.Add("sender_code", notificationRequest.getConfig().ParticipantCode);
            headersDictionary.Add(Constants.TIMESTAMP, GetCurrentMilli());
            headersDictionary.Add(Constants.RECIPIENT_TYPE, notificationRequest.getRecipientType());
            headersDictionary.Add(Constants.RECIPIENTS, notificationRequest.getRecipients());
            Dictionary<string, object> headers = new Dictionary<string, object>();
            headers.Add("alg", "RS256");
            headers.Add(Constants.NOTIFICATION_HEADERS, headersDictionary);
            return headers;
        }

        private static Dictionary<string, object> getJWSRequestPayload(NotificationRequest notificationRequest, Dictionary<string, object> output, string message)
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add(Constants.TOPIC_CODE, notificationRequest.getTopicCode());
            message = getMessage(notificationRequest, output, message);
            payload.Add(Constants.MESSAGE, message);
            return payload;
        }

        private static string getMessage(NotificationRequest notificationRequest, Dictionary<string, object> output, string message)
        {
            Dictionary<string, object> searchRequest = new Dictionary<string, object>
        {
            {"filters", new Dictionary<string, object>()}
        };
            if (notificationRequest.getTemplateParams().Count != 0)
            {
                HcxUtils.initializeHCXCall(JSONUtils.Serialize(searchRequest), Operations.NOTIFICATION_LIST, output, notificationRequest.getConfig());
                if (output.ContainsKey(Constants.ERROR) || output.ContainsKey(ErrorCodes.ERR_DOMAIN_PROCESSING.ToString()))
                {
                    throw new ClientException("Error while resolving the message template");
                }
                if (output.Count > 0) //ismail change 31 august
                {
                  var notificationsList = output["responseObj"];
                    Dictionary<Object,Object> dictio = (Dictionary<Object,Object>)notificationsList;
                    Dictionary<string,Object> dictiotwo = dictio.ToDictionary(x => x.Key.ToString(), x => (Object)x.Value);

                    List<Dictionary<string,Object>> notificationlist = new List<Dictionary<string,Object>>();
                    notificationlist.Add(dictiotwo);
                    var topiccode = notificationRequest.getTopicCode();
                    Dictionary<string,Object> notification = getNotification(notificationlist,topiccode);
                    message = resolveTemplate(notification, notificationRequest.getTemplateParams());
                }
            }
            return message;
        }


        private static string resolveTemplate(Dictionary<string, object> notification, Dictionary<string, string> nData)
        {
            //StringSubstitutor sub = new StringSubstitutor(nData);
           
            var serializedtemplate = JsonConvert.SerializeObject(notification[Constants.TEMPLATE]);
            var json1 = JSONUtils.Deserialize<string>((string)serializedtemplate, typeof(Dictionary<Object, Object>));
            StringBuilder  message = new StringBuilder(json1);
            var versionnumber = nData["version_code"];
            var participantname = nData["participant_name"];
            message.Replace("${participant_name}",participantname);
            message.Replace("${version_code}",versionnumber);
            return message.ToString();
           
            
           // sub.Replace(JsonConvert.DeserializeObject((string)notification[Constants.TEMPLATE]), Constants.MESSAGE);
        }


        private static Dictionary<string,Object> getNotification(List<Dictionary<string,Object>> notificationList, string code)
        {
          
            var list = notificationList[0];
            var finalist = list["notifications"];
            var d = JsonConvert.SerializeObject(finalist.ToString());

           // Object jsonObj = JObject.Parse(d);
            //JArray myArray = (JArray)jsonObj.
            var type = finalist.GetType();
            Dictionary<string,Object> x = notificationList.Where(x1 => x1.ContainsKey("notifications")).FirstOrDefault();
            var  j = x.Where(obj1 => obj1.Key.Equals("notifications")).FirstOrDefault().Value;
            var lisstring = j.ToString();
            var data = JsonConvert.DeserializeObject<IEnumerable<IDictionary<string, object>>>(lisstring);
            var notification = data.Where(mo => mo["topic_code"].Equals(code)).First();
            var dictioobj = notification.ToDictionary(co => co.Key.ToString(), co => (Object)co.Value);
            Optional<Dictionary<string, Object>> lk = new Optional<Dictionary<string, object>>();
            lk = dictioobj;
            if(lk.HasValue)
            {
             return lk.Value; 
            }
            
            else
            {
                throw new ClientException("Topic code is not found in the notification list: " + code);
            }
        }

        private static string getPrivateKey(Config config)
        {
            string privateKey = config.signingPrivateKey;
           // privateKey = privateKey.Replace("-----BEGIN PRIVATE KEY-----", "").Replace("-----END PRIVATE KEY-----", "").Replace("\\s+", "");
            return privateKey;
        }

        public static double GetCurrentMilli() //ismail cr13 related
        {
            DateTime Jan1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan javaSpan = DateTime.UtcNow - Jan1970;
            var x = decimal.Round(Convert.ToDecimal(javaSpan.TotalMilliseconds));
            return Convert.ToDouble(x);         
        }

    }

}
