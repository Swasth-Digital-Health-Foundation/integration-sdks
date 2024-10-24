# Data requirements in the communication request cycle

For communication request (Supporting info) :&#x20;

Sample FHIR request:

{

&#x20; "resourceType": "CommunicationRequest",

&#x20; "id": "d35136a5-9fc3-4610-9911-2ada4ce41b8e",

&#x20; "meta": {

&#x20;   "lastUpdated": "2023-03-09T10:58:15.291+05:30",

&#x20;   "profile": \[ "https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-CommunicationRequest.html" ]

&#x20; },

&#x20; "identifier": \[ {

&#x20;   "system": "http://irdai.gov.in/insurer/123456",

&#x20;   "value": "ABCD123"

&#x20; } ],

&#x20; "basedOn": \[ {

&#x20;   "reference": "Patient/RVH1003"

&#x20; } ],

&#x20; "status": "active",

&#x20; "payload": \[ {

&#x20;   "contentString": "Please provide the accident report and any associated pictures to support your Claim# DEF5647."

&#x20; } ]

}

For communication response (Supporting info) :&#x20;

Sample FHIR response : [https://github.com/Swasth-Digital-Health-Foundation/hcx-platform/blob/sprint-37/demo-app/sample%20FHIR/communication.json](https://github.com/Swasth-Digital-Health-Foundation/hcx-platform/blob/sprint-37/demo-app/sample%20FHIR/communication.json)

For communication request (Consent) :&#x20;

Sample FHIR payload

{

&#x20; "resourceType": "CommunicationRequest",

&#x20; "id": "d35136a5-9fc3-4610-9911-2ada4ce41b8e",

&#x20; "meta": {

&#x20;   "lastUpdated": "2023-03-09T10:58:15.291+05:30",

&#x20;   "profile": \[

&#x20;     "https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-CommunicationRequest.html"

&#x20;   ]

&#x20; },

&#x20; "identifier": \[

&#x20;   {

&#x20;     "system": "http://irdai.gov.in/insurer/123456",

&#x20;     "value": "ABCD123"

&#x20;   }

&#x20; ],

&#x20; "basedOn": \[

&#x20;   {

&#x20;     "reference": "Patient/9008496789"

&#x20;   }

&#x20; ],

&#x20; "status": "active",

&#x20; "payload": \[

&#x20;   {

&#x20;     "contentString": "Please get the verification code from the Beneficiary and provide in the communication response"

&#x20;   }

&#x20; ]

}

Sample FHIR response: [https://github.com/Swasth-Digital-Health-Foundation/fhir-examples](https://github.com/Swasth-Digital-Health-Foundation/fhir-examples)

For communication response (Consent):

Sample FHIR response

{

&#x20; "resourceType": "Bundle",

&#x20; "id": "a8a51340-d761-46d3-ab66-ad5bf5bd397c",

&#x20; "meta": {

&#x20;   "lastUpdated": "2023-03-09T10:58:13.866+05:30",

&#x20;   "profile": \[

&#x20;     "https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-CommunicationBundle.html"

&#x20;   ]

&#x20; },

&#x20; "identifier": {

&#x20;   "system": "https://www.tmh.in/bundle",

&#x20;   "value": "dc045a1b-7c07-461c-8174-f741b3f2a986"

&#x20; },

&#x20; "type": "collection",

&#x20; "timestamp": "2023-03-09T10:58:13.866+05:30",

&#x20; "entry": \[

&#x20;   {

&#x20;     "fullUrl": "Communication/6831e0a6-518c-4865-a81f-8501026711fb",

&#x20;     "resource": {

&#x20;       "resourceType": "Communication",

&#x20;       "id": "6831e0a6-518c-4865-a81f-8501026711fb",

&#x20;       "meta": {

&#x20;         "lastUpdated": "2023-03-09T10:58:13.672+05:30",

&#x20;         "profile": \[

&#x20;           "https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-Communication.html"

&#x20;         ]

&#x20;       },

&#x20;       "identifier": \[

&#x20;         {

&#x20;           "system": "http://www.providerco.com/communication",

&#x20;           "value": "12345"

&#x20;         }

&#x20;       ],

&#x20;       "status": "completed",

&#x20;       "payload": \[

&#x20;         {

&#x20;           "contentString": "12345"

&#x20;         }

&#x20;       ]

&#x20;     }

&#x20;   }

&#x20; ]

}
