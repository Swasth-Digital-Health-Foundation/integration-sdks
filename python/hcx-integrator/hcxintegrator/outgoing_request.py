from jwcrypto import jwk, jwe 
from urllib.parse import urlencode
import logging
import base64
import requests
import uuid
import json
import datetime
from hcxintegrator.utils.hcx_operations import HcxOperations
from hcxintegrator.utils.utils import generateHcxToken, searchRegistry
from hcxintegrator.utils.constants import Constants


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
        self.recipientCode = None


    def validatePayload(self, fhirPayload, operation):
        # TODO: to be implemented
        return True


    def createHeaders(self, recipientCode=None, apiCallId=None, correlationId=None, actionJwe=None,
                      onActionStatus=None, headers:dict={}):
        if headers is None:
            headers = {}
        headers[Constants.ALG] = "RSA-OAEP"
        headers[Constants.ENC] = "A256GCM"
        apiCallId = apiCallId if apiCallId else str(uuid.uuid4())
        headers[Constants.HCX_API_CALL_ID] = apiCallId
        timestamp = datetime.datetime.now().astimezone().replace(microsecond=0).isoformat()
        headers[Constants.HCX_TIMESTAMP] = timestamp
        
        if recipientCode is not None:
            self.recipientCode = recipientCode
            headers[Constants.HCX_SENDER_CODE] = self.participantCode
            headers[Constants.HCX_RECIPIENT_CODE] = recipientCode
            headers[Constants.HCX_CORRELATION_ID] = correlationId if correlationId else str(uuid.uuid4())
        else:
            actionHeaders = actionJwe.split(".")[0]
            actionHeaders = actionHeaders +  '=' * (-len(actionHeaders) % 4)
            print(actionHeaders)
            actionHeaders = base64.b64decode(actionHeaders, validate=False)
            actionHeaders = json.loads(actionHeaders.decode('utf-8'))
            headers[Constants.HCX_SENDER_CODE] = actionHeaders.get(Constants.HCX_SENDER_CODE)
            self.recipientCode = actionHeaders.get(Constants.HCX_RECIPIENT_CODE)
            headers[Constants.HCX_RECIPIENT_CODE] = self.recipientCode
            headers[Constants.CORRELATION_ID] = actionHeaders.get(Constants.CORRELATION_ID)
            headers[Constants.STATUS] = onActionStatus
            print(headers)
            if Constants.WORKFLOW_ID in headers:
                headers[Constants.WORKFLOW_ID] = actionHeaders.get(Constants.WORKFLOW_ID)
        return headers


    def encryptPayload(self,  headers=None, recipientCode=None, fhirPayload=None):
        if self.recipientCode is None:
            raise ValueError("Recipient code can not be empty, must be a string")
        if type(fhirPayload) is not dict:
            raise ValueError("Fhir paylaod must be a dictionary")
        if self.hcxToken is None:
            self.hcxToken = generateHcxToken(self.authBasePath, self.username, self.password)
        regsitry_data = searchRegistry(self.protocolBasePath, self.hcxToken,
                                       searchField = "participant_code", searchValue = self.recipientCode)
        encryption_cert = regsitry_data["participants"][0]["encryption_cert"]
        print(f'Inside encrypt payload: {encryption_cert}')
        requests.packages.urllib3.disable_warnings()   
        public_cert = requests.get(encryption_cert, verify=False)  # verify=False handels firewall off case
        print("get the certificate")
        key = jwk.JWK.from_pem(public_cert.text.encode('utf-8')) 
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

    
    def process(self, fhirPayload, recipientCode, operation:HcxOperations,
                apiCallId=None, correlationId=None, actionJWE=None, onActionStatus=None, domainHeaders=None):
        # self.validatePayload(fhirPayload, operation)
        headers = self.createHeaders(recipientCode, apiCallId, correlationId, actionJWE, onActionStatus, domainHeaders)
        encryptedPaylaod = self.encryptPayload(headers, recipientCode, fhirPayload)
        response = self.initializeHCXCall(operation, encryptedPaylaod)
        return {
            "payload": encryptedPaylaod,
            "response": response
        }