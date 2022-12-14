package io.hcxprotocol.validator;

import ca.uhn.fhir.context.FhirContext;
import ca.uhn.fhir.context.support.DefaultProfileValidationSupport;
import ca.uhn.fhir.parser.IParser;
import ca.uhn.fhir.validation.FhirValidator;
import io.hcxprotocol.createresource.HCXInsurancePlan;
import org.hl7.fhir.common.hapi.validation.support.*;
import org.hl7.fhir.common.hapi.validation.validator.FhirInstanceValidator;
import org.hl7.fhir.r4.model.*;

import java.net.URL;

public class HCXFHIRValidator {

    private static HCXFHIRValidator instance = null;

    private FhirValidator validator = null;

    private HCXFHIRValidator() throws Exception {
        FhirContext fhirContext = FhirContext.forR4();
        fhirContext.setDefaultTypeForProfile("https://ig.hcxprotocol.io/v0.7/StructureDefinition-HCXInsurancePlan.html", HCXInsurancePlan.class);
        // Create a chain that will hold the validation modules
        System.out.println("we have started");
        ValidationSupportChain supportChain = new ValidationSupportChain();

        // DefaultProfileValidationSupport supplies base FHIR definitions. This is generally required
        // even if we are using custom profiles, since those profiles will derive from the base
        // definitions.
        DefaultProfileValidationSupport defaultSupport = new DefaultProfileValidationSupport(fhirContext);
        supportChain.addValidationSupport(defaultSupport);

        // This module supplies several code systems that are commonly used in validation
        supportChain.addValidationSupport(new CommonCodeSystemsTerminologyService(fhirContext));

        // This module implements terminology services for in-memory code validation
        supportChain.addValidationSupport(new InMemoryTerminologyServerValidationSupport(fhirContext));

        IParser parser = fhirContext.newJsonParser();
        String hcxIGBasePath = "https://ig.hcxprotocol.io/v0.7/";
        String nrcesIGBasePath = "https://nrces.in/ndhm/fhir/r4/";
        // test: load HL7 base definition
        //StructureDefinition sdCoverageEligibilityRequest = (StructureDefinition) parser.parseResource(new URL("http://hl7.org/fhir/R4/coverageeligibilityrequest.profile.json").openStream());
        StructureDefinition sdCoverageEligibilityRequestDoc = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-CoverageEligibilityRequestDocument.json").openStream());
        StructureDefinition sdCoverageEligibilityResponseDoc = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-CoverageEligibilityResponseDocument.json").openStream());
        StructureDefinition sdClaimRequestDoc = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-ClaimRequestDocument.json").openStream());
        StructureDefinition sdClaimResponseDoc = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-ClaimResponseDocument.json").openStream());
        StructureDefinition sdCommunicationDoc = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-CommunicationDocument.json").openStream());

        StructureDefinition sdCoverageEligibilityRequest = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-CoverageEligibilityRequest.json").openStream());
        StructureDefinition sdCoverageEligibilityResponse = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-CoverageEligibilityResponse.json").openStream());
        StructureDefinition sdClaim = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-Claim.json").openStream());
        StructureDefinition sdClaimResponse = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-ClaimResponse.json").openStream());
        StructureDefinition sdCommunicationRequest = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-CommunicationRequest.json").openStream());
        StructureDefinition sdCommunication = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-Communication.json").openStream());
        StructureDefinition sdPaymentReconciliation = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-PaymentReconciliation.json").openStream());
        StructureDefinition sdCoverage = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-Coverage.json").openStream());
        StructureDefinition sdPaymentNotice = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-PaymentNotice.json").openStream());
        StructureDefinition sdNRCESPatient = (StructureDefinition) parser.parseResource(new URL(nrcesIGBasePath + "StructureDefinition-Patient.json").openStream());
        StructureDefinition sdNRCESOrganization = (StructureDefinition) parser.parseResource(new URL(nrcesIGBasePath + "StructureDefinition-Organization.json").openStream());
        StructureDefinition sdNRCESPractitioner = (StructureDefinition) parser.parseResource(new URL(nrcesIGBasePath + "StructureDefinition-Practitioner.json").openStream());
        StructureDefinition sdInsurancePlan = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-HCXInsurancePlan.json").openStream());
        StructureDefinition sdHCXProofOfIdentificationExtension = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-HCXProofOfIdentificationExtension.json").openStream());
        StructureDefinition sdHCXProofOfPresenceExtension = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-HCXProofOfPresenceExtension.json").openStream());
        StructureDefinition sdHCXDiagnosticDocumentsExtension = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-HCXDiagnosticDocumentsExtension.json").openStream());
        StructureDefinition sdHCXInformationalMessagesExtension = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-HCXInformationalMessagesExtension.json").openStream());
        StructureDefinition sdHCXQuestionnairesExtension = (StructureDefinition) parser.parseResource(new URL(hcxIGBasePath + "StructureDefinition-HCXQuestionnairesExtension.json").openStream());
        StructureDefinition sdHCXCoverageEleComp = (StructureDefinition) parser.parseResource(new URL("https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-CoverageEligibilityRequestBundle.json").openStream());
        //adding the value sets
        ValueSet vsProofOfIdentity = (ValueSet) parser.parseResource(new URL(hcxIGBasePath + "ValueSet-proof-of-identity-codes.json").openStream());
        ValueSet vsProofOfPresence = (ValueSet) parser.parseResource(new URL(hcxIGBasePath + "ValueSet-proof-of-presence-codes.json").openStream());
        ValueSet vsClinicalDiagnostics = (ValueSet) parser.parseResource(new URL(hcxIGBasePath + "ValueSet-clinical-diagnostics-document-codes.json").openStream());
        ValueSet vsInformationalMessages = (ValueSet) parser.parseResource(new URL(hcxIGBasePath + "ValueSet-informational-messages-codes.json").openStream());


        // Create a PrePopulatedValidationSupport which can be used to load custom definitions.
        PrePopulatedValidationSupport prePopulatedSupport = new PrePopulatedValidationSupport(fhirContext);
        prePopulatedSupport.addStructureDefinition(sdCoverageEligibilityRequest);
        prePopulatedSupport.addStructureDefinition(sdCoverageEligibilityResponse);
        prePopulatedSupport.addStructureDefinition(sdClaimResponse);
        prePopulatedSupport.addStructureDefinition(sdCommunication);
        prePopulatedSupport.addStructureDefinition(sdCoverageEligibilityRequestDoc);
        prePopulatedSupport.addStructureDefinition(sdCoverageEligibilityResponseDoc);
        prePopulatedSupport.addStructureDefinition(sdClaimRequestDoc);
        prePopulatedSupport.addStructureDefinition(sdClaimResponseDoc);
        prePopulatedSupport.addStructureDefinition(sdCoverage);
        prePopulatedSupport.addStructureDefinition(sdPaymentNotice);
        prePopulatedSupport.addStructureDefinition(sdPaymentReconciliation);
        prePopulatedSupport.addStructureDefinition(sdClaim);
        prePopulatedSupport.addStructureDefinition(sdCommunicationRequest);
        prePopulatedSupport.addStructureDefinition(sdCommunicationDoc);
        prePopulatedSupport.addStructureDefinition(sdNRCESPatient);
        prePopulatedSupport.addStructureDefinition(sdNRCESOrganization);
        prePopulatedSupport.addStructureDefinition(sdNRCESPractitioner);
        prePopulatedSupport.addStructureDefinition(sdInsurancePlan);
        prePopulatedSupport.addStructureDefinition(sdHCXProofOfIdentificationExtension);
        prePopulatedSupport.addStructureDefinition(sdHCXProofOfPresenceExtension);
        prePopulatedSupport.addStructureDefinition(sdHCXDiagnosticDocumentsExtension);
        prePopulatedSupport.addStructureDefinition(sdHCXInformationalMessagesExtension);
        prePopulatedSupport.addStructureDefinition(sdHCXQuestionnairesExtension);
        prePopulatedSupport.addValueSet(vsClinicalDiagnostics);
        prePopulatedSupport.addValueSet(vsInformationalMessages);
        prePopulatedSupport.addValueSet(vsProofOfPresence);
        prePopulatedSupport.addValueSet(vsProofOfIdentity);
        prePopulatedSupport.addStructureDefinition(sdHCXCoverageEleComp);

        // Add the custom definitions to the chain
        supportChain.addValidationSupport(prePopulatedSupport);
        CachingValidationSupport cache = new CachingValidationSupport(supportChain);

        // Create a validator using the FhirInstanceValidator module.
        FhirInstanceValidator validatorModule = new FhirInstanceValidator(cache);
        this.validator = fhirContext.newValidator().registerValidatorModule(validatorModule);
    }

    private static HCXFHIRValidator getInstance() throws Exception {
        if (null == instance) 
            instance = new HCXFHIRValidator();

        return instance;
    }

    public static FhirValidator getValidator() throws Exception {
        return getInstance().validator;
    }

}
