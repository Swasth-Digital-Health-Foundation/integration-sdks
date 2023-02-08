using System.Collections.Generic;
using System.Security.Cryptography;

namespace Io.HcxProtocol.Jwe
{
    interface IJweRequest
    {
        Dictionary<string, object> GetEncryptedObject();

        Dictionary<string, object> GetHeaders();

        Dictionary<string, object> GetPayload();

        void EncryptRequest(RSA rsaPublicKey);

        void DecryptRequest(RSA rsaPrivateKey);
    }
}
