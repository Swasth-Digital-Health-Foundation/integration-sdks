/*const Constants = require('./Constants');
const Utils = require('./Utils');
const JSONUtils = require('./JSONUtils');
const ResponseError = require('./ResponseError');
const NotificationRequest = require('./NotificationRequest');
const JweRequest = require('./JweRequest');
const ValidateHelper = require('./ValidateHelper');

class HCXIncomingRequest {

  constructor() { }

  async process(jwePayload, operation, output, config) {
    let error = {};
    let result = false;
    jwePayload = await this.getPayload(jwePayload);
    console.log(`Processing incoming request has started :: operation: ${operation}`);
    if (!this.validateRequest(jwePayload, operation, error)) {
      this.sendResponse(error, output);
    } else if (!await this.decryptPayload(jwePayload, config[Constants.ENCRYPTION_PRIVATE_KEY], output)) {
      this.sendResponse(output, output);
    } else if (!this.validatePayload(output[Constants.FHIR_PAYLOAD], operation, error, config)) {
      this.sendResponse(error, output);
    } else {
      if (await this.sendResponse(error, output)) result = true;
    }
    return result;
  }

  validateRequest(jwePayload, operation, error) {
    return ValidateHelper.getInstance().validateRequest(jwePayload, operation, error);
  }

  async decryptPayload(jwePayload, privateKey, output) {
    try {
      let jweRequest = new JweRequest(JSONUtils.deserialize(jwePayload));
      jweRequest.decryptRequest(privateKey);
      output[Constants.HEADERS] = jweRequest.getHeaders();
      output[Constants.FHIR_PAYLOAD] = JSONUtils.serialize(jweRequest.getPayload());
      console.log("Request is decrypted successfully");
      return true;
    } catch (e) {
      console.error(`Error while decrypting the payload: ${e.message}`);
      throw new Error(`Error while decrypting the payload: ${e.message}`);
    }
  }

  validatePayload(fhirPayload, operation, error, config) {
    if (config[Constants.FHIR_VALIDATION_ENABLED])
      return this.validateFHIR(fhirPayload, operation, error, config);
    else return true;
  }

  async sendResponse(error, output) {
    let responseObj = {};
    responseObj[Constants.TIMESTAMP] = Date.now();
    let result = false;
    if (Object.keys(error).length === 0) {
      let headers = output[Constants.HEADERS];
      responseObj[Constants.API_CALL_ID] = headers[Constants.HCX_API_CALL_ID];
      responseObj[Constants.CORRELATION_ID] = headers[Constants.HCX_CORRELATION_ID];
      console.log(`Processing incoming request has completed :: response: ${JSON.stringify(responseObj)}`);
      result = true;
    } else {
      console.error(`Error while processing the request: ${JSON.stringify(error)}`);
      let code = Object.keys(error)[0];
      let message =  error[code];
      responseObj[Constants.ERROR] = JSONUtils.serialize(new ResponseError(code, message, ""));
    }
    output[Constants.RESPONSE_OBJ] = responseObj;
    return result;
  }

  async getPayload(payload) {
    if (payload.includes(Constants.PAYLOAD) || payload.includes(Constants.HCX_API_CALL_ID)) {
      return payload;
    } else {
      return JSONUtils.serialize({[Constants.PAYLOAD]: payload});
    }
  }
}

module.exports = HCXIncomingRequest;
*/