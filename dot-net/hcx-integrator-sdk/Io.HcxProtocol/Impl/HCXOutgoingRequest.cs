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
    ///     The <b>HCX Outgoing Request</b> class provide the methods to help in creating the JWE Payload and send the request to the sender system from HCX Gateway.
    /// </summary>
    /// <remarks>
    ///     <list type="bullet">
    ///         <item><b>Generate</b> is the main method to use by the integrator(s) to generate JWE Payload.
    ///         <list type="bullet">
    ///             <item>This method handles two types of requests. There are two implementations of <b>Generate</b> method to handle action, on_action API calls.
    ///                 <list type="number">
    ///                     <item>Sending an initial request of HCX workflow action.</item>
    ///                     <item>Sending a response to the incoming HCX workflow action.
    ///                         <list type="bullet">
    ///                             <item>The input request JWE should be used as <i>actionJwe</i>.</item>
    ///                         </list>
    ///                     </item>
    ///                 </list>
    ///             </item>
    ///         </list>
    ///         </item>
    ///         <item>
    ///             <b>ValidatePayload, CreateHeader, EncryptPayload, InitializeHCXCall</b> methods are used by <b>Generate</b> method to compose the JWE payload generation functionality.
    ///             These methods are available for the integrator(s) to use them based on different scenario(s) or use cases.
    ///         </item>
    ///     </list>
    /// </remarks>
    public class HCXOutgoingRequest : FhirPayload, IOutgoingRequest
    {
        public HCXOutgoingRequest() { }

        public bool Generate(string fhirPayload, Operations operation, string recipientCode, Dictionary<string, object> output)
        {
            return Process(fhirPayload, operation, recipientCode, "", "", output);
        }

        public bool Generate(string fhirPayload, Operations operation, string actionJwe, string onActionStatus, Dictionary<string, object> output)
        {
            return Process(fhirPayload, operation, "", actionJwe, onActionStatus, output);
        }

        private bool Process(string fhirPayload, Operations operation, string recipientCode, string actionJwe, string onActionStatus, Dictionary<string, object> output)
        {
            bool result = false;
            try
            {
                Dictionary<string, object> error = new Dictionary<string, object>();
                Dictionary<string, object> headers = new Dictionary<string, object>();
                Dictionary<string, object> response = new Dictionary<string, object>();


                if (!ValidatePayload(fhirPayload, operation, error))
                {
                    foreach (var err in error) output.Add(err.Key, err.Value);
                }
                else if (!CreateHeader(recipientCode, actionJwe, onActionStatus, headers))
                {
                    foreach (var err in headers) output.Add(err.Key, err.Value);
                }
                else if (!EncryptPayload(headers, fhirPayload, output))
                {
                    foreach (var err in error) output.Add(err.Key, err.Value);
                }
                else
                {
                    result = InitializeHCXCall(JSONUtils.Serialize(output), operation, response);
                    foreach (var err in response) output.Add(err.Key, err.Value);
                }
                return result;
            }
            catch (Exception ex)
            {
                // TODO: Exception is handled as domain processing error, we will be enhancing in next version.
                output.Add(ErrorCodes.ERR_DOMAIN_PROCESSING.ToString(), ex.Message.ToString());
                return result;
            }
        }

        public bool CreateHeader(string recipientCode, string actionJwe, string onActionStatus, Dictionary<string, object> headers)
        {
            try
            {
                //Note: [ALG, A256GCM] & [ENC, RSA_OAEP] headers added by JWE-EncryptRequest Method
                headers.Add(Constants.HCX_API_CALL_ID, Guid.NewGuid().ToString());
                headers.Add(Constants.HCX_TIMESTAMP, DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss") + DateTime.Now.ToString("zzz").Replace(":", ""));
                if (!string.IsNullOrEmpty(recipientCode))
                {
                    headers.Add(Constants.HCX_SENDER_CODE, HCXIntegrator.config.ParticipantCode);
                    headers.Add(Constants.HCX_RECIPIENT_CODE, recipientCode);
                    headers.Add(Constants.HCX_CORRELATION_ID, Guid.NewGuid().ToString());
                }
                else
                {
                    Dictionary<string, object> actionHeaders = JSONUtils.DecodeBase64String<Dictionary<string, object>>(actionJwe.Split('.')[0]);
                    headers.Add(Constants.HCX_SENDER_CODE, actionHeaders[Constants.HCX_RECIPIENT_CODE]);
                    headers.Add(Constants.HCX_RECIPIENT_CODE, actionHeaders[Constants.HCX_SENDER_CODE]);
                    headers.Add(Constants.HCX_CORRELATION_ID, actionHeaders[Constants.HCX_CORRELATION_ID]);
                    headers.Add(Constants.STATUS, onActionStatus);
                    if (headers.ContainsKey(Constants.WORKFLOW_ID))
                        headers.Add(Constants.WORKFLOW_ID, actionHeaders[Constants.WORKFLOW_ID].ToString());
                }
                return true;
            }
            catch (Exception ex)
            {
                headers.Add(Constants.ERROR, "Error while creating headers: " + ex.Message.ToString());
                return false;
            }
        }

        public bool EncryptPayload(Dictionary<string, object> headers, string fhirPayload, Dictionary<string, object> encryptPayload)
        {
            try
            {
                string publicKeyUrl = (string)HcxUtils.SearchRegistry(headers[Constants.HCX_RECIPIENT_CODE])[Constants.ENCRYPTION_CERT];
                RSA rsaPublicKey = X509KeyLoader.GetRSAPublicKeyFromPem(publicKeyUrl, PemMode.Url);
                JweRequest jweRequest = new JweRequest(headers, JSONUtils.Deserialize<Dictionary<string, object>>(fhirPayload));
                jweRequest.EncryptRequest(rsaPublicKey);
                encryptPayload.Add(jweRequest.GetEncryptedObject().First().Key, jweRequest.GetEncryptedObject().First().Value);
                return true;
            }
            catch (Exception ex)
            {
                encryptPayload.Add(Constants.ERROR, ex.Message.ToString());
                return false;
            }
        }

        // we are handling the Exception in Process method
        public bool InitializeHCXCall(string jwePayload, Operations operation, Dictionary<string, object> response)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add(Constants.AUTHORIZATION, "Bearer " + HcxUtils.GenerateToken());
            HttpResponse hcxResponse = HttpUtils.Post(HCXIntegrator.config.ProtocolBasePath + operation.getOperation(), headers, jwePayload);
            response.Add(Constants.RESPONSE_OBJ, JSONUtils.Deserialize<Dictionary<string, object>>(hcxResponse.Body));
            return hcxResponse.Status == 202;
        }

    }
}
