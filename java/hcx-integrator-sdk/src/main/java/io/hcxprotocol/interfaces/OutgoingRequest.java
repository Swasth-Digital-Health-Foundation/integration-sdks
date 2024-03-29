package io.hcxprotocol.interfaces;


import com.fasterxml.jackson.core.JsonProcessingException;
import com.typesafe.config.Config;
import io.hcxprotocol.utils.Operations;

import java.util.List;
import java.util.Map;

/**
 * The <b>Outgoing Request</b> Interface provide the methods to help in creating the JWE Payload and send the request to the sender system from HCX Gateway.
 *
 */
public interface OutgoingRequest {

    /**
     * Generates the JWE Payload using FHIR Object, Operation and other parameters part of input. This method is used to handle the action and on_action API request based on the parameters.
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
     * @param recipientCode The recipient code from HCX Participant registry.
     * @param apiCallId The unique id for each request, to use the custom identifier, pass the same or else
     *                  pass empty string("") and method will generate a UUID and uses it.
     * @param correlationId The unique id for all the messages (requests and responses) that are involved in processing of one cycle,
     *                      to use the custom identifier, pass the same or else pass empty string("") and method will generate a UUID and uses it.
     * @param actionJwe The JWE Payload from the incoming request for which the response JWE Payload created here.
     * @param onActionStatus The HCX Protocol header status (x-hcx-status) value to use while creating the JWE Payload.
     * @param domainHeaders The domain headers to use while creating the JWE Payload.
     * @param output A wrapper map to collect the outcome (errors or response) of the JWE Payload generation process using FHIR object.
     * @param config The config instance to get config variables.
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
    boolean process(String fhirPayload, Operations operation, String recipientCode, String apiCallId, String correlationId, String actionJwe, String onActionStatus, Map<String,Object> domainHeaders, Map<String,Object> output, Config config);

    /**
     * Generates the JWE Payload using FHIR Object, Operation and other parameters part of input. This method is used to handle the action and on_action API request based on the parameters.
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
     * @param recipientCode The recipient code from HCX Participant registry.
     * @param apiCallId The unique id for each request, to use the custom identifier, pass the same or else
     *                  pass empty string("") and method will generate a UUID and uses it.
     * @param correlationId The unique id for all the messages (requests and responses) that are involved in processing of one cycle,
     *                      to use the custom identifier, pass the same or else pass empty string("") and method will generate a UUID and uses it.
     * @param workflowId This is an optional header that can be set by providers to the same value for all requests (coverage eligibility check, preauth, claim, etc)
     *                  related to a single admission/case. And when the workflow_id is sent by the originating provider, all other participant systems (payors) must set the same workflow id in all API calls (responses, forwards/redirects, payment notices, etc) related to the workflow.
     * @param actionJwe The JWE Payload from the incoming request for which the response JWE Payload created here.
     * @param onActionStatus The HCX Protocol header status (x-hcx-status) value to use while creating the JWE Payload.
     * @param domainHeaders The domain headers to use while creating the JWE Payload.
     * @param output A wrapper map to collect the outcome (errors or response) of the JWE Payload generation process using FHIR object.
     * @param config The config instance to get config variables.
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
    boolean process(String fhirPayload, Operations operation, String recipientCode, String apiCallId, String correlationId, String workflowId, String actionJwe, String onActionStatus, Map<String,Object> domainHeaders, Map<String,Object> output, Config config);

    /**
     * Validates the FHIR Object structure and required attributes using HCX FHIR IG.
     *
     * @param fhirPayload The FHIR object created by the participant system.
     * @param operation The HCX operation or action defined by specs to understand the functional behaviour.
     * @param error A wrapper map to collect the errors from the FHIR Payload validation.
     * @param config The config instance to get config variables.
     * <pre>
     *    {@code {
     *       "error_code": "error_message"
     *    }}</pre>
     * @return It is a boolean value to understand the validation status of FHIR Payload.
     *  <ol>
     *      <li>true - Validation is successful.</li>
     *      <li>false - Validation is failure.</li>
     *  </ol>
     */
    boolean validatePayload(String fhirPayload, Operations operation, Map<String,Object> error, Config config);

    /**
     * Creates the HCX Protocol Headers using the input parameters.
     *
     * @param senderCode The sender code from HCX Participant registry.
     * @param recipientCode The recipient code from HCX Participant registry.
     * @param apiCallId The unique id for each request, to use the custom identifier, pass the same or else
     *                  pass empty string("") and method will generate a UUID and uses it.
     * @param correlationId The unique id for all the messages (requests and responses) that are involved in processing of one cycle,
     *                      to use the custom identifier, pass the same or else pass empty string("") and method will generate a UUID and uses it.
     * @param workflowId This is an optional header that can be set by providers to the same value for all requests (coverage eligibility check, preauth, claim, etc)
     *                   related to a single admission/case. And when the workflow_id is sent by the originating provider, all other participant systems (payors) must set the same workflow id in all API calls (responses, forwards/redirects, payment notices, etc) related to the workflow.
     * @param actionJwe The JWE Payload from the incoming request for which the response JWE Payload created here.
     * @param onActionStatus The HCX Protocol header status (x-hcx-status) value to use while creating the JEW Payload.
     * @param headers The HCX Protocol headers to create the JWE Payload.
     * @param error A wrapper map to collect the errors from the header creation.
     * @return It is a boolean value to understand the HCX Protocol Headers generation is successful or not.
     * <ol>
     *      <li>true - It is successful.</li>
     *      <li>false - It is failure.</li>
     * </ol>
     */
    boolean createHeader(String senderCode, String recipientCode, String apiCallId, String correlationId, String workflowId, String actionJwe, String onActionStatus, Map<String, Object> headers, Map<String, Object> error);

    /**
     * It generates JWE Payload using the HCX Protocol Headers and FHIR object. The JWE Payload follows RFC7516.
     *
     * @param headers The HCX Protocol headers to create the JWE Payload.
     * @param fhirPayload The FHIR object created by the participant system.
     * @param output A wrapper map to collect the JWE Payload or the error details in case of failure.
     * @param config The config instance to get config variables.
     * <ol>
     *    <li>success output -
     *    <pre>
     *     {@code {
     *       "payload": "" - jwe payload
     *     }}</pre>
     *    </li>
     *    <li>error output -
     *    <pre>
     *    {@code {
     *       "error_code": "error_message"
     *    }}</pre>
     *    </li>
     *  </ol>
     * @return It is a boolean value to understand the encryption of FHIR object is successful or not.
     * <ol>
     *     <li>true - It is successful.</li>
     *     <li>false - It is failure.</li>
     * </ol>
     */
    boolean encryptPayload(Map<String,Object> headers, String fhirPayload, Map<String,Object> output, Config config) throws Exception;

    /**
     * Uses the input parameters and the SDK configuration to call HCX Gateway REST API based on the operation.
     *
     * @param jwePayload The JWE Payload created using HCX Protocol Headers and FHIR object.
     * @param operation The HCX operation or action defined by specs to understand the functional behaviour.
     * @param response A wrapper map to collect response of the REST API call.
     * @param config The config instance to get config variables.
     * <ol>
     *    <li>success response -
     *    <pre>
     *    {@code {
     *       "responseObj": {
     *       "timestamp": , - unix timestamp
     *       "correlation_id": "", - fetched from incoming request
     *       "api_call_id": "" - fetched from incoming request
     *      }
     *    }}</pre>
     *    </li>
     *    <li>error response -
     *    <pre>
     *    {@code {
     *      "responseObj":{
     *       "timestamp": , - unix timestamp
     *       "error": {
     *           "code" : "", - error code
     *           "message": "", - error message
     *           "trace":"" - error trace
     *        }
     *      }
     *    }}</pre>
     *    </li>
     *  </ol>
     * @return It is a boolean value to understand the REST API call execution is successful or not.
     *
     * @throws JsonProcessingException The exception throws when it is having issues in parsing the JSON object.
     */
    boolean initializeHCXCall(String jwePayload, Operations operation, Map<String,Object> response, Config config) throws Exception;

    /**
     * Generates JWS payload and call to notify API.
     * @param topicCode Topic code of the notification.
     * @param recipientType The notification will be sent to a group of participants. To easily define the list, we use role or codes or subscriptions. This property will help to understand the type of identifiers given in recipients property. The values are participant_code, participant_role, subscription.
     * @param recipients The recipients will be identified based on one of the below using recipient_type.
     *                   <ol>
     *                   <li>code: Participant code of the recipient(s) of the notification. Could be one or more based on the need.</li>
     *                   <li>role: Participant role of the recipient(s) of the notification. </li>
     *                   <li>subscription : subscription: list of subscription_ids</li>
     *                   </ol>
     * @param message Resolved notification message. If this is empty, user should pass template parameters.
     * @param templateParams These are the values to be resolved in the notification template message.
     * @param correlationID Custom correlation ID can be passed as an input. If an empty value is passed, the system will generate a correlation ID and adds to the payload.
     * @param output A wrapper map to collect the outcome (errors or response) of the JWS Payload after decoding.
     * <ol>
     *    <li>output -
     *    </li>
     *    <li>success response object -
     *    <pre>
     *    {@code {
           {
             timestamp : ""  ,
             correlation_id : ""
           }
     *    }}</pre>
     *    </li>
     *  </ol>
     * @return It is a boolean value to understand the REST API call execution is successful or not.
     *
     */
    boolean sendNotification(String topicCode, String recipientType, List<String> recipients, String message, Map<String,String> templateParams, String correlationID, Map<String,Object> output, Config config) throws Exception;

}
