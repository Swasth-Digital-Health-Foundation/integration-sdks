using Io.HcxProtocol.Impl;
using Io.HcxProtocol.Init;
using Io.HcxProtocol.Utils;
using System;
using System.Collections.Generic;

namespace UnitTest.Impl
{
    internal class CustomOutgoingRequest : HCXOutgoingRequest
    {
        public override bool ValidatePayload(string fhirPayload, Operations operation, Dictionary<string, object> error, Config config)
        {
            Console.WriteLine("Custom implementation is executed");
            return base.ValidatePayload(fhirPayload, operation, error, config);
        }
    }
}
