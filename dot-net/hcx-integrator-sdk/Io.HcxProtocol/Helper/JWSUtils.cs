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
      
      


        public static string generate(Dictionary<string, object> headers, Dictionary<string, object> payload, string privateKey)
        {
           
            RSA privateRsaKey = X509KeyLoader.GetRSAPrivateKeyFromPem(privateKey, PemMode.FileText);
            var rsaparameter = privateRsaKey.ExportParameters(true);
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaparameter);

                return Jose.JWT.Encode(payload, rsa, Jose.JwsAlgorithm.RS256,headers);
                
              
            }
        }
     
    }
    public class NotificationHeaders
    {
        public string sender_code { get; set; }
        public List<string> recepients { get; set; }
    }
}
