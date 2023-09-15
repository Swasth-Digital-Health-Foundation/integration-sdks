using Hl7.Fhir.ElementModel.Types;
using ICSharpCode.SharpZipLib.Zip;
using Io.HcxProtocol.Dto;
using Io.HcxProtocol.Exceptions;
using Io.HcxProtocol.Helper;
using Io.HcxProtocol.Init;
using Io.HcxProtocol.Interfaces;
using Io.HcxProtocol.Jwe;
using Io.HcxProtocol.Key;
using Io.HcxProtocol.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Config = Io.HcxProtocol.Init.Config;

namespace Io.HcxProtocol.Impl
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

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
    public class HCXIncomingRequest : FhirPayload, IIncomingRequest, IDisposable
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public HCXIncomingRequest() { }
     
        public virtual bool Process(string jwePayload, Operations operation, Dictionary<string, object> output,Config config)
        {
            Dictionary<string, object> error = new Dictionary<string, object>();
            bool result = false;
            jwePayload = FormatPayload(jwePayload);

            if (!ValidateRequest(jwePayload, operation, error))
            {
                SendResponse(error, output);
            }
            else if (!DecryptPayload(jwePayload,config.EncryptionPrivateKey ,output))
            {
                SendResponse(output, output);
            }
        
            else if(config.FhirValidationEnabled)  //Fhirvalidation enable disabled code
            {
                 if (!ValidatePayload(output[Constants.FHIR_PAYLOAD].ToString(), operation, error, config))
                {
                    SendResponse(error, output);
                }
            }
           
            else
            {
                if (SendResponse(error, output)) result = true;
            }
            return result;
        }

        public virtual bool ValidateRequest(string jwePayload, Operations operation, Dictionary<string, object> error)
        {
            return ValidateHelper.GetInstance().ValidateRequest(jwePayload, operation, error);
        }      

        public virtual bool DecryptPayload(string jwePayload,string privateKey, Dictionary<string, object> output)
        {
            try
            {
                JweRequest jweRequest = new JweRequest(JSONUtils.Deserialize<Dictionary<string, object>>(jwePayload));
                RSA rsaPublicKey = X509KeyLoader.GetRSAPrivateKeyFromPem(privateKey, PemMode.FileText);
                jweRequest.DecryptRequest(rsaPublicKey);
                output.Add(Constants.HEADERS, jweRequest.GetHeaders());
                output.Add(Constants.FHIR_PAYLOAD, JSONUtils.Serialize(jweRequest.GetPayload()));
               // _logger.Info("Payload decryption is successful", JSONUtils.Serialize(jweRequest.GetPayload()));
                return true;
            }
            catch (Exception ex)
            {
                output.Add(ErrorCodes.ERR_INVALID_ENCRYPTION.ToString(), "[Decryption error.] " + ex.ToString());
                _logger.Error(ErrorCodes.ERR_INVALID_ENCRYPTION.ToString(), "[Decryption error.] " + ex.ToString());
                return false;
            }
        }

        public  virtual bool ValidatePayload(string fhirPayload, Operations operation, Dictionary<string, object> error, Config config)
        {
            return ValidateFHIR(fhirPayload, operation, error, config);
        }

        public virtual bool SendResponse(Dictionary<string, object> error, Dictionary<string, object> output)
        {
            Dictionary<string, object> responseObj = new Dictionary<string, object>();
            responseObj.Add(Constants.TIMESTAMP, DateTimeUtils.TotalMillisecondsUTC());
            bool result = false;
            try
            {
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
              //  _logger.Info("Response sent successfully");
            }
            catch(Exception ex)
            {
                result = false;
                _logger.Error("Response sending failed " + ex.Message);
            }
           
            return result;
        }

        private  string FormatPayload(string payload)
        {
            string FormattedPayload = string.Empty;
            try
            {
                
                if (payload.Contains(Constants.PAYLOAD) || payload.Contains(Constants.HCX_API_CALL_ID))
                {
                    FormattedPayload = payload;
                    
                }
                else
                {
                    Dictionary<string, string> payloadDict = new Dictionary<string, string>()
                 {
                   { Constants.PAYLOAD, payload }
                 };
                    FormattedPayload= JsonConvert.SerializeObject(payloadDict);
                }
            }
           
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return FormattedPayload;
        }


        private bool disposedValue;
        protected  void Dispose(bool disposing)
        {
            // check if already disposed
            if (!disposedValue)
            {
                if (disposing)
                {
                    // free managed objects here
                }

                // free unmanaged objects here

                // set the bool value to true
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private string getPayload(string payload)
        {
            string FinalPayload = string.Empty;
            try
            {
                if (payload.Contains(Constants.PAYLOAD) || payload.Contains(Constants.HCX_API_CALL_ID))
                {
                    FinalPayload= payload;
                }
                else
                {
                    FinalPayload = JSONUtils.Serialize((Constants.PAYLOAD, payload));
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return FinalPayload;
        }
        public Dictionary<string, object> receiveNotification(string jwsPayload, Dictionary<string, object> output, Config config)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();


            Dictionary<string, Object> payload = JSONUtils.Deserialize<Dictionary<string, object>>(getPayload(jwsPayload));
            NotificationRequest notificationRequest = new NotificationRequest((string)payload["Item2"]);
            if (string.IsNullOrEmpty(notificationRequest.getJwsPayload()))
            {
                throw new ClientException("JWS Token cannot be empty");
            }
            string authToken = HcxUtils.GenerateToken(config);
            string publicKeyUrl = (string)HcxUtils.SearchRegistry(notificationRequest.getSenderCode(), authToken, config.ProtocolBasePath)[Constants.ENCRYPTION_CERT];
            bool isSignatureValid = HcxUtils.isValidSignature((string)payload["Item2"], publicKeyUrl,out output);
            if (output == null)
            {
                output = new Dictionary<string, Object>();
            }
            if (isSignatureValid == false)
            {
                output.Add(Constants.HEADERS, notificationRequest.getHeaders());
                output.Add(Constants.PAYLOAD, notificationRequest.getPayload());
                output.Add(Constants.IS_SIGNATURE_VALID, isSignatureValid);
            }
            return output;


        }

        //                   }



        ~HCXIncomingRequest() { Dispose(disposing: false); }

    }
}
