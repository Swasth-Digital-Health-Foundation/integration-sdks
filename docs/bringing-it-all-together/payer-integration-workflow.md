# Payer Integration Workflow

1. **Onboarding**: Payers complete the HCX onboarding process to receive and respond to claim-related requests from providers.
2. **Eligibility Check Response**:

* Payers receive eligibility check requests from providers through the **Coverage Eligibility Callback API** (`/coverageeligibility/on_check`).
* Payers review the request and respond to the provider using the same API to confirm or deny the patient's coverage for the requested service(s).

3. **Pre-authorization Response**:

* Payers receive pre-authorization requests from providers through the **Pre-authorization Callback API** (`/preauth/on_submit`).
* Payers review the request and respond using the same API to approve or deny the pre-authorization request, based on established criteria.

4. **Claim Processing**:

* Payers receive claim submissions from providers through the **Claim API** (`/claim/on_submit`).
* Payers process the claim, verifying details and ensuring adherence to policies and regulations.
* Payers communicate the claim status (approved, denied, or pending) back to the provider through the same API.

5. **Payment Notification**:

* Upon claim approval and settlement, payers initiate a payment notification to the provider through the **Payment Notification API** (`/paymentnotice/request`).
* Payers receive an acknowledgement from the provider through the **Payment Notification Callback API** (`/paymentnotice/on_request`) confirming receipt of the notification.
