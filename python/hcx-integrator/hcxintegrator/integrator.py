from hcxintegrator.outgoing_request import OutgoingRequest
from hcxintegrator.incoming_request import IncomingRequest
from hcxintegrator.utils.hcx_operations import HcxOperations

class HCXIntegrator:

    def __init__(self, config) -> None:
        self.config = config
        self.protocolBasePath = None
        self.participantCode = None
        self.authBasePath = None
        self.username = None
        self.password = None
        self.encryptionPrivateKeyURL = None
        self.igURL = None
        self.validateConfig()
    
    def validateConfig(self):
        # TODO: check for all contants/attributes needed
        # set all the internal variables
        self.protocolBasePath = self.config.get("protocolBasePath")
        self.participantCode = self.config.get("participantCode")
        self.authBasePath = self.config.get("authBasePath")
        self.username = self.config.get("username")
        self.password = self.config.get("password")
        self.encryptionPrivateKeyURL = self.config.get("encryptionPrivateKeyURL")
        self.igURL = self.config.get("igURL")

    def processOutgoing(self, fhirPayload: str, recipientCode: str, operation: HcxOperations,
                        apiCallId=None, correlationId=None, actionJWE=None, onActionStatus=None, domainHeaders=None):
        """
        Processes Outgoing HCX request, validates payload and headers.
        
        Parameters
        ----------
        fhirPayload : str
            FHIR payload to be sent in outgoing request
        recipientCode : str
            Recipient code 
        operation : HcxOperations
            Operation type from utils.HcxOperations (default=None)
        apiCallId : str, None
            Unique API call id, generated if None
        correlationId : str, None
            Unique correlation id of the api loop
        actionJwe : dict, str, None
            Action jwe, if provided recipient code should be None
        onActionStatus : str, None
        domainHeaders : str, None
        
        Returns
        -------
        response: dict
            Response for the given payload.
        """
        outgoing = OutgoingRequest(self.protocolBasePath, self.participantCode,
                                   self.authBasePath, self.username,
                                   self.password, self.encryptionPrivateKeyURL,
                                   self.igURL)
        response = outgoing.process(fhirPayload, recipientCode, operation,
                                    apiCallId, correlationId, actionJWE, onActionStatus, domainHeaders)
        return response
    
    def processIncoming(self, encryptedPaylaod:str, operation:HcxOperations=None):
        """
        Processes Incoming HCX request, validates payload and headers.
        
        Parameters
        ----------
        encryptedPayload : str
            Encrypted response from outgoing request
        operation : HcxOperations
            Operation type from utils.HcxOperations (default=None)
        
        Returns
        -------
        response: dict
            Response for the given payload.
        """
        incoming = IncomingRequest(self.protocolBasePath, self.participantCode,
                                   self.authBasePath, self.username,
                                   self.password, self.encryptionPrivateKeyURL,
                                   self.igURL)
        resposne = incoming.process(encryptedPaylaod, operation=operation)
        return resposne