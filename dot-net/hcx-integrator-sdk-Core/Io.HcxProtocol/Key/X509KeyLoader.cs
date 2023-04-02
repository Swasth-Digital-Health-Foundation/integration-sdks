using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Io.HcxProtocol.Key
{
    /**
     * Library  : Io.Hcx.Protocol.Core
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    ///     X509KeyLoader class used to load RSA key from Public / Private x509 certificate.
    /// </summary>

    public class X509KeyLoader
    {
        /// <summary>
        ///     This method used to load Public RSA Key from x509 certificate.
        /// </summary>
        /// <param name="pem">It is the source of x509 certificate data.</param>
        /// <param name="pemDataMode">Provide the mode of certificate data as File, Text or Url.</param>
        /// <returns>return Public RSA Key</returns>
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

        /// <summary>
        ///     This method used to load Private RSA Key from x509 certificate.
        /// </summary>
        /// <param name="pem">It is the source of x509 certificate data.</param>
        /// <param name="pemDataMode">Provide the mode of certificate data as File, Text or Url.</param>
        /// <returns>return Private RSA Key</returns>
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
