class Constants:
    HCX_SENDER_CODE = "x-hcx-sender_code"
    HCX_RECIPIENT_CODE = "x-hcx-recipient_code"
    HCX_API_CALL_ID = "x-hcx-api_call_id"
    HCX_CORRELATION_ID = "x-hcx-correlation_id"
    WORKFLOW_ID = "x-hcx-workflow_id"
    HCX_TIMESTAMP = "x-hcx-timestamp"
    STATUS = "x-hcx-status"
    ALG = "alg"
    ENC = "enc"
    ERROR = "error"
    A256GCM = "A256GCM"
    RSA_OAEP = "RSA-OAEP"
    AUTHORIZATION = "Authorization"
    ENCRYPTION_CERT = "encryption_cert"
    PAYLOAD = "payload"
    FHIR_PAYLOAD = "fhirPayload"
    HEADERS = "headers"
    RESPONSE_OBJ = "responseObj"
    TIMESTAMP = "timestamp"
    API_CALL_ID = "api_call_id"
    CORRELATION_ID = "correlation_id"

    DEBUG_FLAG = "x-hcx-debug_flag"
    ERROR_DETAILS = "x-hcx-error_details"
    DEBUG_DETAILS = "x-hcx-debug_details"
    REDIRECT_STATUS = "response.redirect"
    COMPLETE_STATUS = "response.complete"
    PARTIAL_STATUS = "response.partial"

    PROTOCOL_PAYLOAD_LENGTH = 5
    REDIRECT_TO = "x-hcx-redirect_to"
    DEBUG_FLAG_VALUES = ["Error", "Info", "Debug"]
    REQUEST_STATUS_VALUES = ["request.queued", "request.dispatched"]
    ERROR_DETAILS_VALUES = ["code", "message", "trace"]
    ERROR_RESPONSE = "response.error"
    RECIPIENT_ERROR_VALUES = [
        "ERR_INVALID_ENCRYPTION",
        "ERR_INVALID_PAYLOAD",
        "ERR_WRONG_DOMAIN_PAYLOAD",
        "ERR_INVALID_DOMAIN_PAYLOAD",
        "ERR_SENDER_NOT_SUPPORTED",
        "ERR_SERVICE_UNAVAILABLE",
        "ERR_DOMAIN_PROCESSING",
        "ERR_MANDATORY_HEADER_MISSING",
        "ERR_INVALID_API_CALL_ID",
        "ERR_INVALID_CORRELATION_ID",
        "ERR_INVALID_TIMESTAMP",
        "ERR_INVALID_REDIRECT_TO",
        "ERR_INVALID_STATUS",
        "ERR_INVALID_DEBUG_FLAG",
        "ERR_INVALID_ERROR_DETAILS",
        "ERR_INVALID_DEBUG_DETAILS",
        "ERR_INVALID_WORKFLOW_ID",
    ]
    RESPONSE_STATUS_VALUES = [COMPLETE_STATUS, PARTIAL_STATUS, ERROR_RESPONSE, REDIRECT_STATUS]
    CODE = "code"
    MESSAGE = "message"
    PARTICIPANTS = "participants"
    PROTOCOL_BASE_PATH = "protocolBasePath"
    PARTICIPANT_CODE = "participantCode"
    AUTH_BASE_PATH = "authBasePath"
    USERNAME = "username"
    PASSWORD = "password"
    ENCRYPTION_PRIVATE_KEY = "encryptionPrivateKey"
    HCX_IG_BASE_PATH = "hcxIGBasePath"
    NRCES_IG_BASE_PATH = "nrcesIGBasePath"

    FHIR_VALIDATION_ENABLED = "fhirValidationEnabled"
    INCOMING_REQUEST_CLASS = "incomingRequestClass"
    OUTGOING_REQUEST_CLASS = "outgoingRequestClass"
