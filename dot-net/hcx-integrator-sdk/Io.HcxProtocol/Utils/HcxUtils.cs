//using CSharp.security.cert;

using Hl7.Fhir.ElementModel.Types;
using Io.HcxProtocol.Dto;
using Io.HcxProtocol.Exceptions;
using Io.HcxProtocol.Helper;
using Io.HcxProtocol.Init;
using Io.HcxProtocol.Utils;
using NLog;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.Utilities.Zlib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using static Org.BouncyCastle.Math.EC.ECCurve;

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
            else if (!string.IsNullOrEmpty(config.Secrete))
            {
                fields.Add(Constants.SECRET, config.Secrete);
                fields.Add("participant_code", config.ParticipantCode);
            }
            HttpResponse response = HttpUtils.Post(config.ProtocolBasePath+config.ParticipantGenerateToken, headers, fields);

            Dictionary<string, Object> respMap = JSONUtils.Deserialize<Dictionary<string, Object>>(response.getBody());
            string token;
           // token = "eyJhbGciOiJSUzI1NiIsIngtaGN4LW5vdGlmaWNhdGlvbl9oZWFkZXJzIjp7InJlY2lwaWVudF90eXBlIjoicGFydGljaXBhbnRfcm9sZSIsInJlY2lwaWVudHMiOlsicGF5b3IiXSwieC1oY3gtY29ycmVsYXRpb25faWQiOiIxYTVlNGY3MS1iNWRlLTRlNDYtODQ0MC1mNjQ1YWU4NWFiYTEiLCJzZW5kZXJfY29kZSI6InRlc3Rwcm92aWRlcjEuYXBvbGxvQHN3YXN0aC1oY3gtZGV2IiwidGltZXN0YW1wIjoxNjg5MDU5MzEwMjU4fX0.eyJ0b3BpY19jb2RlIjoibm90aWYtcGFydGljaXBhbnQtbmV3LXByb3RvY29sLXZlcnNpb24tc3VwcG9ydCIsIm1lc3NhZ2UiOiJ0ZXN0LXByb3ZpZGVyIG5vdyBzdXBwb3J0cyB2MC44IG9uIGl0cyBwbGF0Zm9ybS4gQWxsIHBhcnRpY2lwYW50cyBhcmUgcmVxdWVzdGVkIHRvIHVwZ3JhZGUgdG8gdjAuOCBvciBhYm92ZSB0byB0cmFuc2FjdCB3aXRoIHRlc3QtcHJvdmlkZXIuIn0.LLqp_pfy2JHekfnr6FrbTWt_oxHh76j1WoJ-g3Uuf599F2mZHUwxAg8mFzAF7LUk7lLgznXdbAU1bkiWzME8CkpkSkqSxzOhbb1XCAy63XbBn9hiHgKjR2hcw3lA2I4Y3fmPPSF6nDEm1_mALiA2AoyzmSttMH9dtXCk-lcXzb5c7BvKss2Gk_42t2DNTNq1HF0wWYWnZfNQdV7-Jcw8jo2bIOVeD8ep774RCp6KLAC_nh68JkMd_kft_clhL8qKwpMfVq-2YRi9Njb4vAOfuMYsTAA8EjL8eJlUxWG7o7JJ1RgGNbpTfg7BzbB7SYI0fcwRjqf9VZnTxfDsTWw1CQ";
            //  token = "eyJhbGciOiJSUzI1NiJ9.eyJzdWIiOiI4NTI3ODUzYy1iNDQyLTQ0ZGItYWVkYS1kYmJkY2Y0NzJkOWIiLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImlzcyI6Imh0dHA6XC9cL2tleWNsb2FrLmtleWNsb2FrLnN2Yy5jbHVzdGVyLmxvY2FsOjgwODBcL2F1dGhcL3JlYWxtc1wvc3dhc3RoLWhjeC1wYXJ0aWNpcGFudHMiLCJ0eXAiOiJCZWFyZXIiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJ0ZXN0cHJvdmlkZXIxQGFwb2xsby5jb20iLCJhdWQiOiJhY2NvdW50IiwiYWNyIjoiMSIsInJlYWxtX2FjY2VzcyI6eyJyb2xlcyI6WyJwcm92aWRlciIsImRlZmF1bHQtcm9sZXMtbmRlYXIiXX0sImF6cCI6InJlZ2lzdHJ5LWZyb250ZW5kIiwicGFydGljaXBhbnRfY29kZSI6InRlc3Rwcm92aWRlcjEuYXBvbGxvQHN3YXN0aC1oY3gtZGV2Iiwic2NvcGUiOiJwcm9maWxlIGVtYWlsIiwiZXhwIjoxNjk1MzAxNDQ0LCJzZXNzaW9uX3N0YXRlIjoiZTI1YjE4MjktMmJmYi00Y2JlLWEzODktYzNiZTQ4Yjk1NzdhIiwiaWF0IjoxNjkzNTczNDQ0LCJqdGkiOiJhMmY0NDAyYS03YzVhLTRlNzgtOTM2NC02ZGRhZDhiZWRkNzEiLCJlbnRpdHkiOlsiT3JnYW5pc2F0aW9uIl0sImVtYWlsIjoidGVzdHByb3ZpZGVyMUBhcG9sbG8uY29tIn0.Su4qQQtciMud91DXv4fNTAS6Ma6twJpy8jJf5edNDx_ACQowQxb228rYLsGcGLdB-c8xob_n9xIbj009lcYP4TZL8o0q7fPOLWPjTRScpRe7BesGPMsbRlM0LdtqEGSkHEI_2znKPQTb220EDLUGWbqCwwwlLartL2ENstYm-xKES45RQ5m8ajBor5KkFxTHMb2MdKUmfS_yrkRhP2I5bwe8s1C2-DsJtAKY_EY5K0l2k4vbTGQ0O-BFWpB9jSZLyuLLcAO-tRPwA_2lFQBovf_5PKp_Yu_Jw_triTAwiuS_dldq9AesbGV3xntzg4XWhB0QfCvloeu5AV-99nsEow";
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

        //public static bool isValidSignature(string payload, string publicKeyUrl)
        //{
        //    // string certificate = IOUtils.tostring(new Uri(publicKeyUrl), StandardCharsets.UTF_8.tostring());
        //    string certificate = new Uri(publicKeyUrl).ToString();
        //    CertificateFactory cf = CertificateFactory.getInstance("X.509");
        //    Stream stream = new System.IO.MemoryStream(Encoding.ASCII.GetBytes(certificate)); //StandardCharsets.UTF_8
        //    Certificate cert = cf.generateCertificate(stream);
        //    PublicKey publicKey = cert.getPublicKey();
        //    string[] parts = payload.Split("\\.");
        //    string data = parts[0] + "." + parts[1];
        //    Signature sig = Signature.getInstance("SHA256withRSA");
        //    sig.initVerify(publicKey);
        //    sig.update(data.getBytes());
        //    byte[] decodedSignature = Base64.getUrlDecoder().decode(parts[2]);
        //    return sig.verify(decodedSignature);
        //}

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
               // logger.info("Processing outgoing request has completed ::  response: {}", response[Constants.RESPONSE_OBJ]);
            }
            else
            {
                //logger.error("Error while processing the outgoing request :: status: {} :: response: {}", status, response[Constants.RESPONSE_OBJ]);
            }
            return result;
        }

    }
}


 
