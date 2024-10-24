# With mock counterparty

Then the integration system enables the APIs as per the HCX Specification to the callbacks for the requests. Swasth's sandbox environment has a Mock service to help with the integration testing and validation.

While making the API call to Mock service, include the below test headers to define and get the expected object in the incoming API.

Below are the test headers defined for the Protocol headers to expect the given input in callback API.

* `x-hcx-debug_flag_test for x-hcx-debug_flag`
* `x-hcx-status_test for x-hcx-status`
* `x-hcx-error_details_test for x-hcx-error_details`
* `x-hcx-debug_details_test for X-hcx-debug_details`
* `x-hcx-test_random - This header will be used to simulate a random response.`
  * The accepted value for this header is true or false.

Once all the possible scenarios are tested and validated with mock, use the below section details to test it with the actual counterparty.
