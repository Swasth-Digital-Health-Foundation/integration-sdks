# HCX Integrator SDK

For information on how to use the SDK, Please refer the [SDK User Manual](https://github.com/Swasth-Digital-Health-Foundation/hcx-platform/wiki/HCX-Integration-SDK-User-Manual).

# JWE-Helper

JWE-Helper library helps use the JSON Web Encryption - [RFC7516](https://datatracker.ietf.org/doc/html/rfc7516) standard. It has chosen the following algorithms for `alg` and `enc`.
```
alg: RSA_OAEP_256
enc: AES_256_GCM
```

## Interface
All the functions of JWE are defined as part of the JweRequest interface.

### Sender's End
The input parameters are the `protected-headers` and the `payload`. These JSON objects are taken as Java `hMap<String, Object>` objects.

The output format of the `encrypted-object` is a JSON object similar to the Flattened JWE JSON Serialization Syntax defined in [Section 7.2.2](https://datatracker.ietf.org/doc/html/rfc7516#section-7.2.2) of RFC7516. The difference is that the JWE-Helper library does not support `unprotected-header`, `header`, and `aad`. The output `encrypted-object` is also a Java `Map<String, Object>` object.
```
{
    "protected": "<integrity-protected header contents>",
    "encrypted_key": "<encrypted key contents>",
    "iv": "<initialization vector contents>",
    "ciphertext": "<ciphertext contents>",
    "tag": "<authentication tag contents>"
}
```

### Receiver's End
The receiver will get the above-defined `encrypted-object`. It will have to pass the `encrypted-object` as a Java `Map<String, Object>` object to the `JweRequest`. After decrypting, the receiver will get `protected-header` and `payload` as Java `Map<String, Object>` objects.

## Usage
Working sample code for the two methods is present in the [JweRequestTest](./src/test/java/org/swasth/jose/jwe/JweRequestTest.java)

### Encrypt the object on Sender's end
```
JweRequest jweRequest = new JweRequest(headers, payload);
jweRequest.encryptRequest((RSAPublicKey) publicKey);
HashMap<String, Object> encryptedObject = jweRequest.getEncryptedObject();
System.out.println("Encrypted Object: " + encryptedObject.toString());
```

### Decrypting the object on Receiver's end
```
JweRequest jweRequest = new JweRequest(encryptedObject);
jweRequest.decryptRequest((RSAPrivateKey) privateKey);
Map<String, Object> retrievedHeader = jweRequest.getHeaders();
Map<String, Object> retrievedPayload = jweRequest.getPayload();
```

## Supportive Functions
Private Key Loader and Public Key Loader are provided as supportive util functions to load private and public keys from X509 formatted value.

### Private Key Loader
It loads a Private Key from an X509 format and returns an RSAPrivateKey object that can be used to decrypt JweRequest. An example of a private key in X509 format is present in the test resources directory. The util functions are available to load the key from a `File`, `String`, or a `Reader` object.
```
RSAPrivateKey rsaPrivateKey =
        PrivateKeyLoader.loadRSAPrivateKeyFromPem(new File(baseURL + filePathToPrivateKey));
```

### Public Key Loader
It loads a Public Key from a self-signed certificate in X509 format and returns an RSAPublicKey object that can be used to encrypt JweRequest. An example of a self-signed certificate is available in the test resources directory. The util functions are available to load from a `File`, `URL`, or a `Reader` object.
```
File file = new File(baseURL + filePathToSelfSignedCertificate);
FileReader fileReader = new FileReader(file);
RSAPublicKey rsaPublicKey = PublicKeyLoader.loadPublicKeyFromX509Certificate(fileReader);
```