### **Introduction:**
The HCX Platform providing the REST API interface to integrate and access the platform. But, it requires knowledge of the API Specification, process to generate the authorization token, refresh the token and use it in each API etc,.

The HCX Integration SDK abstract most of these operations and make it simple for the participants to work with the HCX Platform.
### **About HCX Integration SDK:**
The SDK implemented using Java programming language and accessible as a jar file. The Javadocs also attached along with the release in github repository for the developers to understand the details about each function of the SDK.
### **How to use:**

The hcx-integrator-sdk is published to [maven central repository](https://mvnrepository.com/artifact/io.hcxprotocol/hcx-integrator-sdk) and available for consumption. To use the sdk, add the below dependency in the pom file:
```xml                 
<dependency>
  <groupId>io.hcxprotocol</groupId>
  <artifactId>hcx-integrator-sdk</artifactId>
  <version>1.0.5</version>
</dependency>
```

The SDK expect configuration of the HCX Instance and the participant details to initialise and set the context to use it. The configuration can be given as a Java Map or JSON String. 

It expects the below details as configuration to set the context in the integration environment to use the APIs within the SDK.


|**Config Variable Name**|**Mandatory**|**Details**|
| :-: | :-: | :-: |
|protocolBasePath|yes|Base Path of the HCX Instance to access Protocol APIs.|
|authBasePath|yes|Base Path of the HCX Authentication Service to generate authorization tokens.|
|participantCode|yes|The participant code of the integrator in the HCX participant registry.|
|username|yes|The username of the integrator in the HCX instance.|
|password|yes|The password of the integrator in the HCX instance.|
|encryptionPrivateKey|yes|The private key of the integrator to use it for encryption.|
|incomingRequestClass|no|To override any incoming request process methods, implement a custom class and provide the class name here. By default, it will use HCXIncomingRequest class.|
|outgoingRequestClass|no|To override any outgoing request process methods, implement a custom class and provide the class name here. By default, it will use HCXOutgoingRequest class.|
|fhirValidationEnabled|no|Flag to enable/disable FHIR validations. By default, the flag will be set to true.|

Please use the “getInstance” static method from HCXIntegrator class by passing the configuration to set the context. The SDK won’t work as expected without calling this method with required configuration details. It throws an exception when we access the SDK without initializing it.

Below is the sample code snippet to initialise the SDK.

```java
//Define the configuration as a Java Map.
Map<String, Object> configMap = new HashMap<>();
configMap.put("protocolBasePath", "http://staging-hcx.swasth.app/api/v0.7");
configMap.put("participantCode", "<Participant code shared with you>");
configMap.put("authBasePath", "http://a9dd63de91ee94d59847a1225da8b111-273954130.ap-south-1.elb.amazonaws.com:8080/auth/realms/swasth-health-claim-exchange/protocol/openid-connect/token");
configMap.put("username", "<Email ID used for registration>");
configMap.put("password", "<Your Password>");
configMap.put("encryptionPrivateKey", "-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEF=\n-----END PRIVATE KEY-----");
HCXIntegrator hcxIntegrator = HCXIntegrator.getInstance(configMap);
```

### **Processing An Incoming Request:**
The participant system implements the HCX Protocol API Specification. The incoming request payload from other participants via HCX instance will have the FHIR object. It requires validation, decryption and other steps top process it. Below is the SDK method which will help in executing these steps easily.

```java
//Processing an incoming request to extract the FHIR object.
HCXIntegrator.getInstance(configMap).processIncoming(String jwePayload, Operations operation, Map<String, Object> output);


Sample code for processing an incoming request. For reference, a sample JWE payload is also provided.
//Processing an incoming request
HCXIntegrator hcxIntegrator = HCXIntegrator.getInstance(configMap);
Map<String, Object> output = new HashMap<>();
Map<String, Object> payload = new HashMap<>();
String outputPayload = "eyJ4LWhjeC1yZWNpcGllbnRfY29kZSI6InRlc3Rwcm92aWRlcjEuYXBvbGxvQHN3YXN0aC1oY3gtZGV2IiwieC1oY3gtdGltZXN0YW1wIjoiMjAyMi0xMC0yN1QxMTowNzo0OCswNTMwIiwieC1oY3gtc2VuZGVyX2NvZGUiOiJ0ZXN0cHJvdmlkZXIxLmFwb2xsb0Bzd2FzdGgtaGN4LWRldiIsIngtaGN4LWNvcnJlbGF0aW9uX2lkIjoiZDRmNTZkNzktNDkwOC00YTk5LWE4ZGQtYTNiNzMzZmRlOGQ2IiwiZW5jIjoiQTI1NkdDTSIsImFsZyI6IlJTQS1PQUVQLTI1NiIsIngtaGN4LWFwaV9jYWxsX2lkIjoiMWUxNzk3YmQtZGJlZC00MTkyLWIwYzktY2VmNzcyNzI0YmU1In0.NSYks0P3BizbgpGF7GctpBSFDfSfap2V7AnZ5YCQMy_V0F6IZ1weRbZrBLHDTnPwPOBGGfctXpyqiXfvldMjCI_maakNjagsyC2x0pFC6NGmYhTwjqWmpL2CCaneBf9HikqwuI2cJTK8-DNOkbT9Qj8j-NxyGv1NX8UFI1K90t9e61qJ_Xurp6Qrrt6X_fiW7Jx9Vm54kCS7ZUfGK2rw_EOvc1VydsdWnUABOcmbtcTJiSVecNdRYxiKAIsiZCHULdd92a3hzbqFbyfRu1GqmuEyoimjd7jvdDSuB4bhE6WCIF-wB9Z5d3mj1ZXS8AEGT9YuSFLQTyPfo9Di3-Px-Q.0MliuZkDRADgQXY5.LWL2_vhMA8BEYtmg8SFXVoDNWvwWLZcJ9KxOHR7VWdIRxsO7PfNk7yMhctRtmAnyaaNZYZb0e3nVTai-u9aSixhmbxq6rFe3an0HbPi43BIT4ytal-CRxWoTaoBMZLmNqSr7GAvAXxNxtkSpDRUMlGg-tGdrHmcW6b1KAbA2CxE62XlaMMt5DNarXMSBDMx1_Pz0x9QqwFY-8O1GOIwPVZF59fh2e8ytlsvTh1fgiMZSOLETzN5pgCWGacc1D-tSmDD5kDQZm3SPgwZMbkco2xmKdvfdZa1AX7rDrYO6Pl-WFxv_5SO1CWwtR45tsCCp4pIOv3VUIjwv2YlmoTgXXg-EoLNgA4N8sw4kXLNtV9hMys2JdyQ93quEcFGnPO9jMg9seAT3pq_llpkN1FqE2x7zwWFeAs0g48IFc-39td6kdgOnuVEVsGAyE_OEst8TAhvRBa7EOeZmZpM1fiANdLor2xKWOI2K3o52R2XOYnchOWwjF07eWOPYulvSZQPBc1LZ_1j1myJz4qNH6aKWxkFT99FBddQ_UA49SgHRhYNScs27Ycw0BEZso9UpiXoLDJUKIp0Q717BkM5K1DdwYixkIMSEAM2HZHRaD0fk_8C0Xx0C1WxpZAhOtjKkesd1O2iUA2YbOyClgdASSUjO9KWrSgUevdHdorUDfYjvB3Kb-pjpZEyU18wTUTMg1Pb8Zjwluf_nuYM32K7N7TZlc-mdIwy8VX956eO14hZJNLFAiaf4xQskOCPiuahxD22kn1wpGEbShwE4TJYAsXTlT1xhA86CmDZwxaXSGzuYaNcpsokFCwAPkJ6axQbkPowO_CYrY9Ma5s0fwqdnCyZeUCl5ugeK-T9DtzuXBPsXi5eVky2rvqcO6cXCGCdrNfKXnc7Ehwtrv_ZxaUg0ORk_qVuGPWtfj0o9Ww2hUe8mXh7gp3d74jkDwNHV6X4S4qLApICRDU-Fw3kHZP5S1nvunjE_0FT4f3b4-SR_HAhjwl4-Rw4pRbX4Vu--CK6NfSgmNsixyapPm28tAzDfkHTDhRcHUi67fwGTEYKQvdk0JlV6zPSPopot2sRWRakyy_31oPCtz0qfavP0YT2dJWYGR9cSaJYIxz6S2cK2C0UOIIZw-TGX5zOnOzha1F-0Q3xPOswpHczoe-TafW3F22F5juRoNk7mK-BinUc4RBfBUkQWi6xTF32mLonuoeUkDRamMPX35P8ejYzBB2eDBb16QHIKyGl8R0aV_UYU5C661dG9xKvGJ98VHzFcz5j1J9WutBaMAeTRtROqRHH00psYa0oP2M4qVWR9eEP-jK9jtL8CdFGpNOiCtB7yF9WfE9N20Pqry88TODVMiT-MebZRHhvV3WCv3A3ZMgtVRqwaPoFp33I5GEDYuqDcHED_b4qZf65ZLNp5ptBSA_faSoeUJiRKRm8fep712gWgDS_fDmhdwfgIquY1fHZkofED0k_em28t0EAdJPpv0QqIKcte2vHkl65ql720YXWmYchBpDnclEMpExJg8T2CSd90jHvypi7KaPX2ahKv9zB1qlBTOFST4SWH_2I2_Tfvyhdqud7-ZWO_KFX9blDlph7WVs9rfWXjWV6zPEVquYDL-dJw5Lbk6mtHp2W7Z0ULHPrJqjLfqwjo4gcmD1TLm6LMVVYi7B2jb-_Vum9sxrWkIvVILP-LoCI1MYRvCj1QjrmO9ZFfqAGoIud_BYDKbnU3NoBJ0eWsvucGhrzaZ4-BYj_cOdUblQu08kmCHuxdhzSh8rZ4-vFrQ-9ZP83lHDA0tX5_F2MT_LoZQhph_NedK1bhhzBPaB4och2sY83So8dmgfcIlJXOkV_K6jO517pkHR7FBO-vQcD4850F7StoFZQoGRC4nqPTX6jCjMCNOMKoN1CQ58rmXrAI5LVtfo78iErXCXpsc-IO9HPfRSK-6PwA7cETFyy_To8ZjgozhJg6XUjxnYv4wD-9SpKMJwH39eihFj_bXWhXoHeZmXB8_8YSPf5L0ADirzoo6Pkxr6-zy1AmfNFrGXI-Zfb5BC6mRfRM3g9cTLr_fN3tESHnm3wzSh5dO3gpSHhE21eADFj7A6be8p2BgvvQl_ecm6KwPyO56-0HC4jxLTgZhyhtDuULWS4J6EPN6p2kE_EB_x7N5-RibkqfsbGMHMIRjKR7uzliG-WeYemvBdtkX7AmGuNVpRd_30fHmnE_yZoGdcrJjSeycv5EdqT7CeTBziIK2R78REHroEUJ23C3lvbfrH6Tz8XJ2ktokWxPXRqbwoqd3tcN7T9PoUgSvuSLJ32a7oMGzuYYOTS6JUWmBvGihfqKng-Pf9dxyZCSZ24nhxHNbw3HqW4gKI5viL5d9jUPtaT_GQKOEHNJpDeVMdV7lGgCbogkg5l4te0onSXDOCGEdnSvS3iuqtvT4-zo9tyrWIjUl5NQ8sZiK82-Nd_szEXjJzG88Xq6-oTZEUBqryI0FZjYNkE-ME-6L-zhR8hQdxwAwNMFXHW3EVT0ScCVYCo4EAGeQnQ-HnBEw7MouFlRLiNAf-50hOUBkBau8O8yHey1nrj2bsaeUcAksNUnELXiyCxhKemwFiZvLhCVgh5TTbVYw_Y89DA-MM1zwXC-k6IMqSW7JeCZogoeWr3Zx7l4eps0OvLUoqrAFV3ll8I_XToK-Ik6VQ68VdPj4GazF6cWaXnAHLfvJG_aXcGrAYpZtgUgClHOh2OwHZmCZMSYHdQCxQimnLelRfETan7cqYrkEaWJYUgP3UC3r4sP-JqEaWyYAoFvUoQ9G9U4msM82pagm3qmlXSSbKLPSD3tDwXXPJvZWfghw1VU8ZFpmiLKXanJ5eifWcwoDPPIuAAxYfKhsppg6S98JvBXHLkAchNovaxpjZopRMOkB1kjGBL_Y09d3D7MosAZKfZ6rloz6pHtrHz2RK7OaLQnrKZGVvYm3kuPRexi6p5gQ0wdPNMUNUU02FXBE1s_EASMX595qZE-zQ70Sor5l7YQ1CtM0EUaks1sVwRLyt8dyRi-ezsr6zNZFE4DTP7LUF96Mhb7wesntpOEOLTHeLbAJcC4rXUIN1guzU4vxEa8zqUzjEKy9zqQAUZ2e1mLpGDSoiYbdYHAmX1KRpfaTiUg6uXolUA60Vo-LAzlZ0ddU_NXOJJNSkx-F-U_0vcblf6qxTSUp35ppfxtP0_859WOIgWULId4jSEvi0hHUh2YGgbwh_uIGygjzISEHEdDHJ5mKDFrsGB_Wsw8VSM9-dPvf4C_ShvwxUkvuZh5_A9sjjUoVbz5nLcgTTsMMgM7xbwq-4E7dWKKMFy0gcy856lu5BlAIQ0VGtt6Cx3LtJ9aqOGchSiplhka67509gHuw33iPew2vdleAtJ3TAcSXkBg4Mw2dFqfWRH06gsm5pYjN8JR6RZHrxv8ldssKvV8E6udVUCRE-SkCMxbvCbSbZxNwD5AkbzbKlqf9pCtYNe9ThlgmV9HR7UE6pWPefCloH2DGs-iRSmWLUX1nyqQgs5KOX4YkWAi6He3IMMoFTL1SPNg1Z77RJZAYKvU2Hu2OK5mzUvyFcTLA3u8U6G6DUFjM3NFdhlEaxEbi6pAt7ChK2rRtGGUgJEEhIXU3_yqj5SGGJNplh576m4_Kg0o8XGjmz8LmbOhZflgMrVZsQUJ4UulqWpBOt5PBeBYhRNTi7EeBWQ7bCWQHYHdxJlsGb113wliz9vmtEov61LHubQV1-AAm-gp_FDOCZc9P4nv-auWzbxxjpdHMfngYPkEx6mrga6kst_7RsTkcedKFYjppmIKRtAi3MT2cBcsBGGblqdJ4tT6BOyDRIo3g8mw8RrbQeSKE3w27-tX3F9vJ9c68OMJ4vRsDHXg9XzIozhN7S0Jure-IUoOcG9XpgdW9xHuH_kcb-gg8v4MMRZn0EzPZRWIdVTpxIy7DWvHGgqYIG0JzKCXU3zaCnLow4XtahxVXWH6FNTi9z5dhiMPYIasbCVHMGmg6mjk5WwH0L2zqGsRBr_26ghrd6Pd_yxKk68IrTK7NCsHIvQHGJoqnepklu86AZbb0bST344sXJu5ulkZXMedg9JwrDSNAdmt0I57JmqEG6-29YUnC1V1IysfdttHIc59SNNHRrM4PtLh1_1qwJRNoFvJ8SxQv_GEd3GKf0tUv6kanuOGmof9thHRAozrTDhlms3_qrVe-6OWWAEiapvIRbZCp-unGW8xLHD3bH2pz47dZSTIme3LVAyNALhVV-u195ZUXz3yDKXsR1_uHBbvhk8-4PVKC1QBuZ_VF6frsYSIqoNkbOkOrNGieIWH56MnBjgaUtTYpAFo4U8zaX6TXm-xszNDotUbk269v-9mRh_mC4iBWI3Kwbbl8rbD_hVv_K8A87EJ2Z4ev53TNRrFsMyxlQjWfYdsR0Hfnvi8dFXqEgSU5M3m2RjlKccSaTcu22dt9LVaTBgTdzxrmyJhdtCJ76fpV4NoWiS8YMsK4l1d_e_Ceu4QKnVeJ-dfCUHLvGNnOZlfLu8Qc_ebPL81x1jzxOTo_Xb7GxQeJ35FUM1Zit4_yametU9kPdOUkRdudNlT9GIfwrk6FpM1f9_fNqLzklnEn-YjS5xij7zPS-bRVeRRQidzDEqm1ORi5S2nCoGlNNn_LAjbnSCbWynvfK2lvMQTZuh1RvC7eoLrGhQIStpC8DgjUa_DhXgkksZes1YeqE9vCSsOcPRsiChgyqmkOq36h_gzkDGCLDWyBRHM-M2upAD2DUNw0wFxCbVnA7bFTRL2CZ9K5uYzRFFMdI2txIKZXJ9Dq4LRcYFcLKWnkfHHqmZUT2cSvXToh2VzUNvGR-PZvcGuVRTZRPTF0YasdtEtPmAVTT8UMhkUfd2pl2SCpo336MXystN-bf9NqEC35OJXW3WGCQ1Z9KcOOvArnm0Ukrf1WcaM0w4zD9xpaKIXGYugaW-9ngUhq0eKfJe7wTuApEdGNIn3hRTGkjmxicYdcg.qmYQAxREDxNoAyCwk0e1Dw";

payload.put("payload", outputPayload);
hcxIntegrator.processIncoming(JSONUtils.serialize(payload), Operations.COVERAGE_ELIGIBILITY_CHECK, output);
System.out.println("output of incoming request " + output);
```

Please use the Javadocs to understand the input parameters and response from the method.

### **Generating An Outgoing Request:**

The participant system generate a request payload and send it to other participants of the HCX instance. There are multiple steps involved in generating the final request payload using a FHIR object. The outgoing request payload generated in two scenarios. They are,

1. Initialising a new request for a workflow.
1. Responding to the incoming request.

Below are the SDK methods which will help in executing these steps easily.
```java
// Initialising a new request for a workflow
HCXIntegrator.getInstance(configMap).processOutgoingRequest(String fhirPayload, Operations operation, String recipientCode, String apiCallId, String correlationId, Map<String,Object> domainHeaders, Map<String,Object> output);

// Responding to the incoming request
HCXIntegrator.getInstance(configMap).processOutgoingCallback(String fhirPayload, Operations operation, String apiCallId, String actionJwe, String onActionStatus, Map<String,Object> domainHeaders, Map<String,Object> output);

//Sample code
HCXIntegrator hcxIntegrator = HCXIntegrator.getInstance(configMap);
Map<String, Object> domainHeaders = new HashMap<>();
//Sample FHIR coverage eligibility request    
String commonFhirPayload = "{ \"resourceType\": \"Bundle\", \"id\": \"d4484cdd-1aae-4d21-a92e-8ef749d6d366\", \"meta\": { \"lastUpdated\": \"2022-02-08T21:49:55.458+05:30\" }, \"identifier\": { \"system\": \"https://www.tmh.in/bundle\", \"value\": \"d4484cdd-1aae-4d21-a92e-8ef749d6d366\" }, \"type\": \"document\", \"timestamp\": \"2022-02-08T21:49:55.458+05:30\", \"entry\": [{ \"fullUrl\": \"Composition/42ff4a07-3e36-402f-a99e-29f16c0c9eee\", \"resource\": { \"resourceType\": \"Composition\", \"id\": \"42ff4a07-3e36-402f-a99e-29f16c0c9eee\", \"identifier\": { \"system\": \"https://www.tmh.in/hcx-documents\", \"value\": \"42ff4a07-3e36-402f-a99e-29f16c0c9eee\" }, \"status\": \"final\", \"type\": { \"coding\": [{ \"system\": \"https://www.hcx.org/document-type\", \"code\": \"HcxCoverageEligibilityRequest\", \"display\": \"Coverage Eligibility Request Doc\" }] }, \"subject\": { \"reference\": \"Patient/RVH1003\" }, \"date\": \"2022-02-08T21:49:55+05:30\", \"author\": [{ \"reference\": \"Organization/Tmh01\" }], \"title\": \"Coverage Eligibility Request\", \"section\": [{ \"title\": \"# Eligibility Request\", \"code\": { \"coding\": [{ \"system\": \"https://fhir.loinc.org/CodeSystem/$lookup?system=http://loinc.org&code=10154-3\", \"code\": \"CoverageEligibilityRequest\", \"display\": \"Coverage Eligibility Request\" }] }, \"entry\": [{ \"reference\": \"CoverageEligibilityRequest/dc82673b-8c71-48c2-8a17-16dcb3b035f6\" }] }] } }, { \"fullUrl\": \"Organization/Tmh01\", \"resource\": { \"resourceType\": \"Organization\", \"id\": \"Tmh01\", \"identifier\": [{ \"system\": \"http://abdm.gov.in/facilities\", \"value\": \"HFR-ID-FOR-TMH\" }, { \"system\": \"http://irdai.gov.in/facilities\", \"value\": \"IRDA-ID-FOR-TMH\" } ], \"name\": \"Tata Memorial Hospital\", \"alias\": [ \"TMH\", \"TMC\" ], \"telecom\": [{ \"system\": \"phone\", \"value\": \"(+91) 022-2417-7000\" }], \"address\": [{ \"line\": [ \"Dr Ernest Borges Rd, Parel East, Parel, Mumbai, Maharashtra 400012\" ], \"city\": \"Mumbai\", \"state\": \"Maharashtra\", \"postalCode\": \"400012\", \"country\": \"INDIA\" }], \"endpoint\": [{ \"reference\": \"https://www.tmc.gov.in/\", \"display\": \"Website\" }] } }, { \"fullUrl\": \"Patient/RVH1003\", \"resource\": { \"resourceType\": \"Patient\", \"id\": \"RVH1003\", \"identifier\": [{ \"type\": { \"coding\": [{ \"system\": \"http://terminology.hl7.org/CodeSystem/v2-0203\", \"code\": \"SN\", \"display\": \"Subscriber Number\" }] }, \"system\": \"http://gicofIndia.com/beneficiaries\", \"value\": \"BEN-101\" }, { \"system\": \"http://abdm.gov.in/patients\", \"value\": \"hinapatel@abdm\" } ], \"name\": [{ \"text\": \"Hina Patel\" }], \"gender\": \"female\" } }, { \"fullUrl\": \"CoverageEligibilityRequest/dc82673b-8c71-48c2-8a17-16dcb3b035f6\", \"resource\": { \"resourceType\": \"CoverageEligibilityRequest\", \"id\": \"dc82673b-8c71-48c2-8a17-16dcb3b035f6\", \"identifier\": [{ \"system\": \"https://www.tmh.in/coverage-eligibility-request\", \"value\": \"dc82673b-8c71-48c2-8a17-16dcb3b035f6\" }], \"status\": \"active\", \"purpose\": [ \"discovery\" ], \"patient\": { \"reference\": \"Patient/RVH1003\" }, \"servicedPeriod\": { \"start\": \"2022-02-07T21:49:56+05:30\", \"end\": \"2022-02-09T21:49:56+05:30\" }, \"created\": \"2022-02-08T21:49:56+05:30\", \"provider\": { \"reference\": \"Organization/Tmh01\" }, \"insurer\": { \"reference\": \"Organization/GICOFINDIA\" }, \"insurance\": [{ \"focal\": true, \"coverage\": { \"reference\": \"Coverage/dadde132-ad64-4d18-8c18-1d52d7e86abc\" } }] } }, { \"fullUrl\": \"Organization/GICOFINDIA\", \"resource\": { \"resourceType\": \"Organization\", \"id\": \"GICOFINDIA\", \"identifier\": [{ \"system\": \"http://irdai.gov.in/insurers\", \"value\": \"112\" }], \"name\": \"General Insurance Corporation of India\" } }, { \"fullUrl\": \"Coverage/dadde132-ad64-4d18-8c18-1d52d7e86abc\", \"resource\": { \"resourceType\": \"Coverage\", \"id\": \"dadde132-ad64-4d18-8c18-1d52d7e86abc\", \"identifier\": [{ \"system\": \"https://www.gicofIndia.in/policies\", \"value\": \"policy-RVH1003\" }], \"status\": \"active\", \"subscriber\": { \"reference\": \"Patient/RVH1003\" }, \"subscriberId\": \"SN-RVH1003\", \"beneficiary\": { \"reference\": \"Patient/RVH1003\" }, \"relationship\": { \"coding\": [{ \"system\": \"http://terminology.hl7.org/CodeSystem/subscriber-relationship\", \"code\": \"self\" }] }, \"payor\": [{ \"reference\": \"Organization/GICOFINDIA\" }] } } ] }";

//Creating an empty map to store the generated payload         
Map<String,Object> outmap = new HashMap<>();

//We are using recipient code "1-29482df3-e875-45ef-a4e9-592b6f565782" which is a Mock Payor registered in the HCX and can be used by any participant
hcxIntegrator.processOutgoingRequest(commonFhirPayload, Operations.COVERAGE_ELIGIBILITY_CHECK, "1-29482df3-e875-45ef-a4e9-592b6f565782", ", ", domainHeaders, outmap);
System.out.println("generated payload "+ outmap);
```

Please use the Javadocs to understand the input parameters and response from the method.

### **Overriding SDK Methods:**
Following are the methods in the SDK that can be overriden to customize.

**HCXIncomingRequest Methods:**
1. process - This is main wrapper class, which processes the JWE Payload based on the Operation to validate and extract the FHIR Object.
2. validateRequest - Validates the HCX Protocol Headers from the JWE Payload.
3. decryptPayload - Decrypt the JWE Payload using the Participant System Private Key.
4. validatePayload - Validates the FHIR Object structure and required attributes using FHIR IG.
5. sendResponse - Generates the HCX Protocol API response using validation errors and the output object.

**HCXOutgoingRequest Methods:**
1. process - This is main wrapper class, which is used to generates the JWE Payload using FHIR Object, Operation and other parameters part of input. This method is used to handle the action and on_action API request based on the parameters.
2. validatePayload - Validates the FHIR Object structure and required attributes using FHIR IG.
3. createHeader - Creates the HCX Protocol Headers using the input parameters.
4. encryptPayload - It generates JWE Payload using the HCX Protocol Headers and FHIR object. The JWE Payload follows RFC7516.
5. initializeHCXCall - Uses the input parameters and the SDK configuration to call HCX Gateway REST API based on the operation.

Below is the example of overriding validateRequest method in HCXIncomingRequest.
```java
package org.example.impl;

public class CustomIncomingRequest extends HCXIncomingRequest {
    
    @Override
    public boolean validateRequest(String jwePayload, Operations operation, Map<String, Object> error) {
        //custom implementation
    }
}
```

To use the customized class in process the incoming request, we have to define the custom class in configuration variables and it will be used during runtime.

```java
//Define the configuration as a Java Map.
Map<String, Object> configMap = new HashMap<>();
configMap.put("protocolBasePath", "http://staging-hcx.swasth.app/api/v0.7");
configMap.put("participantCode", "<Participant code shared with you>");
configMap.put("authBasePath", "http://a9dd63de91ee94d59847a1225da8b111-273954130.ap-south-1.elb.amazonaws.com:8080/auth/realms/swasth-health-claim-exchange/protocol/openid-connect/token");
configMap.put("username", "<Email ID used for registration>");
configMap.put("password", "<Your Password>");
configMap.put("encryptionPrivateKey", "-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEF=\n-----END PRIVATE KEY-----");
configMap.put("incomingRequestClass", "org.example.impl.CustomIncomingRequest");
HCXIntegrator hcxIntegrator = HCXIntegrator.getInstance(configMap);
```

Similarly, we can override the methods in HCXOutgoingRequest.
 
