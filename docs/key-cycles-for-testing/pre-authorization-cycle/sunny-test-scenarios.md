# Sunny test scenarios

1. Scenario 1 : Provider submits the pre-auth request using /preauth/submit for the beneficiaries insurance plan and gets the positive approval response from the payor through HCX.
2. Scenario 2: Provider submits the pre-auth request using /preauth/submit for the beneficiaries insurance plan and gets the negative/rejection response from the payer through HCX.
3. Scenario 3 : Provider submits the pre-auth request using /preaut/submit for the beneficiaries insurance plan and gets the partial response (Status - .partial) from the payer through HCX. The response will have the tracking number and partial adjudication status of the pre-auth request.
4. Partial approval : Provider receives the partial approval for the pre-auth amount requested to the payer.
5. Scenario 4 (Enhancement) : Provider is enhancing the pre-auth request, adding an reference identifier of the previous request to the new request and sending it to the HCX for approval. Payer acknowledges the enhanced pre-auth request, processes, and responds to the provider through HCX.
