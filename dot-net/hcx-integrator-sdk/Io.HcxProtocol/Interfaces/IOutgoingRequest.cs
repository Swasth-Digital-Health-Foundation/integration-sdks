using Io.HcxProtocol.Init;
using Io.HcxProtocol.Utils;
using System.Collections.Generic;

namespace Io.HcxProtocol.Interfaces
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    ///     The <b>Outgoing Request</b> Interface provide the methods to help in creating the JWE Payload and send the request to the sender system from HCX Gateway.
    /// </summary>
    public interface IOutgoingRequest
    {
        /// <summary>
        ///     Generates the JWE Payload using FHIR Object, Operation and other parameters part of input. This method is used to handle the action API request.
        /// </summary>
        /// <remarks>
        ///     It has the implementation of the below steps to create JWE Payload and send the request.
        ///     <list type="bullet">
        ///         <item>Validating the FHIR object using HCX FHIR IG.</item>
        ///         <item>Crate HCX Protocol headers based on the request.</item>
        ///         <item>Fetch the sender encryption public key from the HCX participant registry.</item>
        ///         <item>Encrypt the FHIR object along with HCX Protocol headers using <b>RFC7516</b> to create JWE Payload.</item>
        ///         <item>Generate or fetch the authorization token of HCX Gateway.</item>
        ///         <item>Trigger HCX Gateway REST API based on operation.</item>
        ///     </list>
        /// </remarks>
        /// <param name="fhirPayload">The FHIR object created by the participant system.</param>
        /// <param name="operation">The HCX operation or action defined by specs to understand the functional behaviour.</param>
        /// <param name="output">A wrapper object to collect the outcome (errors or response) of the JWE Payload generation process using FHIR object.</param>
        /// <example>
        ///     <list type="number">
        ///         <item>
        ///             output -
        ///             <code>
        ///                 {
        ///                     "payload":{}, -  jwe payload
        ///                     "responseObj":{} - success/error response object
        ///                 }
        ///             </code>
        ///         </item>
        ///         <item>
        ///             success response object -
        ///             <code>
        ///                 {
        ///                     "timestamp": , - unix timestamp
        ///                     "correlation_id": "", - fetched from incoming request
        ///                     "api_call_id": "" - fetched from incoming request
        ///                 }
        ///             </code>
        ///         </item>
        ///         <item>
        ///             error response object -
        ///             <code>
        ///                 {
        ///                     "timestamp": , - unix timestamp
        ///                     "error": {
        ///                         "code" : "", - error code
        ///                         "message": "", - error message
        ///                         "trace":"" - error trace
        ///                     }
        ///                 }
        ///             </code>
        ///         </item>
        ///     </list>
        /// </example>
        /// <returns>
        ///     It is a boolean value to understand the outgoing request generation is successful or not.
        ///     <list type="number">
        ///         <item>true - It is successful.</item>
        ///         <item>false - It is failure.</item>
        ///     </list>
        /// </returns>
        bool Process(string fhirPayload, Operations operation, string recipientCode, string apiCallId, string correlationId, string actionJwe, string onActionStatus, Dictionary<string, object> domainHeaders, Dictionary<string, object> output, Config config);

        /// <summary>
        // This overload of Process Method contains the parameter workflowId.

        /// <summary>
        /// Generates the JWE Payload using FHIR Object, Operation and other parameters part of input. This method is used to handle the action and on_action API request based on the parameters.
        /// It has the implementation of the below steps to create JWE Payload and send the request.
        /// <ul>
        ///     <li>Validating the FHIR object using HCX FHIR IG.</li>
        ///     <li>Crate HCX Protocol headers based on the request and <b>actionJWE</b> payload.</li>
        ///     <li>Fetch the sender encryption public key from the HCX participant registry.</li>
        ///     <li>Encrypt the FHIR object along with HCX Protocol headers using <b>RFC7516</b> to create JWE Payload.</li>
        ///     <li>Generate or fetch the authorization token of HCX Gateway.</li>
        ///     <li>Trigger HCX Gateway REST API based on operation.</li>
        /// </ul> </summary>
        /// <param name="fhirPayload"> The FHIR object created by the participant system. </param>
        /// <param name="operation"> The HCX operation or action defined by specs to understand the functional behaviour. </param>
        /// <param name="recipientCode"> The recipient code from HCX Participant registry. </param>
        /// <param name="apiCallId"> The unique id for each request, to use the custom identifier, pass the same or else
        ///                  pass empty string("") and method will generate a UUID and uses it. </param>
        /// <param name="correlationId"> The unique id for all the messages (requests and responses) that are involved in processing of one cycle,
        ///                      to use the custom identifier, pass the same or else pass empty string("") and method will generate a UUID and uses it. </param>
        /// <param name="workflowId"> This is an optional header that can be set by providers to the same value for all requests (coverage eligibility check, preauth, claim, etc)
        ///                  related to a single admission/case. And when the workflow_id is sent by the originating provider, all other participant systems (payors) must set the same workflow id in all API calls (responses, forwards/redirects, payment notices, etc) related to the workflow. </param>
        /// <param name="actionJwe"> The JWE Payload from the incoming request for which the response JWE Payload created here. </param>
        /// <param name="onActionStatus"> The HCX Protocol header status (x-hcx-status) value to use while creating the JWE Payload. </param>
        /// <param name="domainHeaders"> The domain headers to use while creating the JWE Payload. </param>
        /// <param name="output"> A wrapper map to collect the outcome (errors or response) of the JWE Payload generation process using FHIR object. </param>
        /// <param name="config"> The config instance to get config variables.
        /// <ol>
        ///    <li>output -
        ///    <pre>
        ///    {@code {
        ///       "payload":{}, -  jwe payload
        ///       "responseObj":{} - success/error response object
        ///    }}</pre>
        ///    </li>
        ///    <li>success response object -
        ///    <pre>
        ///    {@code {
        ///       "timestamp": , - unix timestamp
        ///       "correlation_id": "", - fetched from incoming request
        ///       "api_call_id": "" - fetched from incoming request
        ///    }}</pre>
        ///    </li>
        ///    <li>error response object -
        ///    <pre>
        ///    {@code {
        ///       "timestamp": , - unix timestamp
        ///       "error": {
        ///           "code" : "", - error code
        ///           "message": "", - error message
        ///           "trace":"" - error trace
        ///        }
        ///    }}</pre>
        ///    </li>
        ///  </ol> </param>
        /// <returns> It is a boolean value to understand the outgoing request generation is successful or not.
        ///    
        /// <ol>
        ///      <li>true - It is successful.</li>
        ///      <li>false - It is failure.</li>
        /// </ol> </returns>
        bool Process(string fhirPayload, Operations operation, string recipientCode, string apiCallId, string correlationId, string workflowId, string actionJwe, string onActionStatus, Dictionary<string, object> domainHeaders, Dictionary<string, object> output, Config config);

        /// <summary>
        ///     Validates the FHIR Object structure and required attributes using HCX FHIR IG.
        /// </summary>
        /// <param name="fhirPayload">The FHIR object created by the participant system.</param>
        /// <param name="operation">The HCX operation or action defined by specs to understand the functional behaviour.</param>
        /// <param name="error">A wrapper object to collect the errors from the FHIR Payload validation.</param>
        /// <example>
        ///     <list>
        ///         <item>
        ///             <code>
        ///                 {
        ///                     "error_code": "error_message"
        ///                 }
        ///             </code>
        ///         </item>
        ///     </list>
        /// </example>
        /// <returns>
        ///     It is a boolean value to understand the validation status of FHIR Payload.
        ///     <list type="number">
        ///         <item>true - Validation is successful.</item>
        ///         <item>false - Validation is failure.</item>
        ///     </list>
        /// </returns>
        bool ValidatePayload(string fhirPayload, Operations operation, Dictionary<string, object> error, Config config);

        /// <summary>
        ///     Creates the HCX Protocol Headers using the input parameters.
        /// </summary>
        /// <param name="recipientCode">The recipient code from HCX Participant registry.</param>
        /// <param name="actionJwe">The JWE Payload from the incoming request for which the response JWE Payload created here.</param>
        /// <param name="onActionStatus">The HCX Protocol header status (x-hcx-status) value to use while creating the JEW Payload.</param>
        /// <param name="headers">The HCX Protocol headers to create the JWE Payload.</param>
        /// <returns>
        ///      It is a boolean value to understand the HCX Protocol Headers generation is successful or not.
        ///     <list type="number">
        ///         <item>true - It is successful.</item>
        ///         <item>false - It is failure.</item>
        ///     </list>
        /// </returns>
        bool CreateHeader(string senderCode, string recipientCode, string apiCallId, string correlationId, string workflowId, string actionJwe, string onActionStatus, Dictionary<string, object> headers, Dictionary<string, object> error);

        /// <summary>
        ///     It generates JWE Payload using the HCX Protocol Headers and FHIR object. The JWE Payload follows RFC7516.
        /// </summary>
        /// <param name="headers">The HCX Protocol headers to create the JWE Payload.</param>
        /// <param name="fhirPayload">The FHIR object created by the participant system.</param>
        /// <param name="output">A wrapper object to collect the JWE Payload or the error details in case of failure.</param>
        /// <example>
        ///     <list type="number">
        ///         <item>
        ///             success output -
        ///             <code>
        ///                 {
        ///                     "payload": "" - jwe payload
        ///                 }
        ///             </code>
        ///         </item>
        ///         <item>
        ///             error output -
        ///             <code>
        ///                 {
        ///                     "error_code": "error_message"
        ///                 }
        ///             </code>
        ///         </item>
        ///     </list>
        /// </example>
        /// <returns>
        ///     It is a boolean value to understand the encryption of FHIR object is successful or not.
        ///     <list type="number">
        ///         <item>true - It is successful.</item>
        ///         <item>false - It is failure.</item>
        ///     </list>
        /// </returns>
        bool EncryptPayload(Dictionary<string, object> headers, string fhirPayload, Dictionary<string, object> output, Config config);

        /// <summary>
        ///     Uses the input parameters and the SDK configuration to call HCX Gateway REST API based on the operation.
        /// </summary>
        /// <param name="jwePayload">The JWE Payload created using HCX Protocol Headers and FHIR object.</param>
        /// <param name="operation">The HCX operation or action defined by specs to understand the functional behaviour.</param>
        /// <param name="response">A wrapper object to collect response of the REST API call.</param>
        /// <example>
        ///     <list type="number">
        ///         <item>
        ///             success response -
        ///             <code>
        ///                 {
        ///                     "responseObj": {
        ///                         "timestamp": , - unix timestamp
        ///                         "correlation_id": "", - fetched from incoming request
        ///                         "api_call_id": "" - fetched from incoming request
        ///                     }
        ///                 }
        ///             </code>
        ///         </item>
        ///         <item>
        ///             error response -
        ///             <code>
        ///                 {
        ///                     "responseObj":{
        ///                         "timestamp": , - unix timestamp
        ///                         "error": {
        ///                             "code" : "", - error code
        ///                             "message": "", - error message
        ///                             "trace":"" - error trace
        ///                         }
        ///                     }
        ///                 }
        ///             </code>
        ///         </item>
        ///     </list>
        /// </example>
        /// <returns>
        ///     It is a boolean value to understand the REST API call execution is successful or not.
        /// </returns>
        /// <exception cref="JsonProcessingException">
        ///     The exception throws when it is having issues in parsing the JSON object.
        /// </exception>
        bool InitializeHCXCall(string jwePayload, Operations operation, Dictionary<string, object> response, Config config);

        /// <summary>
        /// Generates JWS payload and call to notify API. </summary>
        /// <param name="topicCode"> Topic code of the notification. </param>
        /// <param name="recipientType"> The notification will be sent to a group of participants. To easily define the list, we use role or codes or subscriptions. This property will help to understand the type of identifiers given in recipients property. The values are participant_code, participant_role, subscription. </param>
        /// <param name="recipients"> The recipients will be identified based on one of the below using recipient_type.
        ///                   <ol>
        ///                   <li>code: Participant code of the recipient(s) of the notification. Could be one or more based on the need.</li>
        ///                   <li>role: Participant role of the recipient(s) of the notification. </li>
        ///                   <li>subscription : subscription: list of subscription_ids</li>
        ///                   </ol> </param>
        /// <param name="message"> Resolved notification message. If this is empty, user should pass template parameters. </param>
        /// <param name="templateParams"> These are the values to be resolved in the notification template message. </param>
        /// <param name="correlationID"> Custom correlation ID can be passed as an input. If an empty value is passed, the system will generate a correlation ID and adds to the payload. </param>
        /// <param name="output"> A wrapper map to collect the outcome (errors or response) of the JWS Payload after decoding.
        /// <ol>
        ///    <li>output -
        ///    </li>
        ///    <li>success response object -
        ///    <pre>
        ///    {@code {
        ///         {
        ///           timestamp : ""  ,
        ///           correlation_id : ""
        ///         }
        ///    }}</pre>
        ///    </li>
        ///  </ol> </param>
        /// <returns> It is a boolean value to understand the REST API call execution is successful or not.
        ///    </returns>
        bool SendNotification(string topicCode, string recipientType, List<string> recipients, string message, Dictionary<string, string> templateParams, string correlationID, Dictionary<string, object> output, Config config);
    }
}
