using Io.HcxProtocol.Init;
using Io.HcxProtocol.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Io.HcxProtocol.Impl
{
    internal class CustomIncomingRequest:HCXIncomingRequest
    {
        public override bool ValidateRequest(string jwePayload, Operations operation, Dictionary<string, object> error)
        {
            return base.ValidateRequest(jwePayload, operation, error);
        }
        public override bool SendResponse(Dictionary<string, object> error, Dictionary<string, object> output)
        {
            return base.SendResponse(error, output);
        }
        public override bool DecryptPayload(string jwePayload, string privateKey, Dictionary<string, object> output)
        {
            return base.DecryptPayload(jwePayload, privateKey, output);
        }
        public override bool Process(string jwePayload, Operations operation, Dictionary<string, object> output, Config config)
        {
            return base.Process(jwePayload, operation, output, config);
        }
        public override bool ValidatePayload(string fhirPayload, Operations operation, Dictionary<string, object> error, Config config)
        {
            return base.ValidatePayload(fhirPayload, operation, error, config);
        }
    }
}
