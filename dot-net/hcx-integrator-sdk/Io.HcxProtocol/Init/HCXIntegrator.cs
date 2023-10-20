using Io.HcxProtocol.Utils;
using System.Collections.Generic;

namespace Io.HcxProtocol.Init
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// The methods and variables to access the configuration.
    /// </summary>
    public class HCXIntegrator : BaseIntegrator
    {
        /// <summary>
        /// This method is to initialize config factory by passing the configuration as object.
        /// </summary>
        /// <param name="_config">A object that contains configuration variables and its values.</param>
        /// <exception cref="System.Exception"></exception>

        public static HCXIntegrator GetInstance(Config config)
        {
            HCXIntegrator hcxIntegrator = new HCXIntegrator();
            hcxIntegrator.SetConfig(config);
            return hcxIntegrator;
        }

        public bool ProcessIncoming(string jwePayload, Operations operation, Dictionary<string, object> output)
        {
            return GetIncomingRequest().Process(jwePayload, operation, output, GetConfig());
        }

        public bool ProcessOutgoingRequest(string fhirPayload, Operations operation, string recipientCode, string apiCallId, string correlationId, Dictionary<string, object> domainHeaders, Dictionary<string, object> output)
        {
            return GetOutgoingRequest().Process(fhirPayload, operation, recipientCode, apiCallId, correlationId, "", "", domainHeaders, output, GetConfig());
        }

        //overloaded method with workflowId
        public bool ProcessOutgoingRequest(string fhirPayload, Operations operation, string recipientCode, string apiCallId, string correlationId, string workflowId, Dictionary<string, object> domainHeaders, Dictionary<string, object> output)
        {
            return GetOutgoingRequest().Process(fhirPayload, operation, recipientCode, apiCallId, correlationId, workflowId, "", "", domainHeaders, output, GetConfig());
        }


        public bool ProcessOutgoingCallback(string fhirPayload, Operations operation, string apiCallId, string actionJwe, string onActionStatus, Dictionary<string, object> domainHeaders, Dictionary<string, object> output)
        {
            return GetOutgoingRequest().Process(fhirPayload, operation, "", apiCallId, "", actionJwe, onActionStatus, domainHeaders, output, GetConfig());
        }

        public Dictionary<string, object> ReceiveNotification(string requestBody, Dictionary<string, object> output)
        {
            return GetIncomingRequest().ReceiveNotification(requestBody, output, GetConfig());
        }

        public bool SendNotification(string topicCode, string recipientType, List<string> recepients, string message, Dictionary<string, string> templateParams, Dictionary<string, object> output)
        {
            return GetOutgoingRequest().SendNotification(topicCode, recipientType, recepients, message, templateParams, "", output, GetConfig());
        }
    }
}
