using Io.HcxProtocol.Init;
using Io.HcxProtocol.Utils;
using System;
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
    /// The <b>Incoming Request</b> Interface provide the methods to help in processing the JWE Payload and extract FHIR Object.
    /// </summary>
    public interface IIncomingRequest
    {
        /// <summary>
        ///     Process the JWE Payload based on the Operation to validate and extract the FHIR Object.
        /// </summary>
        /// <remarks>
        ///     It has the implementation of below steps.
        ///     <list type="number">
        ///         <item>Validating HCX Protocol headers</item>
        ///         <item>Decryption of the payload and extracting FHIR object</item>
        ///         <item>Validating the FHIR object using HCX FHIR IG.</item>
        ///     </list>
        /// </remarks>
        /// <param name="jwePayload">The JWE payload from the incoming API request body.</param>
        /// <param name="operation">The HCX operation name.</param>
        /// <param name="output">A wrapper object to collect the outcome (errors or response) of the JWE Payload processing.</param>
        /// <example>
        ///     <list type="number">
        ///         <item>
        ///             output -
        ///             <code>
        ///                 {
        ///                     "headers":{}, - protocol headers
        ///                     "fhirPayload":{}, - fhir object
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
        ///     It is a boolean value to understand the incoming request processing is successful or not.
        ///     <list type="number">
        ///     <item>
        ///         true - It is successful.
        ///     </item>
        ///     <item>
        ///         false - It is failure.
        ///     </item>
        ///     </list>
        /// </returns>
      
        // bool Process(string jwePayload, Operations operation, Dictionary<string, object> output);
        bool Process(string jwePayload, Operations operation, Dictionary<string, object> output, Config config);

        /// <summary>
        ///  Validates the HCX Protocol Headers from the JWE Payload.
        ///  This method is used by [IncomingInterface.process Function] for validate the headers.
        /// </summary>
        /// <param name="jwePayload">The JWE payload from the incoming API request body.</param>
        /// <param name="operation">The HCX operation or action defined by specs to understand the functional behaviour.</param>
        /// <param name="error">A wrapper object to collect the errors from the JWE Payload.</param>
        /// <example>
        ///     <code>
        ///         {
        ///             "error_code": "error_message"
        ///         }
        ///     </code>
        /// </example>
        /// <returns>
        ///     It is a boolean value to understand the validation status of JWE Payload.
        ///     <list type="number">
        ///         <item>true - Validation is successful.</item>
        ///         <item>false - Validation is failure.</item>
        ///     </list>
        /// </returns>
        
        bool ValidateRequest(string jwePayload, Operations operation, Dictionary<string, object> error);
      

        /// <summary>
        ///     Decrypt the JWE Payload using the Participant System Private Key (which is available from the configuration).
        ///     The JWE Payload follows RFC7516.
        /// </summary>
        /// <param name="jwePayload">The JWE payload from the incoming API request body.</param>
        /// <param name="output">A wrapper object to collect the outcome (errors or response) of the JWE Payload after decryption.</param>
        /// <example>
        ///     <list type="number">
        ///         <item>
        ///             success output -
        ///             <code>
        ///                 {
        ///                     "headers":{}, - protocol headers
        ///                     "fhirPayload":{} - fhir object
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
        ///     It is a boolean value to understand the decryption status of JWE Payload.
        ///     <list type="number">
        ///         <item>true - Decryption is successful.</item>
        ///         <item>false - Decryption is failure.</item>
        ///     </list>
        /// </returns>
    
        //  bool DecryptPayload(string jwePayload, Dictionary<string, object> output);
        bool DecryptPayload(string jwePayload, string privateKey, Dictionary<string, object> output);


        /// <summary>
        ///     Validates the FHIR Object structure and required attributes using HCX FHIR IG.
        /// </summary>
        /// <param name="fhirPayload">The FHIR object extracted from the incoming JWE Payload.</param>
        /// <param name="operation">The HCX operation or action defined by specs to understand the functional behaviour.</param>
        /// <param name="error">A wrapper object to collect the errors from the FHIR Payload validation.</param>
        /// <example>
        ///     <code>
        ///         {
        ///             "error_code": "error_message"
        ///         }
        ///     </code>
        /// </example>
        /// <returns>
        ///     It is a boolean value to understand the validation status of FHIR Payload.
        ///     <list type="number">
        ///         <item>true - Validation is successful.</item>
        ///         <item>false - Validation is failure.</item>
        ///     </list>
        /// </returns>
     
        // bool ValidatePayload(string fhirPayload, Operations operation, Dictionary<string, object> error);
        bool ValidatePayload(string fhirPayload, Operations operation, Dictionary<string, object> error, Config config);


        /// <summary>
        ///     Generates the HCX Protocol API response using validation errors and the output object.
        /// </summary>
        /// <param name="error">A wrapper object to collect the errors from the JWE or FHIR Payload validations.</param>
        /// <param name="output">A wrapper object to collect the response of the JWE Payload processing.</param>
        /// <example>
        ///     <list type="number">
        ///         <item>
        ///             output -
        ///             <code>
        ///                 {
        ///                     "headers":{}, - protocol headers
        ///                     "fhirPayload":{}, - fhir object
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
        ///                 }
        ///             </code>
        ///         </item>
        ///     </list>
        /// </example>
        /// <returns>
        ///     It is a boolean value to understand the final status is successful or failure.
        /// </returns>
        bool SendResponse(Dictionary<string, object> error, Dictionary<string, object> output);


        /// <summary>
        /// Validates and decodes the jws payload. </summary>
        /// <param name="jwsPayload"> The JWS payload from the incoming API notification request body. </param>
        /// <param name="output"> A wrapper map to collect the outcome (errors or response) of the JWS Payload after decoding.
        /// <ol>
        ///    <li>output -
        ///    </li>
        ///    <li>success response object -
        ///    <pre>
        ///    {@code {
        ///       "headers": "" - fetched from incoming request
        ///       "payload": "" - fetched from incoming request
        ///       "isSignatureValid" : "" - jws signature is valid or not
        ///    }}</pre>
        ///    </li>
        ///  </ol> </param>
        /// <returns> It is a map object value to get the output of decoded jws payload.
        ///     </returns>

      //  Dictionary<string, Object> receiveNotification(String jwsPayload, Dictionary<string, Object> output, Config config);

    }
}
