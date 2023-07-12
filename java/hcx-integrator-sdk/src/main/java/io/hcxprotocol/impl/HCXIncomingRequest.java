package io.hcxprotocol.impl;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.typesafe.config.Config;
import io.hcxprotocol.dto.NotificationRequest;
import io.hcxprotocol.dto.ResponseError;
import io.hcxprotocol.exception.ClientException;
import io.hcxprotocol.helper.FhirPayload;
import io.hcxprotocol.helper.ValidateHelper;
import io.hcxprotocol.interfaces.IncomingRequest;
import io.hcxprotocol.jwe.JweRequest;
import io.hcxprotocol.utils.Constants;
import io.hcxprotocol.utils.JSONUtils;
import io.hcxprotocol.utils.Operations;
import io.hcxprotocol.utils.Utils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;



/**
 * The <b>HCX Incoming Request</b> class provide the methods to help in processing the JWE Payload and extract FHIR Object.
 *
 * <ul>
 *     <li><b>process</b> is the main method to use by the integrator(s) to validate the JWE Payload and fetch FHIR Object.
 *      <ul>
 *         <li>This method takes the JWE Payload and Operation as input parameters to validate and extract the respective FHIR Object.</li>
 *     </ul>
 *     <li>
 *         <b>validateRequest, validatePayload, decryptPayload, sendResponse</b> methods are used by <b>process</b> method to compose functionality of validating JWE Payload and extracting FHIR Object.
 *         These methods are available for the integrator(s) to use them based on different scenario(s) or use cases.
 *     </li>
 * </ul>
 */
public class HCXIncomingRequest extends FhirPayload implements IncomingRequest {

    private static final Logger logger = LoggerFactory.getLogger(HCXIncomingRequest.class);

    @Override
    public boolean process(String jwePayload, Operations operation, Map<String, Object> output, Config config) throws Exception {
        Map<String, Object> error = new HashMap<>();
        boolean result = false;
        jwePayload = getPayload(jwePayload);
        logger.info("Processing incoming request has started :: operation: {}", operation);
        if (!validateRequest(jwePayload, operation, error)) {
            sendResponse(error, output);
        } else if (!decryptPayload(jwePayload, config.getString(Constants.ENCRYPTION_PRIVATE_KEY), output)) {
            sendResponse(output, output);
        } else if (!validatePayload((String) output.get(Constants.FHIR_PAYLOAD), operation, error, config)) {
            sendResponse(error, output);
        } else {
            if (sendResponse(error, output)) result = true;
        }
        return result;
    }

    @Override
    public boolean validateRequest(String jwePayload, Operations operation, Map<String, Object> error) {
        return ValidateHelper.getInstance().validateRequest(jwePayload, operation, error);
    }

    @Override
    public boolean decryptPayload(String jwePayload, String privateKey, Map<String, Object> output) throws Exception {
        try {
            JweRequest jweRequest = new JweRequest(JSONUtils.deserialize(jwePayload, Map.class));
            jweRequest.decryptRequest(privateKey);
            output.put(Constants.HEADERS, jweRequest.getHeaders());
            output.put(Constants.FHIR_PAYLOAD, JSONUtils.serialize(jweRequest.getPayload()));
            logger.info("Request is decrypted successfully");
            return true;
        } catch (Exception e) {
            logger.error("Error while decrypting the payload: {}", e.getMessage());
            e.printStackTrace();
            throw new Exception("Error while decrypting the payload: " + e.getMessage());
        }
    }

    @Override
    public boolean validatePayload(String fhirPayload, Operations operation, Map<String,Object> error, Config config) {
        if (config.getBoolean(Constants.FHIR_VALIDATION_ENABLED))
            return validateFHIR(fhirPayload, operation, error, config);
        else return true;
    }

    @Override
    public boolean sendResponse(Map<String,Object> error, Map<String,Object> output) throws JsonProcessingException {
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
            responseObj.put(Constants.ERROR, JSONUtils.serialize(new ResponseError(code, message, "")));
        }
        output.put(Constants.RESPONSE_OBJ, responseObj);
        return result;
    }

    private String getPayload(String payload) throws JsonProcessingException {
        if (payload.contains(Constants.PAYLOAD) || payload.contains(Constants.HCX_API_CALL_ID)){
            return payload;
        } else {
            return JSONUtils.serialize(Collections.singletonMap(Constants.PAYLOAD, payload));
        }
    }

    @Override
    public Map<String, Object> receiveNotification(String jwsPayload, Map<String, Object> output, Config config) throws Exception {
        Map<String,Object> payload = JSONUtils.deserialize(getPayload(jwsPayload),Map.class);
        NotificationRequest notificationRequest = new NotificationRequest((String) payload.get(Constants.PAYLOAD));
        if (notificationRequest.getJwsPayload().isEmpty()) {
            throw new ClientException("JWS Token cannot be empty");
        }
        String authToken = Utils.generateToken(config.getString(Constants.USERNAME), config.getString(Constants.PASSWORD), config.getString(Constants.AUTH_BASE_PATH));
        String publicKeyUrl = (String) Utils.searchRegistry(notificationRequest.getSenderCode(), authToken, config.getString(Constants.PROTOCOL_BASE_PATH)).get(Constants.ENCRYPTION_CERT);
        boolean isSignatureValid = Utils.isValidSignature((String) payload.get(Constants.PAYLOAD), publicKeyUrl);
        output.put(Constants.HEADERS, notificationRequest.getHeaders());
        output.put(Constants.PAYLOAD, notificationRequest.getPayload());
        output.put(Constants.IS_SIGNATURE_VALID, isSignatureValid);
        return output;
    }
}
