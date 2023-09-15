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
            string privatekey = getPrivateKey(notificationRequest.getConfig());
            string jwsToken = JWSUtils.generate(headers, payload, privatekey);
            Dictionary<string, object> requestBody = new Dictionary<string, object>();
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
