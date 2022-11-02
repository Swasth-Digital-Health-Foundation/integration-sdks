package io.hcxprotocol.key;

import org.bouncycastle.util.io.pem.PemReader;

import java.io.*;
import java.security.KeyFactory;
import java.security.NoSuchAlgorithmException;
import java.security.interfaces.RSAPrivateKey;
import java.security.spec.InvalidKeySpecException;
import java.security.spec.PKCS8EncodedKeySpec;

public class PrivateKeyLoader {

    public static RSAPrivateKey loadRSAPrivateKeyFromPem(Reader reader) throws NoSuchAlgorithmException, IOException,
            InvalidKeySpecException {
        System.out.println("File reader" +  reader);
        PemReader pemReader = new PemReader(reader);
        PKCS8EncodedKeySpec privateKeySpec = new PKCS8EncodedKeySpec(pemReader.readPemObject().getContent());
        KeyFactory keyFactory = KeyFactory.getInstance("RSA");
        return (RSAPrivateKey) keyFactory.generatePrivate(privateKeySpec);
    }

    public static RSAPrivateKey loadRSAPrivateKeyFromPem(File file) throws IOException, NoSuchAlgorithmException,
            InvalidKeySpecException {
        System.out.println("File " + file);
        FileReader fileReader = new FileReader(file);
        return loadRSAPrivateKeyFromPem(fileReader);
    }

    public static RSAPrivateKey loadRSAPrivateKeyFromPem(String string) throws IOException, NoSuchAlgorithmException,
            InvalidKeySpecException {
        return loadRSAPrivateKeyFromPem(new StringReader(string));
    }

}
