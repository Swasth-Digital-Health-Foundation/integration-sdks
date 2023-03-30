package io.hcxprotocol.impl;

import com.fasterxml.jackson.core.JsonProcessingException;
import io.hcxprotocol.dto.ResponseError;
import io.hcxprotocol.exception.ErrorCodes;
import io.hcxprotocol.helper.FhirPayload;
import io.hcxprotocol.helper.ValidateHelper;
import io.hcxprotocol.interfaces.IncomingRequest;
import io.hcxprotocol.jwe.JweRequest;
import io.hcxprotocol.utils.Constants;
import io.hcxprotocol.utils.JSONUtils;
import io.hcxprotocol.utils.Operations;
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
    public boolean process(String jwePayload, Operations operation, String privateKey, Map<String, Object> output) throws Exception {
        Map<String, Object> error = new HashMap<>();
        boolean result = false;
        jwePayload = formatPayload(jwePayload);
        logger.info("Processing incoming request has started :: operation: {}", operation);
        if (!validateRequest(jwePayload, operation, error)) {
            sendResponse(error, output);
        } else if (!decryptPayload(jwePayload, privateKey, output)) {
            sendResponse(output, output);
        } else if (!validatePayload((String) output.get(Constants.FHIR_PAYLOAD), operation, error)) {
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

    private String formatPayload(String payload) throws JsonProcessingException {
        if (payload.contains(Constants.PAYLOAD) || payload.contains(Constants.HCX_API_CALL_ID)){
            return payload;
        } else {
            return JSONUtils.serialize(Collections.singletonMap(Constants.PAYLOAD, payload));
        }
    }
}
