package io.hcxprotocol.service;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.typesafe.config.Config;
import io.hcxprotocol.dto.NotificationRequest;
import io.hcxprotocol.exception.ClientException;
import io.hcxprotocol.exception.ErrorCodes;
import io.hcxprotocol.utils.*;
import org.apache.commons.text.StringSubstitutor;

import java.util.*;

import static io.hcxprotocol.utils.Constants.*;

public class NotificationService {

    public static void validateNotificationRequest(NotificationRequest notificationRequest) throws ClientException {
        if (notificationRequest.getTopicCode().isEmpty()) {
            throw new ClientException("Topic code cannot be empty");
        } else if (notificationRequest.getMessage().isEmpty() && notificationRequest.getTemplateParams().isEmpty()) {
            throw new ClientException("Either the message or the template parameters are mandatory.");
        } else if (notificationRequest.getRecipients().isEmpty()) {
            throw new ClientException("Recipients cannot be empty");
        } else if (notificationRequest.getRecipientType().isEmpty()) {
            throw new ClientException("Recipient type cannot be empty");
        }
    }

    public static String getMessage(NotificationRequest notificationRequest,Map<String, Object> output, String message) throws Exception {
        Map<String, Object> searchRequest = Map.of("filters", new HashMap<>());
        if (message.isEmpty()) {
            Map<String, Object> responseMap = new HashMap<>();
            Utils.initializeHCXCall(JSONUtils.serialize(searchRequest), Operations.NOTIFICATION_LIST, responseMap, notificationRequest.getConfig());
            output.putAll(responseMap);
            if (output.containsKey(Constants.ERROR) || output.containsKey(ErrorCodes.ERR_DOMAIN_PROCESSING.toString()))
                throw new ClientException("Error while resolving the message template");
            List<Map<String, Object>> notificationsList = ((List<Map<String, Object>>) ((Map<String, Object>) responseMap.get("responseObj")).get("notifications"));
            Map<String, Object> notification = getNotification(notificationsList, notificationRequest.getTopicCode());
            message = resolveTemplate(notification, notificationRequest.getTemplateParams());
        }
        return message;
    }

    public static Map<String, Object> getNotification(List<Map<String, Object>> notificationList, String code) {
        Map<String, Object> notification = new HashMap<>();
        Optional<Map<String, Object>> result = notificationList.stream().filter(obj -> obj.get(TOPIC_CODE).equals(code)).findFirst();
        if (result.isPresent()) notification = result.get();
        return notification;
    }

    public static String getPrivateKey(Config config) {
        String privateKey = config.getString("signingPrivateKey");
        privateKey = privateKey
                .replace("-----BEGIN PRIVATE KEY-----", "")
                .replace("-----END PRIVATE KEY-----", "")
                .replaceAll("\\s+", "");
        return privateKey;
    }

    public static String resolveTemplate(Map<String, Object> notification, Map<String, String> nData) throws JsonProcessingException {
        StringSubstitutor sub = new StringSubstitutor(nData);
        return sub.replace((JSONUtils.deserialize((String) notification.get(Constants.TEMPLATE), Map.class)).get(Constants.MESSAGE));
    }

    public static Map<String, Object> getJWSRequestHeader(NotificationRequest notificationRequest) {
        Map<String, Object> headersMap = new HashMap<>();
        headersMap.put("x-hcx-correlation_id", notificationRequest.getCorrelationId().isEmpty() ? UUID.randomUUID() : notificationRequest.getCorrelationId());
        headersMap.put("sender_code", notificationRequest.getConfig().getString(Constants.PARTICIPANT_CODE));
        headersMap.put(Constants.TIMESTAMP, System.currentTimeMillis());
        headersMap.put(RECIPIENT_TYPE, notificationRequest.getRecipientType());
        headersMap.put(RECIPIENTS, notificationRequest.getRecipients());
        Map<String, Object> headers = new HashMap<>();
        headers.put("alg", "RS256");
        headers.put(NOTIFICATION_HEADERS, headersMap);
        return headers;
    }

    public static Map<String, Object> getJWSRequestPayload(NotificationRequest notificationRequest, Map<String, Object> output, String message) throws Exception {
        Map<String, Object> payload = new HashMap<>();
        payload.put(TOPIC_CODE, notificationRequest.getTopicCode());
        message = NotificationService.getMessage(notificationRequest, output,message);
        payload.put(MESSAGE, message);
        return payload;
    }

    public static Map<String,Object> createNotificationRequest(NotificationRequest notificationRequest,Map<String,Object> output,String message) throws Exception {
        Map<String, Object> headers = NotificationService.getJWSRequestHeader(notificationRequest);
        Map<String,Object> payload = NotificationService.getJWSRequestPayload(notificationRequest,output,message);
        String jwsToken = JWTUtils.generateJWS(headers, payload, NotificationService.getPrivateKey(notificationRequest.getConfig()));
        Map<String, Object> requestBody = new HashMap<>();
        requestBody.put(PAYLOAD, jwsToken);
        return requestBody;
    }
}
