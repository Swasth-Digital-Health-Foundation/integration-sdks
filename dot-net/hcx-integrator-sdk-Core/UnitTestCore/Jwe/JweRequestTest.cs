using Io.HcxProtocol.Jwe;
using Io.HcxProtocol.Key;
using Io.HcxProtocol.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace UnitTestCore.Jwe
{
    [TestClass]
    public class JweRequestTest
    {
        // Note : Copy Pem Certificate Key files on given location or change path.
        string fileNamePublicKey = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "X509FilesSampleFiles", "x509-self-signed-certificate.pem");
        string fileNamePrivateKey = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "X509FilesSampleFiles", "x509-private-key.pem");
        //string urlPublicKey = "https://raw.githubusercontent.com/Swasth-Digital-Health-Foundation/jwe-helper/main/src/test/resources/x509-self-signed-certificate.pem";

        [TestMethod]
        public void EncryptRequest()
        {
            RSA publicRsaKey = X509KeyLoader.GetRSAPublicKeyFromPem(fileNamePublicKey, PemMode.FilePath);

            string fhirPayload = "{ \"resourceType\": \"Bundle\", \"id\": \"d4484cdd-1aae-4d21-a92e-8ef749d6d366\", \"meta\": { \"lastUpdated\": \"2022-02-08T21:49:55.458+05:30\" }, \"identifier\": { \"system\": \"https://www.tmh.in/bundle\", \"value\": \"d4484cdd-1aae-4d21-a92e-8ef749d6d366\" }, \"type\": \"document\", \"timestamp\": \"2022-02-08T21:49:55.458+05:30\", \"entry\": [{ \"fullUrl\": \"Composition/42ff4a07-3e36-402f-a99e-29f16c0c9eee\", \"resource\": { \"resourceType\": \"Composition\", \"id\": \"42ff4a07-3e36-402f-a99e-29f16c0c9eee\", \"identifier\": { \"system\": \"https://www.tmh.in/hcx-documents\", \"value\": \"42ff4a07-3e36-402f-a99e-29f16c0c9eee\" }, \"status\": \"final\", \"type\": { \"coding\": [{ \"system\": \"https://www.hcx.org/document-type\", \"code\": \"HcxCoverageEligibilityRequest\", \"display\": \"Coverage Eligibility Request Doc\" }] }, \"subject\": { \"reference\": \"Patient/RVH1003\" }, \"date\": \"2022-02-08T21:49:55+05:30\", \"author\": [{ \"reference\": \"Organization/Tmh01\" }], \"title\": \"Coverage Eligibility Request\", \"section\": [{ \"title\": \"# Eligibility Request\", \"code\": { \"coding\": [{ \"system\": \"https://fhir.loinc.org/CodeSystem/$lookup?system=http://loinc.org&code=10154-3\", \"code\": \"CoverageEligibilityRequest\", \"display\": \"Coverage Eligibility Request\" }] }, \"entry\": [{ \"reference\": \"CoverageEligibilityRequest/dc82673b-8c71-48c2-8a17-16dcb3b035f6\" }] }] } }, { \"fullUrl\": \"Organization/Tmh01\", \"resource\": { \"resourceType\": \"Organization\", \"id\": \"Tmh01\", \"identifier\": [{ \"system\": \"http://abdm.gov.in/facilities\", \"value\": \"HFR-ID-FOR-TMH\" }, { \"system\": \"http://irdai.gov.in/facilities\", \"value\": \"IRDA-ID-FOR-TMH\" } ], \"name\": \"Tata Memorial Hospital\", \"alias\": [ \"TMH\", \"TMC\" ], \"telecom\": [{ \"system\": \"phone\", \"value\": \"(+91) 022-2417-7000\" }], \"address\": [{ \"line\": [ \"Dr Ernest Borges Rd, Parel East, Parel, Mumbai, Maharashtra 400012\" ], \"city\": \"Mumbai\", \"state\": \"Maharashtra\", \"postalCode\": \"400012\", \"country\": \"INDIA\" }], \"endpoint\": [{ \"reference\": \"https://www.tmc.gov.in/\", \"display\": \"Website\" }] } }, { \"fullUrl\": \"Patient/RVH1003\", \"resource\": { \"resourceType\": \"Patient\", \"id\": \"RVH1003\", \"identifier\": [{ \"type\": { \"coding\": [{ \"system\": \"http://terminology.hl7.org/CodeSystem/v2-0203\", \"code\": \"SN\", \"display\": \"Subscriber Number\" }] }, \"system\": \"http://gicofIndia.com/beneficiaries\", \"value\": \"BEN-101\" }, { \"system\": \"http://abdm.gov.in/patients\", \"value\": \"hinapatel@abdm\" } ], \"name\": [{ \"text\": \"Hina Patel\" }], \"gender\": \"female\" } }, { \"fullUrl\": \"CoverageEligibilityRequest/dc82673b-8c71-48c2-8a17-16dcb3b035f6\", \"resource\": { \"resourceType\": \"CoverageEligibilityRequest\", \"id\": \"dc82673b-8c71-48c2-8a17-16dcb3b035f6\", \"identifier\": [{ \"system\": \"https://www.tmh.in/coverage-eligibility-request\", \"value\": \"dc82673b-8c71-48c2-8a17-16dcb3b035f6\" }], \"status\": \"active\", \"purpose\": [ \"discovery\" ], \"patient\": { \"reference\": \"Patient/RVH1003\" }, \"servicedPeriod\": { \"start\": \"2022-02-07T21:49:56+05:30\", \"end\": \"2022-02-09T21:49:56+05:30\" }, \"created\": \"2022-02-08T21:49:56+05:30\", \"provider\": { \"reference\": \"Organization/Tmh01\" }, \"insurer\": { \"reference\": \"Organization/GICOFINDIA\" }, \"insurance\": [{ \"focal\": true, \"coverage\": { \"reference\": \"Coverage/dadde132-ad64-4d18-8c18-1d52d7e86abc\" } }] } }, { \"fullUrl\": \"Organization/GICOFINDIA\", \"resource\": { \"resourceType\": \"Organization\", \"id\": \"GICOFINDIA\", \"identifier\": [{ \"system\": \"http://irdai.gov.in/insurers\", \"value\": \"112\" }], \"name\": \"General Insurance Corporation of India\" } }, { \"fullUrl\": \"Coverage/dadde132-ad64-4d18-8c18-1d52d7e86abc\", \"resource\": { \"resourceType\": \"Coverage\", \"id\": \"dadde132-ad64-4d18-8c18-1d52d7e86abc\", \"identifier\": [{ \"system\": \"https://www.gicofIndia.in/policies\", \"value\": \"policy-RVH1003\" }], \"status\": \"active\", \"subscriber\": { \"reference\": \"Patient/RVH1003\" }, \"subscriberId\": \"SN-RVH1003\", \"beneficiary\": { \"reference\": \"Patient/RVH1003\" }, \"relationship\": { \"coding\": [{ \"system\": \"http://terminology.hl7.org/CodeSystem/subscriber-relationship\", \"code\": \"self\" }] }, \"payor\": [{ \"reference\": \"Organization/GICOFINDIA\" }] } } ] }";
            var payload = JSONUtils.Deserialize<Dictionary<string, object>>(fhirPayload);

            Dictionary<string, object> headers = new Dictionary<string, object>
            {
                { "HeaderKey1", "Value1" },
                { "HeaderKey2", "Value2" },
                { "HeaderKey3", "Value3" },
                { "HeaderKey4", "Value4" }
            };

            Dictionary<string, object> customPayload = new Dictionary<string, object>
            {
                { "PayloadKey1", "Value1" },
                { "PayloadKey2", "Value2" },
                { "PayloadKey3", "Value3" },
                { "PayloadKey4", "Value4" },
                { "entry", new Dictionary<string, object>
                    {
                        { "entry1", "Value1" },
                        { "entry2", "Value2" },
                        { "entry3", "Value3" },
                        { "entry4", "Value4" }
                    }
                }
            };

            JweRequest jweEnc = new JweRequest(headers, payload);
            jweEnc.EncryptRequest(publicRsaKey);
            Dictionary<string, object> encryptedObject = jweEnc.GetEncryptedObject();
            Assert.IsNotNull(encryptedObject, "EncryptRequest Pass");
        }

        [TestMethod]
        public void DecryptRequest()
        {
            RSA privateRsaKey = X509KeyLoader.GetRSAPrivateKeyFromPem(fileNamePrivateKey, PemMode.FilePath);

            //string encryptedString = "eyJhbGciOiJSU0EtT0FFUC0yNTYiLCJlbmMiOiJBMjU2R0NNIiwiSGVhZGVyS2V5MSI6IlZhbHVlMSIsIkhlYWRlcktleTIiOiJWYWx1ZTIiLCJIZWFkZXJLZXkzIjoiVmFsdWUzIiwiSGVhZGVyS2V5NCI6IlZhbHVlNCJ9.WsyAZi_xhTYtNMmeFgPdwO7F7cTnMQQbGJvuu54vM9ZODgsXza9HAfFt0aepZH_-PCTLUQPD9MjPnebyZsifAWKf1FjazjBqj5ArC-zgdUoGelnpLrPH4_hUWxm3VFnTwts1ZlHFoJEr1srDUip5HUn80z6-8UpS7DWyvrLb7cXkehZTnoihd2wGsqQO96ggaWC05yGsl9pmf4ouK_sBXD6YOYEEBhGXTOWNd9ckxdWc_RUV6H4GlAokovEJb8f3xEJevg8ejXhS-u__l3fzBH3xLutEDT32FjaTBAcZmNitaqvgKZsQtIrovGYhvV57H72LlDyaS_i7QjxG5Uz3ig.Le-KA2lhH_LR6Pgt.l564PDjcd-cSt05HfWYdCWMOciSxKPJouuUNMc1SpX5WesHUt3ZWI0p-jhPtjJqOT50lXiRhSj9FALlpIyF8OdHpv_4KfJ7a7DsFIo2VYbvdFsi86qgfmiVVIRGxqTwGprJ4DFODXlA_jBeaWJP_WJavXFFYKCvGNoSZtzFtH4zJR3QhDPb9lfJP3I7NPZT7DQJKM2xrugGkWaO5ES8WVK7f2QJFT3jI0hfxJx3hfA.TWbfx4t7eXiOS-v5yk_ZmA";
            string encryptedString = "eyJhbGciOiJSU0EtT0FFUC0yNTYiLCJlbmMiOiJBMjU2R0NNIiwiSGVhZGVyS2V5MSI6IlZhbHVlMSIsIkhlYWRlcktleTIiOiJWYWx1ZTIiLCJIZWFkZXJLZXkzIjoiVmFsdWUzIiwiSGVhZGVyS2V5NCI6IlZhbHVlNCJ9.WBAyHhEOOnpbnF8UU0s3Q3mIRXZ_u_kp23T9UzR_R-fmvVtPZiwDtaavxqmsV4Xqfrm3QBAwnkZFUf2ZHhvMa0jIYnOVShpA0m0BJrgX4FOiB71aLa58qHqU6hNcJvxsdOA61qMtAlph17OQUxOSUHZQ4Iv33aIJMQ2BMII5avyWp5DWqP_TXdJ7yWrx5PlXsVW2c4ZsZjJl6LQn8T8y7pcivMvCHOmmAnNaVYmdOWduPzGUAeBG32R2EyLrvlIDjmZ-371178slJGrSwrjSqCwbFiwGFr7DU9oBdYCnSyA5pmB4i4RoF3o9sXTqPLzvY2FMwCU-n0PoBY0HS8OQPQ.e-nD-6CZIwswjlp3.o287wizfbNJqD34-IzG8E_IpNf4bhrONmoj104pDWtP-VHZVnGbv8mH9fEEoGJL7Yr2Dx9nF2rqSnlZ8Gp6H7ZNZP1lHY_c-9wr9E53Mn2TQgp1ZYqRYh8hoMKKXXlcQEyYcIk_hO0753Pvij4g4U6bZ36TxH-3Oky_DdsKRtdIjfexaDqMa4agq_KVWHtRDFF60ip0pReev_jjOT__uWpFDjKHba5uh-6ID9EQfiJ1dgJXrD6IZMQtw-STZQxoxh1eynMcOVc8I7LBt2onTeSBauAHvA_dAbyC8HPIAWQjr2nmh-Aim6JmXp5oGY5KgR54fpMBnh4wDMM1SHUVrd_6hjPdxkLIrH_Z13E17ujZGzC7Zgce4m-hWF1BNoZW6OgRcGRS62TWDMunmXkaB6GBCQnvLuf6-Kaeso9lOY9I8G-0EiMGh0s_9k9fMrjQbxCuKyGG3z-501mfC-Xsel59vN8fGuTnnijs6ju9QT19mcFBPnP37xV7A25L_K5Bq2h3mGVJB2t2uyX79NOF5XLlMrXL2eiwQ_i7mYUbhWWFVyw-aLysC0uqSMsMXZPanwXJTiDQYA2hPs-8g-FF4DajSXFFUBEIwzbQrz2wXBn05QV9aIoyGRFoiFKrOqlJ24PXAF36z3KTrh1b3lCMBnjlV05OwE1I6ySSJHT-_oI9GoOcb6JS4Fwlw_zhKIVblG9lebjw7ztWykMC0iDGWduIK_WqdCMi6DpyBHLY7kItJeF3iYyQ9Kql-2Lmyax3BbMwO_-ejiW8E3CsrD5i660bXgaDIrQGqamQ7cRKLGmT9aaLECwtQxDQGOflWGzasJmps3wbTCY3n8jllnAi_AWXYwQPcn7NCNXyOg6GfZoPrDdsET1OmQI5CtXfHyXP30ZFTT4J6CGpNhJBW-uNjQRf7nUh9tbX7D3-eictQyKgmYUVOvQG6fORI7mSDavbL8OulIBwn8TOONe3Lcnwg_5UfEDkixKekLJs0ryXOwRE0adjK9a3OaxolX6zv26lWCYDD7_VppO-IZFcVHEM-UuSlPanJbQSqPcw-ym36_66LA2tLiosmj-7q5gE2Dx_10ooy-k_0LjBml0S6NHpeMJc858vpCgm26sCXuv91DTPbx7-jzxqjs5UEVyawOahlMGEVT5nsR-r1AEtRAiYqQAo0J2mCgd--QsNO9k9BSXRCvE9iWC2_bBFwVwqEKPBbKFmtSmheO29vbw3yleVuxoKE1WvRqajF_mvS7VYLPwK8pIacfwfeJMFzdZkCYQApzC4Yq1fANYVbXEdwSr8Bjzced6kGrsJlaUNUT8lA266o9ZDfEu5JvWKYJkVjVIC7injKRDbkIRrDSjjM6d4Iqks9O1lhVqSi33ZOJrtu-DTynt14H492gER4aNEPdXseXYAKN-SX7cCaagkjXsoCLbTlrs1cqTmYKAtu9U623dCn5qxPkXK-tUDD9Wv8BdisWWUe0PQ7DUJ5k01OzaL_YGsmQ5bf45KXA4KphcZZ1rFGbiQyIRmSj38vwy8VOfIfVYbuHGORS2Dj7QgEXKqnGYMfqZRhIIqSYmLaHqjkg86ifvb9Ww97O1IrvHGvpvYSDWwhlkt3PIwDNr6cBc035ixkWFbnbqlXGvdljZKwQLyYvgfyvzthxgtFqZtuQ0pGqikkHGqfNQorS_PZQ13uy4_hT1PTnlMmD1x70NVmYQfJ0FHo-AtdlDOG_N_KRk2qB6ktOeH66hvTsy8S5tYJnzYSVH9oGlOTKKqvZA5Ix9P-exmQds6NQP1qhOh23qOjZn6NZi_4leoPEDkWmLLyMpbaHdqCteQ-JxK1juFzSVePgvGSwkDMrm9FCnU5fCxyvxrdxkRZ9Ofq51pF0vnJ8nxE7LA24odjTcTaW5Jd-37Z6zX_5B9l4NdhvuBIVtbIleZlEJUG0_JjyzVtYhYquR403uBIAURImILRRmxwO1WT6xpEe2CKZI5vgq2SkSgaL2rcZi0Cs6-VZZBdxi1VyQJ1NSeMacstQTMJxL87PYeDMU1cRUzkpFp-EPNO7dB9xU0mA6II36U5c5byMygsCPH75mYLQh7Nlrwtwoy6fo3H3P0_z8I6CDDa0FiJB5HK-wuXikWVCabb5LNdqPbX6wBBwide5Nt5LhmbgFgbj6rVrlgsp64gZijSO_TVciTwnnN3XdEW-dQgeSJgCpFgO2l2BxvSHH51N64nDGBszSxJLbTZotuWqW_M42RuzEYfHScpzPMvOA7MvESatcX1OT606gAnuCUUfx0X3EHzbYCbvBBr1XsGRnklsIgSCD0bZrol5iDiXKv92sLR-gdpWExza_suinHa6mcQbDOVIeCEzXcavRwP4EtMn77Yc-_c0_F6Ecnkvm7hz7FU3xdyMh2V1DahhiF07ng46zQEzq7svxzOlqUQ-5QRH_YMEM1oZEaO6X2KgBbjqledaDqOgfL9rP6j5CZaAyubd-m8Un8g1b-z7yCJo9y8Ivxui4Cf7q5ZuKMAV3qmUPrLoOWnKYXNLe9SjpG6g4uwgJq7QAr_65vb9hr1Ls6fNbYYN7BAEwfwJJKfGhI7MixLpdvOXa9lwNW4WSg-hfoJekB0cw9sSJa2Q0JBdXgnT6tMZOWJSLyl0VZN6aiDqST4VNc6OdW2HJjHsDfTdtauG_ahOZS6boLDx9kJqf6RS97BcedwN5UfF6vuKskfUOFwci4dTqShJJ21_Waf4W_mat20iYs90iZh8kgl1dPK24XutTPsXnyWcKwT54Cdf7-_cvLeYeUJd7UdeVx59KxxbD1v5oAra8P75j2Ir3kdhwanfrcJuDJAyIDHqsdbGpqwJqxxvb5UwTxvO46kklUnOKi_V2ZfCodSJ7xG4SIHlkEPQFMeqhiwWS1JH-UcjZElnurDs2BoUsj9JCtwirq_0q1UGE5Z0xcKKN3lLh-6NPdkDCMFfFh-y-WUiih1WJd7p2DL4ne3LwluhsxYgpUwr24xouQr9XzBLkgChrmQPuwP3t4Gc6t9qQr-Y11ytAJWdbFdUyW2rDZ63YFEds1r9ENqyb7REf_MkjBOdamdIlJC8EVt1Y2P0NJUVqjhkVKDK0jUZrUHW0nf8-OTVaBi4etaWDsUBNYBqxf2N3yvqw1ZxZC7j3cYPWWYrF762rh4XIi3aQPV5j6qG5OIjjqpfWS9licHl_i2Jg6BYEMWUixHqk-s7XyIt8ctFaumUoe0JkYLaaalZnuYewuXha4EgcveSyhJCAb8PmtMcmS9Ife6W9XWeD4w7CqISuD0pPWPwSIjN-QQiylB-miAUQ5D07WuOCgXxoOfLUtYzOiIswQIsTsNt92qu4CAeChPKGkMihBU5aEsUEppxW6BZ3uRq7re8xr3fZp-aRA75axfYDZunNbkIDbnTJE_9Cy2akSdPqrw-4BuaJNXKw_Mo2p6v9T9wAWFHIPoye3ojVs0vRasMoQBHHe1SeHNJISogEAbHxgB_ZtbtRSEltlIuAjuqGOrGaRsQWe-Bx7sT287odTNYeU2-FZra0B3iJD61Dm6whmMrYYJtrWGPh5z4cAt5zIDtfVxnYM7KI6rdEd0Ay6SSPO2SGFydV-cRpwqHju5s4fHgFgE0KDipxS2Ev1M7RstDO5X2nC40E0EKnjylHS6f6SlT59md59QciKe0m1Z24adBi-at08HKw7bYRvyVT_rgvHCCcph0-dm4Mp84Hf_iNnh3iEAaYkKkwFT1p6KyqZF3wgJn02jbKJIHWOiVbx5O9lFPZWuhDSA93DtheFrvlglpaAYAsMXgkQuTX4mCMIv_5fpWKQtLQl7GPy__wCDfWbJPfP3HemEheIl9xcH9e16gnnhuPyHTkhmJjSVlAvuzJaw9ZdLo7IV2E-AFUt2SWcXBpzVBeHSgbTEJKlTLjjr00MHULeZjMQxE3OTy8Ndd_aby69RO2sEPOiIrOHuxCPrmmeUqunyQL_W9HdflzLLBqfwWGMhwC8vmk9kRbQRCzUY-CquETCyebqZ3uo9mGLY8KXv-Bw7OdzVVEnoFjawKTV6Qe9CaBGdG9OdIQXBIuWZ4bm2G4BO2YtRmbhXqQxEXXLEMSdHcB64qsuT_bCYruBm5GEQGWCNhrMFxUjUd3PU4bISMOHDbBUDypTjTRfDpqBY31HmFg3vf6OWIPw8ISkL2E-eBOpXO-sxy34C4UJFGXbsl2iDkulXFAXhJtMJTJpT8-d3wy3WxVdtjw3_xzP3MbIFNd9u_AutH9a18MaTzNkZUnO-1X0oKTd4vlAOPEOcR3gbA3110d3WNwre98U19eei9VXsml3jrFG20q_CXfBEuCbOiAnMimjdfIsnD6UngCa0hc_OfzXU28fgk6oOiqENpqY08ff00Rw2F7EiMzs5usot_69N9qp3pI0mIABfUrHuvV0QTaZAPZEN_sT4KetyUIkjvYz96z2tqsH7zecDCupSO3uzZoxNzH7AqNPefcODbIqSyhNtQnmWBUrGZvuAlJIzVa9hForRwS7PhfESenOmBR4oFx4LSoQT9Dj_AA2j60TRhEnzly5bSMEdeNHPZs3mpOvCZDhleARoxaZL0hJayTyhWoj1XxVt06D18uJcODpXxpxlGwyiIavTOxp3DDl01WzIcOq-UCPKGp5VuQiXbZ_kyylzGKmu-ccIFPf0OE0X-iebB6sDOoyfWzNb0HT1ifjZi05TWg8XzuPfxypKxh2G9qjoA9oYvfMyzSav5zkB7K-BIVhIy477YSFjdkBb5wrJToDzB-4Pt9HvUfef84xMGue8KENWVIc4gIC56BdCbossQVyOA4qdI72rTx9MmxVVYmyHjHR-0VED9426KR-embALlP9AuLa-7xzLjqmxtbhLyi5dTi_NtP7ZAQEqStO_LNWf-pICKa22UQ.NSOKsr4UtwlZAdLDqBRygw";

            Dictionary<string, object> encryptedObject = new Dictionary<string, object>();
            encryptedObject.Add(Constants.PAYLOAD, encryptedString);
            JweRequest jweDec = new JweRequest(encryptedObject);
            jweDec.DecryptRequest(privateRsaKey);
            Dictionary<string, object> headers = jweDec.GetHeaders();
            Dictionary<string, object> payload = jweDec.GetPayload();

            Assert.IsNotNull(headers, "DecryptRequest Pass");
            Assert.IsNotNull(payload, "DecryptRequest Pass");
        }

    }
}
