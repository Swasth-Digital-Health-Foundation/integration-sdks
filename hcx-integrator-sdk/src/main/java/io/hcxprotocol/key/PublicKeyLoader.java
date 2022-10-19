package io.hcxprotocol.key;

import org.bouncycastle.util.io.pem.PemObject;
import org.bouncycastle.util.io.pem.PemReader;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.Reader;
import java.net.URL;
import java.security.cert.CertificateException;
import java.security.cert.CertificateFactory;
import java.security.cert.X509Certificate;
import java.security.interfaces.RSAPublicKey;

public class PublicKeyLoader {

    public static RSAPublicKey loadPublicKeyFromX509Certificate(Reader reader) throws IOException,
            CertificateException {
        PemReader pemReader = new PemReader(reader);
        PemObject pemObject = pemReader.readPemObject();
        CertificateFactory certificateFactory = CertificateFactory.getInstance("X.509");
        X509Certificate x509Certificate = (X509Certificate) certificateFactory.generateCertificate(new ByteArrayInputStream(pemObject.getContent()));
        return (RSAPublicKey) x509Certificate.getPublicKey();
    }

    public static RSAPublicKey loadPublicKeyFromX509Certificate(URL url) throws IOException, CertificateException {
        Reader reader = new InputStreamReader(url.openStream());
        return loadPublicKeyFromX509Certificate(reader);
    }

}
