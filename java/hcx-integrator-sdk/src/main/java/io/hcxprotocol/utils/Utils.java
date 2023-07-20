package io.hcxprotocol.utils;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.typesafe.config.Config;
import io.hcxprotocol.dto.HttpResponse;
import io.hcxprotocol.exception.ClientException;
import io.hcxprotocol.exception.ServerException;
import lombok.experimental.UtilityClass;
import org.apache.commons.io.IOUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.ByteArrayInputStream;
import java.io.InputStream;
import java.net.URL;
import java.nio.charset.StandardCharsets;
import java.security.PublicKey;
import java.security.Signature;
import java.security.cert.Certificate;
import java.security.cert.CertificateFactory;
import java.util.Base64;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import static io.hcxprotocol.utils.Constants.PARTICIPANT_GENERATE_TOKEN;
/**
 * The common utils functionality used in HCX Integrator SDK.
 * <ol>
 *     <li>Generation of authentication token using HCX Gateway and Participant System Credentials.</li>
 *     <li>HCX Gateway Participant Registry Search.</li>
 * </ol>
 */

@UtilityClass
public class Utils {

    private static final Logger logger = LoggerFactory.getLogger(Utils.class);

    // TODO: In the initial version we are not handling the token caching, it will be handled in the next version
    public static String generateToken(String username, String password, String protocolBasePath) throws JsonProcessingException, ClientException, ServerException {
        Map<String,String> headers = new HashMap<>();
        headers.put("content-type", "application/x-www-form-urlencoded");
        Map<String,Object> fields = new HashMap<>();
        fields.put("username", username);
        fields.put("password", password);
        HttpResponse response = HttpUtils.post(protocolBasePath + PARTICIPANT_GENERATE_TOKEN, headers, fields);
        Map<String,Object> respMap = JSONUtils.deserialize(response.getBody(), Map.class);
        String token;
        if (response.getStatus() == 200) {
            token = (String) respMap.get("access_token");
        } else if (response.getStatus() == 401) {
            logger.error("Error while generating API access token: Invalid credentials");
            throw new ClientException("Error while generating API access token: Invalid credentials");
        } else {
            logger.error("Error while generating API access token :: status: " + response.status + " :: message: " + respMap);
            throw new ServerException("Error while generating API access token :: status: " + response.status + " :: message: " + respMap);
        }
        return token;
    }

    public static Map<String,Object> searchRegistry(String participantCode, String token, String protocolBasePath) throws Exception {
        String filter = "{\"filters\":{\"participant_code\":{\"eq\":\"" + participantCode + "\"}}}";
        Map<String,String> headers = new HashMap<>();
        headers.put(Constants.AUTHORIZATION, "Bearer " + token);
        HttpResponse response = HttpUtils.post(protocolBasePath + "/participant/search", headers, filter);
        Map<String,Object> respMap = JSONUtils.deserialize(response.getBody(), Map.class);
        List<Map<String,Object>> details;
        if (response.getStatus() == 200) {
            details = (List<Map<String, Object>>) respMap.get(Constants.PARTICIPANTS);
        } else {
            String errMsg;
            if(respMap.get("error") instanceof String) {
                errMsg = respMap.get("error").toString();
            } else {
                errMsg = ((Map<String,Object>) respMap.getOrDefault("error",  new HashMap<>())).getOrDefault("message", respMap).toString();
            }
            logger.error("Error while fetching the participant details from the registry :: status: " + response.getStatus() + " :: message: " + errMsg);
            throw new ServerException("Error while fetching the participant details from the registry :: status: " + response.getStatus() + " :: message: " + errMsg);
        }
        return !details.isEmpty() ? details.get(0) : new HashMap<>();
    }
    public static boolean isValidSignature(String payload, String publicKeyUrl) throws Exception {
        String certificate = IOUtils.toString(new URL(publicKeyUrl), StandardCharsets.UTF_8.toString());
        CertificateFactory cf = CertificateFactory.getInstance("X.509");
        InputStream stream = new ByteArrayInputStream(certificate.getBytes()); //StandardCharsets.UTF_8
        Certificate cert = cf.generateCertificate(stream);
        PublicKey publicKey = cert.getPublicKey();
        String[] parts = payload.split("\\.");
        String data = parts[0] + "." + parts[1];
        Signature sig = Signature.getInstance("SHA256withRSA");
        sig.initVerify(publicKey);
        sig.update(data.getBytes());
        byte[] decodedSignature = Base64.getUrlDecoder().decode(parts[2]);
        return sig.verify(decodedSignature);
    }

    public static boolean initializeHCXCall(String jwePayload, Operations operation, Map<String, Object> response, Config config) throws ServerException, ClientException, JsonProcessingException {
        Map<String,String> headers = new HashMap<>();
        headers.put(Constants.AUTHORIZATION, "Bearer " + Utils.generateToken(config.getString(Constants.USERNAME), config.getString(Constants.PASSWORD), config.getString(Constants.PROTOCOL_BASE_PATH)));
        HttpResponse hcxResponse = HttpUtils.post(config.getString(Constants.PROTOCOL_BASE_PATH) + operation.getOperation(), headers, jwePayload);
        response.put(Constants.RESPONSE_OBJ, JSONUtils.deserialize(hcxResponse.getBody(), Map.class));
        int status = hcxResponse.getStatus();
        boolean result = false;
        if(status == 202 || status == 200){
            result = true;
            logger.info("Processing outgoing request has completed ::  response: {}", response.get(Constants.RESPONSE_OBJ));
        } else {
            logger.error("Error while processing the outgoing request :: status: {} :: response: {}", status, response.get(Constants.RESPONSE_OBJ));
        }
        return result;
    }
}
