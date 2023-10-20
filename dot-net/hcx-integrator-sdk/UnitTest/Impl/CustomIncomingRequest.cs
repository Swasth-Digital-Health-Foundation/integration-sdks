using Io.HcxProtocol.Exceptions;
using Io.HcxProtocol.Impl;
using Io.HcxProtocol.Jwe;
using Io.HcxProtocol.Key;
using Io.HcxProtocol.Utils;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace UnitTest.Impl
{
    public class CustomIncomingRequest : HCXIncomingRequest
    {
        public override bool ValidateRequest(string jwePayload, Operations operation, Dictionary<string, object> error)
        {
            Console.WriteLine("Custom implementation is executed");
            return true;
        }

        public override bool DecryptPayload(string jwePayload, string privateKey, Dictionary<string, object> output)
        {
            try
            {
                JweRequest jweRequest = new JweRequest(JSONUtils.Deserialize<Dictionary<string, object>>(jwePayload));
                RSA rsaPublicKey = X509KeyLoader.GetRSAPrivateKeyFromPem(privateKey, PemMode.FileText);
                jweRequest.DecryptRequest(rsaPublicKey);
                output.Add(Constants.HEADERS, jweRequest.GetHeaders());
                output.Add(Constants.FHIR_PAYLOAD, JSONUtils.Serialize(jweRequest.GetPayload()));
                Console.WriteLine("Custom implementation is executed.");
                return true;
            }
            catch (Exception ex)
            {
                output.Add(ErrorCodes.ERR_INVALID_ENCRYPTION.ToString(), "[Decryption error.] " + ex.ToString());
                return false;
            }
        }
    }
}
