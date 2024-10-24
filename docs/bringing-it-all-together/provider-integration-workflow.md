# Provider Integration Workflow

1. Onboarding: Providers complete the HCX onboarding process, acquiring necessary credentials and establishing secure access to the platform.
2. Check Coverage Eligibility:

* Providers use the **Check Eligibility API** (`/coverageeligibility/check`) to query the payer about a patient's coverage details for specific services.
* The payer responds through the **Coverage Eligibility Callback API** (`/coverageeligibility/on_check`) to inform the provider of the patient's coverage status.

3. Pre-Authorization (Optional):

* Providers can submit a pre-authorization request via the **Pre-authorization API** (`/preauth/submit`) seeking approval for specific services before rendering them.
* The payer responds through the **Pre-authorization Callback API** (`/preauth/on_submit`) to communicate the pre-authorization decision (approval or denial) to the provider.

4. Claim Submission:

* Providers submit the actual claim for processed services through the **Claim API** (`/claim/submit`).
* The payer acknowledges receipt and processes the claim.
* The payer responds through the **Claim Callback API** (`/claim/on_submit`) to inform the provider of the claim status (e.g., accepted, pending, or denied).

5.  Communications:

    This API is useful in enabling the additional message flows in HCX. Communications API will be used for communications between multiple participants in the HCX ecosystem.

    * Please refer to the [Communications APIs documentation](https://docs.hcxprotocol.io/hcx-technical-specifications/open-protocol/key-components-building-blocks/api-structure/supporting-apis#communication-apis)

\
