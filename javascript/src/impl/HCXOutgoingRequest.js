import axios from "axios";
import { v4 as uuidv4 } from "uuid";
import { JWEHelper } from "../jwe/JWEHelper.js";
import { generateToken, searchRegistry, decodeBase64String } from "../utils/utils.js";
import { Constants } from "../utils/Constants.js";
import { ErrorCodes, ResponseMessage } from "../utils/Errors.js";

export class HCXOutgoingRequest {
  constructor(
    protocolBasePath,
    participantCode,
    authBasePath,
    username,
    password,
    secret,
    encryptionPrivateKeyURL,
    igURL
  ) {
    this.protocolBasePath = protocolBasePath;
    this.participantCode = participantCode;
    this.authBasePath = authBasePath;
    this.username = username;
    this.password = password;
    this.secret = secret;
    this.encryptionPrivateKeyURL = encryptionPrivateKeyURL; // not needed in outgoing
    this.igURL = igURL;
    this.hcxToken = null;
    this.Constants = new Constants();
  }

  validatePayload(fhirPayload, operation) {
    // TODO: to be implemented
    return true;
  }

  createHeader(recipientCode, apiCallId, correlationId, onActionStatus, actionJwe, workflowId, headers = {}) {
    
    if(headers = null) {
      headers = {};
    }
    var headers = {
      [this.Constants.ALG]: "RSA-OAEP",
      [this.Constants.ENC]: "A256GCM",
      [this.Constants.HCX_API_CALL_ID]: apiCallId || uuidv4(),
      [this.Constants.HCX_TIMESTAMP]: new Date().toISOString(),
    }
    if (recipientCode.length != 0) {
      headers[this.Constants.HCX_SENDER_CODE] = this.participantCode;
      headers[this.Constants.HCX_RECIPIENT_CODE] = recipientCode;
      headers[this.Constants.HCX_CORRELATION_ID] = correlationId || uuidv4();
      headers[this.Constants.WORKFLOW_ID] = workflowId || uuidv4();
    } else {

      const encodedHeader = actionJwe.split(".")[0];
      const actionHeaders = decodeBase64String(encodedHeader, Map);
      headers[this.Constants.HCX_SENDER_CODE] = actionHeaders["x-hcx-recipient_code"];
      headers[this.Constants.HCX_RECIPIENT_CODE] = actionHeaders["x-hcx-sender_code"];
      headers[this.Constants.HCX_CORRELATION_ID] = actionHeaders["x-hcx-correlation_id"];
      headers[this.Constants.STATUS] = onActionStatus;
      if (this.Constants.WORKFLOW_ID in actionHeaders) {
        headers[this.Constants.WORKFLOW_ID] = actionHeaders[this.Constants.WORKFLOW_ID];
      }
    }
    return headers;
  }

  async encryptPayload(fhirPayload, recipientCode, apiCallId, correlationId, workflowId, actionJwe, onActionStatus) {
    try {
      const headers = this.createHeader(recipientCode, apiCallId, correlationId, onActionStatus, actionJwe, workflowId);
      if (typeof fhirPayload !== "object") {
        throw new Error("Fhir payload must be an object");
      }
      if (!this.hcxToken) {
        const payload = {}
        if(this.username) payload[this.Constants.USERNAME] = this.username;
        if(this.password) payload[this.Constants.PASSWORD] = this.password;
        if(this.secret) payload[this.Constants.SECRET] = this.secret;
        if(this.participantCode) payload["participant_code"] = this.participantCode;
        this.hcxToken = await generateToken(
          this.authBasePath,
          payload
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
    } catch (error) {
      console.log(error.response.data);
      console.error(`Error in encryptPayload: ${error.message}\n${error.stack}`);
      this.error = {
        [ErrorCodes.ERR_INVALID_ENCRYPTION]: ResponseMessage.INVALID_PAYLOAD_VALUES_ERR_MSG
      };
      throw new Error(`Service unavailable: ${error.message}`);
    }
  }

  async initializeHCXCall(operation, jwePayload) {
    try {
      const url = `${this.protocolBasePath}${operation}`;
      if (!this.hcxToken) {
        const payload = {}
        if(this.username) payload[this.Constants.USERNAME] = this.username;
        if(this.password) payload[this.Constants.PASSWORD] = this.password;
        if(this.secret) payload[this.Constants.SECRET] = this.secret;
        if(this.participantCode) payload["participant_code"] = this.participantCode;
        this.hcxToken = await generateToken(
          this.authBasePath,
          payload
        );
      }
      const payload = JSON.stringify({ payload: jwePayload, });
      const headers = {
        Authorization: `Bearer ${this.hcxToken}`,
        "Content-Type": "application/json",
      };
      try {
        const response = await axios.post(url, payload, { headers });
        return response.data;
      } catch (e) {
        console.error(`Initialize HCX: ${e.response.data}`);
      }
    } catch (error) {
      console.error(`Initialize HCX: ${error}`);
      this.error = {
        [ErrorCodes.ERR_SERVICE_UNAVAILABLE]: ResponseMessage.INVALID_STATUS_ERR_MSG
      };
      throw new Error(`Service unavailable: ${error.message}`);
    }
  }

  async process(fhirPayload, recipientCode, operation, apiCallId, correlationId, workflowId, actionJwe, onActionStatus) {
    try {
      const encryptedPayload = await this.encryptPayload(
        fhirPayload, recipientCode, apiCallId, correlationId, workflowId, actionJwe, onActionStatus
      );
      const response = await this.initializeHCXCall(operation, encryptedPayload);
      return {
        payload: encryptedPayload,
        response,
      };
    } catch (error) {
      console.error(`Error in process: ${error}`);
      this.error = {
        [ErrorCodes.ERR_DOMAIN_PROCESSING]: ResponseMessage.INVALID_STATUS_ERR_MSG
      };
      return this.send_response(error)
    }
  }
   
  send_response(outputData) {
    let response_obj = {};
    let presentDate = moment();
    let unix_timestamp = presentDate.unix() * 1000;
    response_obj[this.Constants.TIMESTAMP] = unix_timestamp;
    let result = false;

    if (!outputData) {
      response_obj[this.Constants.API_CALL_ID] = this.Constants.HCX_API_CALL_ID;
      response_obj[this.Constants.CORRELATION_ID] =
        this.Constants.HCX_CORRELATION_ID;
      result = true;
    } else {
      console.error(`Error while encrypting the payload: ${outputData}`);
      response_obj[this.Constants.ERROR] = (`Error while encrypting the payload: ${outputData}`)
    }
    return this.output[this.Constants.RESPONSE_OBJ] = response_obj;
  }
}