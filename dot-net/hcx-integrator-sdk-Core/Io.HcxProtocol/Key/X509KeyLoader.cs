using System;
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

            // Remove the header and footer lines from the PEM string
            string privateKeyText = keyFileAllText.Replace("-----BEGIN PRIVATE KEY-----", "")
                                                .Replace("-----END PRIVATE KEY-----", "")
                                                .Replace("\n", "");
            // Convert the text to a byte array
            byte[] privateKeyBytes = Convert.FromBase64String(privateKeyText);

            // Create an RSA instance from the byte array
            RSA privateRsaKey = RSA.Create();
            privateRsaKey.ImportPkcs8PrivateKey(privateKeyBytes, out _);   // ImportPkcs8PrivateKey() is not awailable in .netstandard 2.0
            return privateRsaKey;
        }
    }
}
