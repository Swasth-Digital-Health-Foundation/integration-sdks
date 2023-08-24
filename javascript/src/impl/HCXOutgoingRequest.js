import axios from "axios";
import { v4 as uuidv4 } from "uuid";
import { JWEHelper } from "../jwe/JWEHelper.js";
import { generateHcxToken, searchRegistry, decodeBase64String } from "../utils/utils.js";
import { Constants } from "../utils/Constants.js";

export class HCXOutgoingRequest {
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
    this.hcxToken = null;
    this.Constants = new Constants();
  }

  validatePayload(fhirPayload, operation) {
    // TODO: to be implemented
    return true;
}

  createHeaders(recipientCode, apiCallId , correlation_Id , onActionStatus , actionJwe,workflowId, headers = {}) {
    var headers = {
    [this.Constants.ALG] : "RSA-OAEP",
    [this.Constants.ENC] : "A256GCM",
    [this.Constants.HCX_API_CALL_ID] : apiCallId || uuidv4(),
    [this.Constants.HCX_TIMESTAMP] : new Date().toISOString(),

  }
    if (recipientCode.length != 0) {
      headers[this.Constants.HCX_SENDER_CODE] = this.participantCode;
      headers[this.Constants.HCX_RECIPIENT_CODE] = recipientCode;
      headers[this.Constants.HCX_CORRELATION_ID] = correlation_Id || uuidv4();
      headers[this.Constants.WORKFLOW_ID] =workflowId|| uuidv4();
    } else {
      
      const encodedHeader = actionJwe.split(".")[0];
      const actionHeaders = decodeBase64String(encodedHeader, Map);
      headers[this.Constants.HCX_SENDER_CODE] = actionHeaders["x-hcx-recipient_code"];
      headers[this.Constants.HCX_RECIPIENT_CODE] = actionHeaders["x-hcx-sender_code"];
      headers[this.Constants.HCX_CORRELATION_ID] = actionHeaders["x-hcx-correlation_id"];
      headers[this.Constants.STATUS] = onActionStatus;
      if(this.Constants.WORKFLOW_ID in actionHeaders){
       headers[this.Constants.WORKFLOW_ID] = actionHeaders[this.Constants.WORKFLOW_ID];
      }
    }
    console.log(headers)
    return headers;
  }

  async encryptPayload( fhirPayload,recipientCode, apiCallId, correlationId,workflowId, actionJwe, onActionStatus) {
    const headers = this.createHeaders(recipientCode,apiCallId,correlationId,onActionStatus,actionJwe,workflowId);
    if (typeof fhirPayload !== "object") {
      throw new Error("Fhir payload must be an object");
    }
    if (!this.hcxToken) {
      this.hcxToken = await generateHcxToken(
        this.authBasePath,
        this.username,
        this.password
      );
    }
    console.log(headers[this.Constants.HCX_RECIPIENT_CODE])
    const registryData = await searchRegistry(
      this.protocolBasePath,
      this.hcxToken,
      headers[this.Constants.HCX_RECIPIENT_CODE],
      "participant_code"
    );
    const publicCert = await axios.get(registryData.participants[0].encryption_cert);
    const encrypted = await JWEHelper.encrypt({
      cert: publicCert.data,
      headers,
      payload: fhirPayload,
    });
    return encrypted;
  }

  async initializeHCXCall(operation, encryptedJWE) {
    const url = `${this.protocolBasePath}${operation}`;
    console.log("The url is "+ url);
    if (!this.hcxToken) {
      this.hcxToken = await generateHcxToken(
        this.authBasePath,
        this.username,
        this.password
      );
    }
    const payload = JSON.stringify({payload: encryptedJWE,});
    const headers = {
      Authorization: `Bearer ${this.hcxToken}`,
      "Content-Type": "application/json",
    };
    try {
      const response = await axios.post(url, payload, { headers });
      return response.data;
    } catch (e) {
      console.error(`Initialize HCX: ${e}`);
    }

  }

  async process(fhirPayload, recipientCode, operation,apiCallId, correlationId, workflowId , actionJwe, onActionStatus) {
    const encryptedPayload = await this.encryptPayload(
 fhirPayload,recipientCode,apiCallId, correlationId,workflowId, actionJwe, onActionStatus
    );
    const response = await this.initializeHCXCall(operation, encryptedPayload);
    return {
      payload: encryptedPayload,
      response,
    };
  }
}
