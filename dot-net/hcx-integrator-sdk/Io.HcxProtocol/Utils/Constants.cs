using Hl7.Fhir.Utility;
using System.Collections.Generic;

namespace Io.HcxProtocol.Utils
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// All the constant variables used in HCX Integrator SDK.
    /// </summary>
    public class Constants
    {
        public static readonly string HCX_SENDER_CODE = "x-hcx-sender_code";
        public static readonly string HCX_RECIPIENT_CODE = "x-hcx-recipient_code";
        public static readonly string HCX_API_CALL_ID = "x-hcx-api_call_id";
        public static readonly string HCX_CORRELATION_ID = "x-hcx-correlation_id";
        public static readonly string WORKFLOW_ID = "x-hcx-workflow_id";
        public static readonly string HCX_TIMESTAMP = "x-hcx-timestamp";
        public static readonly string STATUS = "x-hcx-status";
        public static readonly string ALG = "alg";
        public static readonly string ENC = "enc";
        public static readonly string ERROR = "error";
        public static readonly string A256GCM = "A256GCM";
        public static readonly string RSA_OAEP = "RSA-OAEP";
        public static readonly string AUTHORIZATION = "Authorization";
        public static readonly string ENCRYPTION_CERT = "encryption_cert";
        public static readonly string PAYLOAD = "payload";
        public static readonly string FHIR_PAYLOAD = "fhirPayload";
        public static readonly string HEADERS = "headers";
        public static readonly string RESPONSE_OBJ = "responseObj";
        public static readonly string TIMESTAMP = "timestamp";
        public static readonly string API_CALL_ID = "api_call_id";
        public static readonly string CORRELATION_ID = "correlation_id";

        public static readonly string DEBUG_FLAG = "x-hcx-debug_flag";
        public static readonly string ERROR_DETAILS = "x-hcx-error_details";
        public static readonly string DEBUG_DETAILS = "x-hcx-debug_details";
        public static readonly string REDIRECT_STATUS = "response.redirect";
        public static readonly string COMPLETE_STATUS = "response.complete";
        public static readonly string PARTIAL_STATUS = "response.partial";

        public static readonly int PROTOCOL_PAYLOAD_LENGTH = 5;
        public static readonly string REDIRECT_TO = "x-hcx-redirect_to";

        public static List<string> DEBUG_FLAG_VALUES = new List<string>() { "Error", "Info", "Debug" };
        public static List<string> REQUEST_STATUS_VALUES = new List<string>() { "request.queued", "request.dispatched" };
        public static List<string> ERROR_DETAILS_VALUES = new List<string>() { "code", "message", "trace" };
        public static string ERROR_RESPONSE = "response.error";

        public static List<string> RECIPIENT_ERROR_VALUES = new List<string>() {"ERR_INVALID_ENCRYPTION", "ERR_INVALID_PAYLOAD", "ERR_WRONG_DOMAIN_PAYLOAD", "ERR_INVALID_DOMAIN_PAYLOAD", "ERR_SENDER_NOT_SUPPORTED", "ERR_SERVICE_UNAVAILABLE", "ERR_DOMAIN_PROCESSING", "ERR_MANDATORY_HEADER_MISSING",
            "ERR_INVALID_API_CALL_ID", "ERR_INVALID_CORRELATION_ID", "ERR_INVALID_TIMESTAMP", "ERR_INVALID_REDIRECT_TO", "ERR_INVALID_STATUS", "ERR_INVALID_DEBUG_FLAG", "ERR_INVALID_ERROR_DETAILS", "ERR_INVALID_DEBUG_DETAILS", "ERR_INVALID_WORKFLOW_ID" };

        public static List<string> RESPONSE_STATUS_VALUES = new List<string>() { COMPLETE_STATUS, PARTIAL_STATUS, ERROR_RESPONSE, REDIRECT_STATUS };

        public static readonly string CODE = "code";
        public static readonly string MESSAGE = "message";
        public static readonly string PARTICIPANTS = "participants";


        public static readonly string PROTOCOL_BASE_PATH = "protocolBasePath";
        public static readonly string PARTICIPANT_CODE = "participantCode";
        public static readonly string AUTH_BASE_PATH = "authBasePath";
        public static readonly string USERNAME = "username";
        public static readonly string PASSWORD = "password";
        public static readonly string ENCRYPTION_PRIVATE_KEY = "encryptionPrivateKey";
        public static readonly string HCX_IG_BASE_PATH = "hcxIGBasePath";
        public static readonly string NRCES_IG_BASE_PATH = "nrcesIGBasePath";
        public static readonly string INCOMING_REQUEST_CLASS = "incomingRequestClass";
        public static readonly string OUTGOING_REQUEST_CLASS = "outgoingRequestClass";

        public static readonly string NOTIFICATION_HEADERS = "x-hcx-notification_headers"; 
        public static readonly string SECRET = "secret";
        public static readonly string PARTICIPANT_GENERATE_TOKEN = "/participant/auth/token/generate"; 
        public static readonly string RECIPIENT_TYPE =  "recipient_type";
        public static readonly string RECIPIENTS = "recipients";
       
        public static readonly string TOPIC_CODE = "topic_code";
        public static readonly string TEMPLATE = "template";
        public static readonly string IS_SIGNATURE_VALID = "isSignatureValid"; 
    }
}
