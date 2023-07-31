import axios from 'axios';

import { v4 as uuidv4 } from 'uuid';
uuidv4();



import { JWEHelper } from '../JWEHelper.js';

import {generateHcxToken} from "../utils/utils.js"
import { searchRegistry } from '../utils/utils.js';

export class OutgoingRequest {
    constructor(protocolBasePath, participantCode, authBasePath, username, password, encryptionPrivateKeyURL, igURL) {
        this.protocolBasePath = protocolBasePath;
        this.participantCode = participantCode;
        this.authBasePath = authBasePath;
        this.username = username;
        this.password = password;
        this.encryptionPrivateKeyURL = encryptionPrivateKeyURL; // not needed in outgoing
        this.igURL = igURL;
        this.hcxToken = false;
    }
    createHeaders(recipientCode) {
        return {
            'x-hcx-recipient_code': recipientCode,
            'x-hcx-timestamp': new Date().toISOString(),
            'x-hcx-sender_code': this.participantCode,
            'x-hcx-correlation_id': uuidv4(),
            'x-hcx-workflow_id': uuidv4(),
            'x-hcx-api_call_id': uuidv4(),
        };
    }
    

    async encryptPayload(recipientCode, fhirPayload) {
        if (!recipientCode) {
            throw new Error("Recipient code can not be empty, must be a string");
        }
        if (typeof fhirPayload !== 'object') {
            throw new Error("Fhir payload must be an object");
        }
        if (!this.hcxToken) {
            console.log(`generating token ${this.hcxToken}`)
            this.hcxToken = await generateHcxToken(this.authBasePath, this.username, this.password);
            console.log(`token is ${this.hcxToken}`)
        }
        const registryData = await searchRegistry(this.protocolBasePath, this.hcxToken, recipientCode,"participant_code");

        console.log(registryData)
        const publicCert = await axios.get(registryData.participants[0].encryption_cert);
        console.log(`public cert is ${publicCert}`)
        console.log(publicCert.data)
        const headers = this.createHeaders(recipientCode);
        const encrypted = await JWEHelper.encrypt({
            cert: publicCert.data,
            headers,
            payload: fhirPayload
        });
        console.log(`\n encrypted is ${encrypted}`)
        return encrypted;
    }

    async initializeHCXCall(operation, encryptedJWE) {
        const url = this.protocolBasePath +"/claim/submit";
        console.log("making the API call to url " + url);
        console.log(url)
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
            const response = await axios.post(url, payload, { headers });
            console.log(response.config.data)
            const rd = response.config.data;
            return rd;
        } catch (e) {
            console.error(`Initialize HCX: ${e}`);
        }
    }

    async process(fhirPayload, recipientCode, operation) {
        console.log(this.protocolBasePath)
        console.log(this.protocolBasePath)
        console.log(this.protocolBasePath)
        const encryptedPayload = await this.encryptPayload(recipientCode, fhirPayload);
        const response = await this.initializeHCXCall(operation, encryptedPayload);
        return {
            payload: encryptedPayload,
            response
        };
    }
}

