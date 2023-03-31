using System.Collections.Generic;

namespace Io.HcxProtocol.Dto
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

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
