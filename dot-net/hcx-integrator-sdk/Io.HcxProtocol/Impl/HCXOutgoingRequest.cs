using Io.HcxProtocol.Dto;
using Io.HcxProtocol.Exceptions;
using Io.HcxProtocol.Helper;
using Io.HcxProtocol.Init;
using Io.HcxProtocol.Interfaces;
using Io.HcxProtocol.Jwe;
using Io.HcxProtocol.Key;
using Io.HcxProtocol.Service;
using Io.HcxProtocol.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.IO;



namespace Io.HcxProtocol.Impl
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

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
    public class HCXOutgoingRequest : FhirPayload, IOutgoingRequest, IDisposable
    {

        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public HCXOutgoingRequest()
        {
            
        }
        //public virtual bool Process(string fhirPayload, Operations operation, string recipientCode, string apiCallId, string correlationId,string actionJwe, string onActionStatus, Dictionary<string, Object> domainHeaders, Dictionary<string, Object> output, Config config)
        //{
        //    bool result = false;
        //    try
        //    {
        //        Dictionary<string, Object> error = new Dictionary<string, Object>();
        //        Dictionary<string, Object> headers = new Dictionary<string, Object>(domainHeaders);
        //        Dictionary<string, Object> response = new Dictionary<string, Object>();

        //        if (config.FhirValidationEnabled)  //change ismail
        //        {
        //            if (!ValidatePayload(fhirPayload, operation, error, config))
        //            {
        //                foreach (var err in error) output.Add(err.Key, err.Value);
        //            }
        //        }
        //        if (!CreateHeader(config.ParticipantCode, recipientCode, apiCallId, correlationId, actionJwe, onActionStatus, headers, error))
        //        {
        //            foreach (var err in headers) output.Add(err.Key, err.Value);
        //        }
        //        else if (!EncryptPayload(headers, fhirPayload, output, config))
        //        {
        //            foreach (var err in error) output.Add(err.Key, err.Value);
        //        }
        //        else
        //        {
        //            result = InitializeHCXCall(JSONUtils.Serialize(output), operation, response, config);
        //            foreach (var err in response) output.Add(err.Key, err.Value);
        //        }
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: Exception is handled as domain processing error, we will be enhancing in next version.
        //        output.Add(ErrorCodes.ERR_DOMAIN_PROCESSING.ToString(), ex.Message.ToString());
        //        _logger.Error(ErrorCodes.ERR_DOMAIN_PROCESSING.ToString(), ex.Message.ToString());
        //        return result;
        //    }
        //}

        public virtual  bool Process(string fhirPayload, Operations operation, string recipientCode, string apiCallId, string correlationId, string actionJwe, string onActionStatus, Dictionary<string, Object> domainHeaders, Dictionary<string, Object> output, Config config)
        {
            return ProcessOutgoing(fhirPayload, operation, recipientCode, apiCallId, correlationId, "", actionJwe, onActionStatus, domainHeaders, output, config);
        }

        public  virtual bool Process(string fhirPayload, Operations operation, string recipientCode, string apiCallId, string correlationId, string workflowId, string actionJwe, string onActionStatus, Dictionary<string, Object> domainHeaders, Dictionary<string, Object> output, Config config)
        {
            return ProcessOutgoing(fhirPayload, operation, recipientCode, apiCallId, correlationId, workflowId, actionJwe, onActionStatus, domainHeaders, output, config);
        }


        public  bool ProcessOutgoing(string fhirPayload, Operations operation, string recipientCode, string apiCallId, string correlationId, string workflowId, string actionJwe, string onActionStatus, Dictionary<string, Object> domainHeaders, Dictionary<string, Object> output, Config config)
        {
            bool result = false;
            try
            {
                Dictionary<string, Object> error = new Dictionary<string, Object>();
                Dictionary<string, Object> response = new Dictionary<string, Object>();
                Dictionary<string, Object> headers = new Dictionary<string, Object>(domainHeaders);
               // _logger.Info("Processing outgoing request has started :: operation: {}", operation);

                if (config.FhirValidationEnabled)  //change ismail
                {
                    if (!ValidatePayload(fhirPayload, operation, error, config))
                    {
                        foreach (var err in error) output.Add(err.Key, err.Value);
                    }
                }
                else if (!CreateHeader(config.ParticipantCode, recipientCode, apiCallId, correlationId,workflowId, actionJwe, onActionStatus, headers, error))
                {
                    foreach (var err in headers) output.Add(err.Key, err.Value);
                }
                else if (!EncryptPayload(headers, fhirPayload, output, config))
                {
                    foreach (var err in error) output.Add(err.Key, err.Value);
                }
                else
                {
                    result = InitializeHCXCall(JSONUtils.Serialize(output), operation, response, config);
                    foreach (var res in response) output.Add(res.Key, res.Value);
                }
                if (output.ContainsKey(Constants.ERROR) || output.ContainsKey(ErrorCodes.ERR_DOMAIN_PROCESSING.ToString()))
                {
                    _logger.Error("Error while processing the outgoing request: {}", output);
                }
                return result;
            }
            catch (Exception ex)
            {
                // TODO: Exception is handled as domain processing error, we will be enhancing in next version.
                Console.WriteLine(ex.ToString());
                Console.Write(ex.StackTrace);
                output[ErrorCodes.ERR_DOMAIN_PROCESSING.ToString()] = ex.Message;
                _logger.Error("Error while processing the outgoing request: {}", ex.Message);
                return result;
            }
        }


        public virtual  bool ValidatePayload(string fhirPayload, Operations operation, Dictionary<string, Object> error, Config config)
        {
                return ValidateFHIR(fhirPayload, operation, error, config);                   
        }

        public virtual bool CreateHeader(string senderCode, string recipientCode, string apiCallId, string correlationId,string workflowId,  string actionJwe, string onActionStatus, Dictionary<string, Object> headers, Dictionary<string, Object> error)
        {
            try
            {
                //Note: [ALG, A256GCM] & [ENC, RSA_OAEP] headers added by JWE-EncryptRequest Method

                headers.Add(Constants.HCX_TIMESTAMP, DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss") + DateTime.Now.ToString("zzz").Replace(":", ""));

                if (string.IsNullOrEmpty(apiCallId))
                    apiCallId = Guid.NewGuid().ToString();
                headers.Add(Constants.HCX_API_CALL_ID, apiCallId);

                if (!string.IsNullOrEmpty(recipientCode))
                {
                    headers.Add(Constants.HCX_SENDER_CODE, senderCode);
                    headers.Add(Constants.HCX_RECIPIENT_CODE, recipientCode);
                    if (string.IsNullOrEmpty(correlationId))
                        correlationId = Guid.NewGuid().ToString();
                    headers.Add(Constants.HCX_CORRELATION_ID, correlationId);

                    if (!string.IsNullOrEmpty(workflowId))    //ismail CR No9
                        headers.Add(Constants.WORKFLOW_ID,workflowId);
                   
                }
                else
                {
                    Dictionary<string, Object> actionHeaders = JSONUtils.DecodeBase64String<Dictionary<string, Object>>(actionJwe.Split('.')[0]);
                    headers.Add(Constants.HCX_SENDER_CODE, actionHeaders[Constants.HCX_RECIPIENT_CODE]);
                    headers.Add(Constants.HCX_RECIPIENT_CODE, actionHeaders[Constants.HCX_SENDER_CODE]);
                    headers.Add(Constants.HCX_CORRELATION_ID, actionHeaders[Constants.HCX_CORRELATION_ID]);
                    headers.Add(Constants.STATUS, onActionStatus);
                    if (actionHeaders.ContainsKey(Constants.WORKFLOW_ID))
                        headers.Add(Constants.WORKFLOW_ID, actionHeaders[Constants.WORKFLOW_ID].ToString());
                }
                return true;
            }
            catch (Exception ex)
            {
                headers.Add(Constants.ERROR, "Error while creating headers: " + ex.Message.ToString());
                _logger.Error("Error while creating headers: " + ex.Message.ToString());
                return false;
            }
        }


        public virtual bool EncryptPayload(Dictionary<string, Object> headers, string fhirPayload, Dictionary<string, Object> encryptPayload, Config config)
        {
            try
            {
               
                //  string authToken = HcxUtils.GenerateToken(config.UserName, config.Password, config.AuthBasePath);
                string authToken = HcxUtils.GenerateToken(config);
                string publicKeyUrl = (string)HcxUtils.SearchRegistry((string)headers[Constants.HCX_RECIPIENT_CODE], authToken, config.ProtocolBasePath)[Constants.ENCRYPTION_CERT];

                RSA rsaPublicKey = X509KeyLoader.GetRSAPublicKeyFromPem(publicKeyUrl, PemMode.Url);
                JweRequest jweRequest = new JweRequest(headers, JSONUtils.Deserialize<Dictionary<string, Object>>(fhirPayload));
                jweRequest.EncryptRequest(rsaPublicKey);
                encryptPayload.Add(jweRequest.GetEncryptedObject().First().Key, jweRequest.GetEncryptedObject().First().Value);
                return true;
            }
            catch (Exception ex)
            {
                encryptPayload.Add(Constants.ERROR, ex.Message.ToString());
                _logger.Error("Error while encrypting the payload: {}" + ex.Message.ToString());
                return false;
            }
        }

        // we are handling the Exception in Process method
        public virtual bool InitializeHCXCall(string jwePayload, Operations operation, Dictionary<string, Object> response, Config config)
        {
            bool result = false;
            try
            {
                result = HcxUtils.initializeHCXCall(jwePayload, operation, response, config);
                //Dictionary<string, string> headers = new Dictionary<string, string>();
                //headers.Add(Constants.AUTHORIZATION, "Bearer " + HcxUtils.GenerateToken(config.UserName, config.Password, config.AuthBasePath));
                //HttpResponse hcxResponse = HttpUtils.Post(config.ProtocolBasePath + operation.getOperation(), headers, jwePayload);
                //response.Add(Constants.RESPONSE_OBJ, JSONUtils.Deserialize<Dictionary<string, Object>>(hcxResponse.Body));              

                //if (hcxResponse.Status == 202)
                //{
                //    result = true;
                //}
                //else
                //{
                //    _logger.Error("Error while processing the outgoing request::status: { } ::response: { }", hcxResponse.Status, response[Constants.RESPONSE_OBJ]);

                //}
            }
            catch(WebException ex)
            {
                _logger.Error(ex.Message);
            }
            return result;
        }

        private bool disposedValue;
        protected  void Dispose(bool disposing)
        {
            // check if already disposed
            if (!disposedValue)
            {
                if (disposing)
                {
                    // free managed Objects here
                }
                // free unmanaged Objects here
                // set the bool value to true
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public bool SendNotification(string topicCode, string recipientType, List<string> recipients, string message, Dictionary<string, string> templateParams, string correlationId, Dictionary<string, Object> output, Config config)
        {
            try
            {
                NotificationRequest notificationRequest = new NotificationRequest(topicCode, message, templateParams, recipientType, recipients, correlationId, config);
                NotificationService.validateNotificationRequest(notificationRequest);
                Dictionary<string, Object> requestBody = NotificationService.CreateNotificationRequest(notificationRequest, output, message);
                return InitializeHCXCall(JSONUtils.Serialize(requestBody), Operations.NOTIFICATION_NOTIFY, output, config);

            }
            catch (Exception e)
            {
                _logger.Error("Error while sending the notification: {}", e.Message);
                output.Add(Constants.ERROR, "Error while sending the notifications: " + e.Message);
                return false;
            }

        }

        ~HCXOutgoingRequest() { Dispose(disposing: false); }
    }
}
