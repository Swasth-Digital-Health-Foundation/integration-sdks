import jose from 'node-jose';
import axios from 'axios';

export class JWEHelper {
    
    static async encrypt({ cert, headers, payload, format = 'compact', contentAlg = "A256GCM", alg = "RSA-OAEP-256" }) {
        var keyData;
        console.log(cert)
        if (cert.startsWith("-----BEGIN CERTIFICATE-----") ) {
            keyData = cert;
          } else { 
            console.log(cert.startsWith("-----BEGIN CERTIFICATE-----"));
            console.log(cert.endsWith("-----END CERTIFICATE-----"))
            console.log(cert.length);
            console.log("else me hu")
            keyData = await (await axios.get(cert)).data;
        }
        keyData = cert;
        let key = await jose.JWK.asKey(keyData, "pem");
        const buffer = Buffer.from(JSON.stringify(payload));
        const fields = { alg, ...headers };
        const encrypted = await jose.JWE.createEncrypt({ format, contentAlg, fields }, key).update(buffer).final();
        return encrypted;
    }

    static async decrypt({ cert, payload }) {
        var keyData;
        if (cert.startsWith("-----BEGIN PRIVATE KEY-----") && cert.endsWith("-----END PRIVATE KEY-----")) {
            keyData = cert
        }
        else {
            keyData = await (await axios.get(cert)).data;
        }
        if (!(keyData && payload)) throw new Error('Invalid Input');
        let keystore = JWK.createKeyStore();
        await keystore.add(await JWK.asKey(keyData, "pem"));
        
        let decrypted = await someMethodToDecrypt(payload, keystore);
        const payloadString = decrypted.payload.toString();
        const payloadMap = JSON.parse(payloadString);
        decrypted.payload = payloadMap;
        const { key, plaintext, protected: _, ...rest } = decrypted;
        return rest;
    }
}
