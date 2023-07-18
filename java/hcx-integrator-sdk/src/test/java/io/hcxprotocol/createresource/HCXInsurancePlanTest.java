package io.hcxprotocol.createresource;

import ca.uhn.fhir.context.FhirContext;
import ca.uhn.fhir.parser.IParser;
import ca.uhn.fhir.validation.FhirValidator;
import ca.uhn.fhir.validation.SingleValidationMessage;
import ca.uhn.fhir.validation.ValidationResult;
import io.hcxprotocol.init.HCXIntegrator;
import io.hcxprotocol.validator.HCXFHIRValidator;
import org.hl7.fhir.instance.model.api.IBaseResource;
import org.hl7.fhir.r4.model.*;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.TestInstance;

import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;

@TestInstance(TestInstance.Lifecycle.PER_CLASS)
class HCXInsurancePlanTest {

    private HCXIntegrator hcxIntegrator = null;

    @BeforeAll
    public void initializingConfigMap() throws Exception {
        Map<String,Object> configMap = new HashMap<>();
        configMap.put("protocolBasePath", "https://dev-hcx.swasth.app/api/v0.7");
        configMap.put("participantCode", "testprovider1.apollo@swasth-hcx-dev");
        configMap.put("authBasePath", "https://dev-hcx.swasth.app/auth/realms/swasth-health-claim-exchange/protocol/openid-connect/token");
        configMap.put("username", "testprovider1@apollo.com");
        configMap.put("password", "Opensaber@123");
        configMap.put("encryptionPrivateKey", "-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCG+XLPYiCxrZq71IX+w7uoDGxGI7qy7XaDbL3BJE33ju7rjdrP7wsAOWRvM8BIyWuRZZhl9xG+u7l/7OsZAzGoqI7p+32x+r9IJVzboLDajk6tp/NPg1csc7f2M5Bu6rkLEvrKLz3dgy3Q928rMsD3rSmzBLelfKTo+aDXvCOiw1dMWsZZdkEpCTJxH39Nb2K4S59kO/R2GtSU/QMLq65m34XcMZpDtatA1u1S8JdZNNeMCO+NuFKBzIfvXUCQ8jkf7h612+UP1AYhoyCMFpzUZ9b7liQF9TYpX1Myr/tT75WKuRlkFlcALUrtVskL8KA0w6sA0nX5fORVsuVehVeDAgMBAAECggEAX1n1y5/M7PhxqWO3zYTFGzC7hMlU6XZsFOhLHRjio5KsImgyPlbm9J+W3iA3JLR2c17MTKxAMvg3UbIzW5YwDLAXViC+aW90like8mEQzzVdS7ysXG2ytcqCGUHQNStI0hP0a8T39XbodQl31ZKjU9VW8grRGe12Kse+4ukcW6yRVES+CkyO5BQB+vs3voZavodRGsk/YSt00PtIrFPJgkDuyzzcybKJD9zeJk5W3OGVK1z0on+NXKekRti5FBx/uEkT3+knkz7ZlTDNcyexyeiv7zSL/L6tcszV0Fe0g9vJktqnenEyh4BgbqABPzQR++DaCgW5zsFiQuD0hMadoQKBgQC+rekgpBHsPnbjQ2Ptog9cFzGY6LRGXxVcY7hKBtAZOKAKus5RmMi7Uv7aYJgtX2jt6QJMuE90JLEgdO2vxYG5V7H6Tx+HqH7ftCGZq70A9jFBaba04QAp0r4TnD6v/LM+PGVT8FKtggp+o7gZqXYlSVFm6YzI37G08w43t2j2aQKBgQC1Nluxop8w6pmHxabaFXYomNckziBNMML5GjXW6b0xrzlnZo0p0lTuDtUy2xjaRWRYxb/1lu//LIrWqSGtzu+1mdmV2RbOd26PArKw0pYpXhKFu/W7r6n64/iCisoMJGWSRJVK9X3D4AjPaWOtE+jUTBLOk0lqPJP8K6yiCA6ZCwKBgDLtgDaXm7HdfSN1/Fqbzj5qc3TDsmKZQrtKZw5eg3Y5CYXUHwbsJ7DgmfD5m6uCsCPa+CJFl/MNWcGxeUpZFizKn16bg3BYMIrPMao5lGGNX9p4wbPN5J1HDD1wnc2jULxupSGmLm7pLKRmVeWEvWl4C6XQ+ykrlesef82hzwcBAoGBAKGY3v4y4jlSDCXaqadzWhJr8ffdZUrQwB46NGb5vADxnIRMHHh+G8TLL26RmcET/p93gW518oGg7BLvcpw3nOZaU4HgvQjT0qDvrAApW0V6oZPnAQUlarTU1Uk8kV9wma9tP6E/+K5TPCgSeJPg3FFtoZvcFq0JZoKLRACepL3vAoGAMAUHmNHvDI+v0eyQjQxlmeAscuW0KVAQQR3OdwEwTwdFhp9Il7/mslN1DLBddhj6WtVKLXu85RIGY8I2NhMXLFMgl+q+mvKMFmcTLSJb5bJHyMz/foenGA/3Yl50h9dJRFItApGuEJo/30cG+VmYo2rjtEifktX4mDfbgLsNwsI=\n-----END PRIVATE KEY-----");
        configMap.put("signingPrivateKey", "-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCG+XLPYiCxrZq71IX+w7uoDGxGI7qy7XaDbL3BJE33ju7rjdrP7wsAOWRvM8BIyWuRZZhl9xG+u7l/7OsZAzGoqI7p+32x+r9IJVzboLDajk6tp/NPg1csc7f2M5Bu6rkLEvrKLz3dgy3Q928rMsD3rSmzBLelfKTo+aDXvCOiw1dMWsZZdkEpCTJxH39Nb2K4S59kO/R2GtSU/QMLq65m34XcMZpDtatA1u1S8JdZNNeMCO+NuFKBzIfvXUCQ8jkf7h612+UP1AYhoyCMFpzUZ9b7liQF9TYpX1Myr/tT75WKuRlkFlcALUrtVskL8KA0w6sA0nX5fORVsuVehVeDAgMBAAECggEAX1n1y5/M7PhxqWO3zYTFGzC7hMlU6XZsFOhLHRjio5KsImgyPlbm9J+W3iA3JLR2c17MTKxAMvg3UbIzW5YwDLAXViC+aW90like8mEQzzVdS7ysXG2ytcqCGUHQNStI0hP0a8T39XbodQl31ZKjU9VW8grRGe12Kse+4ukcW6yRVES+CkyO5BQB+vs3voZavodRGsk/YSt00PtIrFPJgkDuyzzcybKJD9zeJk5W3OGVK1z0on+NXKekRti5FBx/uEkT3+knkz7ZlTDNcyexyeiv7zSL/L6tcszV0Fe0g9vJktqnenEyh4BgbqABPzQR++DaCgW5zsFiQuD0hMadoQKBgQC+rekgpBHsPnbjQ2Ptog9cFzGY6LRGXxVcY7hKBtAZOKAKus5RmMi7Uv7aYJgtX2jt6QJMuE90JLEgdO2vxYG5V7H6Tx+HqH7ftCGZq70A9jFBaba04QAp0r4TnD6v/LM+PGVT8FKtggp+o7gZqXYlSVFm6YzI37G08w43t2j2aQKBgQC1Nluxop8w6pmHxabaFXYomNckziBNMML5GjXW6b0xrzlnZo0p0lTuDtUy2xjaRWRYxb/1lu//LIrWqSGtzu+1mdmV2RbOd26PArKw0pYpXhKFu/W7r6n64/iCisoMJGWSRJVK9X3D4AjPaWOtE+jUTBLOk0lqPJP8K6yiCA6ZCwKBgDLtgDaXm7HdfSN1/Fqbzj5qc3TDsmKZQrtKZw5eg3Y5CYXUHwbsJ7DgmfD5m6uCsCPa+CJFl/MNWcGxeUpZFizKn16bg3BYMIrPMao5lGGNX9p4wbPN5J1HDD1wnc2jULxupSGmLm7pLKRmVeWEvWl4C6XQ+ykrlesef82hzwcBAoGBAKGY3v4y4jlSDCXaqadzWhJr8ffdZUrQwB46NGb5vADxnIRMHHh+G8TLL26RmcET/p93gW518oGg7BLvcpw3nOZaU4HgvQjT0qDvrAApW0V6oZPnAQUlarTU1Uk8kV9wma9tP6E/+K5TPCgSeJPg3FFtoZvcFq0JZoKLRACepL3vAoGAMAUHmNHvDI+v0eyQjQxlmeAscuW0KVAQQR3OdwEwTwdFhp9Il7/mslN1DLBddhj6WtVKLXu85RIGY8I2NhMXLFMgl+q+mvKMFmcTLSJb5bJHyMz/foenGA/3Yl50h9dJRFItApGuEJo/30cG+VmYo2rjtEifktX4mDfbgLsNwsI=\n-----END PRIVATE KEY-----");
        hcxIntegrator = HCXIntegrator.getInstance(configMap);
    }

    @Test
    void validateInsurancePlanObject() throws Exception {
        FhirValidator validator = HCXFHIRValidator.getValidator(hcxIntegrator.getConfig());


        HCXInsurancePlan ip = createInsurancePlan();
        printFHIRObject(ip);

        ValidationResult result = validator.validateWithResult(ip);
        System.out.println("Result " + result);
        assertTrue(result.isSuccessful());

        List<SingleValidationMessage> messages = result.getMessages();
        System.out.println("messages " + messages);
        assertEquals(4, messages.size());
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

