import org.hl7.fhir.r4.model.*;

import java.util.HashMap;
import java.util.Map;

public class HCXOrganization {

    public static Organization providerOrganizationExample(){
        //making the hospital org resource
        Organization hos = new Organization();
        hos.setId("WeMeanWell01");
        Meta metaorg1 = new Meta();
        metaorg1.getProfile().add(new CanonicalType("https://nrces.in/ndhm/fhir/r4/StructureDefinition/Organization"));
        hos.setMeta(metaorg1);
        hos.setName("WeMeanWell Hospital");
        hos.getAddress().add(new Address().setText(" Bannerghatta Road, Bengaluru ").setCity("Bengaluru").setCountry("India"));
        hos.getIdentifier().add(new Identifier().setSystem("http://abdm.gov.in/facilities").setValue("HFR-ID-FOR-TMH").setType(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/v2-0203").setCode("AC").setDisplay("Narayana"))));
        return hos;
    }

    public static Organization insurerOrganizationExample(){
        //making an organization resource
        Organization org = new Organization();
        Meta metaorg = new Meta();
        metaorg.getProfile().add(new CanonicalType("https://nrces.in/ndhm/fhir/r4/StructureDefinition/Organization"));
        org.setMeta(metaorg);
        org.setId("GICOFINDIA");
        org.setName("GICOFINDIA");
        org.getIdentifier().add(new Identifier().setSystem("http://irdai.gov.in/insurers").setValue("GICOFINDIA").setType(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/v2-0203").setCode("AC").setDisplay("GOVOFINDIA"))));
        return org;
    }
}
