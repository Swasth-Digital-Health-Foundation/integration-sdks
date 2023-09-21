using Io.HcxProtocol.Init;
using Io.HcxProtocol.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Io.HcxProtocol.Impl
{
    internal class CustomOutgoingRequest :HCXOutgoingRequest
    {
        public override bool ValidatePayload(string fhirPayload, Operations operation, Dictionary<string, object> error, Config config)
        {
            return base.ValidatePayload(fhirPayload, operation, error, config);
        }
        public override bool Process(string fhirPayload, Operations operation, string recipientCode, string apiCallId, string correlationId, string actionJwe, string onActionStatus, Dictionary<string, object> domainHeaders, Dictionary<string, object> output, Config config)
        {
            return base.Process(fhirPayload, operation, recipientCode, apiCallId, correlationId, actionJwe, onActionStatus, domainHeaders, output, config);
        }

        public override bool EncryptPayload(Dictionary<string, object> headers, string fhirPayload, Dictionary<string, object> encryptPayload, Config config)
        {
            return base.EncryptPayload(headers, fhirPayload, encryptPayload, config);
        }
        public override bool InitializeHCXCall(string jwePayload, Operations operation, Dictionary<string, object> response, Config config)
        {
            return base.InitializeHCXCall(jwePayload, operation, response, config);
        }
        public override bool Process(string fhirPayload, Operations operation, string recipientCode, string apiCallId, string correlationId, string workflowId, string actionJwe, string onActionStatus, Dictionary<string, object> domainHeaders, Dictionary<string, object> output, Config config)
        {
            return base.Process(fhirPayload, operation, recipientCode, apiCallId, correlationId, workflowId, actionJwe, onActionStatus, domainHeaders, output, config);
        }

        public override bool CreateHeader(string senderCode, string recipientCode, string apiCallId, string correlationId, string workflowId, string actionJwe, string onActionStatus, Dictionary<string, object> headers, Dictionary<string, object> error)
        {
            return base.CreateHeader(senderCode, recipientCode, apiCallId, correlationId, workflowId, actionJwe, onActionStatus, headers, error);
        }
        

    }
}
