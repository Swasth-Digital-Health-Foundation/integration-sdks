package key;

import io.hcxprotocol.key.PrivateKeyLoader;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.TestInstance;

import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.security.NoSuchAlgorithmException;
import java.security.interfaces.RSAPrivateKey;
import java.security.spec.InvalidKeySpecException;

@TestInstance(TestInstance.Lifecycle.PER_CLASS)
class PrivateKeyLoaderTest {

    private String filePathToPrivateKey = "x509-private-key.pem";
    private String baseURL;

    @BeforeAll
    public void init() {
        ClassLoader classLoader = this.getClass().getClassLoader();
        baseURL = classLoader.getResource("").getFile();
    }

    @Test
    void loadRSAPrivateKeyFromPem() throws IOException, NoSuchAlgorithmException, InvalidKeySpecException {
        File file = new File(baseURL + filePathToPrivateKey);
        FileReader fileReader = new FileReader(file);
        RSAPrivateKey rsaPrivateKey = PrivateKeyLoader.loadRSAPrivateKeyFromPem(fileReader);
        System.out.println("RSA Private Key loaded from a Reader Object: ");
        System.out.println(rsaPrivateKey);
    }

    @Test
    void loadRSAPrivateKeyFromPemFile() throws IOException, NoSuchAlgorithmException, InvalidKeySpecException {
        RSAPrivateKey rsaPrivateKey =
                PrivateKeyLoader.loadRSAPrivateKeyFromPem(new File(baseURL + filePathToPrivateKey));
        System.out.println("RSA Private Key loaded from a File Object: ");
        System.out.println(rsaPrivateKey);
    }

}
