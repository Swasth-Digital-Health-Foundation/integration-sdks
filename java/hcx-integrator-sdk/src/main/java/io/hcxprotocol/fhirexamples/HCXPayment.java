import org.hl7.fhir.r4.model.*;

import java.util.Date;
import java.util.UUID;

public class HCXPayment {

    public static PaymentReconciliation paymentReconciliationExample(){
        PaymentReconciliation pay = new PaymentReconciliation();
        pay.setId(UUID.randomUUID().toString());
        Meta meta = new Meta();
        meta.getProfile().add(new CanonicalType("https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-PaymentReconciliation.html"));
        meta.setLastUpdated(new Date());
        pay.setMeta(meta);
        pay.setStatus(PaymentReconciliation.PaymentReconciliationStatus.ACTIVE);
        pay.setCreated(new Date());
        pay.setRequestor(new Reference("Organization/GICOFINDIA"));
        pay.setOutcome(Enumerations.RemittanceOutcome.COMPLETE);
        pay.setPaymentDate(new Date());
        pay.setPaymentAmount(new Money().setValue(100000).setCurrency("INR"));
        pay.setPaymentIdentifier(new Identifier().setSystem("https://www.tmh.in/paymentreconciliation").setValue("12343"));
        return pay;
    }

    public static PaymentNotice paymentNoticeExample(){
        PaymentNotice pay = new PaymentNotice();
        pay.setId(UUID.randomUUID().toString());
        Meta meta = new Meta();
        meta.getProfile().add(new CanonicalType("https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-PaymentNotice.html"));
        meta.setLastUpdated(new Date());
        pay.setMeta(meta);
        pay.getIdentifier().add(new Identifier().setValue("123435").setSystem("https://www.tmh.in/paymentnotice"));
        pay.setStatus(PaymentNotice.PaymentNoticeStatus.ACTIVE);
        pay.setRequest(new Reference("https://www.tmh.in/fhir/claim/12345"));
        pay.setResponse(new Reference("https://www.tmh.in/fhir/claimresponse/CR12345"));
        pay.setCreated(new Date());
        pay.setProvider(new Reference("Organization/tmh01"));
        pay.setPayment(new Reference("PaymentReconciliation/ER2500"));
        pay.setRecipient(new Reference("Organization/GICOFINDIA"));
        pay.setAmount(new Money().setValue(100000).setCurrency("INR"));
        return pay;
    }
}
