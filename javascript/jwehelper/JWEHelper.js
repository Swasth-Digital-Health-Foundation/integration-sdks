    const { JWK, JWE, parse } = require('node-jose');

    class JWEHelper {

        static async encrypt({ cert, headers, payload, format = 'compact', contentAlg = "A256GCM", alg = "RSA-OAEP-256" }) {
            if (!(cert && headers && payload)) throw new Error('Invalid Input');
            let key = await JWK.asKey(cert, "pem");
            const buffer = Buffer.from(JSON.stringify(payload));
            const fields = { alg, ...headers };
            const encrypted = await JWE.createEncrypt({ format, contentAlg, fields }, key).update(buffer).final();
            return encrypted;
        }

        static async decrypt({ cert, payload }) {
            if (!(cert && payload)) throw new Error('Invalid Input');
            let keystore = JWK.createKeyStore();
            await keystore.add(await JWK.asKey(cert, "pem"));
            let parsedPayload = parse.compact(payload);
            let decrypted = await parsedPayload.perform(keystore);
            return decrypted;
        }
    }

    module.exports = { JWEHelper }