using Io.HcxProtocol.Dto;
using Io.HcxProtocol.Init;
using System.Collections.Generic;
using System.Linq;

namespace Io.HcxProtocol.Utils
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// The common utils functionality used in HCX Integrator SDK.
    /// </summary>
    /// <remarks>
    ///     <list type="bullet">
    ///     <item>Generation of authentication token using HCX Gateway and Participant System Credentials.</item>
    ///     <item>HCX Gateway Participant Registry Search.</item>
    ///     </list>
    /// </remarks>
    public class HcxUtils
    {
        // TODO: In the initial version we are not handling the token caching, it will be handled in the next version
        public static string GenerateToken()
        {
            var headers = new Dictionary<string, string> { { "content-type", "application/x-www-form-urlencoded" } };
            var fields = new Dictionary<string, string>
            {
                {"client_id", "registry-frontend" },
                {"username", HCXIntegrator.config.UserName},
                {"password", HCXIntegrator.config.Password},
                {"grant_type", "password"}
            };
            HttpResponse response = HttpUtils.Post(HCXIntegrator.config.AuthBasePath, headers, fields);
            var responseBody = JSONUtils.Deserialize<Dictionary<string, string>>(response.Body);
            return responseBody["access_token"];
        }

        public static Dictionary<string, object> SearchRegistry(object participantCode)
        {
            var filter = "{\"filters\":{\"participant_code\":{\"eq\":\"" + participantCode + "\"}}}";
            var headers = new Dictionary<string, string> { { Constants.AUTHORIZATION, "Bearer " + GenerateToken() } };
            HttpResponse response = HttpUtils.Post(HCXIntegrator.config.ProtocolBasePath + "/participant/search", headers, filter);
            Dictionary<string, object> respObj;
            IEnumerable<Dictionary<string, object>> details;
            if (response.Status == 200)
            {
                respObj = JSONUtils.Deserialize<Dictionary<string, object>>(response.Body);
                details = JSONUtils.Deserialize<IEnumerable<Dictionary<string, object>>>(respObj[Constants.PARTICIPANTS].ToString());
            }
            else
            {
                throw new System.Exception("Error in fetching the participant details" + response.Status);
            }
            return details.Any() ? details.FirstOrDefault() : new Dictionary<string, object>();
        }
    }
}
