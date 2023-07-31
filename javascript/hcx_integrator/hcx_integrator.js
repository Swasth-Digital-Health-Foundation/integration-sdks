import { OutgoingRequest } from './utils/OutgoingRequest.js';

export class HCXIntegrator {
  constructor(config) {
    this.config = config;
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
   
  processOutgoing(fhirPayload, recipientCode, operation) {
    const outgoing = new OutgoingRequest(
      this.protocolBasePath,
      this.participantCode,
      this.authBasePath,
      this.username,
      this.password,
      this.encryptionPrivateKeyURL,
      this.igURL
    );
    const response = outgoing.process(fhirPayload, recipientCode, operation);
   
    return response;
  }
}

