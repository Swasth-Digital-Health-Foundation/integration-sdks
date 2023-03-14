package io.hcxprotocol.jwe;

import com.nimbusds.jose.JOSEException;

import java.io.IOException;
import java.security.NoSuchAlgorithmException;
import java.security.interfaces.RSAPrivateKey;
import java.security.interfaces.RSAPublicKey;
import java.security.spec.InvalidKeySpecException;
import java.text.ParseException;
import java.util.Map;

public interface JweRequestInterface {

    public Map<String, String> getEncryptedObject();

    public Map<String, Object> getHeaders();
    public Map<String, Object> getPayload();

    public void encryptRequest(RSAPublicKey rsaPublicKey) throws JOSEException;
    public void decryptRequest(String PrivateKey) throws ParseException, IOException, NoSuchAlgorithmException, InvalidKeySpecException, JOSEException;

}
