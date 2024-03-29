﻿namespace Io.HcxProtocol.Exceptions
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// The ENUM containing all the Error Codes defined in HCX Gateway.
    /// </summary>
    public enum ErrorCodes
    {
        ERR_INVALID_PAYLOAD,
        ERR_INVALID_ENCRYPTION,
        ERR_WRONG_DOMAIN_PAYLOAD,
        ERR_INVALID_DOMAIN_PAYLOAD,
        ERR_SENDER_NOT_SUPPORTED,
        ERR_MANDATORY_HEADER_MISSING,
        ERR_INVALID_API_CALL_ID,
        ERR_INVALID_CORRELATION_ID,
        ERR_INVALID_TIMESTAMP,
        ERR_INVALID_REDIRECT_TO,
        ERR_INVALID_STATUS,
        ERR_INVALID_DEBUG_FLAG,
        ERR_INVALID_ERROR_DETAILS,
        ERR_INVALID_DEBUG_DETAILS,
        ERR_INVALID_WORKFLOW_ID,
        ERR_SERVICE_UNAVAILABLE,
        ERR_DOMAIN_PROCESSING
    }
}
