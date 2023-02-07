package io.hcxprotocol.fhirexamples;

import org.hl7.fhir.r4.model.*;

public class HCXPatient {

    public static Patient patientExample(){

        //making a Patient resource
        Patient pat = new Patient();
        pat.setId("RVH1003");
        Meta metapat = new Meta();
        metapat.getProfile().add(new CanonicalType("https://nrces.in/ndhm/fhir/r4/StructureDefinition/Patient"));
        pat.setMeta(metapat);
        pat.getIdentifier().add(new Identifier().setType(new CodeableConcept(new Coding().setSystem( "http://terminology.hl7.org/CodeSystem/v2-0203").setCode("SN").setDisplay("Subscriber Number"))).setSystem("http://gicofIndia.com/beneficiaries").setValue("BEN-101"));
        pat.setGender(Enumerations.AdministrativeGender.FEMALE);
        pat.getName().add(new HumanName().setText("Hina Patel"));
        return pat;
    }
}
