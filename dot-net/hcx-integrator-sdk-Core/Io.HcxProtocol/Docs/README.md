# HCX Integration SDK

## Introduction:

The HCX Platform providing the REST API interface to integrate and access the platform. But, it requires knowledge of the API Specification, process to generate the authorization token, refresh the token and use it in each API etc,.

The HCX Integration SDK abstract most of these operations and make it simple for the participants to work with the HCX Platform.

## About HCX Integration SDK:

The SDK implemented using dot net programming language and accessible as a nuget package file. The latest version of the SDK is available in github repository. We will soon publish it to nuget repository and provide the details.

The docs also attached along with the release in github repository for the developers to understand the details about each function of the SDK.

## How to use:
The hcx-integrator-sdk is published to https://www.nuget.org/  and available for consumption.  To use the sdk, add the below dependency to the project:

The SDK expect configuration of the HCX Instance and the participant details to initialise and set the context to use it. The configuration can be given as a Dictionary or JSON String.

It expects the below details as configuration to set the context in the integration environment to use the APIs within the SDK.



| Config Variable Name	 | Details               |
| ------------ |-----------------------|
| protocolBasePath | Base Path of the HCX Instance to access Protocol APIs. |
| authBasePath	| Base Path of the HCX Authentication Service to generate authorization tokens. |
| participantCode |	The participant code of the integrator in the HCX participant registry.|
| username |	The username of the integrator in the HCX instance.|
| password | The password of the integrator in the HCX instance.|
| encryptionPrivateKey | The private key of the integrator to use it for encryption.|
| igUrl	|The HCX instance FHIR IG URL to access the FHIR bundles for validation.|

Please use the “init” static method from HCXIntegrator class by passing the configuration to set the context. The SDK won’t work as expected without calling this method with required configuration details. It throws an exception when we access the SDK without initializing it.

Below is the sample code snippet to initialise the SDK.

```
//Define the configuration as a Dictionary.

Dictionary<String, Object> configDictionary = new Dictionary<string, object>();
configDictionary.Add("protocolBasePath", "http://staging-hcx.swasth.app/api/v0.7");
configDictionary.Add("participantCode", "<Participant code shared with you>");
configDictionary.Add("authBasePath", "http://a9dd63de91ee94d59847a1225da8b111-273954130.ap-south-1.elb.amazonaws.com:8080/auth/realms/swasth-health-claim-exchange/protocol/openid-connect/token");
configDictionary.Add("username", "<Email ID used for registration>");
configDictionary.Add("password", "<Your Password>");
configDictionary.Add("encryptionPrivateKey", "-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEF=\n-----END PRIVATE KEY-----");
configDictionary.Add("igUrl", "https://ig.hcxprotocol.io/v0.7");
HCXIntegrator.init(configDictionary);
```

## Processing An Incoming Request:

The participant system implements the HCX Protocol API Specification. The incoming request payload from other participants via HCX instance will have the FHIR object. It requires validation, decryption and other steps top process it. Below is the SDK method which will help in executing these steps easily
```
//Processing an incoming request to extract the FHIR object.

hcxIncomingRequest.Process(String jwePayload, Operations operation, Dictionary<String, Object> output);

Sample code for processing an incoming request. For reference, a sample JWE payload is also provided.

//Processing an incoming request

HCXIncomingRequest hcxIncomingRequest = new HCXIncomingRequest();

Dictionary<String, Object> output = new Dictionary<string, object>();
Dictionary<String, Object> payload = new Dictionary<string, object>();
String outputPayload = "eyJhbGciOiJSU0EtT0FFUC0yNTYiLCJlbmMiOiJBMjU2R0NNIiwieC1oY3gtYXBpX2NhbGxfaWQiOiJlMjdjNjNiMS04NmU3LTQ1NmUtYmY0ZC05NTAyYzQ4Njc1ZGEiLCJ4LWhjeC10aW1lc3RhbXAiOiIyMDIzLTAzLTAzVDE3OjAxOjUwKzA1MzAiLCJ4LWhjeC1zZW5kZXJfY29kZSI6InRlc3Rwcm92aWRlcjEuYXBvbGxvQHN3YXN0aC1oY3gtZGV2IiwieC1oY3gtcmVjaXBpZW50X2NvZGUiOiJ0ZXN0cGF5b3IxLmljaWNpQHN3YXN0aC1oY3gtZGV2IiwieC1oY3gtY29ycmVsYXRpb25faWQiOiJiZjBjYmNmMS1jMzEzLTQ1MjItYTAzOC01ODhkMTE1ODBkYzQifQ.SOWo6EIIiU2bHdM6wFHijYpiiJVWqtZ46pbzSHja1QJIc4f3f5Bvpz20skhg6K0gcuxuT6UUEsUwGxdBGRajh9TlgS3y5jFxtWqEeQxNOTST62klTjkFwBFUmgcpf0JRoV6iHLoGzUE4LgAahHPNZ3vor9_9WRoMRGbc1E2Ag8IvD-8e33gKRyM1wr6cwGq4GAYNZLkdf-ZI0Pu_zKz_3ddA_qutJZ1DBnuikretLH1SfraOA6-ehWFZK7Lk9wttSqtD76tD2XADweyV7TCnxQt6x3ngKqxVfm3Gn_QGKEQ8t7_m2rmV5YwtfiH4J3SSFM-5u8ZxVITNGnRnZsx5kg.bgcxu-v3Qyyf6F6S.Dsfm4XQLBAS7hrNqLF-gMru8iyBsoAwdzmrGC0FRSqhHrwl47iZFb67Jaz6mOtxSiMCO1fZwLrKm_SCdrqhzL-6B78jBGChDNsTbJw6oynksGExs1IQK-WC30yAo34hGThPu5Oi4HeIMxzCUFYCOR8XR2h2Viz_r9EqOLjvHIDMs60q6nXF4v0mIwjdK9Jx4WOoCy42VdWLMlXZf4Mvoc7b4biNmgcAkPsOzvD8tZmE-Y5h1jmFgqbomnqwISPoNpUAvS08YJclieVZ9KZSYOSfc8ptmER8vUX9v_HpNITpdqvWJEbiwIJe3IVjHmXlm5vVvU2m2tyHbLIfAX5xw3f3N6RLRwHaPIQWFW4DqU9GzvL9GSglH1BcB5w72m8nhk_0jGHsi4JCAoKkoZAjrk5T3ftUgl5NskZ1_QHUdq1Xt4irQ9P6Q1B5id8Cnw0Ophy4xZWPpog3h-kB0KyGmJMPE1tVGt6w05MXSPJufa96BhGXxfh8fjtIyP43d9gss2VB26I5EdRqkjqXgng9eSdcz94cqpotain2dnb_gRZ0PmnmTkZ6Z_hOPyF46dCFSJkTlIVpRFhOs1RbwUSnOXQHsJ-vET50xdKtybbxHGjWRTqFGWhF0JOorXlKKhw6aW77lJzhBkuWc6FpOy8ZgiEGEaYDSxFhrqOqPHvCcqramrpIQC13c1sCt_JXZPsbJmTZ5z0JGlr5G1CLLsmfWHdm3v_V2bfkZkY3SOeva5mBRTwjGLeuI61RixZ2uP39HNmdEANbnrqI9qaZ-mOMWFBXbq6bSpj2P5uS0YP89cLyYExC-k6bezfqrH8_hj9nJLwsThz783zytAP-eQv43oRXmXq8EdZeGAoI5HijTC2lKR2Hx7Ioi6Kh_BA1gNu35tfRWGt7yE6JlD4fce_vohbYWNAG9vAFs4MMSmCpZ1PHZy_e6D3fKz8f599GolM9nh4BbIqaH2oEsPv7HyUt5Kcgn6ckWI7eHREz6YEIXc5IzJBqAbhgHUoSaUIrAGlqTI5MkoLSQ-6FFZtclbt0nrUDcVB_0JgkJaBLT58nyra7nMQakdMqg7GKSiIYQ8MtObo0d.6EfNNtV7KUxKXrqWNfJ9VQ";
payload.Add("payload", outputPayload);
hcxIncomingRequest.Process(JSONUtils.Serialize(payload), Operations.COVERAGE_ELIGIBILITY_CHECK, output);
BaseRequest baseRequest = new BaseRequest(output);
Console.WriteLine("output of incoming request " + output);
```
## Generating An Outgoing Request:

The participant system generate a request payload and send it to other participants of the HCX instance. There are multiple steps involved in generating the final request payload using a FHIR object. The outgoing request payload generated in two scenarios. They are,
1. Initialising a new request for a workflow.
2. Responding to the incoming request.
Below are the SDK methods which will help in executing these steps easily.
```

// Initialising a new request for a workflow
 HCXOutgoingRequest.Generate(fhirPayload, Operations.operation,Dictionary<String, Object> output);

// Responding to the incoming request
HCXOutgoingRequest.generate(String fhirPayload, Operations operation, String actionJwe, String onActionStatus, Dictionary<String, Object> output);

//Sample code
HCXOutgoingRequest outgoing = new HCXOutgoingRequest();
//Sample FHIR coverage eligibility request   
String commonFhirPayload = "{ \"resourceType\": \"Bundle\", \"id\": \"d4484cdd-1aae-4d21-a92e-8ef749d6d366\", \"meta\": { \"lastUpdated\": \"2022-02-08T21:49:55.458+05:30\" }, \"identifier\": { \"system\": \"https://www.tmh.in/bundle\", \"value\": \"d4484cdd-1aae-4d21-a92e-8ef749d6d366\" }, \"type\": \"document\", \"timestamp\": \"2022-02-08T21:49:55.458+05:30\", \"entry\": [{ \"fullUrl\": \"Composition/42ff4a07-3e36-402f-a99e-29f16c0c9eee\", \"resource\": { \"resourceType\": \"Composition\", \"id\": \"42ff4a07-3e36-402f-a99e-29f16c0c9eee\", \"identifier\": { \"system\": \"https://www.tmh.in/hcx-documents\", \"value\": \"42ff4a07-3e36-402f-a99e-29f16c0c9eee\" }, \"status\": \"final\", \"type\": { \"coding\": [{ \"system\": \"https://www.hcx.org/document-type\", \"code\": \"HcxCoverageEligibilityRequest\", \"display\": \"Coverage Eligibility Request Doc\" }] }, \"subject\": { \"reference\": \"Patient/RVH1003\" }, \"date\": \"2022-02-08T21:49:55+05:30\", \"author\": [{ \"reference\": \"Organization/Tmh01\" }], \"title\": \"Coverage Eligibility Request\", \"section\": [{ \"title\": \"# Eligibility Request\", \"code\": { \"coding\": [{ \"system\": \"https://fhir.loinc.org/CodeSystem/$lookup?system=http://loinc.org&code=10154-3\", \"code\": \"CoverageEligibilityRequest\", \"display\": \"Coverage Eligibility Request\" }] }, \"entry\": [{ \"reference\": \"CoverageEligibilityRequest/dc82673b-8c71-48c2-8a17-16dcb3b035f6\" }] }] } }, { \"fullUrl\": \"Organization/Tmh01\", \"resource\": { \"resourceType\": \"Organization\", \"id\": \"Tmh01\", \"identifier\": [{ \"system\": \"http://abdm.gov.in/facilities\", \"value\": \"HFR-ID-FOR-TMH\" }, { \"system\": \"http://irdai.gov.in/facilities\", \"value\": \"IRDA-ID-FOR-TMH\" } ], \"name\": \"Tata Memorial Hospital\", \"alias\": [ \"TMH\", \"TMC\" ], \"telecom\": [{ \"system\": \"phone\", \"value\": \"(+91) 022-2417-7000\" }], \"address\": [{ \"line\": [ \"Dr Ernest Borges Rd, Parel East, Parel, Mumbai, Maharashtra 400012\" ], \"city\": \"Mumbai\", \"state\": \"Maharashtra\", \"postalCode\": \"400012\", \"country\": \"INDIA\" }], \"endpoint\": [{ \"reference\": \"https://www.tmc.gov.in/\", \"display\": \"Website\" }] } }, { \"fullUrl\": \"Patient/RVH1003\", \"resource\": { \"resourceType\": \"Patient\", \"id\": \"RVH1003\", \"identifier\": [{ \"type\": { \"coding\": [{ \"system\": \"http://terminology.hl7.org/CodeSystem/v2-0203\", \"code\": \"SN\", \"display\": \"Subscriber Number\" }] }, \"system\": \"http://gicofIndia.com/beneficiaries\", \"value\": \"BEN-101\" }, { \"system\": \"http://abdm.gov.in/patients\", \"value\": \"hinapatel@abdm\" } ], \"name\": [{ \"text\": \"Hina Patel\" }], \"gender\": \"female\" } }, { \"fullUrl\": \"CoverageEligibilityRequest/dc82673b-8c71-48c2-8a17-16dcb3b035f6\", \"resource\": { \"resourceType\": \"CoverageEligibilityRequest\", \"id\": \"dc82673b-8c71-48c2-8a17-16dcb3b035f6\", \"identifier\": [{ \"system\": \"https://www.tmh.in/coverage-eligibility-request\", \"value\": \"dc82673b-8c71-48c2-8a17-16dcb3b035f6\" }], \"status\": \"active\", \"purpose\": [ \"discovery\" ], \"patient\": { \"reference\": \"Patient/RVH1003\" }, \"servicedPeriod\": { \"start\": \"2022-02-07T21:49:56+05:30\", \"end\": \"2022-02-09T21:49:56+05:30\" }, \"created\": \"2022-02-08T21:49:56+05:30\", \"provider\": { \"reference\": \"Organization/Tmh01\" }, \"insurer\": { \"reference\": \"Organization/GICOFINDIA\" }, \"insurance\": [{ \"focal\": true, \"coverage\": { \"reference\": \"Coverage/dadde132-ad64-4d18-8c18-1d52d7e86abc\" } }] } }, { \"fullUrl\": \"Organization/GICOFINDIA\", \"resource\": { \"resourceType\": \"Organization\", \"id\": \"GICOFINDIA\", \"identifier\": [{ \"system\": \"http://irdai.gov.in/insurers\", \"value\": \"112\" }], \"name\": \"General Insurance Corporation of India\" } }, { \"fullUrl\": \"Coverage/dadde132-ad64-4d18-8c18-1d52d7e86abc\", \"resource\": { \"resourceType\": \"Coverage\", \"id\": \"dadde132-ad64-4d18-8c18-1d52d7e86abc\", \"identifier\": [{ \"system\": \"https://www.gicofIndia.in/policies\", \"value\": \"policy-RVH1003\" }], \"status\": \"active\", \"subscriber\": { \"reference\": \"Patient/RVH1003\" }, \"subscriberId\": \"SN-RVH1003\", \"beneficiary\": { \"reference\": \"Patient/RVH1003\" }, \"relationship\": { \"coding\": [{ \"system\": \"http://terminology.hl7.org/CodeSystem/subscriber-relationship\", \"code\": \"self\" }] }, \"payor\": [{ \"reference\": \"Organization/GICOFINDIA\" }] } } ] }";

//Creating an empty Dictionary to store the generated payload         
Dictionary<String, Object> output = new Dictionary<string, object>();

//We are using recipient code "1-29482df3-e875-45ef-a4e9-592b6f565782" which is a Mock Payor registered in the HCX and can be used by any participant
outgoing.generate(commonFhirPayload, Operations.COVERAGE_ELIGIBILITY_CHECK, "1-29482df3-e875-45ef-a4e9-592b6f565782",output);
Console.WriteLine("generated payload "+ output);

```