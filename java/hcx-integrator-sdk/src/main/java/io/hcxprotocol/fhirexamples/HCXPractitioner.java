import org.hl7.fhir.r4.model.*;

import java.util.UUID;

public class HCXPractitioner {

    public static Practitioner practitionerExample() {
        //making the hospital org resource
        Practitioner hos = new Practitioner();
        hos.setId("PractitionerViswasKar");
        Meta metaorg1 = new Meta();
        metaorg1.getProfile().add(new CanonicalType("https://nrces.in/ndhm/fhir/r4/StructureDefinition/Practitioner"));
        hos.setMeta(metaorg1);
        hos.getName().add(new HumanName().setText("Dr Viswas kar"));
        hos.getIdentifier().add(new Identifier().setSystem("http://abdm.gov.in/facilities").setValue("DOC-123/456").setType(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/v2-0203").setCode("MD").setDisplay("Medical License number"))));
        return hos;
    }

    public static Practitioner practitionerExample2() {
        //making the hospital org resource
        Practitioner hos = new Practitioner();
        hos.setId("PractitionerXavier");
        Meta metaorg1 = new Meta();
        metaorg1.getProfile().add(new CanonicalType("https://nrces.in/ndhm/fhir/r4/StructureDefinition/Practitioner"));
        hos.setMeta(metaorg1);
        hos.getName().add(new HumanName().setText("Dr Xavier Blades"));
        hos.getIdentifier().add(new Identifier().setSystem("http://abdm.gov.in/facilities").setValue("DOC-987/654").setType(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/v2-0203").setCode("MD").setDisplay("Medical License number"))));
        return hos;
    }

}