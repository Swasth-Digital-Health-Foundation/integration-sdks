import axios from 'axios';
import { jwtDecrypt } from 'jose';

import fs from 'fs';
import moment from 'moment';
import {Constants} from './Constants.js';

export class HCXIncomingRequest {
    constructor(protocolBasePath, participantCode, authBasePath, username, password, encryptionPrivateKeyURL, igURL) {
        this.protocolBasePath = protocolBasePath;
        this.participantCode = participantCode;
        this.authBasePath = authBasePath;
        this.username = username;
        this.password = password;
        this.encryptionPrivateKeyURL = encryptionPrivateKeyURL;
        this.igURL = igURL;
        this.headers = null;
        this.payload = null;
        this.error = {};
        this.output = {};
    }
    validateRequest(jwePayload, operation) {
      if (typeof jwePayload !== 'object') {
          if ('payload' in jwePayload) {
              return validateJWERequest(operation, jwePayload);
          } else {
              if (!operation.startsWith("ON_")) {
                  this.error[ErrorCodes.ERR_INVALID_PAYLOAD] = ResponseMessage.INVALID_JSON_REQUEST_BODY_ERR_MSG;
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
        console.log(`Processing incoming request has started :: operation: ${operation}`);
        // this.validateRequest(payload, operation);
        let decryptedPayload = await this.decryptPayload(payload);
        let header = decryptedPayload.protectedHeader;
        console.log(header)
        this.output[Constants.HEADERS] = header;
        this.output[Constants.PAYLOAD] = decryptedPayload.plaintext;
        let result = this.send_response();
        console.log(result);
        console.log(this.output)
        return this.output;
    }

    async decryptPayload(jwePayload) {
        let privateKey;

        if (this.encryptionPrivateKeyURL.startsWith("https://")) {
            let response = await axios.get(this.encryptionPrivateKeyURL, { verify: false });
            privateKey = response.data;
        } else {
            privateKey = fs.readFileSync(this.encryptionPrivateKeyURL, 'utf8');
        }

        const decryptedPayload = await jwtDecrypt(jwePayload["payload"], privateKey, { complete: true });
        this.headers = decryptedPayload.protectedHeader;
        this.payload = decryptedPayload.plaintext;
        return decryptedPayload;
    }

    send_response() {
        // output = json.loads(output)
        let response_obj = {};
        let presentDate = moment();
        let unix_timestamp = presentDate.unix() * 1000;
        response_obj[Constants.TIMESTAMP] = unix_timestamp;
        let result = false;

        if (!this.error) {
            let headers = this.output.get(Constants.HEADERS);
            response_obj[Constants.API_CALL_ID] = headers.get(Constants.HCX_API_CALL_ID);
            response_obj[Constants.CORRELATION_ID] = headers.get(Constants.HCX_CORRELATION_ID);
            console.log(`Processing incoming request has completed :: response: ${response_obj}`);
            result = true;
        } else {
            console.error(`Error while processing the request: ${this.error}`);
            // Fetching only the first error and constructing the error object 
            let code = Object.keys(this.error)[0];
            let message = Object.values(this.error)[0];
            response_obj[Constants.ERROR] = JSON.stringify({
                [code]: message
            });
        }

        this.output[Constants.RESPONSE_OBJ] = response_obj;
        return result;
    }

    
}
