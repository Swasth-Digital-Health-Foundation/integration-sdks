const axios = require('axios');
const uuid = require('uuid');
const { generateHcxToken, searchRegistry } = require('./utils');
const HcxOperations = require('./hcx_operations');
const { JWEHelper } = require('./jwe_helper');

class OutgoingRequest {
    constructor(protocolBasePath, participantCode, authBasePath, username, password, encryptionPrivateKeyURL, igURL) {
        this.protocolBasePath = protocolBasePath;
        this.participantCode = participantCode;
        this.authBasePath = authBasePath;
        this.username = username;
        this.password = password;
        this.encryptionPrivateKeyURL = encryptionPrivateKeyURL; // not needed in outgoing
        this.igURL = igURL;
        this.hcxToken = null;
    }

    validatePayload(fhirPayload, operation) {
        // TODO: to be implemented
        return true;
    }

    createHeaders(recipientCode) {
        const date = new Date().toISOString();
        return {
            alg: "RSA-OAEP",
            enc: "A256GCM",
            'x-hcx-recipient_code': recipientCode,
            'x-hcx-timestamp': date,
            'x-hcx-sender_code': this.participantCode,
            'x-hcx-correlation_id': uuid.v4(),
            'x-hcx-workflow_id': uuid.v4(),
            'x-hcx-api_call_id': uuid.v4()
        };
    }

    async encryptPayload(recipientCode, fhirPayload) {
        if (!recipientCode) {
            throw new Error("Recipient code can not be empty, must be a string");
        }
        if (typeof fhirPayload !== 'object') {
            throw new Error("Fhir payload must be a object");
        }
        if (!this.hcxToken) {
            this.hcxToken = generateHcxToken(this.authBasePath, this.username, this.password);
        }
        const registryData = searchRegistry(this.protocolBasePath, this.hcxToken, "participant_code", recipientCode);
        const publicCert = registryData.participants[0].encryption_cert;
        const headers = this.createHeaders(recipientCode);
        const encryptedPayload = await JWEHelper.encrypt({ cert: publicCert, headers: headers, payload: fhirPayload });
        return encryptedPayload;
    }

    async initializeHCXCall(operation, encryptedJWE) {
        const url = `${this.protocolBasePath}${operation.value}`;
        console.log(`making the API call to url ${url}`);
        if (!this.hcxToken) {
            this.hcxToken = generateHcxToken(this.authBasePath, this.username, this.password);
        }
        const payload = JSON.stringify({
            payload: encryptedJWE
        });
        const headers = {
            'Authorization': `Bearer ${this.hcxToken}`,
            'Content-Type': 'application/json'
        };
        try {
            const response = await axios.post(url, payload, { headers: headers });
            return response.data;
        } catch (e) {
            console.log(`Initialise HCX: ${e}`);
        }
    }

    async process(fhirPayload, recipientCode, operation) {
        // this.validatePayload(fhirPayload, operation);
        const encryptedPayload = await this.encryptPayload(recipientCode, fhirPayload);
        const response = await this.initializeHCXCall(operation, encryptedPayload);
        return {
            payload: encryptedPayload,
            response: response
        };
    }
}

module.exports = OutgoingRequest;
