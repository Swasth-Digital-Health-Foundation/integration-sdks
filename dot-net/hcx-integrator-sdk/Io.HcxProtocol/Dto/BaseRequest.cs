using Io.HcxProtocol.Exceptions;
using Io.HcxProtocol.Impl;
using Io.HcxProtocol.Interfaces;
using Io.HcxProtocol.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Io.HcxProtocol.Dto
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// This is base class to extract the protocol headers from the jwe/json payload and Validate Headers Data.
    /// </summary>
    public class BaseRequest : IBaseRequest, IDisposable
    {
        private Dictionary<string, object> Payload { get; set; }
        public Dictionary<string, object> ProtocolHeaders { get; set; }

      private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public BaseRequest(Dictionary<string, object> payload)
        {
            this.Payload = payload;
            if (payload.ContainsKey(Constants.PAYLOAD))
                this.ProtocolHeaders = JSONUtils.DecodeBase64String<Dictionary<string, object>>(GetPayloadValues()[0]);
            else
                this.ProtocolHeaders = payload;
        }

        public Dictionary<string, object> GetPayload()
        {
            return Payload;
        }
        public string GetWorkflowId()
        {
            return GetHeader(Constants.WORKFLOW_ID);
        }
        public string GetApiCallId()
        {
            return GetHeader(Constants.HCX_API_CALL_ID);
        }
        public string GetCorrelationId()
        {
            return GetHeader(Constants.HCX_CORRELATION_ID);
        }
        public string GetHcxSenderCode()
        {
            return GetHeader(Constants.HCX_SENDER_CODE);
        }
        public string GetTimestamp()
        {
            return GetHeader(Constants.HCX_TIMESTAMP);
        }
        public string GetDebugFlag()
        {
            return GetHeader(Constants.DEBUG_FLAG);
        }
        public string GetStatus()
        {
            return GetHeader(Constants.STATUS);
        }
        protected string GetHeader(string key)
        {
            string headerValue;

            if (key == Constants.HCX_TIMESTAMP)
            {
                try
                {
                    headerValue = Convert.ToDateTime(ProtocolHeaders[key]).ToString();
                }
                catch (Exception)
                {
                    headerValue = "";
                }
            }
            else
            {
                headerValue = ProtocolHeaders.TryGetValue(key, out var value) ? (string)value : "";
            }

            return headerValue;
        }
        protected Dictionary<string, object> GetHeaderObj(string key)
        {
            string objString = JSONUtils.Serialize(ProtocolHeaders[key]);
            return JSONUtils.Deserialize<Dictionary<string, object>>(objString);
        }

        private void SetHeaderObj(string key, object value)
        {
            ProtocolHeaders.Add(key, value);
        }

        public Dictionary<string, object> GetErrorDetails()
        {
            return GetHeaderObj(Constants.ERROR_DETAILS);
        }
        public Dictionary<string, object> GetDebugDetails()
        {
            return GetHeaderObj(Constants.DEBUG_DETAILS);
        }
        public string GetRedirectTo()
        {
            return GetHeader(Constants.REDIRECT_TO);
        }
        public string[] GetPayloadValues()
        {
            return Payload[Constants.PAYLOAD].ToString().Split('.');
        }
        public bool ValidateHeadersData(List<string> mandatoryHeaders, Operations operation, Dictionary<string, object> error)
        {
            List<string> missingHeaders = mandatoryHeaders.Where(key => !ProtocolHeaders.ContainsKey(key)).ToList();
            if (missingHeaders.Any())
            {
                error.Add(ErrorCodes.ERR_MANDATORY_HEADER_MISSING.ToString(), string.Format(ResponseMessage.INVALID_MANDATORY_ERR_MSG, string.Join(", ", missingHeaders)));
                return true;
            }
            // Validate Header values
           
            if (ValidateCondition(GetApiCallId().GetType()!= typeof(string) , error, ErrorCodes.ERR_INVALID_API_CALL_ID.ToString(), ResponseMessage.INVALID_API_CALL_ID_ERR_MSG)) //change no 5 implemented
                return true;
            if (ValidateCondition(GetCorrelationId().GetType()!=typeof(string), error, ErrorCodes.ERR_INVALID_CORRELATION_ID.ToString(), ResponseMessage.INVALID_CORRELATION_ID_ERR_MSG)) //change no 5 implemented
                return true;
            if (ValidateCondition(!GetTimestamp().IsValidTimestamp(), error, ErrorCodes.ERR_INVALID_TIMESTAMP.ToString(), ResponseMessage.INVALID_TIMESTAMP_ERR_MSG))
                return true;
            if (ValidateCondition(ProtocolHeaders.ContainsKey(Constants.WORKFLOW_ID) && GetWorkflowId().GetType()!=typeof(string), error, ErrorCodes.ERR_INVALID_WORKFLOW_ID.ToString(), ResponseMessage.INVALID_WORKFLOW_ID_ERR_MSG)) //change no 5 implemented
                return true;
            //validating optional headers
            ValidateOptionalHeaders(error);
            // validating onAction headers
            ValidateOnAction(operation, error);

            return false;
        }

        public bool ValidateJwePayload(Dictionary<string, object> error, string[] payloadArr)
        {
            bool status = false;
            try
            {
                if (payloadArr != null && payloadArr.Length != Constants.PROTOCOL_PAYLOAD_LENGTH)
                {
                    error.Add(ErrorCodes.ERR_INVALID_PAYLOAD.ToString(), ResponseMessage.INVALID_PAYLOAD_LENGTH_ERR_MSG);
                   _logger.Error(ErrorCodes.ERR_INVALID_PAYLOAD.ToString(), ResponseMessage.INVALID_PAYLOAD_LENGTH_ERR_MSG);
                    status = true;
                    return status;
                }
                if (payloadArr != null)
                {
                    foreach (string value in payloadArr)
                    {
                        if (string.IsNullOrEmpty(value))
                        {
                            error.Add(ErrorCodes.ERR_INVALID_PAYLOAD.ToString(), ResponseMessage.INVALID_PAYLOAD_VALUES_ERR_MSG);
                          _logger.Error(ErrorCodes.ERR_INVALID_PAYLOAD.ToString(), ResponseMessage.INVALID_PAYLOAD_VALUES_ERR_MSG);
                            status = true;
                            return status;
                        }
                    }
                }
                status = false;
            }
            catch(Exception ex)
            {
                error.Add("Some error occuured", ex.Message.ToString());
                _logger.Error("Some error occuured", ex.Message.ToString());

            }
            return status;
        }

        public bool ValidateCondition(bool condition, Dictionary<string, object> error, string key, string msg)
        {
            if (condition)
            {
                error.Add(key, msg);
                return true;
            }
            return false;
        }
        public bool ValidateDetails(Dictionary<string, object> inputObj, Dictionary<string, object> error, string errorKey, string msg, List<string> rangeValues, string rangeMsg)
        {
            if (inputObj == null || inputObj.Count == 0)
            {
                error.Add(errorKey, msg);
                return true;
            }
            else if (!inputObj.ContainsKey(Constants.CODE) || !inputObj.ContainsKey(Constants.MESSAGE))
            {
                error.Add(errorKey, msg);
                return true;
            }
            foreach (string key in inputObj.Keys)
            {
                if (!rangeValues.Contains(key))
                {
                    error.Add(key, rangeMsg);
                    return true;
                }
            }
            return false;
        }

        public bool ValidateOptionalHeaders(Dictionary<string, object> error)
        {
            if (ProtocolHeaders.ContainsKey(Constants.DEBUG_FLAG) && ValidateValues(GetDebugFlag(), error, ErrorCodes.ERR_INVALID_DEBUG_FLAG.ToString(), ResponseMessage.INVALID_DEBUG_FLAG_ERR_MSG, Constants.DEBUG_FLAG_VALUES, string.Format(ResponseMessage.INVALID_DEBUG_FLAG_RANGE_ERR_MSG, string.Join(", ", Constants.DEBUG_FLAG_VALUES))))
                return true;
            if (ProtocolHeaders.ContainsKey(Constants.ERROR_DETAILS))
            {
                if (ValidateDetails(GetErrorDetails(), error, ErrorCodes.ERR_INVALID_ERROR_DETAILS.ToString(), ResponseMessage.INVALID_ERROR_DETAILS_ERR_MSG, Constants.ERROR_DETAILS_VALUES, string.Format(ResponseMessage.INVALID_ERROR_DETAILS_RANGE_ERR_MSG, string.Join(", ", Constants.ERROR_DETAILS_VALUES))))
                    return true;
                if (ValidateCondition(!Constants.RECIPIENT_ERROR_VALUES.Contains(((Dictionary<string, object>)ProtocolHeaders[Constants.ERROR_DETAILS])[Constants.CODE]), error, ErrorCodes.ERR_INVALID_ERROR_DETAILS.ToString(), ResponseMessage.INVALID_ERROR_DETAILS_CODE_ERR_MSG))
                    return true;
            }
            if (ProtocolHeaders.ContainsKey(Constants.DEBUG_DETAILS))
            {
                return ValidateDetails(GetDebugDetails(), error, ErrorCodes.ERR_INVALID_DEBUG_DETAILS.ToString(), ResponseMessage.INVALID_DEBUG_DETAILS_ERR_MSG, Constants.ERROR_DETAILS_VALUES, string.Format(ResponseMessage.INVALID_DEBUG_DETAILS_RANGE_ERR_MSG, string.Join(", ", Constants.ERROR_DETAILS_VALUES)));
            }
            return false;
        }

        public bool ValidateOnAction(Operations operation, Dictionary<string, object> error)
        {
            if (operation.getOperation().Contains("on_"))
            {
                if (ValidateCondition(!ProtocolHeaders.ContainsKey(Constants.STATUS), error, ErrorCodes.ERR_INVALID_STATUS.ToString(), string.Format(ResponseMessage.INVALID_MANDATORY_ERR_MSG, Constants.STATUS)))
                    return true;
                return ValidateValues(GetStatus(), error, ErrorCodes.ERR_INVALID_STATUS.ToString(), ResponseMessage.INVALID_STATUS_ERR_MSG, Constants.RESPONSE_STATUS_VALUES, string.Format(ResponseMessage.INVALID_STATUS_ON_ACTION_RANGE_ERR_MSG, string.Join(", ", Constants.RESPONSE_STATUS_VALUES)));
            }
            else
            {
                if (ProtocolHeaders.ContainsKey(Constants.STATUS))
                {
                    return ValidateValues(GetStatus(), error, ErrorCodes.ERR_INVALID_STATUS.ToString(), ResponseMessage.INVALID_STATUS_ERR_MSG, Constants.REQUEST_STATUS_VALUES, string.Format(ResponseMessage.INVALID_STATUS_ACTION_RANGE_ERR_MSG, string.Join(", ", Constants.REQUEST_STATUS_VALUES)));
                }
            }
            return false;
        }

        public bool ValidateValues(string inputStr, Dictionary<string, object> error, string key, string msg, List<string> statusValues, string rangeMsg)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                error.Add(key, msg);
                return true;
            }
            else if (!statusValues.Contains(inputStr))
            {
                error.Add(key, rangeMsg);
                return true;
            }
            return false;
        }

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            // check if already disposed
            if (!disposedValue)
            {
                if (disposing)
                {
                    // free managed objects here
                    Payload = null;
                    ProtocolHeaders = null;
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

        ~BaseRequest() { Dispose(disposing: false); }
    }
}
