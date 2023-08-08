import axios from "axios";
import jose from "node-jose";

import fs from "fs";
import moment from "moment";
import { Constants } from "./Constants.js";
import { JWEHelper } from "../JWEHelper.js";
// The other imported modules in the Python code like HcxOperations, generateHcxToken, searchRegistry, validateJWERequest,
// validateJsonRequest, ErrorCodes, ResponseMessage would need JavaScript counterparts, which are not provided here.

export class HCXIncomingRequest {
    
  constructor(
    protocolBasePath,
    participantCode,
    authBasePath,
    username,
    password,
    encryptionPrivateKeyURL,
    igURL
  ) {
    this.protocolBasePath = protocolBasePath;
    this.participantCode = participantCode;
    this.authBasePath = authBasePath;
    this.username = username;
    this.password = password;
    this.encryptionPrivateKeyURL = encryptionPrivateKeyURL;
    this.igURL = igURL;
    this.headers = null;
    this.payload = null;
    this.error = 0;
    this.output ={};
    this.Constants = new Constants()
  }
  validateRequest(jwePayload, operation) {
    if (typeof jwePayload !== "object") {
      if ("payload" in jwePayload) {
        return validateJWERequest(operation, jwePayload);
      } else {
        if (!operation.startsWith("ON_")) {
          this.error[ErrorCodes.ERR_INVALID_PAYLOAD] =
            ResponseMessage.INVALID_JSON_REQUEST_BODY_ERR_MSG;
          return false;
        }
        if (!validateJsonRequest(operation, this.error, jwePayload)) {
          return false;
        }
      }
    }
  }

  validatePayload(fhirPayload, operation) {
    // TODO: fhir validation to be implemented
    return true;
  }

 

  async process(payload, operation) {
    console.log(
      `Processing incoming request has started :: operation: ${operation}`
    );
    // this.validateRequest(payload, operation);
    try {
      console.log("in process");
      var cert
       //new JWEhelper class object 
      if (this.encryptionPrivateKeyURL.startsWith("https://")) 
     
      {
      console.log("in if of decrypt");
      console.log(this.encryptionPrivateKeyURL);
      let response = await axios.get(this.encryptionPrivateKeyURL, {
       verify: false,
    });
    console.log(response.data)
    cert =response.data
     
  }
      console.log(cert)
      let decryptedPayload = await JWEHelper.decrypt({cert, payload})
      console.log("thise is return of decrypt")
      console.log(decryptedPayload)
      let header = decryptedPayload.header;


      console.log(header);

      this.output[this.Constants.HEADERS] = header;
      this.output[this.Constants.PAYLOAD] = decryptedPayload.payload;


      const payload2 = decryptedPayload.payload

      console.log(payload2)

      let result = this.send_response();
      console.log(result);
      
      return this.output
    } catch (e) {
      console.log(e);
    }
  }

  // async decryptPayload(jwePayload) {
  //   let privateKey;

  //   if (this.encryptionPrivateKeyURL.startsWith("https://")) {
  //     console.log("in if of decrypt");
  //     console.log(this.encryptionPrivateKeyURL);
  //     let response = await axios.get(this.encryptionPrivateKeyURL, {
  //       verify: false,
  //     });
  //     console.log(response.data);
  //     const privateKeyPem = response.data;

  //     privateKey = await jose.JWK.asKey(privateKeyPem, "pem");
  //     console.log(privateKey);
  //   } else {
  //     privateKey = fs.readFileSync(this.encryptionPrivateKeyURL, "utf8");
  //   }
   
    
    // const protectedHeader = {'alg': 'RSA-OAEP', 'enc': 'A256GCM', 'api_call_id': '571f347e-d0a0-4d0d-8261-3a1df228814d', 'x-hcx-timestamp': '2023-08-07T23:38:10+05:30', 'x-hcx-sender_code': 'testprovider1.swasthmock@swasth-hcx-staging', 'x-hcx-recipient_code': 'testpayor1.swasthmock@swasth-hcx-staging', 'x-hcx-correlation_id': null}
  //   // const payload = {"resourceType": "Bundle", "id": "98aa81af-7a49-4159-a8ed-35e721d6ae74", "meta": {"lastUpdated": "2023-02-20T14:03:15.013+05:30", "profile": ["https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-ClaimRequestBundle.html"]}, "identifier": {"system": "https://www.tmh.in/bundle", "value": "7ee7ee1a-fcad-49c3-8127-aa70c7a4dc0d"}, "type": "collection", "timestamp": "2023-02-20T14:03:15.013+05:30", "entry": [{"fullUrl": "Claim/bb1eea08-8739-4f14-b541-04622f18450c", "resource": {"resourceType": "Claim", "id": "bb1eea08-8739-4f14-b541-04622f18450c", "meta": {"lastUpdated": "2023-02-20T14:03:14.918+05:30", "profile": ["https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-Claim.html"]}, "identifier": [{"system": "http://identifiersystem.com", "value": "IdentifierValue"}], "status": "active", "type": {"coding": [{"system": "http://terminology.hl7.org/CodeSystem/claim-type", "code": "institutional"}]}, "use": "claim", "patient": {"reference": "Patient/RVH1003"}, "created": "2023-02-20T14:03:14+05:30", "insurer": {"reference": "Organization/GICOFINDIA"}, "provider": {"reference": "Organization/WeMeanWell01"}, "priority": {"coding": [{"system": "http://terminology.hl7.org/CodeSystem/processpriority", "code": "normal"}]}, "payee": {"type": {"coding": [{"system": "http://terminology.hl7.org/CodeSystem/payeetype", "code": "provider"}]}, "party": {"reference": "Organization/WeMeanWell01"}}, "careTeam": [{"sequence": 4, "provider": {"reference": "Organization/WeMeanWell01"}}], "diagnosis": [{"sequence": 1, "diagnosisCodeableConcept": {"coding": [{"system": "http://irdai.com", "code": "E906184", "display": "SINGLE INCISION LAPAROSCOPIC APPENDECTOMY"}], "text": "SINGLE INCISION LAPAROSCOPIC APPENDECTOMY"}, "type": [{"coding": [{"system": "http://terminology.hl7.org/CodeSystem/ex-diagnosistype", "code": "admitting", "display": "Admitting Diagnosis"}]}]}], "insurance": [{"sequence": 1, "focal": true, "coverage": {"reference": "Coverage/COVERAGE1"}}], "item": [{"sequence": 1, "productOrService": {"coding": [{"system": "https://irdai.gov.in/package-code", "code": "E101021", "display": "Twin Sharing Ac"}]}, "unitPrice": {"value": 100000, "currency": "INR"}}, {"sequence": 1, "productOrService": {"coding": [{"system": "https://irdai.gov.in/package-code", "code": "E924260", "display": "CLINICAL TOXICOLOGY SCREEN, BLOOD"}]}, "unitPrice": {"value": 2000, "currency": "INR"}}, {"sequence": 1, "productOrService": {"coding": [{"system": "https://irdai.gov.in/package-code", "code": "E924261", "display": "CLINICAL TOXICOLOGY SCREEN,URINE"}]}, "unitPrice": {"value": 1000, "currency": "INR"}}, {"sequence": 1, "productOrService": {"coding": [{"system": "https://irdai.gov.in/package-code", "code": "E507029", "display": "ECG"}]}, "unitPrice": {"value": 5000, "currency": "INR"}}, {"sequence": 1, "productOrService": {"coding": [{"system": "https://irdai.gov.in/package-code", "code": "E6080377", "display": "UltraSound Abdomen"}]}, "unitPrice": {"value": 5000, "currency": "INR"}}, {"sequence": 1, "productOrService": {"coding": [{"system": "https://irdai.gov.in/package-code", "code": "502001", "display": "Surgeons Charges"}]}, "unitPrice": {"value": 1000, "currency": "INR"}}, {"sequence": 1, "productOrService": {"coding": [{"system": "https://irdai.gov.in/package-code", "code": "5020021", "display": "Anesthesiologist charges"}]}, "unitPrice": {"value": 1000, "currency": "INR"}}, {"sequence": 1, "productOrService": {"coding": [{"system": "https://irdai.gov.in/package-code", "code": "E6080373", "display": "Physician charges"}]}, "unitPrice": {"value": 1000, "currency": "INR"}}, {"sequence": 1, "productOrService": {"coding": [{"system": "https://irdai.gov.in/package-code", "code": "201008", "display": "Recovery Room"}]}, "unitPrice": {"value": 10000, "currency": "INR"}}, {"sequence": 1, "productOrService": {"coding": [{"system": "https://irdai.gov.in/package-code", "code": "406003", "display": "intra -venous (iv) set"}]}, "unitPrice": {"value": 5000, "currency": "INR"}}, {"sequence": 1, "productOrService": {"coding": [{"system": "https://irdai.gov.in/package-code", "code": "E507353", "display": "Oral Medication"}]}, "unitPrice": {"value": 5000, "currency": "INR"}}, {"sequence": 1, "productOrService": {"coding": [{"system": "https://irdai.gov.in/package-code", "code": "E925171", "display": "Hospital charges"}]}, "unitPrice": {"value": 5000, "currency": "INR"}}, {"sequence": 1, "productOrService": {"coding": [{"system": "https://irdai.gov.in/package-code", "code": "501001", "display": "Consultation Charges"}]}, "unitPrice": {"value": 5000, "currency": "INR"}}], "total": {"value": 146000.0, "currency": "INR"}}}, {"fullUrl": "Organization/WeMeanWell01", "resource": {"resourceType": "Organization", "id": "WeMeanWell01", "meta": {"profile": ["https://nrces.in/ndhm/fhir/r4/StructureDefinition/Organization"]}, "identifier": [{"type": {"coding": [{"system": "http://terminology.hl7.org/CodeSystem/v2-0203", "code": "AC", "display": "Narayana"}]}, "system": "http://abdm.gov.in/facilities", "value": "HFR-ID-FOR-TMH"}], "name": "WeMeanWell Hospital", "address": [{"text": " Bannerghatta Road, Bengaluru ", "city": "Bengaluru", "country": "India"}]}}, {"fullUrl": "Organization/GICOFINDIA", "resource": {"resourceType": "Organization", "id": "GICOFINDIA", "meta": {"profile": ["https://nrces.in/ndhm/fhir/r4/StructureDefinition/Organization"]}, "identifier": [{"type": {"coding": [{"system": "http://terminology.hl7.org/CodeSystem/v2-0203", "code": "AC", "display": "GOVOFINDIA"}]}, "system": "http://irdai.gov.in/insurers", "value": "GICOFINDIA"}], "name": "GICOFINDIA"}}, {"fullUrl": "Patient/RVH1003", "resource": {"resourceType": "Patient", "id": "RVH1003", "meta": {"profile": ["https://nrces.in/ndhm/fhir/r4/StructureDefinition/Patient"]}, "identifier": [{"type": {"coding": [{"system": "http://terminology.hl7.org/CodeSystem/v2-0203", "code": "SN", "display": "Subscriber Number"}]}, "system": "http://gicofIndia.com/beneficiaries", "value": "BEN-101"}], "name": [{"text": "Prasidh Dixit"}], "gender": "male", "birthDate": "1960-09-26", "address": [{"text": "#39 Kalena Agrahara, Kamanahalli, Bengaluru - 560056", "city": "Bengaluru", "state": "Karnataka", "postalCode": "560056", "country": "India"}]}}, {"fullUrl": "Coverage/COVERAGE1", "resource": {"resourceType": "Coverage", "id": "COVERAGE1", "meta": {"profile": ["https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-Coverage.html"]}, "identifier": [{"system": "https://www.gicofIndia.in/policies", "value": "policy-RVH1003"}], "status": "active", "subscriber": {"reference": "Patient/RVH1003"}, "subscriberId": "2XX8971", "beneficiary": {"reference": "Patient/RVH1003"}, "relationship": {"coding": [{"system": "http://terminology.hl7.org/CodeSystem/subscriber-relationship", "code": "self"}]}, "payor": [{"reference": "Organization/GICOFINDIA"}]}}]}
  //   console.log(payload);

  //   return {payload,protectedHeader}
  // }

  send_response() {
    // output = json.loads(output)
    let response_obj = {};
    let presentDate = moment();
    let unix_timestamp = presentDate.unix() * 1000;
    response_obj[this.Constants.TIMESTAMP] = unix_timestamp;
    let result = false;

    if (!this.error) {
      console.log("here")
      
      let headers = this.Constants.HEADERS
      console.log(headers)
      response_obj[this.Constants.API_CALL_ID] = this.Constants.HCX_API_CALL_ID;
      response_obj[this.Constants.CORRELATION_ID] = this.Constants.HCX_CORRELATION_ID

      console.log(
        `Processing incoming request has completed :: response: ${response_obj}`
      );
      result = true;
    } else {
      console.error(`Error while processing the request: ${this.error}`);
      // Fetching only the first error and constructing the error object
      let code = Object.keys(this.error)[0];
      let message = Object.values(this.error)[0];
      response_obj[this.Constants.ERROR] = JSON.stringify({
        [code]: message,
      });
    }

    this.output[this.Constants.RESPONSE_OBJ] = response_obj;
    return result;
  }
}