import axios from "axios";

import { v4 as uuidv4 } from "uuid";
uuidv4();

import { JWEHelper } from "../JWEHelper.js";

import { generateHcxToken } from "./utils.js";
import { searchRegistry } from "./utils.js";
import { decodeBase64String } from "./utils.js";
import { Constants } from "./Constants.js";

export class OutgoingRequest {
 
  
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
    this.encryptionPrivateKeyURL = encryptionPrivateKeyURL; // not needed in outgoing
    this.igURL = igURL;
    this.hcxToken = false;
    this.Constants = new Constants()
  }
  createHeaders(recipientCode,onActionStatus=null,headers= {}) {
    
    
    // headers[this.Constants.ALG] = "RSA-OAEP"
    // headers[this.Constants.ENC] = "A256GCM"
    headers[this.Constants.API_CALL_ID] = uuidv4()
    headers[this.Constants.HCX_TIMESTAMP] = new Date().toISOString()
    headers[this.Constants.WORKFLOW_ID] = uuidv4()
    if (recipientCode.length!=0) {
        headers[this.Constants.HCX_SENDER_CODE] = this.participantCode
        headers[this.Constants.HCX_RECIPIENT_CODE] = recipientCode
        headers[this.Constants.HCX_CORRELATION_ID] = uuidv4()
        
        
      
    } else {

      const actionJwe = "eyJ4LWhjeC1yZWNpcGllbnRfY29kZSI6InRlc3Rwcm92aWRlcjEuYXBvbGxvQHN3YXN0aC1oY3gtZGV2IiwieC1oY3gtdGltZXN0YW1wIjoiMjAyMi0xMC0yN1QxMTowNzo0OCswNTMwIiwieC1oY3gtc2VuZGVyX2NvZGUiOiJ0ZXN0cHJvdmlkZXIxLmFwb2xsb0Bzd2FzdGgtaGN4LWRldiIsIngtaGN4LWNvcnJlbGF0aW9uX2lkIjoiZDRmNTZkNzktNDkwOC00YTk5LWE4ZGQtYTNiNzMzZmRlOGQ2IiwiZW5jIjoiQTI1NkdDTSIsImFsZyI6IlJTQS1PQUVQLTI1NiIsIngtaGN4LWFwaV9jYWxsX2lkIjoiMWUxNzk3YmQtZGJlZC00MTkyLWIwYzktY2VmNzcyNzI0YmU1In0"; 
      const encodedHeader = actionJwe.split(".")[0];
      const actionHeaders = decodeBase64String(encodedHeader, Map);
      console.log("i m  in else")
      console.log(actionHeaders)
  const newObject = {
    "recipient_code": actionHeaders["x-hcx-sender_code"],
    "sender_code":actionHeaders["x-hcx-recipient_code"],
    "correlation_Id": actionHeaders["x-hcx-correlation_id"]
  };
  
  
  
      headers[this.Constants.HCX_SENDER_CODE] = newObject.recipient_code
      headers[this.Constants.HCX_RECIPIENT_CODE] = newObject.sender_code
      headers[this.Constants.CORRELATION_ID] = newObject.correlation_Id
      
      headers[this.Constants.STATUS] = onActionStatus
      
      
    }
     
    console.log(headers)
    return   {
        'x-hcx-recipient_code': recipientCode,
        'x-hcx-timestamp': new Date().toISOString(),
        'x-hcx-sender_code': this.participantCode,
        'x-hcx-correlation_id': uuidv4(),
        'x-hcx-workflow_id': uuidv4(),
        'x-hcx-api_call_id': uuidv4(),
    };
  }

  async encryptPayload(recipientCode, fhirPayload) {
    const headers = this.createHeaders(recipientCode);
    
    console.log(headers)
    console.log("upto headers")
    if (!headers[this.Constants.HCX_RECIPIENT_CODE]) {
      throw new Error("Recipient code can not be empty, must be a string");
    }
    if (typeof fhirPayload !== "object") {
      throw new Error("Fhir payload must be an object");
    }
    if (!this.hcxToken) {
      
      this.hcxToken = await generateHcxToken(
        this.authBasePath,
        this.username,
        this.password
      );
      console.log(`token is ${this.hcxToken}`);
    }
    console.log(this.Constants.HCX_RECIPIENT_CODE)
    console.log(headers[this.Constants.HCX_RECIPIENT_CODE])
    const registryData = await searchRegistry(
      this.protocolBasePath,
      this.hcxToken,
       headers[this.Constants.HCX_RECIPIENT_CODE],
      "participant_code"
    );

    console.log(registryData);
    const publicCert = await axios.get(
      registryData.participants[0].encryption_cert
    );
    console.log(`public cert is ${publicCert}`);
    console.log(publicCert.data);

    const encrypted = await JWEHelper.encrypt({
      cert: publicCert.data,
      headers,
      payload: fhirPayload,
    });
    console.log(`\n encrypted is ${encrypted}`);
    return encrypted;
  }

  async initializeHCXCall(operation, encryptedJWE) {
    const url = this.protocolBasePath + "/claim/submit";
    console.log("making the API call to url " + url);
    console.log(url);
    if (!this.hcxToken) {
      this.hcxToken = generateHcxToken(
        this.authBasePath,
        this.username,
        this.password
      );
    } 
    console.log(this.hcxToken)
    const payload = JSON.stringify({
      payload: encryptedJWE,
    });
    const headers = {
      'Authorization': `Bearer ${this.hcxToken}`,
      "Content-Type": "application/json",
    };
    try {
      console.log("payload is ")
      console.log(payload)
      const response = await axios.post(url, payload, { headers });
      console.log(response.config.data);
      const rd = response.config.data;
      return rd;
    } catch (e) {
      console.error(`Initialize HCX: ${e}`);
    }
  }

  async process(fhirPayload, recipientCode, operation) {
    console.log(this.protocolBasePath);
    console.log(this.Constants.ALG)
    const encryptedPayload = await this.encryptPayload(
      recipientCode,
      fhirPayload
    );
    const response = await this.initializeHCXCall(operation, encryptedPayload);
    return {
      payload: encryptedPayload,
      response,
    };
  }
}