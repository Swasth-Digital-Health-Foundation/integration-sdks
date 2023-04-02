package io.hcxprotocol.helper;

import ca.uhn.fhir.validation.FhirValidator;
import ca.uhn.fhir.validation.ResultSeverityEnum;
import ca.uhn.fhir.validation.SingleValidationMessage;
import ca.uhn.fhir.validation.ValidationResult;
import com.typesafe.config.Config;
import io.hcxprotocol.exception.ErrorCodes;
import io.hcxprotocol.impl.HCXIncomingRequest;
import io.hcxprotocol.utils.Constants;
import io.hcxprotocol.utils.JSONUtils;
import io.hcxprotocol.utils.Operations;
import io.hcxprotocol.validator.HCXFHIRValidator;
import org.apache.commons.lang3.StringUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * Implementation of FHIR validation using HCX FHIR IG.
 */
public abstract class FhirPayload {

    private static final Logger logger = LoggerFactory.getLogger(FhirPayload.class);

    public boolean validatePayload(String fhirPayload, Operations operation, Map<String,Object> error, Config config) {
        boolean returnBool = true;
        try {
            FhirValidator validator = HCXFHIRValidator.getValidator(config.getString(Constants.HCX_IG_BASE_PATH), config.getString(Constants.NRCES_IG_BASE_PATH));
            ValidationResult result = validator.validateWithResult(fhirPayload);
            List<SingleValidationMessage> messages = result.getMessages();
            Map<String, Object> map = JSONUtils.deserialize(fhirPayload, Map.class);
            if (!StringUtils.equalsIgnoreCase((String) map.get("resourceType"), operation.getFhirResourceType())) {
                error.put(String.valueOf(ErrorCodes.ERR_WRONG_DOMAIN_PAYLOAD), "Incorrect eObject is sent as the domain payload");
                return false;
            }
            List<String> errMessages = new ArrayList<>();
            for (SingleValidationMessage message : messages) {
                if (message.getSeverity() == ResultSeverityEnum.ERROR) {
                    errMessages.add(message.getMessage());
                    error.put(String.valueOf(ErrorCodes.ERR_INVALID_DOMAIN_PAYLOAD), errMessages);
                    returnBool = false;
                }
            }
            logger.info("FHIR Payload is validated successfully");
        }catch (Exception e){
            e.printStackTrace();
            error.put(String.valueOf(ErrorCodes.ERR_INVALID_DOMAIN_PAYLOAD), e.getMessage());
        }
        return returnBool;
    }
}
