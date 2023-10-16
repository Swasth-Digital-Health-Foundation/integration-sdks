using Io.HcxProtocol.Dto;
using Io.HcxProtocol.Exceptions;
using Io.HcxProtocol.Utils;
using System;
using System.Collections.Generic;

namespace Io.HcxProtocol.Helper
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// This is to validate the incoming request protocol headers.
    /// </summary>
    public class ValidateHelper
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private static ValidateHelper validateHelper = null;

        private ValidateHelper() { }

        public static ValidateHelper GetInstance()
        {
            if (validateHelper == null)
                validateHelper = new ValidateHelper();
            return validateHelper;
        }

        /// <summary>
        ///     Validates the incoming payload by verifying the structure and contents of the headers inside the payload
        /// </summary>
        /// <remarks>
        ///     ERR_INVALID_PAYLOAD:
        ///     <list type="number">
        ///         <item>Request body is not a valid JWE token (as defined in RFC 7516)</item>
        ///         <item>Any mandatory elements of JWE token are missing</item>
        ///         <item>Any elements of the JWE token are in invalid format</item>
        ///     </list>
        ///     HCX Protocol errors:
        ///     <list type="bullet">
        ///         <item>ERR_MANDATORY_HEADER_MISSING</item>
        ///         <item>ERR_INVALID_API_CALL_ID</item>
        ///         <item>ERR_INVALID_CORRELATION_ID: check only for the format correctness</item>
        ///         <item>ERR_INVALID_TIMESTAMP</item>
        ///         <item>ERR_INVALID_REDIRECT_TO</item>
        ///         <item>ERR_INVALID_STATUS</item>
        ///         <item>ERR_INVALID_DEBUG_FLAG</item>
        ///         <item>ERR_INVALID_ERROR_DETAILS</item>
        ///         <item>ERR_INVALID_DEBUG_DETAILS</item>
        ///         <item>ERR_INVALID_WORKFLOW_ID: check only for the format correctness</item>
        ///     </list>
        /// </remarks>
        /// <param name="payload">json string with payload</param>
        /// <param name="operation">which operation is being processed</param>
        /// <param name="error">holds any validation errors</param>
        /// <returns>
        ///     true if it is valid request otherwise returns false along with proper error message in the error object
        /// </returns>
        public bool ValidateRequest(string payload, Operations operation, Dictionary<string, object> error)
        {
            try
            {
                // Convert the input string into a object
                Dictionary<string, object> requestBody = JSONUtils.Deserialize<Dictionary<string, object>>(payload);
                if (requestBody.ContainsKey(Constants.PAYLOAD))
                {
                    if (ValidateJweRequest(operation, error, requestBody)) return false;
                }
                else
                {
                    if (!operation.ToString().Contains("ON_"))
                    {
                        error.Add(ErrorCodes.ERR_INVALID_PAYLOAD.ToString(), ResponseMessage.INVALID_JSON_REQUEST_BODY_ERR_MSG);
                        return false;
                    }
                    if (!ValidateJsonRequest(operation, error, requestBody)) return false;
                }
            }
            catch (Exception ex)
            {
                error.Add(ErrorCodes.ERR_INVALID_PAYLOAD.ToString(), ex.ToString());
                _logger.Error(ErrorCodes.ERR_INVALID_PAYLOAD.ToString(), ex.ToString());
                return false;
            }
            return true;
        }

        private bool ValidateJweRequest(Operations operation, Dictionary<string, object> error, Dictionary<string, object> requestBody)
        {
            // Fetch the value of the only key(payload) from the object
            JWERequest jweRequest = new JWERequest(requestBody);

            // Split the extracted value into an array using . as a delimiter
            string[] payloadArr = jweRequest.GetPayloadValues();
            if (jweRequest.ValidateJwePayload(error, payloadArr)) return true;

            // Validate the headers and if there are any failures add the corresponding error message to the error object
            // protocol_mandatory_headers:x-hcx-sender_code, x-hcx-recipient_code, x-hcx-api_call_id, x-hcx-timestamp, x-hcx-correlation_id
            return jweRequest.ValidateHeadersData(new List<string> { Constants.ALG, Constants.ENC, Constants.HCX_SENDER_CODE, Constants.HCX_RECIPIENT_CODE, Constants.HCX_API_CALL_ID, Constants.HCX_TIMESTAMP, Constants.HCX_CORRELATION_ID }, operation, error);
        }

        private bool ValidateJsonRequest(Operations operation, Dictionary<string, object> error, Dictionary<string, object> requestBody)
        {
            JSONRequest jsonRequest = new JSONRequest(requestBody);
            List<string> headersList;

            if (Constants.ERROR_RESPONSE.Equals(jsonRequest.GetStatus(), StringComparison.OrdinalIgnoreCase))
            {
                //error_mandatory_headers:x-hcx-status, x-hcx-sender_code, x-hcx-recipient_code, x-hcx-error_details, x-hcx-correlation_id, x-hcx-api_call_id, x-hcx-timestamp
                headersList = new List<string> { Constants.STATUS, Constants.HCX_SENDER_CODE, Constants.HCX_RECIPIENT_CODE, Constants.ERROR_DETAILS, Constants.HCX_CORRELATION_ID, Constants.HCX_API_CALL_ID, Constants.HCX_TIMESTAMP };
                return jsonRequest.ValidateHeadersData(headersList, operation, error);
            }
            else
            {
                //redirect_mandatory_headers:x-hcx-sender_code, x-hcx-recipient_code, x-hcx-api_call_id, x-hcx-timestamp, x-hcx-correlation_id, x-hcx-status, x-hcx-redirect_to
                headersList = new List<string> { Constants.HCX_SENDER_CODE, Constants.HCX_RECIPIENT_CODE, Constants.HCX_API_CALL_ID, Constants.HCX_TIMESTAMP, Constants.HCX_CORRELATION_ID, Constants.STATUS, Constants.REDIRECT_TO };
                if (jsonRequest.ValidateHeadersData(headersList, operation, error))
                    return true;
                return jsonRequest.ValidateRedirect(error);
            }
        }
    }
}
