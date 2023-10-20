using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Io.HcxProtocol.Key
{
    /**
     * Library  : Io.Hcx.Protocol
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

        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public static RSA GetRSAPublicKeyFromPem(string pem, PemMode pemDataMode)
        {
            try
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
            catch (AggregateException ex)
            {
                string errMessage = "";
                foreach (var errInner in ex.InnerExceptions)
                {
                    errMessage += errInner.ToString() + "\n";
                }
                _logger.Error("[PublicKey reading error] " + errMessage);
                throw new System.Exception("[PublicKey reading error] " + errMessage);
            }
            catch (System.Exception ex)
            {
                _logger.Error("[PublicKey reading error] " + ex.Message);
                throw new System.Exception("[PublicKey reading error] " + ex.Message.ToString());
            }
        }

        /// <summary>
        ///     This method used to load Private RSA Key from x509 certificate.
        /// </summary>
        /// <param name="pem">It is the source of x509 certificate data.</param>
        /// <param name="pemDataMode">Provide the mode of certificate data as File, Text or Url.</param>
        /// <returns>return Private RSA Key</returns>
        public static RSA GetRSAPrivateKeyFromPem(string pem, PemMode pemDataMode)
        {
            try
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
            catch (AggregateException ex)
            {
                string errMessage = "";
                foreach (var errInner in ex.InnerExceptions)
                {
                    errMessage += errInner.ToString() + "\n";
                }
                _logger.Error("[PrivateKey reading error]" + errMessage);
                throw new System.Exception("[PrivateKey reading error] " + errMessage);
            }
            catch (System.Exception ex)
            {
                _logger.Error("[PrivateKey reading error]" + ex.Message);
                throw new System.Exception("[PrivateKey reading error] " + ex.Message);
            }
        }
    }
}
