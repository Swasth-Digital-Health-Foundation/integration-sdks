using Io.HcxProtocol.Exceptions;
using Io.HcxProtocol.Utils;
using System;
using System.Collections.Generic;

namespace Io.HcxProtocol.Dto
{
    /// <summary>
    /// This is to handle json payload specific validations for error and redirect scenarios.
    /// </summary>
    public class JSONRequest : BaseRequest
    {
        public JSONRequest(Dictionary<string, object> payload) : base(payload)
        {
        }

        public bool ValidateRedirect(Dictionary<string, object> error)
        {
            if (ValidateCondition(string.IsNullOrEmpty(GetRedirectTo()), error, ErrorCodes.ERR_INVALID_REDIRECT_TO.ToString(), string.Format(ResponseMessage.INVALID_REDIRECT_ERR_MSG, Constants.REDIRECT_TO)))
                return true;
            if (ValidateCondition(GetHcxSenderCode().Equals(GetRedirectTo(), StringComparison.OrdinalIgnoreCase), error, ErrorCodes.ERR_INVALID_REDIRECT_TO.ToString(), ResponseMessage.INVALID_REDIRECT_SELF_ERR_MSG))
                return true;
            return false;
        }
    }
}
