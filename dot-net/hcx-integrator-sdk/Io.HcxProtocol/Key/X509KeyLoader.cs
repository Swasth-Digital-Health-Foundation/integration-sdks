using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Io.HcxProtocol.Key
{
    public enum PemMode
    {
        FilePath,
        FileText,
        Url
    }

    public class X509KeyLoader
    {
        public static RSA GetRSAPublicKeyFromPem(string pem, PemMode pemDataMode)
        {
            string keyFileAllText = null;
            X509Certificate2 cert = null;

            if (pemDataMode.Equals(PemMode.FilePath))
            {
                cert = new X509Certificate2(pem);
            }
            else if (pemDataMode.Equals(PemMode.FileText))
            {
                keyFileAllText = pem;
            }
            else if (pemDataMode.Equals(PemMode.Url))
            {
                keyFileAllText = new HttpClient().GetStringAsync(pem).Result;
            }

            if (!string.IsNullOrEmpty(keyFileAllText))
            {
                cert = new X509Certificate2(Encoding.ASCII.GetBytes(keyFileAllText));
            }

            return cert.GetRSAPublicKey();
        }

        public static RSA GetRSAPrivateKeyFromPem(string pem, PemMode pemDataMode)
        {
            string keyFileAllText = "";

            if (pemDataMode.Equals(PemMode.FilePath))
            {
                keyFileAllText = File.ReadAllText(pem);
            }
            else if (pemDataMode.Equals(PemMode.FileText))
            {
                keyFileAllText = pem;
            }
            else if (pemDataMode.Equals(PemMode.Url))
            {
                keyFileAllText = new HttpClient().GetStringAsync(pem).Result;
            }

            using (TextReader privateKeyTextReader = new StringReader(keyFileAllText))
            {
                RsaPrivateCrtKeyParameters readKeyPair = (RsaPrivateCrtKeyParameters)new PemReader(privateKeyTextReader).ReadObject();

                return DotNetUtilities.ToRSA(readKeyPair);
            }
        }
    }
}
