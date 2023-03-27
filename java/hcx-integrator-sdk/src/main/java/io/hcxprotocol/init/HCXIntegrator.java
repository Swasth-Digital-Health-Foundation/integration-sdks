package io.hcxprotocol.init;

import com.typesafe.config.Config;
import com.typesafe.config.ConfigFactory;
import io.hcxprotocol.impl.HCXIncomingRequest;
import io.hcxprotocol.impl.HCXOutgoingRequest;
import io.hcxprotocol.utils.Constants;
import io.hcxprotocol.utils.Operations;
import org.apache.commons.lang3.StringUtils;

import java.util.Arrays;
import java.util.List;
import java.util.Map;

/**
 * This class contains the methods to initialize config variables and process incoming and outgoing requests.
 */
public class HCXIntegrator {

    private Config config;

    public HCXIntegrator() {
    }

    /**
     * This method is to initialize config factory by passing the configuration as Map.
     *
     * @param configMap A Map that contains configuration variables and its values.
     */
    public static HCXIntegrator getInstance(Map<String,Object> configMap) throws Exception {
        HCXIntegrator hcxIntegrator = new HCXIntegrator();
        hcxIntegrator.setConfig(ConfigFactory.parseMap(configMap));
        if(hcxIntegrator.getConfig() == null)
            throw new Exception("Please initialize the configuration variables, in order to initialize the SDK");
        validateConfig(hcxIntegrator);
        return hcxIntegrator;
    }

    /**
     * Process the JWE Payload based on the Operation to validate and extract the FHIR Object.
     * It has the implementation of below steps.
     * <ol>
     *     <li>Validating HCX Protocol headers</li>
     *     <li>Decryption of the payload and extracting FHIR object</li>
     *     <li>Validating the FHIR object using HCX FHIR IG.</li>
     * </ol>
     * @param jwePayload The JWE payload from the incoming API request body.
     * @param operation The HCX operation name.
     * @param output A wrapper map to collect the outcome (errors or response) of the JWE Payload processing.
     * <ol>
     *    <li>output -
     *    <pre>
     *    {@code {
     *       "headers":{}, - protocol headers
     *       "fhirPayload":{}, - fhir object
     *       "responseObj":{} - success/error response object
     *    }}</pre>
     *    </li>
     *    <li>success response object -
     *    <pre>
     *    {@code {
     *       "timestamp": , - unix timestamp
     *       "correlation_id": "", - fetched from incoming request
     *       "api_call_id": "" - fetched from incoming request
     *    }}</pre>
     *    </li>
     *    <li>error response object -
     *    <pre>
     *    {@code {
     *       "timestamp": , - unix timestamp
     *       "error": {
     *           "code" : "", - error code
     *           "message": "", - error message
     *           "trace":"" - error trace
     *        }
     *    }}</pre>
     *    </li>
     *  </ol>
     *
     * @return It is a boolean value to understand the incoming request processing is successful or not.
     *  <ol>
     *      <li>true - It is successful.</li>
     *      <li>false - It is failure.</li>
     *  </ol>
     *
     */
    public boolean processIncoming(String jwePayload, Operations operation, Map<String, Object> output) throws Exception {
        return new HCXIncomingRequest().process(jwePayload, operation, getPrivateKey(), output);
    }

    /**
     * Generates the JWE Payload using FHIR Object, Operation and other parameters part of input. This method is used to handle the action API request.
     * It has the implementation of the below steps to create JWE Payload and send the request.
     * <ul>
     *     <li>Validating the FHIR object using HCX FHIR IG.</li>
     *     <li>Crate HCX Protocol headers based on the request.</li>
     *     <li>Fetch the sender encryption public key from the HCX participant registry.</li>
     *     <li>Encrypt the FHIR object along with HCX Protocol headers using <b>RFC7516</b> to create JWE Payload.</li>
     *     <li>Generate or fetch the authorization token of HCX Gateway.</li>
     *     <li>Trigger HCX Gateway REST API based on operation.</li>
     * </ul>
     * @param fhirPayload The FHIR object created by the participant system.
     * @param operation The HCX operation or action defined by specs to understand the functional behaviour.
     * @param recipientCode The recipient code from HCX Participant registry.
     * @param apiCallId The unique id for each request, to use the custom identifier, pass the same or else
     *                  pass an empty string("") and method will generate a UUID and uses it.
     * @param correlationId The unique id for all the messages (requests and responses) that are involved in processing of one cycle,
     *                      to use the custom identifier, pass the same or else pass empty string("") and method will generate a UUID and uses it.
     * @param output A wrapper map to collect the outcome (errors or response) of the JWE Payload generation process using FHIR object.
     * <ol>
     *    <li>output -
     *    <pre>
     *    {@code {
     *       "payload":{}, -  jwe payload
     *       "responseObj":{} - success/error response object
     *    }}</pre>
     *    </li>
     *    <li>success response object -
     *    <pre>
     *    {@code {
     *       "timestamp": , - unix timestamp
     *       "correlation_id": "", - fetched from incoming request
     *       "api_call_id": "" - fetched from incoming request
     *    }}</pre>
     *    </li>
     *    <li>error response object -
     *    <pre>
     *    {@code {
     *       "timestamp": , - unix timestamp
     *       "error": {
     *           "code" : "", - error code
     *           "message": "", - error message
     *           "trace":"" - error trace
     *        }
     *    }}</pre>
     *    </li>
     *  </ol>
     * @return It is a boolean value to understand the outgoing request generation is successful or not.
     *
     * <ol>
     *      <li>true - It is successful.</li>
     *      <li>false - It is failure.</li>
     * </ol>
     */
    public boolean processOutgoing(String fhirPayload, Operations operation, String recipientCode, String apiCallId, String correlationId, Map<String,Object> output) {
        return new HCXOutgoingRequest().generate(fhirPayload, operation, recipientCode, apiCallId, correlationId, output, config);
    }

    /**
     * Generates the JWE Payload using FHIR Object, Operation and other parameters part of input. This method is used to handle the on_action API request.
     * It has the implementation of the below steps to create JWE Payload and send the request.
     * <ul>
     *     <li>Validating the FHIR object using HCX FHIR IG.</li>
     *     <li>Crate HCX Protocol headers based on the request and <b>actionJWE</b> payload.</li>
     *     <li>Fetch the sender encryption public key from the HCX participant registry.</li>
     *     <li>Encrypt the FHIR object along with HCX Protocol headers using <b>RFC7516</b> to create JWE Payload.</li>
     *     <li>Generate or fetch the authorization token of HCX Gateway.</li>
     *     <li>Trigger HCX Gateway REST API based on operation.</li>
     * </ul>
     * @param fhirPayload The FHIR object created by the participant system.
     * @param operation The HCX operation or action defined by specs to understand the functional behaviour.
     * @param apiCallId The unique id for each request, to use the custom identifier, pass the same or else
     *                  pass empty string("") and method will generate a UUID and uses it.
     * @param correlationId The unique id for all the messages (requests and responses) that are involved in processing of one cycle,
     *                      to use the custom identifier, pass the same or else pass empty string("") and method will generate a UUID and uses it.
     * @param actionJwe The JWE Payload from the incoming request for which the response JWE Payload created here.
     * @param onActionStatus The HCX Protocol header status (x-hcx-status) value to use while creating the JEW Payload.
     * @param output A wrapper map to collect the outcome (errors or response) of the JWE Payload generation process using FHIR object.
     * <ol>
     *    <li>output -
     *    <pre>
     *    {@code {
     *       "payload":{}, -  jwe payload
     *       "responseObj":{} - success/error response object
     *    }}</pre>
     *    </li>
     *    <li>success response object -
     *    <pre>
     *    {@code {
     *       "timestamp": , - unix timestamp
     *       "correlation_id": "", - fetched from incoming request
     *       "api_call_id": "" - fetched from incoming request
     *    }}</pre>
     *    </li>
     *    <li>error response object -
     *    <pre>
     *    {@code {
     *       "timestamp": , - unix timestamp
     *       "error": {
     *           "code" : "", - error code
     *           "message": "", - error message
     *           "trace":"" - error trace
     *        }
     *    }}</pre>
     *    </li>
     *  </ol>
     * @return It is a boolean value to understand the outgoing request generation is successful or not.
     *
     * <ol>
     *      <li>true - It is successful.</li>
     *      <li>false - It is failure.</li>
     * </ol>
     */
    public boolean processOutgoing(String fhirPayload, Operations operation, String apiCallId, String correlationId, String actionJwe, String onActionStatus, Map<String,Object> output) {
        return new HCXOutgoingRequest().generate(fhirPayload, operation, apiCallId, correlationId, actionJwe, onActionStatus, output, config);
    }

    private static void validateConfig(HCXIntegrator hcxIntegrator) throws Exception {
        List<String> props = Arrays.asList(Constants.PROTOCOL_BASE_PATH, Constants.PARTICIPANT_CODE, Constants.AUTH_BASE_PATH, Constants.USERNAME, Constants.PASSWORD, Constants.ENCRYPTION_PRIVATE_KEY, Constants.IG_URL);
        for(String prop: props){
            if(!hcxIntegrator.getConfig().hasPathOrNull(prop) || StringUtils.isEmpty(hcxIntegrator.getConfig().getString(prop)))
                throw new Exception(prop + " is missing or has empty value, please add to the configuration.");
        }
    }
    public String getHCXProtocolBasePath() {
        return config.getString(Constants.PROTOCOL_BASE_PATH);
    }

    public String getParticipantCode() {
        return config.getString(Constants.PARTICIPANT_CODE);
    }

    public String getAuthBasePath() {
        return config.getString(Constants.AUTH_BASE_PATH);
    }

    public String getUsername() {
        return config.getString(Constants.USERNAME);
    }

    public String getPassword() {
        return config.getString(Constants.PASSWORD);
    }

    public String getPrivateKey() {
        return config.getString(Constants.ENCRYPTION_PRIVATE_KEY);
    }

    public String getIGUrl() {
        return config.getString(Constants.IG_URL);
    }

    private void setConfig(Config config){
        this.config = config;
    }

    private Config getConfig(){
        return this.config;
    }

}
