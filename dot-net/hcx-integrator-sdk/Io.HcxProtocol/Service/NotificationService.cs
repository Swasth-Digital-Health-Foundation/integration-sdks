using Io.HcxProtocol.Dto;
using Io.HcxProtocol.Exceptions;
using Io.HcxProtocol.Helper;
using Io.HcxProtocol.Init;
using Io.HcxProtocol.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Io.HcxProtocol.Service
{
    public class NotificationService
    {
        private NotificationService() { }

        public static void ValidateNotificationRequest(NotificationRequest notificationRequest)
        {
            if (string.IsNullOrEmpty(notificationRequest.GetTopicCode()))
            {
                throw new ClientException("Topic code cannot be empty.");
            }
            else if (string.IsNullOrEmpty(notificationRequest.GetMessage()) && notificationRequest.GetTemplateParams().Count == 0)
            {
                throw new ClientException("Either the message or the template parameters are mandatory.");
            }
            else if (notificationRequest.GetRecipients().Count == 0)
            {
                throw new ClientException("Recipients cannot be empty.");
            }
            else if (string.IsNullOrEmpty(notificationRequest.GetRecipientType()))
            {
                throw new ClientException("Recipient type cannot be empty.");
            }
        }

        public static Dictionary<string, object> CreateNotificationRequest(NotificationRequest notificationRequest, Dictionary<string, object> output, string message)
        {
            Dictionary<string, object> headers = GetJWSRequestHeader(notificationRequest);
            Dictionary<string, object> payload = GetJWSRequestPayload(notificationRequest, output, message);
            string privatekey = GetPrivateKey(notificationRequest.GetConfig());
            string jwsToken = JWSUtils.Generate(headers, payload, privatekey);
            Dictionary<string, object> requestBody = new Dictionary<string, object>();
            requestBody.Add("payload", jwsToken);
            return requestBody;
        }

        private static Dictionary<string, object> GetJWSRequestHeader(NotificationRequest notificationRequest)
        {
            var guid = Guid.NewGuid().ToString();
            Dictionary<string, object> headersDictionary = new Dictionary<string, object>();
            headersDictionary.Add("x-hcx-correlation_id", string.IsNullOrEmpty(notificationRequest.GetCorrelationId()) ? guid : notificationRequest.GetCorrelationId());
            headersDictionary.Add("sender_code", notificationRequest.GetConfig().ParticipantCode);
            headersDictionary.Add(Constants.TIMESTAMP, GetCurrentMilli());
            headersDictionary.Add(Constants.RECIPIENT_TYPE, notificationRequest.GetRecipientType());
            headersDictionary.Add(Constants.RECIPIENTS, notificationRequest.GetRecipients());
            Dictionary<string, object> headers = new Dictionary<string, object>();
            headers.Add("alg", "RS256");
            headers.Add(Constants.NOTIFICATION_HEADERS, headersDictionary);
            return headers;
        }

        private static Dictionary<string, object> GetJWSRequestPayload(NotificationRequest notificationRequest, Dictionary<string, object> output, string message)
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add(Constants.TOPIC_CODE, notificationRequest.GetTopicCode());
            message = GetMessage(notificationRequest, output, message);
            payload.Add(Constants.MESSAGE, message);
            return payload;
        }

        private static string GetMessage(NotificationRequest notificationRequest, Dictionary<string, object> output, string message)
        {
            Dictionary<string, object> searchRequest = new Dictionary<string, object>
            {
                {"filters", new Dictionary<string, object>()}
            };
            if (notificationRequest.GetTemplateParams().Count != 0)
            {
                HcxUtils.InitializeHCXCall(JSONUtils.Serialize(searchRequest), Operations.NOTIFICATION_LIST, output, notificationRequest.GetConfig());
                if (output.ContainsKey(Constants.ERROR) || output.ContainsKey(ErrorCodes.ERR_DOMAIN_PROCESSING.ToString()))
                {
                    throw new ClientException("Error while resolving the message template.");
                }
                if (output.Count > 0)
                {
                    var notificationsList = output["responseObj"];
                    Dictionary<object, object> dictio = (Dictionary<object, object>)notificationsList;
                    Dictionary<string, object> dictiotwo = dictio.ToDictionary(x => x.Key.ToString(), x => (object)x.Value);

                    List<Dictionary<string, object>> notificationlist = new List<Dictionary<string, object>>();
                    notificationlist.Add(dictiotwo);
                    var topiccode = notificationRequest.GetTopicCode();
                    Dictionary<string, object> notification = GetNotification(notificationlist, topiccode);
                    message = ResolveTemplate(notification, notificationRequest.GetTemplateParams());
                }
            }
            return message;
        }

        private static string ResolveTemplate(Dictionary<string, object> notification, Dictionary<string, string> nData)
        {
            var template = JsonConvert.SerializeObject(notification[Constants.TEMPLATE]);
            var versionnumber = nData["version_code"];
            var participantname = nData["participant_name"];
            template = template.Replace("${participant_name}", participantname);
            template = template.Replace("${version_code}", versionnumber);
            return template.ToString();
        }

        private static Dictionary<string, object> GetNotification(List<Dictionary<string, object>> notificationList, string code)
        {
            var list = notificationList[0];
            var finalist = list["notifications"];
            var d = JsonConvert.SerializeObject(finalist.ToString());

            var type = finalist.GetType();
            Dictionary<string, object> x = notificationList.Where(x1 => x1.ContainsKey("notifications")).FirstOrDefault();
            var j = x.Where(obj1 => obj1.Key.Equals("notifications")).FirstOrDefault().Value;
            var lisstring = j.ToString();
            var data = JsonConvert.DeserializeObject<IEnumerable<IDictionary<string, object>>>(lisstring);
            var notification = data.Where(mo => mo["topic_code"].Equals(code)).First();
            var dictioobj = notification.ToDictionary(co => co.Key.ToString(), co => (object)co.Value);
            if (dictioobj.Count > 1)
            {
                return dictioobj;
            }
            else
            {
                throw new ClientException("Topic code is not found in the notification list: " + code);
            }
        }

        private static string GetPrivateKey(Config config)
        {
            return config.SigningPrivateKey;
        }

        public static double GetCurrentMilli()
        {
            DateTime Jan1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan javaSpan = DateTime.UtcNow - Jan1970;
            var x = decimal.Round(Convert.ToDecimal(javaSpan.TotalMilliseconds));
            return Convert.ToDouble(x);
        }
    }
}
