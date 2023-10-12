import os, inspect
from hcxintegrator.integrator import HCXIntegrator
from hcxintegrator.utils.hcx_operations import HcxOperations
from hcxintegrator.utils.utils import compare_output
import json

curdir = os.path.dirname(os.path.abspath(inspect.getfile(inspect.currentframe())))

config = {
    "participantCode": "testprovider1.swasthmock@swasth-hcx-staging",
    "authBasePath": "http://staging-hcx.swasth.app/api/v0.8/participant/auth/token/generate",
    "protocolBasePath": "https://staging-hcx.swasth.app/api/v0.8",
    "encryptionPrivateKeyURL": "https://raw.githubusercontent.com/Swasth-Digital-Health-Foundation/hcx-platform/main/hcx-apis/src/test/resources/examples/x509-private-key.pem",
    "username": "testprovider1@swasthmock.com",
    "password": "Opensaber@123",
    "igUrl": "https://ig.hcxprotocol.io/v0.7.1"}

with open(os.path.join(curdir, "response_outgoing.json"), "r") as outfile:
    outgoing = outfile.read()
outgoing = json.loads(outgoing)

# Initialising the HCX integrator
hcxIntegrator = HCXIntegrator(config=config)

# Incoming process call
incoming = hcxIntegrator.processIncoming(outgoing, operation=HcxOperations.CLAIM_SUBMIT)
# print(incoming)

try:
    with open(os.path.join(curdir, "response_incoming.json"), "r") as f:
        pretest = json.load(f)
    if compare_output(incoming, pretest):
        print(f'Test incoming successful!')
except Exception as e:
    print(f'Test failed! Following exception occured: {e}')
