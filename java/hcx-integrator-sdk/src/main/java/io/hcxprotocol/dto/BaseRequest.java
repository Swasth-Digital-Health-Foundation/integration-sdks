package io.hcxprotocol.dto;

import io.hcxprotocol.exception.ErrorCodes;
import io.hcxprotocol.utils.DateTimeUtils;
import io.hcxprotocol.utils.JSONUtils;
import io.hcxprotocol.utils.Operations;
import org.apache.commons.collections.MapUtils;
import org.apache.commons.lang3.StringUtils;

import java.text.MessageFormat;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

import static io.hcxprotocol.utils.Constants.*;
import static io.hcxprotocol.utils.ResponseMessage.*;

/**
 * This is base class to extract the protocol headers from the jwe/json payload.
 */
public class BaseRequest {

    public Map<String, Object> protocolHeaders;
    Map<String, Object> payload;

    public BaseRequest(Map<String, Object> payload) throws Exception {
        this.payload = payload;
        if (payload.containsKey(PAYLOAD))
            this.protocolHeaders = JSONUtils.decodeBase64String(getPayloadValues()[0], Map.class);
        else
            this.protocolHeaders = payload;
    }

    public String getWorkflowId() {
        return getHeader(WORKFLOW_ID);
    }

    public String getApiCallId() {
        return getHeader(HCX_API_CALL_ID);
    }

    public String getCorrelationId() {
        return getHeader(HCX_CORRELATION_ID);
    }

    public String getHcxSenderCode() {
        return getHeader(HCX_SENDER_CODE);
    }

    public String getTimestamp() {
        return getHeader(HCX_TIMESTAMP);
    }

    public String getDebugFlag() {
        return getHeader(DEBUG_FLAG);
    }

    public String getStatus() {
        return getHeader(STATUS);
    }

    protected String getHeader(String key) {
        return (String) protocolHeaders.getOrDefault(key, "");
    }

    public Map<String, Object> getPayload() {
        return payload;
    }

    protected Map<String, Object> getHeaderMap(String key) {
        return (Map<String, Object>) protocolHeaders.getOrDefault(key, new HashMap<>());
    }

    public Map<String, Object> getHeaders(){
        return this.protocolHeaders;
    }

    private void setHeaderMap(String key, Object value) {
        protocolHeaders.put(key, value);
    }

    public Map<String, Object> getErrorDetails() {
        return getHeaderMap(ERROR_DETAILS);
    }

    public Map<String, Object> getDebugDetails() {
        return getHeaderMap(DEBUG_DETAILS);
    }

    public String getRedirectTo() {
        return getHeader(REDIRECT_TO);
    }

    public String[] getPayloadValues() {
        return ((String) getPayload().get(PAYLOAD)).split("\\.");
    }

    public boolean validateHeadersData(List<String> mandatoryHeaders, Operations operation, Map<String, Object> error) {
        boolean result = false;
        List<String> missingHeaders = mandatoryHeaders.stream().filter(key -> !protocolHeaders.containsKey(key)).collect(Collectors.toList());
        if (!missingHeaders.isEmpty()) {
            error.put(ErrorCodes.ERR_MANDATORY_HEADER_MISSING.toString(), MessageFormat.format(INVALID_MANDATORY_ERR_MSG, missingHeaders));
            return true;
        }
        // Validate Header values
        if (validateCondition(!isString(getHeaders().get(HCX_API_CALL_ID)), error, ErrorCodes.ERR_INVALID_API_CALL_ID.toString(), INVALID_API_CALL_ID_ERR_MSG))
            return true;
        if (validateCondition(!isString(getHeaders().get(HCX_CORRELATION_ID)), error, ErrorCodes.ERR_INVALID_CORRELATION_ID.toString(), INVALID_CORRELATION_ID_ERR_MSG))
            return true;
        if (validateCondition(!DateTimeUtils.validTimestamp(getTimestamp()), error, ErrorCodes.ERR_INVALID_TIMESTAMP.toString(), INVALID_TIMESTAMP_ERR_MSG))
            return true;
        if (validateCondition(protocolHeaders.containsKey(WORKFLOW_ID) && !isString(getHeaders().get(WORKFLOW_ID)), error, ErrorCodes.ERR_INVALID_WORKFLOW_ID.toString(), INVALID_WORKFLOW_ID_ERR_MSG))
            return true;
        //validating option headers
        result = validateOptionalHeaders(error);
        // validating onAction headers
        result = validateOnAction(operation,error);
        return result;

    }
    public boolean validateJwePayload(Map<String, Object> error, String[] payloadArr) {
        if (payloadArr != null && payloadArr.length != PROTOCOL_PAYLOAD_LENGTH) {
            error.put(ErrorCodes.ERR_INVALID_PAYLOAD.toString(), INVALID_PAYLOAD_LENGTH_ERR_MSG);
            return true;
        }
        if (payloadArr != null) {
            for (String value : payloadArr) {
                if (value == null || value.isEmpty()) {
                    error.put(ErrorCodes.ERR_INVALID_PAYLOAD.toString(), INVALID_PAYLOAD_VALUES_ERR_MSG);
                    return true;
                }
            }
        }
        return false;
    }

    public boolean validateCondition(Boolean condition, Map<String, Object> error, String key, String msg) {
        if (condition) {
            error.put(key, msg);
            return true;
        }
        return false;
    }

    public boolean isString(Object value) {
        return value instanceof String && !StringUtils.isEmpty((String) value);
    }

    public boolean validateDetails(Map<String, Object> inputMap, Map<String, Object> error, String errorKey, String msg, List<String> rangeValues, String rangeMsg) {
        if (MapUtils.isEmpty(inputMap)) {
            error.put(errorKey, msg);
            return true;
        } else if (!inputMap.containsKey(CODE) || !inputMap.containsKey(MESSAGE)) {
            error.put(errorKey, msg);
            return true;
        }
        for (String key : inputMap.keySet()) {
            if (!rangeValues.contains(key)) {
                error.put(key, rangeMsg);
                return true;
            }
        }
        return false;
    }

    public boolean validateOptionalHeaders(Map<String, Object> error) {
        if (protocolHeaders.containsKey(DEBUG_FLAG) && validateValues(getDebugFlag(), error, ErrorCodes.ERR_INVALID_DEBUG_FLAG.toString(), INVALID_DEBUG_FLAG_ERR_MSG, DEBUG_FLAG_VALUES, MessageFormat.format(INVALID_DEBUG_FLAG_RANGE_ERR_MSG, DEBUG_FLAG_VALUES)))
                return true;
        if (protocolHeaders.containsKey(ERROR_DETAILS)) {
            if (validateDetails(getErrorDetails(), error, ErrorCodes.ERR_INVALID_ERROR_DETAILS.toString(), INVALID_ERROR_DETAILS_ERR_MSG, ERROR_DETAILS_VALUES, MessageFormat.format(INVALID_ERROR_DETAILS_RANGE_ERR_MSG, ERROR_DETAILS_VALUES)))
                return true;
            if (validateCondition(!RECIPIENT_ERROR_VALUES.contains(((Map<String, Object>) protocolHeaders.get(ERROR_DETAILS)).get(CODE)), error, ErrorCodes.ERR_INVALID_ERROR_DETAILS.toString(), INVALID_ERROR_DETAILS_CODE_ERR_MSG))
                return true;

        }
        if (protocolHeaders.containsKey(DEBUG_DETAILS)) {
            return validateDetails(getDebugDetails(), error, ErrorCodes.ERR_INVALID_DEBUG_DETAILS.toString(), INVALID_DEBUG_DETAILS_ERR_MSG, ERROR_DETAILS_VALUES, MessageFormat.format(INVALID_DEBUG_DETAILS_RANGE_ERR_MSG, ERROR_DETAILS_VALUES));
        }
        return false;
    }

    public boolean validateOnAction(Operations operation, Map<String,Object> error){
        if (operation.getOperation().contains("on_")) {
            if (validateCondition(!protocolHeaders.containsKey(STATUS), error, ErrorCodes.ERR_INVALID_STATUS.toString(), MessageFormat.format(INVALID_MANDATORY_ERR_MSG, STATUS)))
                return true;
            return validateValues(getStatus(), error, ErrorCodes.ERR_INVALID_STATUS.toString(), INVALID_STATUS_ERR_MSG, RESPONSE_STATUS_VALUES, MessageFormat.format(INVALID_STATUS_ON_ACTION_RANGE_ERR_MSG, RESPONSE_STATUS_VALUES));
        }
        else {
            if (protocolHeaders.containsKey(STATUS)) {
                return validateValues(getStatus(), error, ErrorCodes.ERR_INVALID_STATUS.toString(), INVALID_STATUS_ERR_MSG, REQUEST_STATUS_VALUES, MessageFormat.format(INVALID_STATUS_ACTION_RANGE_ERR_MSG, REQUEST_STATUS_VALUES));
                }
        }
        return false;
    }



    public boolean validateValues(String inputStr, Map<String, Object> error, String key, String msg, List<String> statusValues, String rangeMsg) {
        if (StringUtils.isEmpty(inputStr)) {
            error.put(key, msg);
            return true;
        } else if (!statusValues.contains(inputStr)) {
            error.put(key, rangeMsg);
            return true;
        }
        return false;
    }

}
