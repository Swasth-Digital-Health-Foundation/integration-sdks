namespace Io.HcxProtocol.Utils
{
    /**
     * Library  : Io.Hcx.Protocol.Core
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// All the error messages used in HCX Integrator SDK.
    /// </summary>
    public class ResponseMessage
    {
        public static readonly string INVALID_PAYLOAD_LENGTH_ERR_MSG = "Mandatory elements of JWE token are missing.Should have all 5 elements";
        public static readonly string INVALID_PAYLOAD_VALUES_ERR_MSG = "Payload contains null or empty values";
        public static readonly string INVALID_API_CALL_ID_ERR_MSG = "API call id should be a valid UUID";
        public static readonly string INVALID_CORRELATION_ID_ERR_MSG = "Correlation id should be a valid UUID";
        public static readonly string INVALID_WORKFLOW_ID_ERR_MSG = "Workflow id should be a valid UUID";
        public static readonly string INVALID_TIMESTAMP_ERR_MSG = "Timestamp cannot be in future date";
        public static readonly string INVALID_DEBUG_FLAG_ERR_MSG = "Debug flag cannot be null, empty and other than 'String'";
        public static readonly string INVALID_DEBUG_FLAG_RANGE_ERR_MSG = "Debug flag cannot be other than {0}";
        public static readonly string INVALID_ERROR_DETAILS_ERR_MSG = "Error details cannot be null, empty and other than 'JSON Object' with mandatory fields code or message";
        public static readonly string INVALID_ERROR_DETAILS_RANGE_ERR_MSG = "Error details should contain only: {0}";
        public static readonly string INVALID_ERROR_DETAILS_CODE_ERR_MSG = "Invalid Error Code";
        public static readonly string INVALID_DEBUG_DETAILS_ERR_MSG = "Debug details cannot be null, empty and other than 'JSON Object' with mandatory fields code or message";
        public static readonly string INVALID_DEBUG_DETAILS_RANGE_ERR_MSG = "Debug details should contain only: {0}";
        public static readonly string INVALID_STATUS_ERR_MSG = "Status cannot be null, empty and other than 'String'";
        public static readonly string INVALID_STATUS_ACTION_RANGE_ERR_MSG = "Status value for action API calls can be only: {0}";
        public static readonly string INVALID_STATUS_ON_ACTION_RANGE_ERR_MSG = "Status value for on_action API calls can be only: {0}";

        public static readonly string INVALID_MANDATORY_ERR_MSG = "Mandatory headers are missing: {0}";
        public static readonly string INVALID_REDIRECT_ERR_MSG = "Redirect requests must have valid participant code for field {0}";
        public static readonly string INVALID_REDIRECT_SELF_ERR_MSG = "Sender can not redirect request to self";
        public static readonly string INVALID_JSON_REQUEST_BODY_ERR_MSG = "Request body should be a proper JWE object for action API calls";
    }
}
