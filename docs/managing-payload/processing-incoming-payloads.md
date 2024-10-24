# Processing incoming payloads

1. Ensure Request and Response structure for APIs will follow HCX Specifications as done in [HCX APIs](https://raw.githubusercontent.com/Swasth-Digital-Health-Foundation/hcx-specs/v0.7/API%20Definitions/openapi\_hcx.yaml).
2. Authenticate the API caller (HCX) validity using the bearer token in the HTTP request.
3. Decode the header in the payload and do HCX Protocol Header validation as per the document [Protocol Errors](https://docs.hcxprotocol.io/hcx-technical-specifications/open-protocol/key-components-building-blocks/error-descriptions)
4. Verify signature and decrypt the payload using your private key (as the sender is expected to encrypt the payload using your public key). You can use your respective tech stack libraries for JWE to perform this step.
5. Perform schema validations for the decrypted ciphertext which contains FHIR Object in JSON format as per [Swasth FHIR](https://ig.hcxprotocol.io/v0.7.1/index.html) implementation.
6. Validate the FHIR object (JSON) for Business validations.
7. Create the acknowledgement response to be sent back to HCX. Schema for response can be found [here\
   ](https://raw.githubusercontent.com/Swasth-Digital-Health-Foundation/hcx-specs/v0.7/API%20Definitions/openapi\_hcx.yaml)
8. Following operations will be performed to form the response body for the API call received:

* timestamp is generated while sending the response
* correlation\_id and api\_call\_id are extracted from the payload after decoding the header from the payload

Sample Success Response:

`{`

&#x20; `"timestamp": "2022-02-11T09:43:35.156+0000",`

&#x20; `"correlation_id": "1661b567-ebdf-491f-a29b-f49ab1c0a09c",`

&#x20; `"api_call_id": "8fa9a8bc-aab4-4260-927d-0186456bfd9a"`

`}`

8. Perform the business logic with the FHIR object received in the incoming request payload. This step would be specific to your platform and may require mapping incoming FHIR payload attributes to your internal data structure. Receive the final response from your internal system and use the steps below to send the corresponding callback response to the sender of the request.
9. Create the encrypted payload for on action call to be sent to the sender as detailed in the next section.
10. API request body for on\_action can be one of the following schemas (depending on the nature of the response):

* \#/components/schemas/JWEPayload
* \#/components/schemas/ProtocolResponse
* Refer to [HCX API specifications](https://raw.githubusercontent.com/Swasth-Digital-Health-Foundation/hcx-specs/v0.7/API%20Definitions/openapi\_hcx.yaml) for more details on the schema.
