### **Introduction:**
The HCX Platform provides a REST API interface to integrate and access the platform. However, working with the API requires knowledge of the API Specification, token generation process, token refresh, and token usage for each API call.

The HCX Integration SDK abstracts most of these operations making it easier for the participants to work with the HCX Platform.

### **About HCX Integration SDK:**
The HCX Integration SDK is now also implemented in Javascript and is available as a [pip package](). The GitHub repository also includes docs, which provide detailed information about each function of the SDK.

### **How to use:**
To use the SDK, use command 
```bash
git clone https://github.com/Swasth-Digital-Health-Foundation/integration-sdks 
cd python/hcx-integrator
pip install .
```
The hcx-integrator-sdk will now be available in your node-modules.
The SDK expects configuration of the HCX Instance and participant details to initialize and set the context for usage. The configuration can be provided as an object.

The following configuration details are required to set the context in the integration environment for using the SDK:

|**Config Variable Name**|**Mandatory**|**Details**|
| :-: | :-: | :-: |
|protocolBasePath|yes|Base Path of the HCX Instance to access Protocol APIs.|
|authBasePath|yes|Base Path to get a token for authentication|
|participantCode|yes|The participant code of the integrator in the HCX participant registry.|
|username|yes|The username of the integrator in the HCX instance.|
|password|conditionally optional|The password of the integrator in the HCX instance. If password is not provided, secret should be provided|
|encryptionPrivateKey|yes|The private key of the integrator to use it for encryption.|


### Initiating the SDK
Initialise SDK with participant password:
```py
config:dict = {
  "participantCode": "Participant Code",
  "authBasePath": "The Auth Base Path url",
  "protocolBasePath": "The Protocol Base Path url",
  "encryptionPrivateKeyURL": "Encryption Key Url",
  "username": "Username",
  "password": "Password",
  "igUrl": "igUrl"
}
```


### Processing An Incoming Request:

The participant system implements the HCX Protocol API Specification. The incoming request payload from other participants via HCX instance will contain a FHIR object, which needs to undergo validation, decryption, and other processing steps. To simplify these steps, the SDK provides a method for processing incoming requests:

```py
# Initialising the HCX integrator
hcxIntegrator = HCXIntegrator(config=config)
# Incoming process call
incoming = hcxIntegrator.processIncoming(outgoing, operation=HcxOperations.CLAIM_SUBMIT)
```

### **Generating An Outgoing Request:**

The participant system generates a request payload and send it to other participants of the HCX instance. The process involves multiple steps using a FHIR object. There are two scenarios for generating the outgoing request payload:

1. Initialising a new request for a workflow.
2. Responding to the incoming request.

Below are the SDK methods which will help in executing these steps easily.

```py
# Initialising the HCX integrator
hcxIntegrator = HCXIntegrator(config=config)

# Outgoing process call for new request
response = hcxIntegrator.processOutgoing(fhirPayload, recipientCode="string", operation=HcxOperations.CLAIM_SUBMIT)

# Outgoing callback to respond to an incoming request
response = hcxIntegrator.processOutgoing(fhirPayload, None, HcxOperations.CLAIM_ON_SUBMIT, actionJwe,  apiCallId, correlationId, workflowId, onActionStatus)
```

### License
This project is licensed under the MIT License.