using Io.HcxProtocol.Utils;
using Jose;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Io.HcxProtocol.Jwe
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    ///     The <b>Jwe Request</b> class provide the methods to help to Encrypt & Decrypt the payload with RSA Key and return Encrypted payload / Decrypted FHIR object & Headers..
    /// </summary>

    public class JweRequest: IJweRequest
    {
        public static JweAlgorithm KEY_MANAGEMENT_ALGORITHM = JweAlgorithm.RSA_OAEP_256;
        public static JweEncryption CONTENT_ENCRYPTION_ALGORITHM = JweEncryption.A256GCM;

        private Dictionary<string, object> headers;
        private Dictionary<string, object> payload;
        private Dictionary<string, object> encryptedObject;

        public JweRequest(Dictionary<string, object> headers, Dictionary<string, object> payload)
        {
            this.headers = headers;
            this.payload = payload;
        }

        public JweRequest(Dictionary<string, object> encryptedObject)
        {
            this.encryptedObject = encryptedObject;
        }

        public Dictionary<string, object> GetEncryptedObject()
        {
            return encryptedObject;
        }

        public Dictionary<string, object> GetHeaders()
        {
            return headers;
        }

        public Dictionary<string, object> GetPayload()
        {
            return payload;
        }

        public void EncryptRequest(RSA rsaPublicKey)
        {
            string payloadString = JSONUtils.Serialize(payload);
            string tokenString = Jose.JWT.Encode(payloadString, rsaPublicKey, KEY_MANAGEMENT_ALGORITHM, CONTENT_ENCRYPTION_ALGORITHM, extraHeaders: headers);
            encryptedObject = new Dictionary<string, object>() { { Constants.PAYLOAD, tokenString } };
        }

        public void DecryptRequest(RSA rsaPrivateKey)
        {
            string tokenString = encryptedObject[Constants.PAYLOAD].ToString();
            payload = Jose.JWT.Decode<Dictionary<string, object>>(tokenString, rsaPrivateKey);
            headers = Jose.JWT.Headers<Dictionary<string, object>>(tokenString);

            //Remove default headers
            headers.Remove("alg");
            headers.Remove("enc");
        }
    }
}
