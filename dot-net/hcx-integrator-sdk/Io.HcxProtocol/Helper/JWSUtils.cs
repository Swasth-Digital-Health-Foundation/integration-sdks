using Jose;
using System;
using System.Collections.Generic;
using System.Text;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using System.IO;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Tls;
using System.Buffers.Text;

using Microsoft.CodeAnalysis.Operations;
using Hl7.Fhir.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Net.Sockets;
using JWT.Algorithms;
using JWT.Builder;
using JwtHeader = System.IdentityModel.Tokens.Jwt.JwtHeader;
using Org.BouncyCastle.Ocsp;
using System.Linq;
using JWT.Serializers;
using JWT;
using System.Security.Cryptography.X509Certificates;
using static Jose.Jwk;
using Org.BouncyCastle.Utilities;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Io.HcxProtocol.Key;

namespace Io.HcxProtocol.Helper
{
    public class JWSUtils
    {
      
        public static string generate1(Dictionary<string, object> headers, Dictionary<string, object> payload, string privateKey)
        {
            StringBuilder s = new StringBuilder(privateKey);
            s.Replace("-----", "");
            privateKey = s.ToString();



            // byte[] privateKeyDecoded = Convert.FromBase64String(privateKey);
            // Pkcs8EncryptedPrivateKeyInfo spec = new Pkcs8EncryptedPrivateKeyInfo(privateKeyDecoded);
            //var rsapkey= PrivateKeyFactory.CreateKey(spec.GetEncryptedData());

            var Claim = new[]
            {
                  new  System.Security.Claims.Claim("topic_code",payload["topic_code"].ToString()),


            };
            var RSAPrivateKey = new  SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
            var Credentials = new SigningCredentials(RSAPrivateKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.RsaSha256);

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken("xyz", "xyz", Claim, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), signingCredentials: Credentials);
            //var finaltoken = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
              return string.Empty;

           

          


           // IJwtAlgorithm algorithm = new RS256Algorithm(,);
            //JwtBuilder jwt = new JwtBuilder();
            //foreach(var item in headers)
            //{
            //    jwt.AddHeader(item.Key, item.Value);
            //}
            //foreach(var item in payload)
            //{
            //    jwt.AddClaim(item.Key, item.Value);
            //}
            
            //jwt.WithAlgorithm()
         
            //return jwt.
         
        }


        public static string generate2(Dictionary<string, object> headers, Dictionary<string, object> payload, string privateKey)
        {
            //StringBuilder str = new StringBuilder(privateKey);
            //str.Replace("-----BEGIN PRIVATE KEY-----\n", "");
            //str.Replace("\n-----END PRIVATE KEY-----", "");
            //privateKey = str.ToString();
            RSA privateRsaKey = X509KeyLoader.GetRSAPrivateKeyFromPem(privateKey, PemMode.FileText);
            var rsaparameter = privateRsaKey.ExportParameters(true);
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaparameter);

                return Jose.JWT.Encode(payload, rsa, Jose.JwsAlgorithm.RS256,headers);
            }
        }


        public static  void ReadFromFile(string privatekey)
        {

            var bytesToDecrypt = Convert.FromBase64String(privatekey); // string to decrypt, base64 encoded

            AsymmetricCipherKeyPair keyPair;

            using (var reader = File.OpenText(@"c:\privatekey.pem"))
                
                // file containing RSA PKCS1 private key
                keyPair = (AsymmetricCipherKeyPair)new PemReader(reader).ReadObject();

            var decryptEngine = new Pkcs1Encoding(new RsaEngine());
            decryptEngine.Init(false, keyPair.Private);

            var decrypted = Encoding.UTF8.GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));
        }


        //public static string CreateToken(Dictionary<string,object> payload, string privateRsaKey)
        //{

        //    // byte[] bytes = System.Text.Encoding.Unicode.GetBytes(privateRsaKey);

           

            
          
        //}



        





        //public void generate3(Dictionary<string, object> headers, Dictionary<string, object> payload, string privateKey)
        //{

        //    // reading the content of a private key PEM file, PKCS8 encoded 


        //    // keeping only the payload of the key 

        //    byte[] privateKeyRaw = Convert.FromBase64String(privateKey);

        //    // creating the RSA key 
        //    RSACryptoServiceProvider provider = new RSACryptoServiceProvider();



        //   provider.ImportPkcs8PrivateKey(new ReadOnlySpan<byte>(privateKeyRaw), out _);
        //    RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(provider);

        //    // Generating the token 
        //    var now = DateTime.UtcNow;

        //    var Claim = new[]
        //        {

        //          new  System.Security.Claims.Claim("topic_code",payload["topic_code"].ToString())
        //         //new System.Security.Claims.Claim("x-hcx-notification_headers",headers["x-hcx-notification_headers"].ToString()),
        //         // new System.Security.Claims.Claim("alg",headers["alg"].ToString())

        //    };

        //    var handler = new JwtSecurityTokenHandler();

        //    var token = new JwtSecurityToken
        //    (
        //        "YOUR_CLIENTID",
        //        "https://AAAS_PLATFORM/idp/YOUR_TENANT/authn/token",
        //        Claim,
        //        now.AddMilliseconds(-30),
        //        now.AddMinutes(60),
        //        new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256)
        //    );

        //    // handler.WriteToken(token) returns the token ready to send to AaaS !
        //    Console.WriteLine(handler.WriteToken(token));



        //}


    }
    public class NotificationHeaders
    {
        public string sender_code { get; set; }
        public List<string> recepients { get; set; }
    }
}
