import { HCXOutgoingRequest } from "../src/impl/HCXOutgoingRequest.js";
import { HCXIncomingRequest } from "../src/impl/HCXIncomingRequest.js";
import { ErrorCodes, ResponseMessage } from "../src/utils/Errors.js";

export class HCXIntegrator {
  constructor(config) {
    this.config = config;
    this.protocolBasePath = null;
    this.participantCode = null;
    this.authBasePath = null;
    this.username = null;
    this.password = null;
    this.encryptionPrivateKeyURL = null;
    this.igURL = null;
    this.validateConfig();
  }

  validateConfig() {
    this.protocolBasePath = this.config.protocolBasePath;
    this.participantCode = this.config.participantCode;
    this.authBasePath = this.config.authBasePath;
    this.username = this.config.username;
    this.password = this.config.password;
    this.encryptionPrivateKeyURL = this.config.encryptionPrivateKeyURL;
    this.igURL = this.config.igURL;
  }

  async processOutgoingRequest(fhirPayload, recipientCode, operation, apiCallId, correlationId, workflowId) {
    try {
      const outgoing = new HCXOutgoingRequest(
        this.protocolBasePath,
        this.participantCode,
        this.authBasePath,
        this.username,
        this.password,
        this.encryptionPrivateKeyURL,
        this.igURL
      );
      const response = await outgoing.process(
        fhirPayload,
        recipientCode,
        operation, apiCallId, correlationId, workflowId
      );
      return response;
    } catch (error) {
      console.error(`Error in processOutgoingRequest: ${error.message}\n${error.stack}`);
      this.error = {
        [ErrorCodes.OUTGOING_PROCESSING_FAILED]: ResponseMessage.OUTGOING_PROCESSING_FAILED
      };
      throw new Error(`Outgoing Request Processing failed: ${error.message}`);
    }
  }
  async processOutgoingCallback(fhirPayload, recipientCode, operation, actionJwe, apiCallId, correlationId, workflowId, onActionStatus) {
    try {
      const outgoing = new HCXOutgoingRequest(
        this.protocolBasePath,
        this.participantCode,
        this.authBasePath,
        this.username,
        this.password,
        this.encryptionPrivateKeyURL,
        this.igURL
      );
      if (recipientCode) recipientCode = ""
      const response = await outgoing.process(
        fhirPayload, recipientCode, operation, apiCallId, correlationId, workflowId, actionJwe, onActionStatus
      );
      return response;
    } catch (error) {
      console.error(`Error in processOutgoingCallback: ${error.message}`);
      this.error = {
        [ErrorCodes.ERR_OUTGOING_CALLBACK]: ResponseMessage.OUTGOING_CALLBACK_FAILED
      };
      throw new Error("Outgoing callback processing failed.");
    }
  }


  async processIncoming(encryptedPayload, operation) {
    try {
      let incoming = new HCXIncomingRequest(
        this.protocolBasePath,
        this.participantCode,
        this.authBasePath,
        this.username,
        this.password,
        this.encryptionPrivateKeyURL,
        this.igURL
      );
      let response = await incoming.process(encryptedPayload, operation);
      return response;
    }
    catch (error) {
      console.error(`Error in processIncoming: ${error.message}`);
      this.error = {
        [ErrorCodes.ERR_INCOMING_PROCESSING]: ResponseMessage.INCOMING_PROCESSING_FAILED
      };
      throw new Error("Incoming request processing failed.");
    }
  }
}
