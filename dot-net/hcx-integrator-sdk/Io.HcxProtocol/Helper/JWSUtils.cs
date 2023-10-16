using Io.HcxProtocol.Key;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Io.HcxProtocol.Helper
{
    public class JWSUtils
    {
        public static string Generate(Dictionary<string, object> headers, Dictionary<string, object> payload, string privateKey)
        {
            RSA privateRsaKey = X509KeyLoader.GetRSAPrivateKeyFromPem(privateKey, PemMode.FileText);
            var rsaparameter = privateRsaKey.ExportParameters(true);
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaparameter);

                return Jose.JWT.Encode(payload, rsa, Jose.JwsAlgorithm.RS256, headers);
            }
        }
    }
}
