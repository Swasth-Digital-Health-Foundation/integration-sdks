import org.hl7.fhir.r4.model.*;

import java.util.Date;
import java.util.UUID;

public class HCXCommunicationRequest {

    public static CommunicationRequest communicationRequestExample(){
        CommunicationRequest comReq = new CommunicationRequest();
        comReq.setId(UUID.randomUUID().toString());
        Meta meta = new Meta();
        meta.getProfile().add(new CanonicalType("https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-CommunicationRequest.html"));
        meta.setLastUpdated(new Date());
        comReq.setMeta(meta);
        comReq.getIdentifier().add(new Identifier().setSystem("http://irdai.gov.in/insurer/123456").setValue("ABCD123"));
        comReq.setStatus(CommunicationRequest.CommunicationRequestStatus.ACTIVE);
        comReq.getBasedOn().add(new Reference("Patient/RVH1003"));
        comReq.getPayload().add(new CommunicationRequest.CommunicationRequestPayloadComponent().setContent(new StringType("Please provide the accident report and any associated pictures to support your Claim# DEF5647.")));
        return comReq;
    }


    public static Communication communicationExample(){
        Communication comm =  new Communication();
        comm.setId(UUID.randomUUID().toString());
        Meta meta = new Meta();
        meta.getProfile().add(new CanonicalType("https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-Communication.html"));
        meta.setLastUpdated(new Date());
        comm.setMeta(meta);
        comm.getIdentifier().add(new Identifier().setSystem("http://www.providerco.com/communication").setValue("12345"));
        comm.setStatus(Communication.CommunicationStatus.COMPLETED);
        comm.getPayload().add(new Communication.CommunicationPayloadComponent().setContent(new Attachment().setContentType("application/pdf").setData("abcd".getBytes()).setTitle("accident_notes.pdf").setCreation(new Date())));
        return comm;
    }
}
