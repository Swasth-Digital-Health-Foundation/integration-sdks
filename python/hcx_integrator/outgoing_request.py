from jwcrypto import jwk, jwe 
from urllib.parse import urlencode
import requests
import uuid
import json
import datetime
from utils.hcx_operations import HcxOperations
from utils.utils import generateHcxToken, searchRegistry


class OutgoingRequest:

    def __init__(self, protocolBasePath, participantCode,
                 authBasePath, username, password,
                 encryptionPrivateKeyURL, igURL):
        self.protocolBasePath = protocolBasePath
        self.participantCode = participantCode
        self.authBasePath = authBasePath
        self.username = username
        self.password = password
        self.encryptionPrivateKeyURL = encryptionPrivateKeyURL  # not needed in outgoing
        self.igURL = igURL
        self.hcxToken = None


    def validatePayload(fhirPayload, operation):
        # TODO: to be implemented
        return True


    def createHeaders(self, recipientCode=None):
        hcx_headers = {
                    "alg":"RSA-OAEP",
                    "enc":"A256GCM",
                    "x-hcx-recipient_code": recipientCode,
                    "x-hcx-timestamp": datetime.datetime.now().astimezone().replace(microsecond=0).isoformat(),
                    "x-hcx-sender_code": self.participantCode,
                    "x-hcx-correlation_id": str(uuid.uuid4()),
                    "x-hcx-workflow_id": str(uuid.uuid4()),
                    "x-hcx-api_call_id": str(uuid.uuid4())
                    }
        return hcx_headers


    def encryptPayload(self, recipientCode=None, fhirPayload=None):
        if recipientCode is None:
            raise ValueError("Recipient code can not be empty, must be a string")
        if type(fhirPayload) is not dict:
            raise ValueError("Fhir paylaod must be a dictionary")
        if self.hcxToken is None:
            self.hcxToken = generateHcxToken(self.authBasePath, self.username, self.password)
        regsitry_data = searchRegistry(self.protocolBasePath, self.hcxToken,
                                       searchField = "participant_code", searchValue = recipientCode)       
        public_cert = requests.get(regsitry_data["participants"][0]["encryption_cert"], verify=False)  # verify=False handels firewall off case
        key = jwk.JWK.from_pem(public_cert.text.encode('utf-8')) 
        headers = self.createHeaders(recipientCode)
        jwePayload = jwe.JWE(str(json.dumps(fhirPayload)),recipient=key,protected=json.dumps(headers))
        enc = jwePayload.serialize(compact=True)
        return enc


    def initializeHCXCall(self, operation, encryptedJWE):
        url = "".join(self.protocolBasePath + operation.value)
        print("making the API call to url " + url)
        if self.hcxToken is None:
            self.hcxToken = generateHcxToken(self.authBasePath, self.username, self.password)
        payload = json.dumps({
            "payload": encryptedJWE
        })
        headers = {
            'Authorization': 'Bearer ' + self.hcxToken,
            'Content-Type': 'application/json'
        }
        try:
            response = requests.request("POST", url, headers=headers, data=payload)
            return dict(json.loads(response.text))
        except Exception as e:
            print(f'Initialise HCX: {e}')

    
    def process(self, fhirPayload, recipientCode, operation:HcxOperations):
        # self.validatePayload(fhirPayload, operation)
        encryptedPaylaod = self.encryptPayload(recipientCode, fhirPayload)
        response = self.initializeHCXCall(operation, encryptedPaylaod)
        return {
            "paylaod": encryptedPaylaod,
            "response": response
        }