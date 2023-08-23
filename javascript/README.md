# HCX Integrator SDK
An SDK for integrating with HCX protocol.

### Installation
To install the hcx-integrator-sdk package, run:
```
npm install hcx-integrator-sdk
```

### Usage:
To initialize the SDK
```
import { HCXIntegrator, HcxOperations } from 'hcx-integrator-sdk';

const config = {
    // Add your configurations here
};

const hcxIntegrator = new HCXIntegrator(config);
```
### Submitting a Claim

```
const fhirPayload = {
    // Add the FHIR Payload here
};

const operation = HcxOperations.CLAIM_SUBMIT;

async function submitClaim() {
    const responseOutgoing = await hcxIntegrator.processOutgoing(fhirPayload, "", operation);
    const responseIncoming = await hcxIntegrator.processIncoming(responseOutgoing.payload, operation);

    console.log(responseOutgoing);
    console.log(responseIncoming);
}

submitClaim();
```
*Kindly note that this is a sample code. Actual implementation might differ based on the use casse.*

### License
This project is licensed under the MIT License.
