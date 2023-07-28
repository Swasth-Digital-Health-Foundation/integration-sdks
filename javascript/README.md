# JavaScript Integrator SDK
What is it?
The HCX SDK(Software Development Kit) is a set of libraries, code samples, and documentation using which developers can create their own software applications that interact with the HCX platform.

What is HCX?
It is a platform designed to streamline and secure the exchange of claims data between payors and providers.
Various operations such as sending and receiving data, encryption and decryption of data, validation of payloads can be performed.

## HCX SDK Initialization

To begin working with the HCX SDK, we need to initialize the HCXIntegrator object with the configuration settings.

```javascript
object config = {
  participantCode: "your-participant-code", // Unique identifier provided by HCX
  protocolBasePath: "protocol-base-path", // Base path of the HCX instance to access Protocol APIs
  authBasePath: "auth-base-path", // Base path for authentication
  username: "your-username", // Username of the integrator
  password: "your-password", // Password of the integrator
  encryptionPrivateKey: "your-private-key", // Private key used for encryption
  igUrl: "ig-url" // IG URL
};
```
`protocolBasePath`: Basically this is the URL prefix from where all the protocol related API operations are accessed from.

`authBasePath`: This is similar to the `protocolBasePath`, but specifically refers to the part of the URL where the authentication APIs are hosted.

`const hcxIntegrator = new HCXIntegrator(config);`

The configuration object allows the SDK to handle authentication and data security protocols, ensuring communication with the HCX platform is secure.

## Outgoing Methods

Outgoing methods in the HCX SDK will handle the preparation and dispatch of data to the HCX platform.

### Validate FHIR object
This function validates the payload according to HCX specifications. It accepts a FHIR payload, an operation, and an error object. It returns a boolean to indicate if the payload validation passed.
```javascript
const fhirPayload = "your-payload"; // FHIR Payload(JSON Format)
const operation = "your-operation"; // The operation to be performed
const error = {}; // An object to capture any errors

const isValid = hcxIntegrator.validatePayload(jwePayload, operation, error);
```

### Create header for the payload.
A method used to create the necessary headers for the payload. This varies based on the type of outgoing request call. We have 2 types of outgoing request. One is where directly create a new outgoing request.(Here we create action headers). In the other type of outgoing request, we generally respond to the outgoing request. (Here we will create `on_action` headers.) It requires `recipientCode` in `action call` and `recipientCode` along with `actionJwe` for the `on action call` parameters and returns an object that can be used as a header in your request.

```javascript
const senderCode = "sender-code";(optional)
const recipientCode = "recipient-code";
const apiCallId = "api-call-id"(optional);
const correlationId = "correlation-id";(optional)
const actionJwe = "action-jwe";(used for on action calls)
const onActionStatus = "on-action-status"(optional);

headers = hcxIntegrator.create_headers(senderCode, recipientCode, apiCallId, correlationId, actionJwe, onActionStatus);
```

### Encrypt the payload
Encrypts the FHIR payload using the recipient's public certificate.
```javascript
const fhirPayload = "fhir-payload"; // FHIR payload
const encryptedPayload = hcxIntegrator.encrypt_payload(headers, fhirPayload); // Encrypted FHIR payload
```
- If successful - return JWS object ({“payload”:”adsds”})
- If failed - return map contains error codes and messages

### searchRegistry
It interacts with the HCX registry. You provide a search field and a search value, and it returns an object with the registry fields in it. It is used to find the public certificate from the registry.
```javascript
const searchField = "search-field";
const searchValue = "search-value";
const registryResults = hcxIntegrator.searchRegistry(searchField, searchValue);
```
### Initialize HCX call
- Generate the token using the config and add to the call
- Call the HCX API
- Received response will be added to response as
```javascript
  {
	“responseObj”:{
		success/error response
	}
  }
```
```javascript
const response = hcxIntegrator.initializeHCXCall(encryptedPayload, operation);
```
### Wrapper Classes
- For action API request
```Javascript
function(fhirPayload, operation, recipientCode) {
  return output
}
```
- For on_action API request
```JavaScript
  function generate(fhirPayload, operation, recipientCode, apiCallId, correlationId, actionJwe, onActionStatus, domainHeaders) {
    return output
}
```
The output will be an `object` and will be
- On success:
```JavaScript
{
    "payload":{}, # jwe payload
    "responseObj": {
     "timestamp": , # unix timestamp
     "correlation_id": "", # fetched from incoming request
     "api_call_id": "" # fetched from incoming request
}
```
- On Failure:
```JavaScript
{
    "payload":{}, # jwe payload
    "responseObj":{
     "timestamp": , # unix timestamp
     "error": {
       "code" : "", # error code
       "message": "", # error message
       "trace":"" # error trace (what is error trace)
     }
   }
}
```
## Incoming Methods

Incoming methods handle incoming requests from the HCX platform.

### Protocol validations
This function validates the incoming request as per the HCX protocol. It returns a boolean indicating whether the request is valid. The error handling can be found [here](https://docs.hcxprotocol.io/hcx-technical-specifications/open-protocol/key-components-building-blocks/error-descriptions)
```javascript
const isValidRequest = hcxIntegrator.validateRequest(jwePayload, operation, error);
```
### Decrypt Payload
Decrypts the received payload.
```javascript
function decryptPayload(encryptedString) {
returns Object
}
```
The Object returned can be:
- On success
```
{
	   “headers”: {},
	   “fhirPayload”: {}
}
```
- On failure
```
{
	"error_code": "error_message"
}
```
# Domain object Validation
Validates the decrypted payload. It is the same function used in the Outgoing Requests.
```javascript
const isValidPayload = hcxIntegrator.validatePayload(fhirPayload, operation, error);
```
# Acknowledgment
`sendResponse`: Sends the response back to the HCX gateway. This could be a success response if there were no errors or an error response.
```javascript
const output = {}; // Output to send back
hcxIntegrator.sendResponse(error, output);
```
The responses looks like:
- On success
```
{
   "headers":{}, - protocol headers
   "fhirPayload":{}, - fhir object
   "responseObj": {
   "timestamp": , - unix timestamp
   "correlation_id": "", - fetched from incoming request
   "api_call_id": "" - fetched from incoming request
}
```
- On failure:
```
{   
   "headers":{}, - protocol headers
   "fhirPayload":{}, - fhir objec
   "responseObj":{
   "timestamp": , - unix timestamp
   "error": {
        "code" : "", - error code
        "message": "", - error message
        "trace":"" - error trace
     }
}
```
### process
Orchestrates the entire process of handling an 
- incoming request,
- including validation,
- decryption,
- payload validation,
- response sending.
```javascript
const processOutput = hcxIntegrator.process(jwePayload, operation);
```
## HcxOperations

This class encapsulates the available operations that can be performed. These are defined as static strings, representing different endpoints.

```javascript
class HcxOperations {
        COVERAGE_ELIGIBILITY_CHECK ="/coverageeligibility/check"
        COVERAGE_ELIGIBILITY_ON_CHECK = "/coverageeligibility/on_check"
        PRE_AUTH_SUBMIT = "/preauth/submit"
        PRE_AUTH_ON_SUBMIT = "/preauth/on_submit"
        CLAIM_SUBMIT = "/claim/submit"
        CLAIM_ON_SUBMIT = "/claim/on_submit"
        PAYMENT_NOTICE_REQUEST = "/paymentnotice/request"
        PAYMENT_NOTICE_ON_REQUEST = "/paymentnotice/on_request"
        HCX_STATUS = "/hcx/status"
        HCX_ON_STATUS = "/hcx/on_status"
        COMMUNICATION_REQUEST = "/communication/request"
        COMMUNICATION_ON_REQUEST = "/communication/on_request"
        PREDETERMINATION_SUBMIT = "/predetermination/submit"
        PREDETERMINATION_ON_SUBMIT = "/predetermination/on_submit"
}
```
Overall, the HCX SDK for JavaScript provides a powerful interface for working with the HCX platform, enabling developers to handle complex healthcare data exchanges with relative ease.
The working of the HCX protocol, can be summarised in this diagram:

![Summary](https://github.com/claddyk/JavaScript-T/blob/main/workflow2.png)

