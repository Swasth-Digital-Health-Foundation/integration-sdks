package io.hcxprotocol.impl;

import io.hcxprotocol.init.HCXIntegrator;
import io.hcxprotocol.utils.Constants;
import io.hcxprotocol.utils.JSONUtils;
import io.hcxprotocol.utils.Operations;
import org.apache.commons.lang3.ObjectUtils;
import org.junit.jupiter.api.Test;
import org.springframework.test.web.servlet.MockMvc;

import java.util.HashMap;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.assertEquals;


public class HCXIncomingRequestTest {

    protected MockMvc mockMvc;

    @Test
    void HCXIncomingRequestTest() throws Exception {
        Map<String, Object> configMap = new HashMap<>();
        configMap.put("protocolBasePath", "http://dev-hcx.swasth.app/api/v0.7");
        configMap.put("participantCode", "testprovider1.apollo@swasth-hcx-dev");
        configMap.put("authBasePath", "http://aef8101c432d04a659e6317b3e5031cf-1674586068.ap-south-1.elb.amazonaws.com:8080/auth/realms/swasth-health-claim-exchange/protocol/openid-connect/token");
        configMap.put("username", "testprovider1@apollo.com");
        configMap.put("password", "Opensaber@123");
        configMap.put("encryptionPrivateKey", "-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCG+XLPYiCxrZq71IX+w7uoDGxGI7qy7XaDbL3BJE33ju7rjdrP7wsAOWRvM8BIyWuRZZhl9xG+u7l/7OsZAzGoqI7p+32x+r9IJVzboLDajk6tp/NPg1csc7f2M5Bu6rkLEvrKLz3dgy3Q928rMsD3rSmzBLelfKTo+aDXvCOiw1dMWsZZdkEpCTJxH39Nb2K4S59kO/R2GtSU/QMLq65m34XcMZpDtatA1u1S8JdZNNeMCO+NuFKBzIfvXUCQ8jkf7h612+UP1AYhoyCMFpzUZ9b7liQF9TYpX1Myr/tT75WKuRlkFlcALUrtVskL8KA0w6sA0nX5fORVsuVehVeDAgMBAAECggEAX1n1y5/M7PhxqWO3zYTFGzC7hMlU6XZsFOhLHRjio5KsImgyPlbm9J+W3iA3JLR2c17MTKxAMvg3UbIzW5YwDLAXViC+aW90like8mEQzzVdS7ysXG2ytcqCGUHQNStI0hP0a8T39XbodQl31ZKjU9VW8grRGe12Kse+4ukcW6yRVES+CkyO5BQB+vs3voZavodRGsk/YSt00PtIrFPJgkDuyzzcybKJD9zeJk5W3OGVK1z0on+NXKekRti5FBx/uEkT3+knkz7ZlTDNcyexyeiv7zSL/L6tcszV0Fe0g9vJktqnenEyh4BgbqABPzQR++DaCgW5zsFiQuD0hMadoQKBgQC+rekgpBHsPnbjQ2Ptog9cFzGY6LRGXxVcY7hKBtAZOKAKus5RmMi7Uv7aYJgtX2jt6QJMuE90JLEgdO2vxYG5V7H6Tx+HqH7ftCGZq70A9jFBaba04QAp0r4TnD6v/LM+PGVT8FKtggp+o7gZqXYlSVFm6YzI37G08w43t2j2aQKBgQC1Nluxop8w6pmHxabaFXYomNckziBNMML5GjXW6b0xrzlnZo0p0lTuDtUy2xjaRWRYxb/1lu//LIrWqSGtzu+1mdmV2RbOd26PArKw0pYpXhKFu/W7r6n64/iCisoMJGWSRJVK9X3D4AjPaWOtE+jUTBLOk0lqPJP8K6yiCA6ZCwKBgDLtgDaXm7HdfSN1/Fqbzj5qc3TDsmKZQrtKZw5eg3Y5CYXUHwbsJ7DgmfD5m6uCsCPa+CJFl/MNWcGxeUpZFizKn16bg3BYMIrPMao5lGGNX9p4wbPN5J1HDD1wnc2jULxupSGmLm7pLKRmVeWEvWl4C6XQ+ykrlesef82hzwcBAoGBAKGY3v4y4jlSDCXaqadzWhJr8ffdZUrQwB46NGb5vADxnIRMHHh+G8TLL26RmcET/p93gW518oGg7BLvcpw3nOZaU4HgvQjT0qDvrAApW0V6oZPnAQUlarTU1Uk8kV9wma9tP6E/+K5TPCgSeJPg3FFtoZvcFq0JZoKLRACepL3vAoGAMAUHmNHvDI+v0eyQjQxlmeAscuW0KVAQQR3OdwEwTwdFhp9Il7/mslN1DLBddhj6WtVKLXu85RIGY8I2NhMXLFMgl+q+mvKMFmcTLSJb5bJHyMz/foenGA/3Yl50h9dJRFItApGuEJo/30cG+VmYo2rjtEifktX4mDfbgLsNwsI=\n-----END PRIVATE KEY-----");
        configMap.put("igUrl", "123");
        HCXIntegrator.init(configMap);
        HCXIntegrator hcxIntegrator = HCXIntegrator.getInstance();

        // Testcase for OutgoingRequest
        HCXOutgoingRequest hcxOutgoingRequest = new HCXOutgoingRequest();
        Map<String, Object> output = new HashMap<>();
        String fhirPayload = "{ \"resourceType\": \"Bundle\", \"id\": \"d4484cdd-1aae-4d21-a92e-8ef749d6d366\", \"meta\": { \"lastUpdated\": \"2022-02-08T21:49:55.458+05:30\" }, \"identifier\": { \"system\": \"https://www.tmh.in/bundle\", \"value\": \"d4484cdd-1aae-4d21-a92e-8ef749d6d366\" }, \"type\": \"document\", \"timestamp\": \"2022-02-08T21:49:55.458+05:30\", \"entry\": [{ \"fullUrl\": \"Composition/42ff4a07-3e36-402f-a99e-29f16c0c9eee\", \"resource\": { \"resourceType\": \"Composition\", \"id\": \"42ff4a07-3e36-402f-a99e-29f16c0c9eee\", \"identifier\": { \"system\": \"https://www.tmh.in/hcx-documents\", \"value\": \"42ff4a07-3e36-402f-a99e-29f16c0c9eee\" }, \"status\": \"final\", \"type\": { \"coding\": [{ \"system\": \"https://www.hcx.org/document-type\", \"code\": \"HcxCoverageEligibilityRequest\", \"display\": \"Coverage Eligibility Request Doc\" }] }, \"subject\": { \"reference\": \"Patient/RVH1003\" }, \"date\": \"2022-02-08T21:49:55+05:30\", \"author\": [{ \"reference\": \"Organization/Tmh01\" }], \"title\": \"Coverage Eligibility Request\", \"section\": [{ \"title\": \"# Eligibility Request\", \"code\": { \"coding\": [{ \"system\": \"https://fhir.loinc.org/CodeSystem/$lookup?system=http://loinc.org&code=10154-3\", \"code\": \"CoverageEligibilityRequest\", \"display\": \"Coverage Eligibility Request\" }] }, \"entry\": [{ \"reference\": \"CoverageEligibilityRequest/dc82673b-8c71-48c2-8a17-16dcb3b035f6\" }] }] } }, { \"fullUrl\": \"Organization/Tmh01\", \"resource\": { \"resourceType\": \"Organization\", \"id\": \"Tmh01\", \"identifier\": [{ \"system\": \"http://abdm.gov.in/facilities\", \"value\": \"HFR-ID-FOR-TMH\" }, { \"system\": \"http://irdai.gov.in/facilities\", \"value\": \"IRDA-ID-FOR-TMH\" } ], \"name\": \"Tata Memorial Hospital\", \"alias\": [ \"TMH\", \"TMC\" ], \"telecom\": [{ \"system\": \"phone\", \"value\": \"(+91) 022-2417-7000\" }], \"address\": [{ \"line\": [ \"Dr Ernest Borges Rd, Parel East, Parel, Mumbai, Maharashtra 400012\" ], \"city\": \"Mumbai\", \"state\": \"Maharashtra\", \"postalCode\": \"400012\", \"country\": \"INDIA\" }], \"endpoint\": [{ \"reference\": \"https://www.tmc.gov.in/\", \"display\": \"Website\" }] } }, { \"fullUrl\": \"Patient/RVH1003\", \"resource\": { \"resourceType\": \"Patient\", \"id\": \"RVH1003\", \"identifier\": [{ \"type\": { \"coding\": [{ \"system\": \"http://terminology.hl7.org/CodeSystem/v2-0203\", \"code\": \"SN\", \"display\": \"Subscriber Number\" }] }, \"system\": \"http://gicofIndia.com/beneficiaries\", \"value\": \"BEN-101\" }, { \"system\": \"http://abdm.gov.in/patients\", \"value\": \"hinapatel@abdm\" } ], \"name\": [{ \"text\": \"Hina Patel\" }], \"gender\": \"female\" } }, { \"fullUrl\": \"CoverageEligibilityRequest/dc82673b-8c71-48c2-8a17-16dcb3b035f6\", \"resource\": { \"resourceType\": \"CoverageEligibilityRequest\", \"id\": \"dc82673b-8c71-48c2-8a17-16dcb3b035f6\", \"identifier\": [{ \"system\": \"https://www.tmh.in/coverage-eligibility-request\", \"value\": \"dc82673b-8c71-48c2-8a17-16dcb3b035f6\" }], \"status\": \"active\", \"purpose\": [ \"discovery\" ], \"patient\": { \"reference\": \"Patient/RVH1003\" }, \"servicedPeriod\": { \"start\": \"2022-02-07T21:49:56+05:30\", \"end\": \"2022-02-09T21:49:56+05:30\" }, \"created\": \"2022-02-08T21:49:56+05:30\", \"provider\": { \"reference\": \"Organization/Tmh01\" }, \"insurer\": { \"reference\": \"Organization/GICOFINDIA\" }, \"insurance\": [{ \"focal\": true, \"coverage\": { \"reference\": \"Coverage/dadde132-ad64-4d18-8c18-1d52d7e86abc\" } }] } }, { \"fullUrl\": \"Organization/GICOFINDIA\", \"resource\": { \"resourceType\": \"Organization\", \"id\": \"GICOFINDIA\", \"identifier\": [{ \"system\": \"http://irdai.gov.in/insurers\", \"value\": \"112\" }], \"name\": \"General Insurance Corporation of India\" } }, { \"fullUrl\": \"Coverage/dadde132-ad64-4d18-8c18-1d52d7e86abc\", \"resource\": { \"resourceType\": \"Coverage\", \"id\": \"dadde132-ad64-4d18-8c18-1d52d7e86abc\", \"identifier\": [{ \"system\": \"https://www.gicofIndia.in/policies\", \"value\": \"policy-RVH1003\" }], \"status\": \"active\", \"subscriber\": { \"reference\": \"Patient/RVH1003\" }, \"subscriberId\": \"SN-RVH1003\", \"beneficiary\": { \"reference\": \"Patient/RVH1003\" }, \"relationship\": { \"coding\": [{ \"system\": \"http://terminology.hl7.org/CodeSystem/subscriber-relationship\", \"code\": \"self\" }] }, \"payor\": [{ \"reference\": \"Organization/GICOFINDIA\" }] } } ] }";
        hcxOutgoingRequest.generate(fhirPayload, Operations.COVERAGE_ELIGIBILITY_CHECK, "testpayor1.icici@swasth-hcx-dev", output);
        hcxOutgoingRequest.generate(fhirPayload, Operations.COVERAGE_ELIGIBILITY_ON_CHECK, "actionjwe", "response.partial", output);
        hcxOutgoingRequest.createHeader(null, (String) output.get(Constants.PAYLOAD),"response_partial",output);
        hcxOutgoingRequest.encryptPayload(configMap,null,output);
        hcxOutgoingRequest.generate(null,Operations.COVERAGE_ELIGIBILITY_ON_CHECK,"actionjwe","response.partial",output);

        System.out.println(output);
        assertEquals("http://dev-hcx.swasth.app/api/v0.7", hcxIntegrator.getHCXProtocolBasePath());
        assertEquals("testprovider1.apollo@swasth-hcx-dev", hcxIntegrator.getParticipantCode());

        // Testcases for IncomingRequest
        HCXIncomingRequest hcxIncomingRequest = new HCXIncomingRequest();
        Map<String, Object> payload = new HashMap<>();
        payload.put(Constants.PAYLOAD, output.get(Constants.PAYLOAD));
        hcxIncomingRequest.process(JSONUtils.serialize(payload), Operations.COVERAGE_ELIGIBILITY_CHECK, output);
        hcxIncomingRequest.process("invalid payload",Operations.COVERAGE_ELIGIBILITY_CHECK,output);
        hcxIncomingRequest.decryptPayload(null,output);
        System.out.println(output);
    }



}
