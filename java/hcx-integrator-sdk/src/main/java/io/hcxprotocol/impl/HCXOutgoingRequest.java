package io.hcxprotocol.impl;

import com.typesafe.config.Config;
import io.hcxprotocol.dto.NotificationRequest;
import io.hcxprotocol.exception.ErrorCodes;
import io.hcxprotocol.helper.FhirPayload;
import io.hcxprotocol.interfaces.OutgoingRequest;
import io.hcxprotocol.jwe.JweRequest;
import io.hcxprotocol.key.PublicKeyLoader;
import io.hcxprotocol.service.NotificationService;
import io.hcxprotocol.utils.Constants;
import io.hcxprotocol.utils.JSONUtils;
import io.hcxprotocol.utils.Operations;
import io.hcxprotocol.utils.Utils;
import org.apache.commons.io.IOUtils;
import org.apache.commons.lang3.StringUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.ByteArrayInputStream;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.Reader;
import java.net.URL;
import java.nio.charset.StandardCharsets;
import java.security.interfaces.RSAPublicKey;
import java.text.SimpleDateFormat;
import java.util.*;

/**
 * The <b>HCX Outgoing Request</b> class provide the methods to help in creating the JWE Payload and send the request to the sender system from HCX Gateway.
 *
 * <ul>
 *     <li><b>generate</b> is the main method to use by the integrator(s) to generate JWE Payload.
 *     <ul>
 *         <li>This method handles two types of requests. There are two implementations of <b>generate</b> method to handle action, on_action API calls.
 *         <ol>
 *            <li>
 *              Sending an initial request of HCX workflow action.
 *            </li>
 *            <li>Sending a response to the incoming HCX workflow action.
 *              <ul>
 *                  <li>The input request JWE should be used as <i>actionJwe</i>.</li>
 *              </ul>
 *            </li>
 *        </ol></li>
 *     </ul></li>
 *     <li>
 *         <b>validatePayload, createHeader, encryptPayload, initializeHCXCall</b> methods are used by <b>generate</b> method to compose the JWE payload generation functionality.
 *         These methods are available for the integrator(s) to use them based on different scenario(s) or use cases.
 *     </li>
 * </ul>
 *
 */
public class HCXOutgoingRequest extends FhirPayload implements OutgoingRequest {

    private static final Logger logger = LoggerFactory.getLogger(HCXOutgoingRequest.class);

    private NotificationService notificationService;

    @Override
    public boolean process(String fhirPayload, Operations operation, String recipientCode, String apiCallId, String correlationId, String actionJwe, String onActionStatus, Map<String, Object> domainHeaders, Map<String, Object> output, Config config) {
        return processOutgoing(fhirPayload, operation, recipientCode, apiCallId, correlationId, "", actionJwe, onActionStatus, domainHeaders, output, config);
    }

    @Override
    public boolean process(String fhirPayload, Operations operation, String recipientCode, String apiCallId, String correlationId, String workflowId, String actionJwe, String onActionStatus, Map<String, Object> domainHeaders, Map<String, Object> output, Config config) {
        return processOutgoing(fhirPayload, operation, recipientCode, apiCallId, correlationId, workflowId, actionJwe, onActionStatus, domainHeaders, output, config);
    }

    public boolean processOutgoing(String fhirPayload, Operations operation, String recipientCode, String apiCallId, String correlationId, String workflowId, String actionJwe, String onActionStatus, Map<String, Object> domainHeaders, Map<String, Object> output, Config config) {
        boolean result = false;
        try {
            Map<String, Object> error = new HashMap<>();
            Map<String, Object> response = new HashMap<>();
            Map<String, Object> headers = new HashMap<>(domainHeaders);
            logger.info("Processing outgoing request has started :: operation: {}", operation);
            if (!validatePayload(fhirPayload, operation, error, config)) {
                output.putAll(error);
            } else if (!createHeader(config.getString(Constants.PARTICIPANT_CODE), recipientCode, apiCallId, correlationId, workflowId, actionJwe, onActionStatus, headers, error)) {
                output.putAll(error);
            } else if (!encryptPayload(headers, fhirPayload, output, config)) {
                output.putAll(error);
            } else {
                result = initializeHCXCall(JSONUtils.serialize(output), operation, response, config);
                output.putAll(response);
            }
            if (output.containsKey(Constants.ERROR) || output.containsKey(ErrorCodes.ERR_DOMAIN_PROCESSING.toString()))
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

    public boolean validatePayload(String fhirPayload, Operations operation, Map<String,Object> error, Config config) {
        if (config.getBoolean(Constants.FHIR_VALIDATION_ENABLED))
            return validateFHIR(fhirPayload, operation, error, config);
        else return true;
    }

    @Override
    public boolean createHeader(String senderCode, String recipientCode, String apiCallId, String correlationId, String workflowId, String actionJwe, String onActionStatus, Map<String, Object> headers, Map<String, Object> error) {
        try {
            headers.put(Constants.ALG, Constants.A256GCM);
            headers.put(Constants.ENC, Constants.RSA_OAEP);
            headers.put(Constants.HCX_TIMESTAMP, new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ssZ").format(new Date()));
            if (StringUtils.isEmpty(apiCallId))
                apiCallId = UUID.randomUUID().toString();
            headers.put(Constants.HCX_API_CALL_ID, apiCallId);
            logger.info("recipient key: {}", recipientCode);
            if (!StringUtils.isEmpty(recipientCode)) {
                headers.put(Constants.HCX_SENDER_CODE, senderCode);
                headers.put(Constants.HCX_RECIPIENT_CODE, recipientCode);
                if (StringUtils.isEmpty(correlationId))
                    correlationId = UUID.randomUUID().toString();
                headers.put(Constants.HCX_CORRELATION_ID, correlationId);
                if (!StringUtils.isEmpty(workflowId))
                    headers.put(Constants.WORKFLOW_ID, workflowId);
            } else {
                Map<String, Object> actionHeaders = JSONUtils.decodeBase64String(actionJwe.split("\\.")[0], Map.class);
                headers.put(Constants.HCX_SENDER_CODE, actionHeaders.get(Constants.HCX_RECIPIENT_CODE));
                headers.put(Constants.HCX_RECIPIENT_CODE, actionHeaders.get(Constants.HCX_SENDER_CODE));
                headers.put(Constants.HCX_CORRELATION_ID, actionHeaders.get(Constants.HCX_CORRELATION_ID));
                headers.put(Constants.STATUS, onActionStatus);
                if (actionHeaders.containsKey(Constants.WORKFLOW_ID))
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
    public boolean encryptPayload(Map<String, Object> headers, String fhirPayload, Map<String, Object> output, Config config) throws Exception {
        try {
            String authToken = Utils.generateToken(config);
            String publicKeyUrl = (String) Utils.searchRegistry(headers.get(Constants.HCX_RECIPIENT_CODE).toString(), authToken, config.getString(Constants.PROTOCOL_BASE_PATH)).get(Constants.ENCRYPTION_CERT);
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
            logger.error("Error while encrypting the payload: {}", e.getMessage());
            e.printStackTrace();
            output.put(Constants.ERROR,"Error while encrypting the payload: "+ e.getMessage());
            return false;
        }
    }

    // Exception is handled in processFunction method
    @Override
    public boolean initializeHCXCall(String jwePayload, Operations operation, Map<String, Object> response, Config config) throws Exception {
        return Utils.initializeHCXCall(jwePayload, operation, response, config);
    }

    @Override
    public boolean sendNotification(String topicCode, String recipientType, List<String> recipients, String message, Map<String, String> templateParams, String correlationId, Map<String, Object> output, Config config) throws Exception {
        try {
            NotificationRequest notificationRequest = new NotificationRequest(topicCode, message, templateParams, recipientType, recipients, correlationId, config);
            NotificationService.validateNotificationRequest(notificationRequest);
            Map<String, Object> requestBody = NotificationService.createNotificationRequest(notificationRequest, output, message);
            return initializeHCXCall(JSONUtils.serialize(requestBody), Operations.NOTIFICATION_NOTIFY, output, config);
        } catch (Exception e) {
            logger.error("Error while sending the notification: {}", e.getMessage());
            output.put(Constants.ERROR, "Error while sending the notifications: " + e.getMessage());
            return false;
        }
    }
}
