# Managing Payload

To ensure that sensitive information in the actual domain objects is not accessible even to the HCX, the protocol requires the payload defined by the domain data specifications to be encrypted using an asymmetric encryption key of the final recipient established at the time of participant onboarding into the registry. The API then carries the encrypted value of the payload.

The payload encryption is expected to be performed using JSON web encryption[ RFC7516](https://datatracker.ietf.org/doc/html/rfc7516) as per the algorithm and enclosed values fixed in the protected headers section above. At this point in time, no unprotected headers are envisioned in the HCX Protocol. We also do not envision multiple recipient delivery in the current version of the HCX protocol.

More information about payload encryption/decryption is available in the [Message Security](https://docs.swasth.app/hcx-specifications/hcx-technical-specifications/open-protocol/data-security-and-privacy/message-security-and-integrity) section in the specifications.

The Payers/Providers system needs to perform the following steps when a payload is received or to create a payload. The HCX team has also developed a Mock Payer/Provider system which also performs the same steps as given below:
