# Formulating outgoing payloads

A Request body for on\_action calls needs to be created. Creating the HCX Protocol headers for response would need a few details from the protocol header of the incoming request, hence please ensure that it was preserved. The following steps need to be performed to create the payload for the callback response:

1. Update sender id (x-hcx-sender\_code) with your id.
2. Update the recipient id with the id of the sender (x-hcx-sender\_code) of the incoming request.
3. Assign a unique id (uuid) to x-hcx-api\_call\_id.
4. Copy correlation id (x-hcx-correlation\_id) from the incoming request header.
5. x-hcx-status shall be one of response.complete, response.error, response.partial or response.redirect.

* In the case of response.complete and response.partial the request body will follow the below schema
  * \#/components/schemas/JWEPayload schema
* In case of response.error, the request body will follow one of the below schemas
  * \#/components/schemas/JWEPayload for errors related to the payload during business evaluation (after successful decryption)
  * \#/components/schemas/ProtocolResponse which will have error details in it. This schema will be used for Protocol Header related errors and unable to access the ciphertext due to any reasons.
* In case of response.redirect, the request body will follow the below schema
  * In case of response.redirect, the request body will follow the below schema
    * \#/components/schemas/ProtocolResponse
    * Also, a redirect\_to header will be added to the protocol header

6. Payload for the request body for the callback (on\_check/on\_submit) will be a sample FHIR object as per the specifications. Eg. if coverageeligibility/check call has been made with payload as CoverageEligibilityRequest document then the response should have the CoverageEligibilityResponse document in the coverageeligibility/on\_check call to the HCX > Provider
7. Encrypted and signed the response data and protocol headers with Recipient's Public Key which can be accessed using HCX Registry APIs.
8. A new JWE payload is created with the headers and payload created in the previous steps.
9. If not already available, create the Bearer token using your Username and Password shared during the onboarding process. Below is the curl snippet for token generation.

`curl --location --request POST 'https://staging-hcx.swasth.app/auth/realms/swasth-health-claim-exchange/protocol/openid-connect/token' \`

`--header 'content-type: application/x-www-form-urlencoded' \`

`--data-urlencode 'client_id=registry-frontend' \`

`--data-urlencode 'username=<REGISTERED_EMAIL>' \`

`--data-urlencode 'password=<PASSWORD>' \`

`--data-urlencode 'grant_type=password'`

10. Post the callback to the HCX using the Bearer token and payload.
