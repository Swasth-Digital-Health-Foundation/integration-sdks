const OutgoingRequest = require('/home/claddy/Documents/integration-sdks/javascript/HCX Integrator/Impl/outgoing_request');
const HcxOperations = require('./src/utils/hcx_operations');

class HCXIntegrator {
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
        // TODO: check for all contants/attributes needed
        // set all the internal variables
        this.protocolBasePath = this.config["protocolBasePath"];
        this.participantCode = this.config["participantCode"];
        this.authBasePath = this.config["authBasePath"];
        this.username = this.config["username"];
        this.password = this.config["password"];
        this.encryptionPrivateKeyURL = this.config["encryptionPrivateKeyURL"];
        this.igURL = this.config["igURL"];
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

module.exports = HCXIntegrator;