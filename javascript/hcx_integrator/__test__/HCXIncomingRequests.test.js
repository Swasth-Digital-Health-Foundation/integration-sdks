/*const HCXIncomingRequest = require('./HCXIncomingRequest');

describe('HCXIncomingRequest', function() {
  let hcxIncomingRequest;

  beforeEach(function() {
    hcxIncomingRequest = new HCXIncomingRequest();
    jest.spyOn(hcxIncomingRequest, 'getPayload').mockImplementation(() => Promise.resolve("dummyPayload"));
    jest.spyOn(hcxIncomingRequest, 'validateRequest').mockImplementation(() => true);
    jest.spyOn(hcxIncomingRequest, 'decryptPayload').mockImplementation(() => Promise.resolve(true));
    jest.spyOn(hcxIncomingRequest, 'validatePayload').mockImplementation(() => true);
    jest.spyOn(hcxIncomingRequest, 'sendResponse').mockImplementation(() => Promise.resolve(true));
  });

  it('should process the incoming request successfully', async function() {
    const jwePayload = "dummyPayload";
    const operation = "dummyOperation";
    const output = {};
    const config = {
      ENCRYPTION_PRIVATE_KEY: "dummyPrivateKey",
      FHIR_VALIDATION_ENABLED: true,
      HCX_API_CALL_ID: "dummyApiCallId",
      HCX_CORRELATION_ID: "dummyCorrelationId"
    };

    const result = await hcxIncomingRequest.process(jwePayload, operation, output, config);

    expect(result).toBe(true);
    expect(output).toHaveProperty('RESPONSE_OBJ');
  });
});
*/