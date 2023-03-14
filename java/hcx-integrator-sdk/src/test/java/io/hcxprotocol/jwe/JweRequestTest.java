package io.hcxprotocol.jwe;

import com.nimbusds.jose.JOSEException;
import org.bouncycastle.util.io.pem.PemObject;
import org.bouncycastle.util.io.pem.PemReader;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.TestInstance;

import java.io.*;
import java.security.KeyFactory;
import java.security.NoSuchAlgorithmException;
import java.security.cert.CertificateException;
import java.security.cert.CertificateFactory;
import java.security.cert.X509Certificate;
import java.security.interfaces.RSAPrivateKey;
import java.security.interfaces.RSAPublicKey;
import java.security.spec.InvalidKeySpecException;
import java.security.spec.PKCS8EncodedKeySpec;
import java.text.ParseException;
import java.util.HashMap;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.assertFalse;


@TestInstance(TestInstance.Lifecycle.PER_CLASS)
class JweRequestTest {

    RSAPublicKey rsaPublicKey;
    RSAPrivateKey rsaPrivateKey;
    Map<String, Object> headers, payload;
    Map<String, String> encryptedObject;

    private String filePathToSelfSignedCertificate = "x509-self-signed-certificate.pem";
    private String filePathToPrivateKey = "x509-private-key.pem";

    private String baseURL;

    @BeforeAll
    public void init() throws NoSuchAlgorithmException, IOException, InvalidKeySpecException, CertificateException {
        ClassLoader classLoader = this.getClass().getClassLoader();
        baseURL = classLoader.getResource("").getFile();

        initKeys();
        initContent();
    }

    private File getFile(String fileName) {
        return new File(baseURL + File.separator + fileName);
    }

    private void initKeys() throws NoSuchAlgorithmException, IOException, InvalidKeySpecException, CertificateException {
        FileReader fileReader = new FileReader(getFile(filePathToPrivateKey));
        PemReader pemReader = new PemReader(fileReader);
        PemObject pemObject = pemReader.readPemObject();
        PKCS8EncodedKeySpec privateKeySpec = new PKCS8EncodedKeySpec(pemObject.getContent());
        KeyFactory factory = KeyFactory.getInstance("RSA");
        rsaPrivateKey = (RSAPrivateKey) factory.generatePrivate(privateKeySpec);
        fileReader = new FileReader(getFile(filePathToSelfSignedCertificate));
        pemReader = new PemReader(fileReader);
        pemObject = pemReader.readPemObject();
        CertificateFactory certificateFactory = CertificateFactory.getInstance("X.509");
        X509Certificate x509Certificate = (X509Certificate) certificateFactory.generateCertificate(new ByteArrayInputStream(pemObject.getContent()));
        rsaPublicKey = (RSAPublicKey) x509Certificate.getPublicKey();
    }

    private void initContent() {
        headers = new HashMap<>();
        headers.put("headerKey", "headerValue");
        payload = new HashMap<>();
        payload.put("io/hcxprotocol/key", "value");
    }

    @Test
     void testEncrypt() throws JOSEException {
        JweRequest jweRequest = new JweRequest(headers, payload);
        jweRequest.encryptRequest(rsaPublicKey);
        encryptedObject = jweRequest.getEncryptedObject();
        assertFalse(encryptedObject.isEmpty());
    }

    @Test
    public void testDecrypt() throws ParseException, JOSEException, IOException, NoSuchAlgorithmException, InvalidKeySpecException {
        JweRequest jweRequest = new JweRequest(encryptedObject);
        jweRequest.decryptRequest("-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCG+XLPYiCxrZq71IX+w7uoDGxGI7qy7XaDbL3BJE33ju7rjdrP7wsAOWRvM8BIyWuRZZhl9xG+u7l/7OsZAzGoqI7p+32x+r9IJVzboLDajk6tp/NPg1csc7f2M5Bu6rkLEvrKLz3dgy3Q928rMsD3rSmzBLelfKTo+aDXvCOiw1dMWsZZdkEpCTJxH39Nb2K4S59kO/R2GtSU/QMLq65m34XcMZpDtatA1u1S8JdZNNeMCO+NuFKBzIfvXUCQ8jkf7h612+UP1AYhoyCMFpzUZ9b7liQF9TYpX1Myr/tT75WKuRlkFlcALUrtVskL8KA0w6sA0nX5fORVsuVehVeDAgMBAAECggEAX1n1y5/M7PhxqWO3zYTFGzC7hMlU6XZsFOhLHRjio5KsImgyPlbm9J+W3iA3JLR2c17MTKxAMvg3UbIzW5YwDLAXViC+aW90like8mEQzzVdS7ysXG2ytcqCGUHQNStI0hP0a8T39XbodQl31ZKjU9VW8grRGe12Kse+4ukcW6yRVES+CkyO5BQB+vs3voZavodRGsk/YSt00PtIrFPJgkDuyzzcybKJD9zeJk5W3OGVK1z0on+NXKekRti5FBx/uEkT3+knkz7ZlTDNcyexyeiv7zSL/L6tcszV0Fe0g9vJktqnenEyh4BgbqABPzQR++DaCgW5zsFiQuD0hMadoQKBgQC+rekgpBHsPnbjQ2Ptog9cFzGY6LRGXxVcY7hKBtAZOKAKus5RmMi7Uv7aYJgtX2jt6QJMuE90JLEgdO2vxYG5V7H6Tx+HqH7ftCGZq70A9jFBaba04QAp0r4TnD6v/LM+PGVT8FKtggp+o7gZqXYlSVFm6YzI37G08w43t2j2aQKBgQC1Nluxop8w6pmHxabaFXYomNckziBNMML5GjXW6b0xrzlnZo0p0lTuDtUy2xjaRWRYxb/1lu//LIrWqSGtzu+1mdmV2RbOd26PArKw0pYpXhKFu/W7r6n64/iCisoMJGWSRJVK9X3D4AjPaWOtE+jUTBLOk0lqPJP8K6yiCA6ZCwKBgDLtgDaXm7HdfSN1/Fqbzj5qc3TDsmKZQrtKZw5eg3Y5CYXUHwbsJ7DgmfD5m6uCsCPa+CJFl/MNWcGxeUpZFizKn16bg3BYMIrPMao5lGGNX9p4wbPN5J1HDD1wnc2jULxupSGmLm7pLKRmVeWEvWl4C6XQ+ykrlesef82hzwcBAoGBAKGY3v4y4jlSDCXaqadzWhJr8ffdZUrQwB46NGb5vADxnIRMHHh+G8TLL26RmcET/p93gW518oGg7BLvcpw3nOZaU4HgvQjT0qDvrAApW0V6oZPnAQUlarTU1Uk8kV9wma9tP6E/+K5TPCgSeJPg3FFtoZvcFq0JZoKLRACepL3vAoGAMAUHmNHvDI+v0eyQjQxlmeAscuW0KVAQQR3OdwEwTwdFhp9Il7/mslN1DLBddhj6WtVKLXu85RIGY8I2NhMXLFMgl+q+mvKMFmcTLSJb5bJHyMz/foenGA/3Yl50h9dJRFItApGuEJo/30cG+VmYo2rjtEifktX4mDfbgLsNwsI=\n-----END PRIVATE KEY-----");
        Map<String, Object> getHeaders = jweRequest.getHeaders();
        assertFalse(getHeaders.isEmpty());
    }

}
