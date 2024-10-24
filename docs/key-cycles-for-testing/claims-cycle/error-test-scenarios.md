# Error test scenarios

1. Error handling scenarios mentioned in the HCX protocol tested in the context of pre-auth cycle. Key scenarios in this section:&#x20;

* Invalid/Inactive HCX ID for both sender and receiver
* Missing mandatory header information like API call ID, correlation ID, timestamp, etc.
* Invalid JWE token for decryption
* Invalid or wrong domain payload

More error testing scenarios are listed [here in the HCX protocol](https://docs.hcxprotocol.io/hcx-technical-specifications/open-protocol/key-components-building-blocks/error-descriptions).
