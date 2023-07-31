import jose from 'node-jose';
import axios from 'axios';

export class JWEHelper {

    static async encrypt({ cert, headers, payload, format = 'compact', contentAlg = "A256GCM", alg = "RSA-OAEP-256" }) {
        let keyData = cert;
        let key = await jose.JWK.asKey(keyData, "pem"); // Updated this line
        const buffer = Buffer.from(JSON.stringify(payload));
        const fields = { alg, ...headers };
        const encrypted = await jose.JWE.createEncrypt({ format, contentAlg, fields }, key).update(buffer).final(); // Updated this line
        return encrypted;
    }

    static async decrypt({ cert, payload }) {
        var keyData;
        if (cert.startsWith("-----BEGIN PRIVATE KEY-----") && cert.endsWith("-----END PRIVATE KEY-----")) {
            keyData = cert
        }
        else {
            keyData = await (await axios.get(cert)).data; // Corrected this line
        }
        if (!(keyData && payload)) throw new Error('Invalid Input');
        let keystore = JWK.createKeyStore();
        await keystore.add(await JWK.asKey(keyData, "pem"));
        
        // Assuming you have the correct method to decrypt the payload
        let decrypted = await someMethodToDecrypt(payload, keystore); // Replace this line with the correct method
        const payloadString = decrypted.payload.toString();
        const payloadMap = JSON.parse(payloadString);
        decrypted.payload = payloadMap;
        const { key, plaintext, protected: _, ...rest } = decrypted;
        return rest;
    }
}
