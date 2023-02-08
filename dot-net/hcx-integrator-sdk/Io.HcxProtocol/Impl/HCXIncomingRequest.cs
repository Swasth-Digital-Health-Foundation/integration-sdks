using Io.HcxProtocol.Dto;
using Io.HcxProtocol.Exceptions;
using Io.HcxProtocol.Helper;
using Io.HcxProtocol.Init;
using Io.HcxProtocol.Interfaces;
using Io.HcxProtocol.Jwe;
using Io.HcxProtocol.Key;
using Io.HcxProtocol.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Io.HcxProtocol.Impl
{
    /// <summary>
    ///     The <b>HCX Incoming Request</b> class provide the methods to help in processing the JWE Payload and extract FHIR Object.
    /// </summary>
    /// <remarks>
    ///     <list type="bullet">
    ///         <item><b>Process</b> is the main method to use by the integrator(s) to validate the JWE Payload and fetch FHIR Object.</item>
    ///         <list type="bullet">
    ///             <item>This method takes the JWE Payload and Operation as input parameters to validate and extract the respective FHIR Object.</item>
    ///         </list>
    ///         <item><b>ValidateRequest, ValidatePayload, DecryptPayload, SendResponse</b> methods are used by <b>Process</b> method to compose functionality of validating JWE Payload and extracting FHIR Object.
    ///                  These methods are available for the integrator(s) to use them based on different scenario(s) or use cases.</item>
    ///     </list>
    /// </remarks>
    public class HCXIncomingRequest : FhirPayload, IIncomingRequest
    {
        public HCXIncomingRequest() { }

        public bool Process(string jwePayload, Operations operation, Dictionary<string, object> output)
        {
            Dictionary<string, object> error = new Dictionary<string, object>();
            bool result = false;
            if (!ValidateRequest(jwePayload, operation, error))
            {
                SendResponse(error, output);
            }
            else if (!DecryptPayload(jwePayload, output))
            {
                SendResponse(output, output);
            }
            else if (!ValidatePayload(output[Constants.FHIR_PAYLOAD].ToString(), operation, error))
            {
                SendResponse(error, output);
            }
            else
            {
                if (SendResponse(error, output)) result = true;
            }
            return result;
        }

        public bool ValidateRequest(string jwePayload, Operations operation, Dictionary<string, object> error)
        {
            return ValidateHelper.GetInstance().ValidateRequest(jwePayload, operation, error);
        }

        public bool DecryptPayload(string jwePayload, Dictionary<string, object> output)
        {
            try
            {
                JweRequest jweRequest = new JweRequest(JSONUtils.Deserialize<Dictionary<string, object>>(jwePayload));
                RSA rsaPublicKey = X509KeyLoader.GetRSAPrivateKeyFromPem(HCXIntegrator.config.EncryptionPrivateKey, PemMode.FileText);
                jweRequest.DecryptRequest(rsaPublicKey);
                output.Add(Constants.HEADERS, jweRequest.GetHeaders());
                output.Add(Constants.FHIR_PAYLOAD, JSONUtils.Serialize(jweRequest.GetPayload()));

                return true;
            }
            catch (Exception ex)
            {
                output.Add(ErrorCodes.ERR_INVALID_ENCRYPTION.ToString(), "[Decryption error.] " + ex.Message.ToString());

                return false;
            }
        }

        public bool SendResponse(Dictionary<string, object> error, Dictionary<string, object> output)
        {
            Dictionary<string, object> responseObj = new Dictionary<string, object>();
            responseObj.Add(Constants.TIMESTAMP, DateTimeUtils.TotalMillisecondsUTC());
            bool result = false;
            if (error.Count == 0)
            {
                Dictionary<string, object> headers = (Dictionary<string, object>)output[Constants.HEADERS];
                responseObj.Add(Constants.API_CALL_ID, headers[Constants.HCX_API_CALL_ID]);
                responseObj.Add(Constants.CORRELATION_ID, headers[Constants.HCX_CORRELATION_ID]);
                result = true;
            }
            else
            {
                // Fetching only the first error and constructing the error object
                string code = error.First().Key;
                responseObj.Add(Constants.ERROR, new ResponseError(code, error[code].ToString(), ""));
            }
            output.Add(Constants.RESPONSE_OBJ, responseObj);
            return result;
        }
    }
}
