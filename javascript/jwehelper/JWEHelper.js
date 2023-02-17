const { JWK, JWE, parse } = require('node-jose');
const axios = require('axios');


class JWEHelper {

    static async encrypt({ cert, headers, payload, format = 'compact', contentAlg = "A256GCM", alg = "RSA-OAEP-256" }) {
        var keyData;
        if (cert.startsWith("-----BEGIN CERTIFICATE-----") && cert.endsWith("-----END CERTIFICATE-----")) {
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
        if (cert.startsWith("-----BEGIN PRIVATE KEY-----") && cert.endsWith("-----END PRIVATE KEY-----")) {
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
        return decrypted;
    }
}

module.exports = { JWEHelper }