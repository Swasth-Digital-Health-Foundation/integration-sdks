package io.hcxprotocol.init;

import io.hcxprotocol.impl.HCXIncomingRequest;
import io.hcxprotocol.impl.HCXOutgoingRequest;
import io.hcxprotocol.utils.Operations;

import java.util.Map;

/**
 * This class contains the methods to initialize configuration variables, process incoming and outgoing requests.
 */
public class HCXIntegrator extends BaseIntegrator {

    /**
     * This method is to initialize config factory by passing the configuration as Map.
     *
     * @param config A Map that contains configuration variables and its values.
     */
    public static HCXIntegrator getInstance(Map<String,Object> config) throws Exception {
        HCXIntegrator hcxIntegrator = new HCXIntegrator();
        hcxIntegrator.setConfig(config);
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
        return new HCXIncomingRequest().process(jwePayload, operation, output, getConfig());
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
    public boolean processOutgoingRequest(String fhirPayload, Operations operation, String recipientCode, String apiCallId, String correlationId, Map<String,Object> domainHeaders, Map<String,Object> output) {
        return new HCXOutgoingRequest().generateOutgoingRequest(fhirPayload, operation, recipientCode, apiCallId, correlationId, domainHeaders, output, getConfig());
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
    public boolean processOutgoingCallback(String fhirPayload, Operations operation, String apiCallId, String actionJwe, String onActionStatus, Map<String,Object> domainHeaders, Map<String,Object> output) {
        return new HCXOutgoingRequest().generateOutgoingCallback(fhirPayload, operation, apiCallId, actionJwe, onActionStatus, domainHeaders, output, getConfig());
    }

}
