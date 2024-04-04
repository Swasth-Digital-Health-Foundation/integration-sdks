import axios from "axios";
import moment from "moment";
import { Constants } from "../utils/Constants.js";
import { JWEHelper } from "../jwe/JWEHelper.js";
import { ErrorCodes, ResponseMessage } from "../utils/Errors.js"

export class HCXIncomingRequest {
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
    this.encryptionPrivateKeyURL = encryptionPrivateKeyURL;
    this.igURL = igURL;
    this.headers = null;
    this.payload = null;
    this.error = 0;
    this.output = {};
    this.Constants = new Constants();
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
    return true;
  }

  async process(payload, operation) {
    try {
      var cert;
      if (this.encryptionPrivateKeyURL.startsWith("https://")) {
        let response = await axios.get(this.encryptionPrivateKeyURL, {
          verify: false,
        });
        cert = response.data;
      }
      let decryptedPayload = await JWEHelper.decrypt({ cert, payload });
      let header = decryptedPayload.header;
      this.output[this.Constants.HEADERS] = header;
      this.output[this.Constants.PAYLOAD] = decryptedPayload.payload;
      return this.output;
    } catch (e) {
      console.log(e.message);
      return this.send_response(e)
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
      console.error(`Error while decrypting the payload: ${outputData}`);
      response_obj[this.Constants.ERROR] = outputData.message
    }
    return this.output[this.Constants.RESPONSE_OBJ] = response_obj;
  }
}
