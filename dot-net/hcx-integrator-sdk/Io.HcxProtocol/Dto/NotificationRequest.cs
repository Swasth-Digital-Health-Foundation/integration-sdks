using Hl7.Fhir.Utility;
using Io.HcxProtocol.Impl;
using Io.HcxProtocol.Init;
using Io.HcxProtocol.Utils;
using Org.BouncyCastle.Cms;
using System;
using System.Collections.Generic;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Config = Io.HcxProtocol.Init.Config;

namespace Io.HcxProtocol.Dto
{
    public  class NotificationRequest
    {
        private string  jwsPayload;
        private string  topicCode;
        private List<string> recipients;
        private string  recipientType;
        private string  message;
        private string  correlationId;
        private Dictionary<string, string> templateParams;
        private Init.Config config;

        public NotificationRequest(string jwsPayload)
        {
            this.jwsPayload = jwsPayload;
        }

        public NotificationRequest(string topicCode, string message, Dictionary<string, string> templateParams, string recipientType, List<string> recipients, string correlationId, Init.Config config)
        {
            this.topicCode = topicCode;
            this.message = message;
            this.templateParams = templateParams;
            this.recipientType = recipientType;
            this.recipients = recipients;
            this.correlationId = correlationId;
            this.config = config;
        }

        public string getJwsPayload()
        {
            return jwsPayload;
        }

        public Dictionary<string, Object> getHeaders() 
        {
            var x = jwsPayload.Split('.');
            var j= JSONUtils.DecodeBase64String<Dictionary<string,object>>(jwsPayload.Split('.')[0].ToString());
          
            return j;
          }

    public Dictionary<string,Object> getPayload() 
    {
        return JSONUtils.DecodeBase64String<Dictionary<string,object>>(jwsPayload.Split('.')[0].ToString());
    }


   public string getTopicCode()
    {
    return topicCode;
    }

    public List<string> getRecipients()
    {
    return recipients;
    }

   public string getRecipientType()
   {
    return recipientType;
   }

   public string getMessage()
   {
    return message;
   }

   public Dictionary<string,string> getTemplateParams()
   {
    return templateParams;
   }

   public string getCorrelationId()
    {
    return correlationId;
    }

public Config getConfig()
{
    return config;
}

public Dictionary<string, Object> notificationHeaders() 
{
            var x = getHeaders()[Constants.NOTIFICATION_HEADERS];
            var notificationheaders = JSONUtils.Deserialize<Dictionary<string,Object>>(x.ToString());
            return notificationheaders;
    }

    public string getSenderCode() 
{
        return (string) notificationHeaders()["sender_code"];
    }



}
}
