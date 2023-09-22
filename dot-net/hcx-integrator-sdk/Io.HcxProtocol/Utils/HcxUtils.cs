//using CSharp.security.cert;


using Hl7.Fhir.ElementModel.Types;
using Io.HcxProtocol.Dto;
using Io.HcxProtocol.Exceptions;
using Io.HcxProtocol.Helper;
using Io.HcxProtocol.Init;
using Io.HcxProtocol.Key;
using Io.HcxProtocol.Utils;
using Jose;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.Utilities.Zlib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Reflection.PortableExecutable;

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
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
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

        public static string GenerateToken(Io.HcxProtocol.Init.Config config)  //cr 12
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
            HttpResponse response = HttpUtils.Post(config.ProtocolBasePath+config.ParticipantGenerateToken, headers, fields);

            Dictionary<string, Object> respMap = JSONUtils.Deserialize<Dictionary<string, Object>>(response.getBody());
            string token;
          
            if (response.getStatus() == 200)
            {
                token = (string)respMap["access_token"];
            }
            else if (response.getStatus() == 401)
            {
                _logger.Error("Error while generating API access token: Invalid credentials");
                throw new ClientException("Error while generating API access token: Invalid credentials");
            }
            else
            {
                _logger.Error("Error while generating API access token :: status: " + response.Status + " :: message: " + respMap);
                throw new ServerException("Error while generating API access token :: status: " + response.Status + " :: message: " + respMap);
            }
            return token;

        }

        public static Dictionary<string, object> SearchRegistry(string participantCode,string token,string protocolBasePath)
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

        public static bool isValidSignature(string payload, string publicKeyUrl,out Dictionary<string,Object> output)
        {
          //  payload = "eyJhbGciOiJSUzI1NiIsIngtaGN4LW5vdGlmaWNhdGlvbl9oZWFkZXJzIjp7IngtaGN4LWNvcnJlbGF0aW9uX2lkIjoiZWI1YzJjZmItOTViMy00NzNlLTkxODEtZTJiNThjMmMxYzhhIiwic2VuZGVyX2NvZGUiOiJ0ZXN0cHJvdmlkZXIxLmFwb2xsb0Bzd2FzdGgtaGN4LWRldiIsInRpbWVzdGFtcCI6MTY5NDY4OTM0NTU2OSwicmVjaXBpZW50X3R5cGUiOiJwYXJ0aWNpcGFudF9yb2xlIiwicmVjaXBpZW50cyI6WyJwYXlvciJdfX0.eyJ0b3BpY19jb2RlIjoibm90aWYtcGFydGljaXBhbnQtbmV3LXByb3RvY29sLXZlcnNpb24tc3VwcG9ydCIsIm1lc3NhZ2UiOiJ7XCJtZXNzYWdlXCI6IFwidGVzdC1wcm92aWRlciBub3cgc3VwcG9ydHMgdjAuOCBvbiBpdHMgcGxhdGZvcm0uIEFsbCBwYXJ0aWNpcGFudHMgYXJlIHJlcXVlc3RlZCB0byB1cGdyYWRlIHRvIHYwLjggb3IgYWJvdmUgdG8gdHJhbnNhY3Qgd2l0aCB0ZXN0LXByb3ZpZGVyLlwifSJ9.Rbtbq2LueuEB4u53MhC8uCAfXmHvZSBFaRIaNV_PnnRDIjTlF5HneFt28RwyTsADcOXrEQKzU9Fcp-oeh8fUF1LywMWaxbmjfQ_KvlWgjXJEC15MB2PObg7SEO2KL55smM7Up03bBgZinbgYikPigJ0dTyHoOfbRa0oF2DtDz0liKhBujWDCesJCJI8vfcFYaCpbe9WPqBOV00MCpHxy9brCC2S66xvOr02SZA8evP_h8FxylsBZ3yQ3m_vkVNAHYTboiwpTXEx5O7-N0A6kSjzGLFvp9ua6bNjfWC6awexIgBcxA5mve7JlU3VoC5YsS1VA5tsydmGnXBAvlBJ6zg";
            bool isValid = false;

            RSA rsaPublicKey = X509KeyLoader.GetRSAPublicKeyFromPem(publicKeyUrl, PemMode.Url);
           
          var  payloadDictionary = Jose.JWT.Decode<Dictionary<string, object>>(payload, rsaPublicKey);
          var  headersDictionary = Jose.JWT.Headers<Dictionary<string, object>>(payload);
            output = new Dictionary<string, object>();
            if (payloadDictionary.Count > 0)
            {
                isValid = true;
                foreach (var item in payloadDictionary)
                {
                    output.Add(item.Key, item.Value);
                }
            }
            if(headersDictionary.Count>0)
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
 
        public static bool initializeHCXCall(string jwePayload, Operations operation, Dictionary<string, object> response, Init.Config config)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers[Constants.AUTHORIZATION] = "Bearer " + HcxUtils.GenerateToken(config);
            HttpResponse hcxResponse = HttpUtils.Post(config.ProtocolBasePath + operation.getOperation(), headers, jwePayload);
            response[Constants.RESPONSE_OBJ] = JSONUtils.Deserialize<Dictionary<Object,Object>>(hcxResponse.getBody(), typeof(Dictionary<Object,Object>));

            int status = hcxResponse.getStatus();
            bool result = false;
           
            if (status == 202 || status == 200)
            {
                result = true;
            }
            else
            {
            }
            return result;
        }

    }
}


 
