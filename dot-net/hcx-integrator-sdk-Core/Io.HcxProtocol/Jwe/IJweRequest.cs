using System.Collections.Generic;
using System.Security.Cryptography;

namespace Io.HcxProtocol.Jwe
{
    /**
     * Library  : Io.Hcx.Protocol.Core
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    ///     The <b>Jwe Request</b> Interface provide the methods to help to Encrypt & Decrypt the payload with RSA Key and return Encrypted payload / Decrypted FHIR object & Headers.
    /// </summary>

    interface IJweRequest
    {
        /// <summary>
        ///     It returns the encrypted payload Object.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetEncryptedObject();

        /// <summary>
        ///     It returns the HCX Protocol headers.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetHeaders();

        /// <summary>
        ///     It returns the Json FHIR object.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetPayload();

        /// <summary>
        ///     Encrypt the FHIR object and its Headers with Public RSA key and prepare the JWE Payload.
        /// </summary>
        /// <param name="rsaPublicKey"></param>
        void EncryptRequest(RSA rsaPublicKey);

        /// <summary>
        ///     Decrypt the JWE Payload with Private RSA key and extract the FHIR Object and its Headers.
        /// </summary>
        /// <param name="rsaPrivateKey">Private RSA Key</param>
        void DecryptRequest(RSA rsaPrivateKey);
    }
}
