import org.hl7.fhir.r4.model.*;
import java.util.Date;
import java.util.UUID;

public class HCXClaim {

    public static Claim claimExample(){
        //Creating the Claims request
        Claim claim = new Claim();
        claim.setId(UUID.randomUUID().toString());
        Meta metaClaim = new Meta();
        metaClaim.getProfile().add(new CanonicalType("https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-Claim.html"));
        metaClaim.setLastUpdated(new Date());
        claim.setMeta(metaClaim);
        claim.setStatus(org.hl7.fhir.r4.model.Claim.ClaimStatus.ACTIVE);
        claim.setType(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/claim-type").setCode("institutional")));
        claim.setUse(org.hl7.fhir.r4.model.Claim.Use.CLAIM);
        claim.setPatient(new Reference("Patient/RVH1003"));
        claim.addIdentifier(new Identifier().setSystem("http://identifiersystem.com").setValue("IdentifierValue"));
        claim.setCreated(new Date());
        claim.setInsurer(new Reference("Organization/GICOFINDIA"));
        claim.setProvider(new Reference("Organization/WeMeanWell01"));
        claim.setPriority(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/processpriority").setCode("normal")));
        claim.setPayee(new org.hl7.fhir.r4.model.Claim.PayeeComponent().setParty(new Reference("Organization/WeMeanWell01")).setType(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/payeetype").setCode("provider"))));
        claim.getCareTeam().add(new org.hl7.fhir.r4.model.Claim.CareTeamComponent().setSequence(4).setProvider(new Reference("Organization/WeMeanWell01")));
        claim.addInsurance(new org.hl7.fhir.r4.model.Claim.InsuranceComponent().setFocal(true).setCoverage(new Reference("Coverage/COVERAGE1")).setSequence(1));
        claim.getDiagnosis().add(new Claim.DiagnosisComponent().addType(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/ex-diagnosistype").setCode("admitting").setDisplay("Admitting Diagnosis"))).setSequence(1).setDiagnosis(new CodeableConcept(new Coding().setSystem("http://irdai.com").setCode("E906184").setDisplay("SINGLE INCISION LAPAROSCOPIC APPENDECTOMY")).setText("SINGLE INCISION LAPAROSCOPIC APPENDECTOMY")));
        claim.getDiagnosis().add(new Claim.DiagnosisComponent().addType(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/ex-diagnosistype").setCode("admitting").setDisplay("Admitting Diagnosis"))).setDiagnosis(new Reference("Condition/1234")));
        //adding procedure items to the claims component
        //admission fees
        claim.getItem().add(new org.hl7.fhir.r4.model.Claim.ItemComponent().setSequence(1).setProductOrService(new CodeableConcept(new Coding().setCode("E101021").setSystem("https://irdai.gov.in/package-code").setDisplay("Twin Sharing Ac"))).setUnitPrice(new Money().setValue(100000).setCurrency("INR")));
        //tests
        claim.getItem().add(new org.hl7.fhir.r4.model.Claim.ItemComponent().setSequence(1).setProductOrService(new CodeableConcept(new Coding().setSystem("https://irdai.gov.in/package-code").setCode("E924260").setDisplay("CLINICAL TOXICOLOGY SCREEN, BLOOD"))).setUnitPrice(new Money().setValue(2000).setCurrency("INR")));
        claim.getItem().add(new org.hl7.fhir.r4.model.Claim.ItemComponent().setSequence(1).setProductOrService(new CodeableConcept(new Coding().setSystem("https://irdai.gov.in/package-code").setCode("E924261").setDisplay("CLINICAL TOXICOLOGY SCREEN,URINE"))).setUnitPrice(new Money().setValue(1000).setCurrency("INR")));
        claim.getItem().add(new org.hl7.fhir.r4.model.Claim.ItemComponent().setSequence(1).setProductOrService(new CodeableConcept(new Coding().setSystem("https://irdai.gov.in/package-code").setCode("E507029").setDisplay("ECG"))).setUnitPrice(new Money().setValue(5000).setCurrency("INR")));
        claim.getItem().add(new org.hl7.fhir.r4.model.Claim.ItemComponent().setSequence(1).setProductOrService(new CodeableConcept(new Coding().setSystem("https://irdai.gov.in/package-code").setCode("E6080377").setDisplay("UltraSound Abdomen"))).setUnitPrice(new Money().setValue(5000).setCurrency("INR")));

        //Consultation Medication and Hospital Charges
        claim.getItem().add(new org.hl7.fhir.r4.model.Claim.ItemComponent().setSequence(1).setProductOrService(new CodeableConcept(new Coding().setSystem("https://irdai.gov.in/package-code").setCode("502001").setDisplay("Surgeons Charges"))).setUnitPrice(new Money().setValue(1000).setCurrency("INR")));
        claim.getItem().add(new org.hl7.fhir.r4.model.Claim.ItemComponent().setSequence(1).setProductOrService(new CodeableConcept(new Coding().setSystem("https://irdai.gov.in/package-code").setCode("5020021").setDisplay("Anesthesiologist charges"))).setUnitPrice(new Money().setValue(1000).setCurrency("INR")));
        claim.getItem().add(new org.hl7.fhir.r4.model.Claim.ItemComponent().setSequence(1).setProductOrService(new CodeableConcept(new Coding().setSystem("https://irdai.gov.in/package-code").setCode("E6080373").setDisplay("Physician charges"))).setUnitPrice(new Money().setValue(1000).setCurrency("INR")));
        claim.getItem().add(new org.hl7.fhir.r4.model.Claim.ItemComponent().setSequence(1).setProductOrService(new CodeableConcept(new Coding().setSystem("https://irdai.gov.in/package-code").setCode("201008").setDisplay("Recovery Room"))).setUnitPrice(new Money().setValue(10000).setCurrency("INR")));
        claim.getItem().add(new org.hl7.fhir.r4.model.Claim.ItemComponent().setSequence(1).setProductOrService(new CodeableConcept(new Coding().setSystem("https://irdai.gov.in/package-code").setCode("406003").setDisplay("intra -venous (iv) set"))).setUnitPrice(new Money().setValue(5000).setCurrency("INR")));
        claim.getItem().add(new org.hl7.fhir.r4.model.Claim.ItemComponent().setSequence(1).setProductOrService(new CodeableConcept(new Coding().setSystem("https://irdai.gov.in/package-code").setCode("E507353").setDisplay("Oral Medication"))).setUnitPrice(new Money().setValue(5000).setCurrency("INR")));
        claim.getItem().add(new org.hl7.fhir.r4.model.Claim.ItemComponent().setSequence(1).setProductOrService(new CodeableConcept(new Coding().setSystem("https://irdai.gov.in/package-code").setCode("E925171").setDisplay("Hospital charges"))).setUnitPrice(new Money().setValue(5000).setCurrency("INR")));
        claim.getItem().add(new org.hl7.fhir.r4.model.Claim.ItemComponent().setSequence(1).setProductOrService(new CodeableConcept(new Coding().setSystem("https://irdai.gov.in/package-code").setCode("501001").setDisplay("Consultation Charges"))).setUnitPrice(new Money().setValue(5000).setCurrency("INR")));

        //total cost
        claim.setTotal(new Money().setCurrency("INR").setValue(146000.0));
        return claim;
    }

    public static ClaimResponse claimResponseExample(){
        //Creating Claim response
        ClaimResponse claimRes = new ClaimResponse();
        claimRes.setId(UUID.randomUUID().toString());
        Meta metaClaimRes = new Meta();
        metaClaimRes.getProfile().add(new CanonicalType("https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-ClaimResponse.html"));
        metaClaimRes.setLastUpdated(new Date());
        claimRes.setMeta(metaClaimRes);
        claimRes.setStatus(ClaimResponse.ClaimResponseStatus.ACTIVE);
        claimRes.addIdentifier(new Identifier().setSystem("http://identifiersystem.com").setValue(UUID.randomUUID().toString()));
        claimRes.setType(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/claim-type").setCode("institutional")));
        claimRes.setUse(ClaimResponse.Use.CLAIM);
        claimRes.setPatient(new Reference("Patient/RVH1003"));
        claimRes.setCreated(new Date());
        claimRes.setInsurer(new Reference("Organization/GICOFINDIA"));
        claimRes.setRequestor(new Reference("Organization/WeMeanWell01"));
        claimRes.setRequest(new Reference("Claim/CLAIM1"));
        claimRes.setOutcome(ClaimResponse.RemittanceOutcome.ERROR);
        claimRes.getTotal().add(new ClaimResponse.TotalComponent().setCategory(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/adjudication").setCode("benefit"))).setAmount(new Money().setValue(80000).setCurrency("INR")));
        claimRes.getError().add(new ClaimResponse.ErrorComponent(new CodeableConcept(new Coding().setSystem("http://hcxprotocol.io/codes/claim-error-codes").setCode("AUTH-005").setDisplay("Claim information is inconsistent with pre-certified/authorized services"))));
        return claimRes;
    }
}
