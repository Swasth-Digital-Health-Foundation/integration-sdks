using System.Collections.Generic;

namespace Io.HcxProtocol.Dto
{
    /// <summary>
    /// This is to handle jwe payload specific validations.
    /// </summary>
    public class JWERequest : BaseRequest
    {
        public JWERequest(Dictionary<string, object> payload) : base(payload)
        {

        }
    }
}
