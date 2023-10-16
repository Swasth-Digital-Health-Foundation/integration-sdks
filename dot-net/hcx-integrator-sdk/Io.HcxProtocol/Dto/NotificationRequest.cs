using Io.HcxProtocol.Init;
using Io.HcxProtocol.Utils;
using System.Collections.Generic;

namespace Io.HcxProtocol.Dto
{
    public class NotificationRequest
    {
        private string jwsPayload;
        private string topicCode;
        private List<string> recipients;
        private string recipientType;
        private string message;
        private string correlationId;
        private Dictionary<string, string> templateParams;
        private Config config;

        public NotificationRequest(string jwsPayload)
        {
            this.jwsPayload = jwsPayload;
        }

        public NotificationRequest(string topicCode, string message, Dictionary<string, string> templateParams, string recipientType, List<string> recipients, string correlationId, Config config)
        {
            this.topicCode = topicCode;
            this.message = message;
            this.templateParams = templateParams;
            this.recipientType = recipientType;
            this.recipients = recipients;
            this.correlationId = correlationId;
            this.config = config;
        }

        public string GetJwsPayload()
        {
            return jwsPayload;
        }

        public Dictionary<string, object> GetHeaders()
        {
            var x = jwsPayload.Split('.');
            var j = JSONUtils.DecodeBase64String<Dictionary<string, object>>(jwsPayload.Split('.')[0].ToString());

            return j;
        }

        public Dictionary<string, object> GetPayload()
        {
            return JSONUtils.DecodeBase64String<Dictionary<string, object>>(jwsPayload.Split('.')[0].ToString());
        }

        public string GetTopicCode()
        {
            return topicCode;
        }

        public List<string> GetRecipients()
        {
            return recipients;
        }

        public string GetRecipientType()
        {
            return recipientType;
        }

        public string GetMessage()
        {
            return message;
        }

        public Dictionary<string, string> GetTemplateParams()
        {
            return templateParams;
        }

        public string GetCorrelationId()
        {
            return correlationId;
        }

        public Config GetConfig()
        {
            return config;
        }

        public Dictionary<string, object> NotificationHeaders()
        {
            var x = GetHeaders()[Constants.NOTIFICATION_HEADERS];
            var notificationheaders = JSONUtils.Deserialize<Dictionary<string, object>>(x.ToString());
            return notificationheaders;
        }

        public string GetSenderCode()
        {
            return (string)NotificationHeaders()["sender_code"];
        }
    }
}
