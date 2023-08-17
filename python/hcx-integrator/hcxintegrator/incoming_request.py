from jwcrypto import jwk, jwe 
from urllib.parse import urlencode
import requests
import uuid
import json
import datetime
from hcxintegrator.utils.hcx_operations import HcxOperations
from hcxintegrator.utils.utils import generateHcxToken, searchRegistry
from hcxintegrator.utils.validator import validateJWERequest, validateJsonRequest
from hcxintegrator.utils.constants import Constants
from hcxintegrator.utils.errors import ErrorCodes, ResponseMessage
import logging


class IncomingRequest:

    def __init__(self, protocolBasePath, participantCode,
                 authBasePath, username, password,
                 encryptionPrivateKeyURL, igURL):
        self.protocolBasePath = protocolBasePath
        self.participantCode = participantCode
        self.authBasePath = authBasePath
        self.username = username
        self.password = password
        self.encryptionPrivateKeyURL = encryptionPrivateKeyURL
        self.igURL = igURL
        self.headers = None
        self.payload = None
        self.error = {}
        self.output = {}

    def validateRequest(self, jwePayload, operation):
        if not isinstance(jwePayload, dict): 
            if "payload" in jwePayload:
                return validateJWERequest(operation, jwePayload)
            else:
                if not operation.startswith("ON_"):
                    self.error[ErrorCodes.ERR_INVALID_PAYLOAD] = ResponseMessage.INVALID_JSON_REQUEST_BODY_ERR_MSG
                    return False
                if not validateJsonRequest(operation, self.error, jwePayload):
                    return False

    def decryptPayload(self, jwePayload):
        if self.encryptionPrivateKeyURL.startswith("https://"):
            privateKey = requests.get(self.encryptionPrivateKeyURL, verify=False)
            privateKey = jwk.JWK.from_pem(privateKey.text.encode('utf-8'))
        else:
            privateKey = jwk.JWK.from_pem(self.encryptionPrivateKeyURL.encode('utf-8'))
        jwe_token = jwe.JWE()
        jwe_token.deserialize(jwePayload["payload"], key=privateKey)
        jwe_token.decrypt(key=privateKey)
        self.headers = jwe_token.jose_header
        self.payload = jwe_token.payload
        return jwe_token
            

    def validatePaylaod(self, fhirPayload, operation):
        #  TODO: fhir validation to be implemented
        return True

    def send_response(self):
        # output = json.loads(output)
        response_obj = {}
        presentDate = datetime.datetime.now()
        unix_timestamp = datetime.datetime.timestamp(presentDate)*1000
        response_obj[Constants.TIMESTAMP] = int(unix_timestamp)
        result = False

        if not self.error:
            headers = self.output.get(Constants.HEADERS)
            response_obj[Constants.API_CALL_ID] = headers.get(Constants.HCX_API_CALL_ID)
            response_obj[Constants.CORRELATION_ID] = headers.get(Constants.HCX_CORRELATION_ID)
            logging.info(f'Processing incoming request has completed :: response: {response_obj}')
            result = True
        else:
            logging.error(f'Error while processing the request: {self.error}')
            # Fetching only the first error and constructing the error object 
            code = self.error.keys()[0]
            message = self.error.values()[0]
            response_obj[Constants.ERROR] = json.dumps({
                code: message
            })

        self.output[Constants.RESPONSE_OBJ] = response_obj
        return result

    def process(self, payload, operation):
        error = {}
        result = False
        logging.info(f'Processing incoming request has started :: operation: {operation}')
        self.validateRequest(payload, operation)
        decrypted_paylaod = self.decryptPayload(payload)
        header = decrypted_paylaod.jose_header
        payload = json.loads(decrypted_paylaod.payload.decode('UTF-8'))
        self.output[Constants.HEADERS] = header
        self.output[Constants.PAYLOAD] = payload
        result = self.send_response()
        print(result)
        return self.output