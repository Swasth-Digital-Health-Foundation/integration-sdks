using Io.HcxProtocol.Dto;
using Io.HcxProtocol.Exceptions;
using Io.HcxProtocol.Init;
using Io.HcxProtocol.Key;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

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
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        // TODO: In the initial version we are not handling the token caching, it will be handled in the next version
        public static string GenerateToken(string username, string password, string authBasePath)
        {
            var headers = new Dictionary<string, string> { { "Content-Type", "application/x-www-form-urlencoded" } };
            var fields = new Dictionary<string, string>
            {
                {"client_id", "registry-frontend" },
                {"username", username},
                {"password", password},
                {"grant_type", "password"}
            };

            HttpResponse response = HttpUtils.Post(authBasePath, headers, fields);
            var responseBody = JSONUtils.Deserialize<Dictionary<string, string>>(response.Body);

            if (response.Status == 200)
            {
                return responseBody["access_token"];
            }
            else if (response.Status == 401)
            {
                throw new System.Exception("Error while generating API access token: Invalid credentials");
            }
            else
            {
                throw new System.Exception("Error while generating API access token :: status: " + response.Status + " :: message: " + response.Body);
            }
        }

        public static string GenerateToken(Config config)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/x-www-form-urlencoded");
            Dictionary<string, string> fields = new Dictionary<string, string>();
            fields.Add(Constants.USERNAME, config.UserName);
            if (!string.IsNullOrEmpty(config.Password))
            {
                fields.Add(Constants.PASSWORD, config.Password);
            }
            else if (!string.IsNullOrEmpty(config.Secret))
            {
                fields.Add(Constants.SECRET, config.Secret);
                fields.Add("participant_code", config.ParticipantCode);
            }
            HttpResponse response = HttpUtils.Post(config.ProtocolBasePath + config.ParticipantGenerateToken, headers, fields);

            Dictionary<string, object> resp = JSONUtils.Deserialize<Dictionary<string, object>>(response.Body);
            string token;

            if (response.Status == 200)
            {
                token = (string)resp["access_token"];
            }
            else if (response.Status == 401)
            {
                _logger.Error("Error while generating API access token: Invalid credentials");
                throw new ClientException("Error while generating API access token: Invalid credentials");
            }
            else
            {
                _logger.Error("Error while generating API access token :: status: " + response.Status + " :: message: " + resp);
                throw new ClientException("Error while generating API access token :: status: " + response.Status + " :: message: " + resp);
            }
            return token;
        }

        public static Dictionary<string, object> SearchRegistry(string participantCode, string token, string protocolBasePath)
        {
            var filter = "{\"filters\":{\"participant_code\":{\"eq\":\"" + participantCode + "\"}}}";
            var headers = new Dictionary<string, string> { { Constants.AUTHORIZATION, "Bearer " + token } };
            HttpResponse response = HttpUtils.Post(protocolBasePath + "/participant/search", headers, filter);
            Dictionary<string, object> respObj;
            IEnumerable<Dictionary<string, object>> details;

            if (response.Status == 200)
            {
                respObj = JSONUtils.Deserialize<Dictionary<string, object>>(response.Body);
                details = JSONUtils.Deserialize<IEnumerable<Dictionary<string, object>>>(respObj[Constants.PARTICIPANTS].ToString());
            }
            else
            {
                throw new System.Exception("Error while fetching the participant details from the registry :: status: " + response.Status + " :: " + response.Body);
            }
            return details.Any() ? details.FirstOrDefault() : new Dictionary<string, object>();
        }

        public static bool IsValidSignature(string payload, string publicKeyUrl, out Dictionary<string, object> output)
        {
            bool isValid = false;
            RSA rsaPublicKey = X509KeyLoader.GetRSAPublicKeyFromPem(publicKeyUrl, PemMode.Url);

            var payloadDictionary = Jose.JWT.Decode<Dictionary<string, object>>(payload, rsaPublicKey);
            var headersDictionary = Jose.JWT.Headers<Dictionary<string, object>>(payload);
            output = new Dictionary<string, object>();

            if (payloadDictionary.Count > 0)
            {
                isValid = true;
                foreach (var item in payloadDictionary)
                {
                    output.Add(item.Key, item.Value);
                }
            }
            if (headersDictionary.Count > 0)
            {
                foreach (var item in headersDictionary)
                {
                    output.Add(item.Key, item.Value);
                }
                headersDictionary.Remove("alg");
                headersDictionary.Remove("enc");
            }

            return isValid;
        }

        public static bool InitializeHCXCall(string jwePayload, Operations operation, Dictionary<string, object> response, Config config)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers[Constants.AUTHORIZATION] = "Bearer " + HcxUtils.GenerateToken(config);
            HttpResponse hcxResponse = HttpUtils.Post(config.ProtocolBasePath + operation.getOperation(), headers, jwePayload);
            response[Constants.RESPONSE_OBJ] = JSONUtils.Deserialize<Dictionary<object, object>>(hcxResponse.Body);

            int status = hcxResponse.Status;
            bool result = false;

            if (status == 202 || status == 200)
            {
                result = true;
                // logger.info("Processing outgoing request has completed ::  response: {}", response[Constants.RESPONSE_OBJ]);
            }
            else
            {
                // logger.error("Error while processing the outgoing request :: status: {} :: response: {}", status, response[Constants.RESPONSE_OBJ]);
            }
            return result;
        }

    }
}
