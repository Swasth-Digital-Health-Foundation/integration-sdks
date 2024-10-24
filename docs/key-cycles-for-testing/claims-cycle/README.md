# Claims cycle

Providers and payers use claims API pair (/claim/submit and /claim/on\_submit) to submit claim requests (and resubmit updated request) to HCX gateway, for HCX gateway to route the same request to payor, and get the claim response to the providers.

The request payload for this request will be created as per the [Claim Request Bundle](https://ig.hcxprotocol.io/v0.8/StructureDefinition-ClaimRequestBundle.html) defined in HCX specification and serialised as per the guidelines in HCX Specifications.
