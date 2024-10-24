# Data requirements in the pre-auth cycle

For pre-auth request :&#x20;

Sample Header:&#x20;

{

&#x20; "alg": "RSA-OAEP",

&#x20; "enc": "A256GCM",

&#x20; "x-hcx-recipient\_code": "1-29482df3-e875-45ef-a4e9-592b6f565782",  //Payors participant code as recipient&#x20;

&#x20; "x-hcx-timestamp": "2022-07-22T15:33:50.319+05:30",

&#x20; "x-hcx-sender\_code": “1-521eaec7-8cb9-4b6c-8b4e-4dba300af6f4",    // Providers participant code as sender

&#x20; "x-hcx-correlation\_id": "b382ec3a-76de-4dba-84ee-3b05a6f05b03", uuid    &#x20;

&#x20; "x-hcx-workflow\_id": "280f544a-d46e-4b5c-8ce7-15d3d68be2f2", uuid

&#x20; "x-hcx-api\_call\_id": "35b55567-e907-46c9-848a-90608f4acd18" uuid

}

FHIR Payload Sample : [https://github.com/Swasth-Digital-Health-Foundation/fhir-examples](https://github.com/Swasth-Digital-Health-Foundation/fhir-examples)

Note: In FHIR object “use” has to be “preauth”

For pre-auth response :&#x20;

{

&#x20; "alg": "RSA-OAEP",

&#x20; "enc": "A256GCM",

&#x20; "x-hcx-sender\_code": "1-29482df3-e875-45ef-a4e9-592b6f565782",  //Payors participant code as sender

&#x20; "x-hcx-timestamp": "2022-07-22T15:33:50.319+05:30",

&#x20; "x-hcx-recipient\_code": “1-521eaec7-8cb9-4b6c-8b4e-4dba300af6f4",    // Providers participant code as recipient

&#x20; "x-hcx-correlation\_id": "b382ec3a-76de-4dba-84ee-3b05a6f05b03", uuid recommended   &#x20;

&#x20; "x-hcx-workflow\_id": "280f544a-d46e-4b5c-8ce7-15d3d68be2f2", uuid recommended

&#x20; "x-hcx-api\_call\_id": "35b55567-e907-46c9-848a-90608f4acd18" uuid recommended

&#x20; “X-hcx-status” : “response.complete”

}

FHIR Payload Sample :&#x20;

[https://github.com/Swasth-Digital-Health-Foundation/fhir-examples](https://github.com/Swasth-Digital-Health-Foundation/fhir-examples)

Note: In FHIR object “use” has to be “preauth”

For pre-auth partial response :&#x20;

{

&#x20; "alg": "RSA-OAEP",

&#x20; "enc": "A256GCM",

&#x20; "x-hcx-sender\_code": "1-29482df3-e875-45ef-a4e9-592b6f565782",  //Payors participant code as sender

&#x20; "x-hcx-timestamp": "2022-07-22T15:33:50.319+05:30",

&#x20; "x-hcx-recipient\_code": “1-521eaec7-8cb9-4b6c-8b4e-4dba300af6f4",    // Providers participant code as recipient

&#x20; "x-hcx-correlation\_id": "b382ec3a-76de-4dba-84ee-3b05a6f05b03", uuid recommended   &#x20;

&#x20; "x-hcx-workflow\_id": "280f544a-d46e-4b5c-8ce7-15d3d68be2f2", uuid recommended

&#x20; "x-hcx-api\_call\_id": "35b55567-e907-46c9-848a-90608f4acd18" uuid recommended

&#x20; “X-hcx-status” : “response.partial” // in case of partial response

}

FHIR Payload Sample :&#x20;

[ https://github.com/Swasth-Digital-Health-Foundation/fhir-examples](https://github.com/Swasth-Digital-Health-Foundation/fhir-examples)

Note: In FHIR object “use” has to be “preauth”

For pre-auth enhancement request:&#x20;

For enhancement requests, the payload and header structure remain unchanged. In the header, please use the same workflow id as used in the initial pre-auth request. \
