import { HCXOutgoingRequest } from "../src/impl/HCXOutgoingRequest.js";
import { HCXIncomingRequest } from "../src/impl/HCXIncomingRequest.js";

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

  async processOutgoing(fhirPayload, recipientCode, operation) {
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
      operation
    );
    return response;
  }
  async processIncoming(encryptedPayload, operation = null) {
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
}
