class HCXFHIRValidator {
    constructor(hcxIGBasePath, nrcesIGBasePath) {
        this.fhirContext = FhirContext.forR4();
        this.supportChain = new ValidationSupportChain();

        this.downloadAndExtractZip("nrces_definitions", nrcesIGBasePath, "nrces_definitions.zip");
        this.downloadAndExtractZip("hcx_definitions", hcxIGBasePath, "hcx_definitions.zip");

        this.supportChain.addValidationSupport(new DefaultProfileValidationSupport(this.fhirContext));
        this.supportChain.addValidationSupport(new CommonCodeSystemsTerminologyService(this.fhirContext));
        this.supportChain.addValidationSupport(new InMemoryTerminologyServerValidationSupport(this.fhirContext));

        this.loadProfiles("nrces_definitions");
        this.loadProfiles("hcx_definitions");

        this.validator = this.fhirContext.newValidator();
    }

    static getInstance(hcxIGBasePath, nrcesIGBasePath) {
        if (!this.instance) {
            this.instance = new HCXFHIRValidator(hcxIGBasePath, nrcesIGBasePath);
        }
        return this.instance;
    }

    static getValidator(config) {
        return HCXFHIRValidator.getInstance(config.HCX_IG_BASE_PATH, config.NRCES_IG_BASE_PATH).validator;
    }
}

class FhirPayload {
    validateFHIR(fhirPayload, operation, error, config) {
        let returnBool = true;
        try {
            let validator = HCXFHIRValidator.getValidator(config);
            let result = validator.validateWithResult(fhirPayload);
            let messages = result.getMessages();
            let map = JSON.parse(fhirPayload);
            if (map.resourceType !== operation.getFhirResourceType()) {
                error[ErrorCodes.ERR_WRONG_DOMAIN_PAYLOAD] = "Incorrect eObject is sent as the domain payload";
                return false;
            }
            let errMessages = [];
            for (let message of messages) {
                if (message.getSeverity() === "ERROR") {
                    errMessages.push(message.getMessage());
                    error[ErrorCodes.ERR_INVALID_DOMAIN_PAYLOAD] = errMessages;
                    returnBool = false;
                }
            }
        } catch (e) {
            error[ErrorCodes.ERR_INVALID_DOMAIN_PAYLOAD] = e.getMessage();
        }
        return returnBool;
    }
}
