using Io.HcxProtocol.Utils;
using System.Collections.Generic;

namespace Io.HcxProtocol.Interfaces
{
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
        bool Generate(string fhirPayload, Operations operation, string recipientCode, Dictionary<string, object> output);

        /// <summary>
        ///     Generates the JWE Payload using FHIR Object, Operation and other parameters part of input. This method is used to handle the on_action API request.
        ///     <para>It has the implementation of the below steps to create JWE Payload and send the request.</para>
        /// </summary>
        /// <remarks>
        ///     <list type="bullet">
        ///         <item>Validating the FHIR object using HCX FHIR IG.</item>
        ///         <item>Create HCX Protocol headers based on the request and <b>actionJWE</b> payload.</item>
        ///         <item>Fetch the sender encryption public key from the HCX participant registry.</item>
        ///         <item>Encrypt the FHIR object along with HCX Protocol headers using <b>RFC7516</b> to create JWE Payload.</item>
        ///         <item>Generate or fetch the authorization token of HCX Gateway.</item>
        ///         <item>Trigger HCX Gateway REST API based on operation.</item>
        ///     </list>
        /// </remarks>
        /// <param name="fhirPayload">The FHIR object created by the participant system.</param>
        /// <param name="operation">The HCX operation or action defined by specs to understand the functional behaviour.</param>
        /// <param name="actionJwe">The JWE Payload from the incoming request for which the response JWE Payload created here.</param>
        /// <param name="onActionStatus">The HCX Protocol header status (x-hcx-status) value to use while creating the JWE Payload.</param>
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
        bool Generate(string fhirPayload, Operations operation, string actionJwe, string onActionStatus, Dictionary<string, object> output);

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
        bool ValidatePayload(string fhirPayload, Operations operation, Dictionary<string, object> error);

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
        bool CreateHeader(string recipientCode, string actionJwe, string onActionStatus, Dictionary<string, object> headers);

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
        bool EncryptPayload(Dictionary<string, object> headers, string fhirPayload, Dictionary<string, object> output);

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
        bool InitializeHCXCall(string jwePayload, Operations operation, Dictionary<string, object> response);

    }
}
