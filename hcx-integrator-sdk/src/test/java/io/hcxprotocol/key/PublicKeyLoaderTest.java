package io.hcxprotocol.key;

import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.TestInstance;

import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.net.URL;
import java.security.cert.CertificateException;
import java.security.interfaces.RSAPublicKey;

import static org.junit.jupiter.api.Assertions.*;


@TestInstance(TestInstance.Lifecycle.PER_CLASS)
class PublicKeyLoaderTest {

    private final String filePathToSelfSignedCertificate = "x509-self-signed-certificate.pem";
    private String baseURL;

    @BeforeAll
    public void init() {
        ClassLoader classLoader = this.getClass().getClassLoader();
        baseURL = classLoader.getResource("").getFile();
    }

    @Test
    void loadPublicKeyFromX509Certificate() throws IOException, CertificateException {
        File file = new File(baseURL + filePathToSelfSignedCertificate);
        FileReader fileReader = new FileReader(file);
        RSAPublicKey rsaPublicKey = PublicKeyLoader.loadPublicKeyFromX509Certificate(fileReader);
        assertNotNull(rsaPublicKey);
    }
    @Test
    void testLoadPublicKeyFromX509CertificateUrl() throws IOException, CertificateException {
        URL url = new URL("https://raw.githubusercontent.com/rushang7-eGov/jwe-helper/master/src/test/resources/x509-self-signed-certificate.pem");
        RSAPublicKey rsaPublicKey = PublicKeyLoader.loadPublicKeyFromX509Certificate(url);
        assertNotNull(rsaPublicKey);
    }

}
