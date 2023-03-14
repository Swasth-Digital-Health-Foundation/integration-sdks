package io.hcxprotocol.impl;

import com.typesafe.config.Config;
import com.typesafe.config.ConfigFactory;
import io.hcxprotocol.dto.HttpResponse;
import io.hcxprotocol.dto.ResponseError;
import io.hcxprotocol.exception.ErrorCodes;
import io.hcxprotocol.helper.FhirPayload;
import io.hcxprotocol.helper.ValidateHelper;
import io.hcxprotocol.interfaces.IHCXIntegrator;
import io.hcxprotocol.jwe.JweRequest;
import io.hcxprotocol.key.PublicKeyLoader;
import io.hcxprotocol.utils.*;
import org.apache.commons.io.IOUtils;
import org.apache.commons.lang3.StringUtils;
import org.bouncycastle.util.io.pem.PemObject;
import org.bouncycastle.util.io.pem.PemReader;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.*;
import java.net.URL;
import java.nio.charset.StandardCharsets;
import java.security.KeyFactory;
import java.security.NoSuchAlgorithmException;
import java.security.interfaces.RSAPrivateKey;
import java.security.interfaces.RSAPublicKey;
import java.security.spec.InvalidKeySpecException;
import java.security.spec.PKCS8EncodedKeySpec;
import java.text.SimpleDateFormat;
import java.util.*;

/**
 * The methods and variables to access the configuration. Enumeration of error codes and operations.
 */
public class HCXIntegrator extends FhirPayload implements IHCXIntegrator {

    private static final Logger logger = LoggerFactory.getLogger(HCXIntegrator.class);
    static  HCXIntegrator hcxIntegrator = null;

    private static Config config = null;

    private static Map<String,Object> contextMap = new HashMap<>();

    private HCXIntegrator() {
    }

    /**
     * This method is to get HCX Integrator instance by passing the configuration as Map.
     *
     * @param configMap A Map that contains configuration variables and its values..
     */
    public static HCXIntegrator getInstance(Map<String,Object> configMap) throws Exception {
        config = ConfigFactory.parseMap(configMap);
        validateConfig();
        contextMap.put(getInstance().getParticipantCode(), config);
        return getInstance();
    }

    /**
     * This method is to get HCX Integrator instance of an initialized participant.
     *
     * @param participantCode The participant code to fetch the context.
     */
    public static HCXIntegrator getInstance(String participantCode) throws Exception {
        if(!contextMap.containsKey(participantCode)) {
            throw new Exception("Context is not initialized for this participant: " + participantCode);
        } else {
            config = (Config) contextMap.get(participantCode);
            return getInstance();
        }
    }

    public static HCXIntegrator getInstance() throws Exception {
        if (hcxIntegrator == null)
            hcxIntegrator = new HCXIntegrator();
        return hcxIntegrator;
    }

    private static void validateConfig() throws Exception {
        if(config == null)
            throw new Exception("Please initialize the configuration variables, in order to initialize the SDK");
        List<String> props = Arrays.asList("protocolBasePath", "participantCode", "authBasePath", "username", "password", "encryptionPrivateKey", "igUrl");
        for(String prop: props){
            if(!config.hasPathOrNull(prop) || StringUtils.isEmpty(config.getString(prop)))
                throw new Exception(prop + " is missing or has empty value, please add to the configuration.");
        }
    }

    public String getHCXProtocolBasePath() {
        return config.getString("protocolBasePath");
    }

    public String getParticipantCode() {
        return config.getString("participantCode");
    }

    public String getAuthBasePath() {
        return config.getString("authBasePath");
    }

    public String getUsername() {
        return config.getString("username");
    }

    public String getPassword() {
        return config.getString("password");
    }

    public String getPrivateKey() {
        return config.getString("encryptionPrivateKey");
    }

    public String getIGUrl() {
        return config.getString("igUrl");
    }

    public Map<String,Object> getContextMap() {
        return contextMap;
    }

    @Override
    public boolean incomingReqProcess(String jwePayload, Operations operation, Map<String, Object> output) {
        Map<String, Object> error = new HashMap<>();
        boolean result = false;
        logger.info("Processing incoming request has started :: operation: {}", operation);
        if (!ValidateHelper.getInstance().validateRequest(jwePayload, operation, error)) {
            sendResponse(error, output);
        } else if (!decryptPayload(jwePayload, output)) {
            sendResponse(output, output);
        } else if (!validatePayload((String) output.get(Constants.FHIR_PAYLOAD), operation, error)) {
            sendResponse(error, output);
        } else {
            if (sendResponse(error, output)) result = true;
        }
        return result;
    }

    @Override
    public boolean decryptPayload(String jwePayload, Map<String, Object> output) {
        try {
            JweRequest jweRequest = new JweRequest(JSONUtils.deserialize(jwePayload, Map.class));
            jweRequest.decryptRequest(hcxIntegrator.getPrivateKey());
            output.put(Constants.HEADERS, jweRequest.getHeaders());
            output.put(Constants.FHIR_PAYLOAD, JSONUtils.serialize(jweRequest.getPayload()));
            logger.info("Request is decrypted successfully");
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            output.put(ErrorCodes.ERR_INVALID_ENCRYPTION.toString(), e.getMessage());
            return false;
        }
    }

    @Override
    public boolean sendResponse(Map<String,Object> error, Map<String,Object> output) {
        Map<String, Object> responseObj = new HashMap<>();
        responseObj.put(Constants.TIMESTAMP, System.currentTimeMillis());
        boolean result = false;
        if (error.isEmpty()) {
            Map<String, Object> headers = (Map<String, Object>) output.get(Constants.HEADERS);
            responseObj.put(Constants.API_CALL_ID, headers.get(Constants.HCX_API_CALL_ID));
            responseObj.put(Constants.CORRELATION_ID, headers.get(Constants.HCX_CORRELATION_ID));
            logger.info("Processing incoming request has completed :: response: {}", responseObj);
            result = true;
        } else {
            logger.error("Error while processing the request: {}", error);
            // Fetching only the first error and constructing the error object
            String code = (String) error.keySet().toArray()[0];
            String message =  error.get(code).toString();
            responseObj.put(Constants.ERROR, new ResponseError(code, message, "").toString());
        }
        output.put(Constants.RESPONSE_OBJ, responseObj);
        return result;
    }

    @Override
    public boolean outgoingReqProcess(String fhirPayload, Operations operation, String recipientCode, Map<String,Object> output){
        return process(fhirPayload, operation, recipientCode, "", "", output);
    }

    @Override
    public boolean outgoingReqProcess(String fhirPayload, Operations operation, String actionJwe, String onActionStatus, Map<String,Object> output){
        return process(fhirPayload, operation, "", actionJwe, onActionStatus, output);
    }

    private boolean process(String fhirPayload, Operations operation, String recipientCode, String actionJwe, String onActionStatus, Map<String,Object> output){
        boolean result = false;
        try {
            Map<String, Object> error = new HashMap<>();
            Map<String, Object> headers = new HashMap<>();
            Map<String, Object> response = new HashMap<>();
            logger.info("Processing outgoing request has started :: operation: {}", operation);
            if (!validatePayload(fhirPayload, operation, error)) {
                output.putAll(error);
            } else if (!createHeader(recipientCode, actionJwe, onActionStatus, headers, error)) {
                output.putAll(error);
            } else if (!encryptPayload(headers, fhirPayload, output)) {
                output.putAll(error);
            } else {
                result = initializeHCXCall(JSONUtils.serialize(output), operation, response);
                output.putAll(response);
            }
            if(output.containsKey(Constants.ERROR) || output.containsKey(ErrorCodes.ERR_DOMAIN_PROCESSING.toString()))
                logger.error("Error while processing the outgoing request: {}", output);
            return result;
        } catch (Exception ex) {
            // TODO: Exception is handled as domain processing error, we will be enhancing in next version.
            ex.printStackTrace();
            output.put(ErrorCodes.ERR_DOMAIN_PROCESSING.toString(), ex.getMessage());
            logger.error("Error while processing the outgoing request: {}", ex.getMessage());
            return result;
        }
    }

    @Override
    public boolean createHeader(String recipientCode, String actionJwe, String onActionStatus, Map<String, Object> headers, Map<String, Object> error) {
        try {
            headers.put(Constants.ALG, Constants.A256GCM);
            headers.put(Constants.ENC, Constants.RSA_OAEP);
            headers.put(Constants.HCX_API_CALL_ID, UUID.randomUUID().toString());
            headers.put(Constants.HCX_TIMESTAMP,  new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ssZ").format(new Date()));
            if (!StringUtils.isEmpty(recipientCode)) {
                headers.put(Constants.HCX_SENDER_CODE, hcxIntegrator.getParticipantCode());
                headers.put(Constants.HCX_RECIPIENT_CODE, recipientCode);
                headers.put(Constants.HCX_CORRELATION_ID, UUID.randomUUID().toString());
            } else {
                Map<String,Object> actionHeaders = JSONUtils.decodeBase64String(actionJwe.split("\\.")[0], Map.class);
                headers.put(Constants.HCX_SENDER_CODE,  actionHeaders.get(Constants.HCX_RECIPIENT_CODE));
                headers.put(Constants.HCX_RECIPIENT_CODE, actionHeaders.get(Constants.HCX_SENDER_CODE));
                headers.put(Constants.HCX_CORRELATION_ID, actionHeaders.get(Constants.HCX_CORRELATION_ID));
                headers.put(Constants.STATUS, onActionStatus);
                if(headers.containsKey(Constants.WORKFLOW_ID))
                    headers.put(Constants.WORKFLOW_ID, actionHeaders.get(Constants.WORKFLOW_ID));
            }
            logger.info("Request headers are created: " + headers);
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            error.put(Constants.ERROR, "Error while creating headers: " + e.getMessage());
            return false;
        }
    }

    @Override
    public boolean encryptPayload(Map<String,Object> headers, String fhirPayload, Map<String,Object> output) {
        try {
            String publicKeyUrl = (String) Utils.searchRegistry(headers.get(Constants.HCX_RECIPIENT_CODE)).get(Constants.ENCRYPTION_CERT);
            String certificate = IOUtils.toString(new URL(publicKeyUrl), StandardCharsets.UTF_8.toString());
            InputStream stream = new ByteArrayInputStream(certificate.getBytes());
            Reader fileReader = new InputStreamReader(stream);
            RSAPublicKey rsaPublicKey = PublicKeyLoader.loadPublicKeyFromX509Certificate(fileReader);
            JweRequest jweRequest = new JweRequest(headers, JSONUtils.deserialize(fhirPayload, Map.class));
            jweRequest.encryptRequest(rsaPublicKey);
            output.putAll(jweRequest.getEncryptedObject());
            logger.info("Payload is encrypted successfully");
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            output.put(Constants.ERROR, e.getMessage());
            return false;
        }
    }

    // we are handling the Exception in processFunction method
    @Override
    public boolean initializeHCXCall(String jwePayload, Operations operation, Map<String,Object> response) throws Exception {
        Map<String,String> headers = new HashMap<>();
        headers.put(Constants.AUTHORIZATION, "Bearer " + Utils.generateToken());
        HttpResponse hcxResponse = HttpUtils.post(hcxIntegrator.getHCXProtocolBasePath() + operation.getOperation(), headers, jwePayload);
        response.put(Constants.RESPONSE_OBJ, JSONUtils.deserialize(hcxResponse.getBody(), Map.class));
        int status = hcxResponse.getStatus();
        boolean result = false;
        if(status == 202){
            result = true;
            logger.info("Processing outgoing request has completed ::  response: {}", response.get(Constants.RESPONSE_OBJ));
        } else {
            logger.error("Error while processing the outgoing request :: status: {} :: response: {}", status, response.get(Constants.RESPONSE_OBJ));
        }
        return result;
    }

}
