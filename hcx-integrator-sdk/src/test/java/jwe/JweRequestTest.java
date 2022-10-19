package jwe;

import com.nimbusds.jose.JOSEException;
import io.hcxprotocol.jwe.JweRequest;
import org.bouncycastle.util.io.pem.PemObject;
import org.bouncycastle.util.io.pem.PemReader;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.TestInstance;

import java.io.ByteArrayInputStream;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
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
        payload.put("key", "value");
    }

    @Test
    public void testEncrypt() throws JOSEException, ParseException {
        System.out.println("Provided Header: " + headers);
        System.out.println("Provided Payload: " + payload);

        JweRequest jweRequest = new JweRequest(headers, payload);
        jweRequest.encryptRequest(rsaPublicKey);
        encryptedObject = jweRequest.getEncryptedObject();
        System.out.println("Encrypted Object: " + encryptedObject.toString());
    }

    @Test
    public void testDecrypt() throws ParseException, JOSEException {
        JweRequest jweRequest = new JweRequest(encryptedObject);
        jweRequest.decryptRequest(rsaPrivateKey);
        Map<String, Object> retrievedHeader = jweRequest.getHeaders();
        Map<String, Object> retrievedPayload = jweRequest.getPayload();

        System.out.println("Retrieved Header: " + retrievedHeader);
        System.out.println("Retrieved Payload:  " + retrievedPayload);
    }

}
