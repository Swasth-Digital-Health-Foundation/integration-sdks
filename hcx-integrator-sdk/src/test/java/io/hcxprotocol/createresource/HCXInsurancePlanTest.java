package io.hcxprotocol.createresource;

import ca.uhn.fhir.context.FhirContext;
import ca.uhn.fhir.parser.IParser;
import ca.uhn.fhir.validation.FhirValidator;
import ca.uhn.fhir.validation.SingleValidationMessage;
import ca.uhn.fhir.validation.ValidationResult;
import io.hcxprotocol.validator.HCXFHIRValidator;
import org.hl7.fhir.instance.model.api.IBaseResource;
import org.hl7.fhir.r4.model.*;
import org.junit.jupiter.api.Test;

import java.util.Collections;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;

class HCXInsurancePlanTest {

    @Test
    void validateInsurancePlanObject() throws Exception {
        FhirValidator validator = HCXFHIRValidator.getValidator();


        HCXInsurancePlan ip = createInsurancePlan();
        printFHIRObject(ip);

        ValidationResult result = validator.validateWithResult(ip);
        assertTrue(result.isSuccessful());

        List<SingleValidationMessage> messages = result.getMessages();
        assertTrue(messages.size() == 4);
        messages.forEach(message -> System.out.println(message.getSeverity() + " -- " + message.getLocationString() + " -- " + message.getMessage()));
    }

    private void printFHIRObject(IBaseResource ip) {
        IParser p = FhirContext.forR4().newJsonParser().setPrettyPrint(true);
        String message = p.encodeResourceToString(ip);
    }

    private HCXInsurancePlan createInsurancePlan() {
        //creating a new HCXInsurancePlan object and validating it against the HCX SD
        HCXInsurancePlan ip = new HCXInsurancePlan("AB_GUJ_Plan1", Enumerations.PublicationStatus.ACTIVE, "PMJAY-Mukhyamantri Amrutam & Mukhyamantri Vatsalya");

        //Creating the Identification Extension Object
        HCXInsurancePlan.IdentificationExtension idExt = new HCXInsurancePlan.IdentificationExtension();
        idExt.setProofOfIdDocumentCode("12345", "Aadhar Card");
        idExt.setClinicalDiagnosticDocumentClaimUse("preauthorization");
        idExt.getProofOfIdDocumentCode();
        idExt.getProofOfIdentificationDocumentMimeType();
        idExt.setProofOfIdentificationDocumentMimeType(new CodeType());
        idExt.getProofOfIdDocumentCode();
        idExt.isProofOfIdentificationDocumentRequiredFlag();
        idExt.setProofOfIdentificationDocumentRequiredFlag(new BooleanType());
        idExt.getClinicalDiagnosticDocumentClaimUse();
        idExt.getDocumentationUrl();


        //Creating Presence Extension Object
        HCXInsurancePlan.PresenceExtension preExt = new HCXInsurancePlan.PresenceExtension();
        preExt.setProofOfPresenceDocumentCode("12345", "Aadhar Verification XML");
        preExt.setClinicalDiagnosticDocumentClaimUse("preauthorization");
        preExt.getProofOfPresenceDocumentCode();
        preExt.isProofOfPresenceDocumentRequiredFlag();
        preExt.setProofOfPresenceDocumentRequiredFlag(new BooleanType());
        preExt.getProofOfPresenceDocumentMimeType();
        preExt.setProofOfPresenceDocumentMimeType(new CodeType());
        preExt.getClinicalDiagnosticDocumentClaimUse();
        preExt.getDocumentationUrl();


        //Adding the above extensions to Insurance Plan Object
        HCXInsurancePlan.InsurancePlanPlanComponent plan = new HCXInsurancePlan.InsurancePlanPlanComponent();
        plan.setIdentificationExtension(idExt);
        plan.setPresenceExtension(preExt);
        plan.getIdentificationExtension();
        plan.getPresenceExtension();
        plan.setType("PMJAY_GUJ_GOLD_CARD");

        //specific cost
        InsurancePlan.InsurancePlanPlanSpecificCostComponent spc = new InsurancePlan.InsurancePlanPlanSpecificCostComponent();
        spc.setCategory(new CodeableConcept().setCoding(Collections.singletonList(new Coding().setCode("Inpatient-packages").setSystem("http://terminologyServer/ValueSets/cost-category"))));

        //specific cost benefit component
        HCXInsurancePlan.PlanBenefitComponent pbf = new HCXInsurancePlan.PlanBenefitComponent();
        pbf.setType(new CodeableConcept().setCoding(Collections.singletonList(new Coding().setCode("HBP_PACKAGE_00003").setSystem("http://terminologyServer/ValueSets/packages"))));
        pbf.getDiagnosticDocumentsExtension();
        pbf.getInformationalMessagesExtension();
        pbf.getQuestionnairesExtension();


        //adding extensions to benefit component
        HCXInsurancePlan.DiagnosticDocumentsExtension dde = new HCXInsurancePlan.DiagnosticDocumentsExtension();
        dde.setClinicalDiagnosticDocumentCode(new CodeableConcept(new Coding().setSystem("https://hcx-valuesets/proofOfIdentificationDocumentCodes").setCode("MAND0001").setVersion("1.0.0").setDisplay("Post Treatment clinical photograph")));
        dde.setDocumentationUrl(new UrlType("http://documentation-url"));
        dde.setClinicalDiagnosticDocumentClaimUse(new CodeType("claim"));
        dde.getClinicalDiagnosticDocumentCode();
        dde.getClinicalDiagnosticDocumentRequiredFlag();
        dde.setClinicalDiagnosticDocumentRequiredFlag(new BooleanType());
        dde.setClinicalDiagnosticDocumentMimeType(new CodeType());
        dde.setClinicalDiagnosticDocumentMimeType(new CodeType());
        dde.getClinicalDiagnosticDocumentMimeType();
        dde.getClinicalDiagnosticDocumentClaimUse();
        dde.getDocumentationUrl();


        HCXInsurancePlan.InformationalMessagesExtension ime = new HCXInsurancePlan.InformationalMessagesExtension();
        ime.setInformationalMessagesCode(new CodeableConcept(new Coding().setSystem("https://hcx-valuesets/InformationalMessagesCodes").setCode("12343").setVersion("1.0.0").setDisplay("Information Message 1")));
        ime.setDocumentationUrl(new UrlType("http://documntation-url"));
        ime.setInformationalMessageClaimUse(new CodeType("claim"));
        ime.getDocumentationUrl();
        ime.getInformationalMessageCode();
        ime.setInformationalMessageCode(new BooleanType());
        ime.getInformationalMessageClaimUse();
        ime.getInformationalMessageCode();
        ime.getInformationalMessagesCode();


        HCXInsurancePlan.QuestionnairesExtension qe = new HCXInsurancePlan.QuestionnairesExtension();
        qe.setQuestionnaire(new Reference("Questionnnaire/1"));
        qe.setDocumentationUrl(new UrlType("http://documentation-url"));
        qe.setQuestionnaireClaimUse(new CodeType("claim"));
        qe.getQuestionnaire();
        qe.getQuestionnaireRequiredFlag();
        qe.getQuestionnaireClaimUse();
        qe.setQuestionnaireRequiredFlag(new BooleanType());
        qe.getDocumentationUrl();


        pbf.setDiagnosticDocumentsExtension(dde);
        pbf.setInformationalMessagesExtension(ime);
        pbf.setQuestionnairesExtension(qe);


        InsurancePlan.PlanBenefitCostComponent pbc = new InsurancePlan.PlanBenefitCostComponent();
        pbc.setType(new CodeableConcept().setCoding(Collections.singletonList(new Coding().setCode("hospitalization").setSystem("http://terminologyServer/ValueSets/pacakgeCostTypes"))));

        pbf.addCost(pbc);
        spc.addBenefit(pbf);

        plan.addSpecificCost(spc);
        ip.addPlan(plan);
        return ip;
    }

    @Test
    void insurancePlanPlanComponsentTest() {
        HCXInsurancePlan.InsurancePlanPlanComponent plan = new HCXInsurancePlan.InsurancePlanPlanComponent();
        boolean isValid = plan.isEmpty();
        assertFalse(isValid);
    }

    @Test
    void identificationExtensionTest() {
        HCXInsurancePlan.IdentificationExtension idExt = new HCXInsurancePlan.IdentificationExtension();
        // copy method
        HCXInsurancePlan.IdentificationExtension copyIdeExt = idExt.copy();
        boolean isValid = idExt.isEmpty();
        assertFalse(isValid);
        assert (idExt.toString() != copyIdeExt.toString());

    }

    @Test
    void presenceExtensionTest() {
        HCXInsurancePlan.PresenceExtension preExt = new HCXInsurancePlan.PresenceExtension();
        // copy method
        HCXInsurancePlan.PresenceExtension copyPreExt = preExt.copy();
        boolean isValid = preExt.isEmpty();
        assertFalse(isValid);
        assert (preExt.toString() != copyPreExt.toString());


    }

    @Test
    void diagnosticDocumentsExtension() {
        HCXInsurancePlan.DiagnosticDocumentsExtension dde = new HCXInsurancePlan.DiagnosticDocumentsExtension();
        // copy method
        HCXInsurancePlan.DiagnosticDocumentsExtension copyDde = dde.copy();
        boolean isValid = dde.isEmpty();
        assertTrue(isValid);
        assert (dde.toString() != copyDde.toString());


    }

    @Test
    void informationalMessagesExtension() {
        HCXInsurancePlan.InformationalMessagesExtension ime = new HCXInsurancePlan.InformationalMessagesExtension();
        // copy method
        HCXInsurancePlan.InformationalMessagesExtension copyIme = ime.copy();
        boolean isValid = ime.isEmpty();
        assertTrue(isValid);
        assert (ime.toString() != ime.toString());


    }

    @Test
    void questionnairesExtension() {
        HCXInsurancePlan.QuestionnairesExtension qe = new HCXInsurancePlan.QuestionnairesExtension();
        // copy method
        HCXInsurancePlan.QuestionnairesExtension copyQe = qe.copy();
        boolean isValid = qe.isEmpty();
        assertTrue(isValid);
        assert (qe.toString() != copyQe.toString());

    }


}

