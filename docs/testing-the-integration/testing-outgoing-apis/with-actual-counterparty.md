# With actual counterparty

Configure the Sandbox environment base path to send the Outgoing API call to test and validate the API requests. Please follow the below steps to create the request payload for any counterparty in the sandbox environment.

* Use the username and password to generate the API access token.
* Call the Participant list API to get the details of the required participant.
* Use the selected participant public key to encrypt and generate the request payload as per the HCX Protocol API Specification.
* Make the API call to the Sandbox environment by providing the x-hcx\_sender\_code as the selected participant id.
