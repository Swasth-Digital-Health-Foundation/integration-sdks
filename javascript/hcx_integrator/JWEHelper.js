import jose from 'node-jose'
const { JWK, JWE, parse } = jose;
import axios from 'axios';


export class JWEHelper {

    static async encrypt({ cert, headers, payload, format = 'compact', contentAlg = "A256GCM", alg = "RSA-OAEP-256" }) {
        var keyData;
        if (cert.startsWith("-----BEGIN CERTIFICATE-----")) {
            keyData = cert;
        }
        else {
            keyData = await (await axios.default.get(cert)).data
        }
        if (!(keyData && headers && payload)) throw new Error('Invalid Input');
        let key = await JWK.asKey(keyData, "pem");
        const buffer = Buffer.from(JSON.stringify(payload));
        const fields = { alg, ...headers };
        const encrypted = await JWE.createEncrypt({ format, contentAlg, fields }, key).update(buffer).final();
        return encrypted;
    }

    static async decrypt({ cert, payload }) {
        var keyData;
        if (cert.startsWith("-----BEGIN PRIVATE KEY-----")) {
            keyData = cert
        }
        else {
            keyData = await (await axios.default.get(cert)).data
        }
        if (!(keyData && payload)) throw new Error('Invalid Input');
        let keystore = JWK.createKeyStore();
        await keystore.add(await JWK.asKey(keyData, "pem"));
        let parsedPayload = parse.compact(payload);
        let decrypted = await parsedPayload.perform(keystore);
        const payloadString = decrypted.payload.toString();
        const payloadMap = JSON.parse(payloadString);
        decrypted.payload = payloadMap;
        const { key, plaintext, protected: _, ...rest } = decrypted;
        return rest;
    }
}
