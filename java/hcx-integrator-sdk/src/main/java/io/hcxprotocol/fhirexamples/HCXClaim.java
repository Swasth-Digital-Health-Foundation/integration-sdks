package io.hcxprotocol.fhirexamples;

import org.hl7.fhir.r4.model.*;

import java.util.Date;

public class HCXClaim {

    public static Claim claimExample(){
        //Creating the Claims request
        Claim claim = new Claim();
        Meta metaClaim = new Meta();
        metaClaim.getProfile().add(new CanonicalType("https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-Claim.html"));
        metaClaim.setLastUpdated(new Date());
        claim.setMeta(metaClaim);
        claim.setStatus(Claim.ClaimStatus.ACTIVE);
        claim.setType(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/claim-type").setCode("institutional")));
        claim.setUse(Claim.Use.CLAIM);
        claim.setPatient(new Reference("Patient/RVH1003"));
        claim.addIdentifier(new Identifier().setSystem("http://identifiersystem.com").setValue("IdentifierValue"));
        claim.setCreated(new Date());
        claim.setInsurer(new Reference("Organization/GICOFINDIA"));
        claim.setProvider(new Reference("Organization/Tmh01"));
        claim.setPriority(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/processpriority").setCode("normal")));
        claim.setPayee(new Claim.PayeeComponent().setParty(new Reference("Organization/Tmh01")).setType(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/payeetype").setCode("provider"))));
        claim.getCareTeam().add(new Claim.CareTeamComponent().setSequence(4).setProvider(new Reference("Organization/Tmh01")));
        claim.addInsurance(new Claim.InsuranceComponent().setFocal(true).setCoverage(new Reference("Coverage/COVERAGE1")).setSequence(1));
        claim.getItem().add(new Claim.ItemComponent().setSequence(1).setProductOrService(new CodeableConcept(new Coding().setSystem("https://pmjay.gov.in/hbp-package-code").setCode("ID003").setDisplay("Treatment of COVID-19 Infection"))).setUnitPrice(new Money().setValue(100000).setCurrency("INR")));
        return claim;
    }

    public static ClaimResponse claimResponseExample(){
        //Creating Claim response
        ClaimResponse claimRes = new ClaimResponse();
        Meta metaClaimRes = new Meta();
        metaClaimRes.getProfile().add(new CanonicalType("https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-ClaimResponse.html"));
        metaClaimRes.setLastUpdated(new Date());
        claimRes.setMeta(metaClaimRes);
        claimRes.setStatus(ClaimResponse.ClaimResponseStatus.ACTIVE);
        claimRes.addIdentifier(new Identifier().setSystem("http://identifiersystem.com").setValue("IdentifierValue"));
        claimRes.setType(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/claim-type").setCode("institutional")));
        claimRes.setUse(ClaimResponse.Use.CLAIM);
        claimRes.setPatient(new Reference("Patient/RVH1003"));
        claimRes.setCreated(new Date());
        claimRes.setInsurer(new Reference("Organization/GICOFINDIA"));
        claimRes.setRequestor(new Reference("Organization/Tmh01"));
        claimRes.setRequest(new Reference("Claim/CLAIM1"));
        claimRes.setOutcome(ClaimResponse.RemittanceOutcome.COMPLETE);
        claimRes.getTotal().add(new ClaimResponse.TotalComponent().setCategory(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/adjudication").setCode("benefit"))).setAmount(new Money().setValue(80000).setCurrency("INR")));
        return claimRes;
    }
}
