# Error test scenarios

1. Error handling scenarios mentioned in the HCX protocoltested in the context of coverage eligibility cycle. Key scenarios in this section:&#x20;

* Invalid/Inactive HCX ID for both sender and receiver
* Missing mandatory header information like API call ID, correlation ID, timestamp, etc.
* Invalid JWE token for decryption
* Invalid or wrong domain payload

1. **Consent fails:** If the provider consent fails, then the payer resends the consent token for claim verification [as discussed here](https://docs.google.com/document/d/1z9hRQZ3X0a7\_B22mYQhSPVn5d10vk8iEn58r0fR-tlo/edit#heading=h.tx49wmixofzg).
2. **Consent fails:** If the provider consent fails, then the payer rejects the claim request and sends the claim adjudication status using /claim/on\_submit.

More error testing scenarios are listed [here in the HCX protocol](https://docs.hcxprotocol.io/hcx-technical-specifications/open-protocol/key-components-building-blocks/error-descriptions).
