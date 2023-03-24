import org.hl7.fhir.r4.model.*;

import java.util.HashMap;
import java.util.Map;

public class HCXCoverage {

    public static Coverage coverageExample(){
        //making the coverage resource
        Coverage cov = new Coverage();
        cov.setId("COVERAGE1");
        Meta metacov = new Meta();
        metacov.getProfile().add(new CanonicalType("https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-Coverage.html"));
        cov.setMeta(metacov);
        cov.setStatus(Coverage.CoverageStatus.ACTIVE);
        cov.getIdentifier().add(new Identifier().setValue("policy-RVH1003").setSystem("https://www.gicofIndia.in/policies"));
        cov.getSubscriber().setReference("Patient/RVH1003");
        cov.setSubscriberId("2XX8971");
        cov.getBeneficiary().setReference( "Patient/RVH1003");
        cov.setRelationship(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/subscriber-relationship").setCode("self")));
        cov.getPayor().add(new Reference("Organization/GICOFINDIA"));
        return cov;
    }
}
