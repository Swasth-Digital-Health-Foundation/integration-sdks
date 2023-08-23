import { HCXOutgoingRequest } from "../src/impl/HCXOutgoingRequest.js";
import { HCXIncomingRequest } from "../src/impl/HCXIncomingRequest.js";
import responseJSON from "../src/response.json" assert { type: "json" };

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

  async processOutgoing(fhirPayload, recipientCode, operation, apiCallId, correlationId, workflowId, actionJwe, onActionStatus) {
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
      operation, apiCallId, correlationId, workflowId, actionJwe, onActionStatus
    );
    return response;
  }
  async processOutgoingCallback(fhirPayload, recipientCode, operation, apiCallId, correlationId, workflowId, onActionStatus){
    const actionJwe = responseJSON.payload;
    const outgoing = new HCXOutgoingRequest(
      this.protocolBasePath,
      this.participantCode,
      this.authBasePath,
      this.username,
      this.password,
      this.encryptionPrivateKeyURL,
      this.igURL
    );
  if(recipientCode)recipientCode=""
    const response = await outgoing.process(
      fhirPayload, recipientCode, operation, apiCallId, correlationId, workflowId, actionJwe, onActionStatus
    );
    return response;
}


  async processIncoming(encryptedPayload, operation, apiCallId, correlationId, workflowId, actionJwe, onActionStatus) {
    let incoming = new HCXIncomingRequest(
      this.protocolBasePath,
      this.participantCode,
      this.authBasePath,
      this.username,
      this.password,
      this.encryptionPrivateKeyURL,
      this.igURL
    );
    let response = await incoming.process(encryptedPayload, operation, apiCallId, correlationId, workflowId, actionJwe, onActionStatus);
    return response;
  }
}
