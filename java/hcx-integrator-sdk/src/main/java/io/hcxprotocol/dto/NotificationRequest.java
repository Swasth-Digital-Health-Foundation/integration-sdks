package io.hcxprotocol.dto;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.typesafe.config.Config;
import io.hcxprotocol.utils.JSONUtils;

import java.util.List;
import java.util.Map;

public class NotificationRequest {
    private String jwsPayload;
    private String topicCode;
    private List<String> recipients;
    private String recipientType;
    private String message;
    private String correlationId;
    private Map<String,String> templateParams;

    private Config config;
    public NotificationRequest(String jwsPayload){
        this.jwsPayload = jwsPayload;
    }

    public NotificationRequest(String topicCode, String message, Map<String, String> templateParams, String recipientType, List<String> recipients,String correlationId,Config config) {
        this.topicCode = topicCode;
        this.message = message;
        this.templateParams = templateParams;
        this.recipientType = recipientType;
        this.recipients = recipients;
        this.correlationId = correlationId;
        this.config = config;
    }

    public String getJwsPayload() {
        return jwsPayload;
    }

    public Map<String,Object> getHeaders() throws JsonProcessingException {
       return JSONUtils.decodeBase64String(jwsPayload.split("\\.")[0], Map.class);
    }

    public Map<String,Object> getPayload() throws JsonProcessingException {
        return JSONUtils.decodeBase64String(jwsPayload.split("\\.")[1], Map.class);
    }
    public String getTopicCode() {
        return topicCode;
    }
    public List<String> getRecipients() {
        return recipients;
    }
    public String getRecipientType() {
        return recipientType;
    }

    public String getMessage() {
        return message;
    }
    public Map<String, String> getTemplateParams() {
        return templateParams;
    }

    public String getCorrelationId() {
        return correlationId;
    }

    public Config getConfig() {
        return config;
    }

}
