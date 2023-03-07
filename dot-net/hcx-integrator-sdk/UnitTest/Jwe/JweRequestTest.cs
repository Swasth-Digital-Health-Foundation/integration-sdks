using Io.HcxProtocol.Jwe;
using Io.HcxProtocol.Key;
using Io.HcxProtocol.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace UnitTest.Jwe
{
    [TestClass]
    public class JweRequestTest
    {
        // Note : Copy Pem Certificate Key files on given location or change path.
        string fileNamePublicKey = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "X509FilesSampleFiles", "x509-self-signed-certificate.pem");
        string fileNamePrivateKey = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "X509FilesSampleFiles", "x509-private-key.pem");
        //string urlPublicKey = "https://raw.githubusercontent.com/Swasth-Digital-Health-Foundation/jwe-helper/main/src/test/resources/x509-self-signed-certificate.pem";

        [TestMethod]
        public void EncryptRequest()
        {
            RSA publicRsaKey = X509KeyLoader.GetRSAPublicKeyFromPem(fileNamePublicKey, PemMode.FilePath);

            Dictionary<string, object> headers = new Dictionary<string, object>
            {
                { "HeaderKey1", "Value1" },
                { "HeaderKey2", "Value2" },
                { "HeaderKey3", "Value3" },
                { "HeaderKey4", "Value4" }
            };

            Dictionary<string, object> payload = new Dictionary<string, object>
            {
                { "PayloadKey1", "Value1" },
                { "PayloadKey2", "Value2" },
                { "PayloadKey3", "Value3" },
                { "PayloadKey4", "Value4" }
            };

            JweRequest jweEnc = new JweRequest(headers, payload);
            jweEnc.EncryptRequest(publicRsaKey);
            Dictionary<string, object> encryptedObject = jweEnc.GetEncryptedObject();
            Assert.IsNotNull(encryptedObject, "EncryptRequest Pass");
        }

        [TestMethod]
        public void DecryptRequest()
        {
            RSA privateRsaKey = X509KeyLoader.GetRSAPrivateKeyFromPem(fileNamePrivateKey, PemMode.FilePath);

            string encryptedString = "eyJhbGciOiJSU0EtT0FFUC0yNTYiLCJlbmMiOiJBMjU2R0NNIiwiSGVhZGVyMSI6IlZhbHVlMSIsIkhlYWRlcjIiOiJWYWx1ZTIiLCJIZWFkZXIzIjoiVmFsdWUzIiwiSGVhZGVyNCI6IlZhbHVlNCJ9.aDDPuq-48u4QGtY4jQvEzhRoJ3tvTrB-03ToPl6LZanGiRRuZ3gaHY4H9UFP5WqmStg_05q3inYTAHR56-jAS8Rn5tgH6kPtskwRRLud1tJOHrKcv_7safsnfCTCADy53nTPCqBEDEYH8WuMZFKaCE66KZOyFNFhH3wNCgJz55ZOeQtOdFU1I851NzaNIIdI45p6guk-hBQYMsBKNan8fw7GehJNoprpeTHEwVEuYXdodke5eevy7nt21VvZqbTwDcQKcLU-Ezi_OXoLQXMML-WlTAAWVzak0KBSH6cGSArtcRFm9f64iBbztZX_aNrkyiWOvhLxRwyR3cCTex-QPQ.c6V9oFQ7f4ZNBbHe.fnA1NqkVuJK4gGrA1paA2thhgIVgTHAaVrbPVbDhsKzwnQCmR1HpRDuACwrbBTY95TWePqfdLtfQ7d80_ncWQNQ.6oJ3EegALmMilrodUH1L6w";
            Dictionary<string, object> encryptedObject = new Dictionary<string, object>();
            encryptedObject.Add(Constants.PAYLOAD, encryptedString);
            JweRequest jweDec = new JweRequest(encryptedObject);
            jweDec.DecryptRequest(privateRsaKey);
            Dictionary<string, object> headers = jweDec.GetHeaders();
            Dictionary<string, object> payload = jweDec.GetPayload();

            Assert.IsNotNull(headers, "DecryptRequest Pass");
            Assert.IsNotNull(payload, "DecryptRequest Pass");
        }

    }
}
