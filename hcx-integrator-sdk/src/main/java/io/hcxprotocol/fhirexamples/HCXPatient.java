import org.hl7.fhir.r4.model.*;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;

public class HCXPatient {

    public static Patient patientExample() {

        //making a Patient resource
        Patient pat = new Patient();
        pat.setId("RVH1003");
        Meta metapat = new Meta();
        metapat.getProfile().add(new CanonicalType("https://nrces.in/ndhm/fhir/r4/StructureDefinition/Patient"));
        pat.setMeta(metapat);
        pat.getIdentifier().add(new Identifier().setType(new CodeableConcept(new Coding().setSystem("http://terminology.hl7.org/CodeSystem/v2-0203").setCode("SN").setDisplay("Subscriber Number"))).setSystem("http://gicofIndia.com/beneficiaries").setValue("BEN-101"));
        //pat.getIdentifier().add(new Identifier().setType(new CodeableConcept(new Coding().setSystem("https://nrces.in/ndhm/fhir/r4/CodeSystem/ndhm-identifier-type-code").setCode("PMJAY").setDisplay("Pradhan Mantri Jan Aarogya Yojana (PMJAY) ID"))).setValue("PMJAY-0101"));
        pat.setGender(Enumerations.AdministrativeGender.MALE);
        pat.getName().add(new HumanName().setText("Prasidh Dixit"));
        String date_string = "26-09-1960";
        SimpleDateFormat formatter = new SimpleDateFormat("dd-MM-yyyy");
        Date date;
        try {
            date = formatter.parse(date_string);
        } catch (ParseException e) {
            throw new RuntimeException(e);
        }
        pat.setBirthDate(date);
        pat.addAddress((new Address().setText("#39 Kalena Agrahara, Kamanahalli, Bengaluru - 560056").setCity("Bengaluru").setPostalCode("560056").setState("Karnataka").setCountry("India")));
        return pat;
    }
}
