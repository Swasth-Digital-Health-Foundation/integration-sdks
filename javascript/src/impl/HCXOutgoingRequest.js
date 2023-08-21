import axios from "axios";
import { v4 as uuidv4 } from "uuid";
import responseJSON from "../response.json" assert { type: "json" };
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

  createHeaders(recipientCode = null, apiCallId = null, correlation_Id = null, onActionStatus = null, actionJwe = null, headers = {}) {
    var headers = {
    [this.Constants.ALG] : "RSA-OAEP",
    [this.Constants.ENC] : "A256GCM",
    [this.Constants.HCX_API_CALL_ID] : apiCallId || uuidv4(),
    [this.Constants.HCX_TIMESTAMP] : new Date().toISOString(),
    [this.Constants.WORKFLOW_ID] : uuidv4()
  }
    if (recipientCode.length != 0) {
      headers[this.Constants.HCX_SENDER_CODE] = this.participantCode;
      headers[this.Constants.HCX_RECIPIENT_CODE] = recipientCode;
      headers[this.Constants.HCX_CORRELATION_ID] = correlation_Id || uuidv4();
    } else {
      const actionJwe = responseJSON.payload;
      const encodedHeader = actionJwe.split(".")[0];
      const actionHeaders = decodeBase64String(encodedHeader, Map);
      headers[this.Constants.HCX_SENDER_CODE] = actionHeaders["x-hcx-sender_code"];
      headers[this.Constants.HCX_RECIPIENT_CODE] = actionHeaders["x-hcx-recipient_code"];
      headers[this.Constants.HCX_CORRELATION_ID] = actionHeaders["x-hcx-correlation_id"];
    }
    return headers;
  }

  async encryptPayload(recipientCode, fhirPayload) {
    const headers = this.createHeaders(recipientCode);
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
    const url = `${this.protocolBasePath}/claim/submit`;
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

  async process(fhirPayload, recipientCode, operation) {
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
