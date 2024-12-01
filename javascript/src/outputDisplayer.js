import { HCXIntegrator } from "./hcx_integrator.js";
import { HcxOperations } from "./utils/hcx_operations.js";

// const config = {
//   participantCode: "testprovider1.swasthmock@swasth-hcx-staging",
//   authBasePath:
//     "http://staging-hcx.swasth.app/api/v0.8/participant/auth/token/generate",
//   protocolBasePath: "https://staging-hcx.swasth.app/api/v0.8",
//   encryptionPrivateKeyURL:
//     "https://raw.githubusercontent.com/Swasth-Digital-Health-Foundation/hcx-platform/main/hcx-apis/src/test/resources/examples/x509-private-key.pem",
//   username: "testprovider1@swasthmock.com",
//   password: "Opensaber@123",
//   igUrl: "https://ig.hcxprotocol.io/v0.7.1",
// };

const config = {
  participantCode: "hcxtest6.yopmail@swasth-hcx",
  authBasePath:
    "http://dev-hcx.swasth.app/api/v0.8/participant/auth/token/generate",
  protocolBasePath: "https://dev-hcx.swasth.app/api/v0.8",
  encryptionPrivateKeyURL:
    "https://raw.githubusercontent.com/Swasth-Digital-Health-Foundation/hcx-platform/main/hcx-apis/src/test/resources/examples/x509-private-key.pem",
  username: "phirangikrutika@gmail.com",
  secret: "HlU$G6s7l#orq@B2jzxxJi6h",
  igUrl: "https://ig.hcxprotocol.io/v0.7.1",
};

// const config2 = {
//   participantCode: "testpayor1.swasthmock@swasth-hcx-staging",
//   authBasePath:
//     "http://staging-hcx.swasth.app/api/v0.8/participant/auth/token/generate",
//   protocolBasePath: "https://staging-hcx.swasth.app/api/v0.8",
//   encryptionPrivateKeyURL:
//     "https://raw.githubusercontent.com/Swasth-Digital-Health-Foundation/hcx-platform/main/hcx-apis/src/test/resources/examples/x509-private-key.pem",
//   username: "testpayor1@swasthmock.com",
//   password: "Opensaber@123",
//   igUrl: "https://ig.hcxprotocol.io/v0.7.1",
// };

const config2 = {
   participantCode: "hcxtest6.yopmail@swasth-hcx",
  authBasePath:
    "http://dev-hcx.swasth.app/api/v0.8/participant/auth/token/generate",
  protocolBasePath: "https://dev-hcx.swasth.app/api/v0.8",
  encryptionPrivateKeyURL:
    "https://raw.githubusercontent.com/Swasth-Digital-Health-Foundation/hcx-platform/main/hcx-apis/src/test/resources/examples/x509-private-key.pem",
  username: "phirangikrutika@gmail.com",
  secret: "HlU$G6s7l#orq@B2jzxxJi6h",
  igUrl: "https://ig.hcxprotocol.io/v0.7.1",
};

const fhirPayload = {
  resourceType: "Bundle",
  id: "98aa81af-7a49-4159-a8ed-35e721d6ae74",
  meta: {
    lastUpdated: "2023-02-20T14:03:15.013+05:30",
    profile: [
      "https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-ClaimRequestBundle.html",
    ],
  },
  identifier: {
    system: "https://www.tmh.in/bundle",
    value: "7ee7ee1a-fcad-49c3-8127-aa70c7a4dc0d",
  },
  type: "collection",
  timestamp: "2023-02-20T14:03:15.013+05:30",
  entry: [
    {
      fullUrl: "Claim/bb1eea08-8739-4f14-b541-04622f18450c",
      resource: {
        resourceType: "Claim",
        id: "bb1eea08-8739-4f14-b541-04622f18450c",
        meta: {
          lastUpdated: "2023-02-20T14:03:14.918+05:30",
          profile: [
            "https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-Claim.html",
          ],
        },
        identifier: [
          {
            system: "http://identifiersystem.com",
            value: "IdentifierValue",
          },
        ],
        status: "active",
        type: {
          coding: [
            {
              system: "http://terminology.hl7.org/CodeSystem/claim-type",
              code: "institutional",
            },
          ],
        },
        use: "claim",
        patient: {
          reference: "Patient/RVH1003",
        },
        created: "2023-02-20T14:03:14+05:30",
        insurer: {
          reference: "Organization/GICOFINDIA",
        },
        provider: {
          reference: "Organization/WeMeanWell01",
        },
        priority: {
          coding: [
            {
              system: "http://terminology.hl7.org/CodeSystem/processpriority",
              code: "normal",
            },
          ],
        },
        payee: {
          type: {
            coding: [
              {
                system: "http://terminology.hl7.org/CodeSystem/payeetype",
                code: "provider",
              },
            ],
          },
          party: {
            reference: "Organization/WeMeanWell01",
          },
        },
        careTeam: [
          {
            sequence: 4,
            provider: {
              reference: "Organization/WeMeanWell01",
            },
          },
        ],
        diagnosis: [
          {
            sequence: 1,
            diagnosisCodeableConcept: {
              coding: [
                {
                  system: "http://irdai.com",
                  code: "E906184",
                  display: "SINGLE INCISION LAPAROSCOPIC APPENDECTOMY",
                },
              ],
              text: "SINGLE INCISION LAPAROSCOPIC APPENDECTOMY",
            },
            type: [
              {
                coding: [
                  {
                    system:
                      "http://terminology.hl7.org/CodeSystem/ex-diagnosistype",
                    code: "admitting",
                    display: "Admitting Diagnosis",
                  },
                ],
              },
            ],
          },
        ],
        insurance: [
          {
            sequence: 1,
            focal: true,
            coverage: {
              reference: "Coverage/COVERAGE1",
            },
          },
        ],
        item: [
          {
            sequence: 1,
            productOrService: {
              coding: [
                {
                  system: "https://irdai.gov.in/package-code",
                  code: "E101021",
                  display: "Twin Sharing Ac",
                },
              ],
            },
            unitPrice: {
              value: 100000,
              currency: "INR",
            },
          },
          {
            sequence: 1,
            productOrService: {
              coding: [
                {
                  system: "https://irdai.gov.in/package-code",
                  code: "E924260",
                  display: "CLINICAL TOXICOLOGY SCREEN, BLOOD",
                },
              ],
            },
            unitPrice: {
              value: 2000,
              currency: "INR",
            },
          },
          {
            sequence: 1,
            productOrService: {
              coding: [
                {
                  system: "https://irdai.gov.in/package-code",
                  code: "E924261",
                  display: "CLINICAL TOXICOLOGY SCREEN,URINE",
                },
              ],
            },
            unitPrice: {
              value: 1000,
              currency: "INR",
            },
          },
          {
            sequence: 1,
            productOrService: {
              coding: [
                {
                  system: "https://irdai.gov.in/package-code",
                  code: "E507029",
                  display: "ECG",
                },
              ],
            },
            unitPrice: {
              value: 5000,
              currency: "INR",
            },
          },
          {
            sequence: 1,
            productOrService: {
              coding: [
                {
                  system: "https://irdai.gov.in/package-code",
                  code: "E6080377",
                  display: "UltraSound Abdomen",
                },
              ],
            },
            unitPrice: {
              value: 5000,
              currency: "INR",
            },
          },
          {
            sequence: 1,
            productOrService: {
              coding: [
                {
                  system: "https://irdai.gov.in/package-code",
                  code: "502001",
                  display: "Surgeons Charges",
                },
              ],
            },
            unitPrice: {
              value: 1000,
              currency: "INR",
            },
          },
          {
            sequence: 1,
            productOrService: {
              coding: [
                {
                  system: "https://irdai.gov.in/package-code",
                  code: "5020021",
                  display: "Anesthesiologist charges",
                },
              ],
            },
            unitPrice: {
              value: 1000,
              currency: "INR",
            },
          },
          {
            sequence: 1,
            productOrService: {
              coding: [
                {
                  system: "https://irdai.gov.in/package-code",
                  code: "E6080373",
                  display: "Physician charges",
                },
              ],
            },
            unitPrice: {
              value: 1000,
              currency: "INR",
            },
          },
          {
            sequence: 1,
            productOrService: {
              coding: [
                {
                  system: "https://irdai.gov.in/package-code",
                  code: "201008",
                  display: "Recovery Room",
                },
              ],
            },
            unitPrice: {
              value: 10000,
              currency: "INR",
            },
          },
          {
            sequence: 1,
            productOrService: {
              coding: [
                {
                  system: "https://irdai.gov.in/package-code",
                  code: "406003",
                  display: "intra -venous (iv) set",
                },
              ],
            },
            unitPrice: {
              value: 5000,
              currency: "INR",
            },
          },
          {
            sequence: 1,
            productOrService: {
              coding: [
                {
                  system: "https://irdai.gov.in/package-code",
                  code: "E507353",
                  display: "Oral Medication",
                },
              ],
            },
            unitPrice: {
              value: 5000,
              currency: "INR",
            },
          },
          {
            sequence: 1,
            productOrService: {
              coding: [
                {
                  system: "https://irdai.gov.in/package-code",
                  code: "E925171",
                  display: "Hospital charges",
                },
              ],
            },
            unitPrice: {
              value: 5000,
              currency: "INR",
            },
          },
          {
            sequence: 1,
            productOrService: {
              coding: [
                {
                  system: "https://irdai.gov.in/package-code",
                  code: "501001",
                  display: "Consultation Charges",
                },
              ],
            },
            unitPrice: {
              value: 5000,
              currency: "INR",
            },
          },
        ],
        total: {
          value: 146000.0,
          currency: "INR",
        },
      },
    },
    {
      fullUrl: "Organization/WeMeanWell01",
      resource: {
        resourceType: "Organization",
        id: "WeMeanWell01",
        meta: {
          profile: [
            "https://nrces.in/ndhm/fhir/r4/StructureDefinition/Organization",
          ],
        },
        identifier: [
          {
            type: {
              coding: [
                {
                  system: "http://terminology.hl7.org/CodeSystem/v2-0203",
                  code: "AC",
                  display: "Narayana",
                },
              ],
            },
            system: "http://abdm.gov.in/facilities",
            value: "HFR-ID-FOR-TMH",
          },
        ],
        name: "WeMeanWell Hospital",
        address: [
          {
            text: " Bannerghatta Road, Bengaluru ",
            city: "Bengaluru",
            country: "India",
          },
        ],
      },
    },
    {
      fullUrl: "Organization/GICOFINDIA",
      resource: {
        resourceType: "Organization",
        id: "GICOFINDIA",
        meta: {
          profile: [
            "https://nrces.in/ndhm/fhir/r4/StructureDefinition/Organization",
          ],
        },
        identifier: [
          {
            type: {
              coding: [
                {
                  system: "http://terminology.hl7.org/CodeSystem/v2-0203",
                  code: "AC",
                  display: "GOVOFINDIA",
                },
              ],
            },
            system: "http://irdai.gov.in/insurers",
            value: "GICOFINDIA",
          },
        ],
        name: "GICOFINDIA",
      },
    },
    {
      fullUrl: "Patient/RVH1003",
      resource: {
        resourceType: "Patient",
        id: "RVH1003",
        meta: {
          profile: [
            "https://nrces.in/ndhm/fhir/r4/StructureDefinition/Patient",
          ],
        },
        identifier: [
          {
            type: {
              coding: [
                {
                  system: "http://terminology.hl7.org/CodeSystem/v2-0203",
                  code: "SN",
                  display: "Subscriber Number",
                },
              ],
            },
            system: "http://gicofIndia.com/beneficiaries",
            value: "BEN-101",
          },
        ],
        name: [
          {
            text: "Prasidh Dixit",
          },
        ],
        gender: "male",
        birthDate: "1960-09-26",
        address: [
          {
            text: "#39 Kalena Agrahara, Kamanahalli, Bengaluru - 560056",
            city: "Bengaluru",
            state: "Karnataka",
            postalCode: "560056",
            country: "India",
          },
        ],
      },
    },
    {
      fullUrl: "Coverage/COVERAGE1",
      resource: {
        resourceType: "Coverage",
        id: "COVERAGE1",
        meta: {
          profile: [
            "https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-Coverage.html",
          ],
        },
        identifier: [
          {
            system: "https://www.gicofIndia.in/policies",
            value: "policy-RVH1003",
          },
        ],
        status: "active",
        subscriber: {
          reference: "Patient/RVH1003",
        },
        subscriberId: "2XX8971",
        beneficiary: {
          reference: "Patient/RVH1003",
        },
        relationship: {
          coding: [
            {
              system:
                "http://terminology.hl7.org/CodeSystem/subscriber-relationship",
              code: "self",
            },
          ],
        },
        payor: [
          {
            reference: "Organization/GICOFINDIA",
          },
        ],
      },
    },
  ],
};

const hcxIntegrator = new HCXIntegrator(config);
const hcxIntegrator2 = new HCXIntegrator(config2);

const operation = HcxOperations.CLAIM_SUBMIT;
const operation2 = HcxOperations.CLAIM_ON_SUBMIT;
const actionJwe = "eyJhbGciOiAiUlNBLU9BRVAiLCAiZW5jIjogIkEyNTZHQ00iLCAieC1oY3gtYXBpX2NhbGxfaWQiOiAiNWNiZTcxMjItYTFlNC00NTI4LWJiNGYtYjRjZWZkOWQwMzc0IiwgIngtaGN4LXRpbWVzdGFtcCI6ICIyMDIzLTA4LTE3VDE4OjQzOjQ4KzA1OjMwIiwgIngtaGN4LXNlbmRlcl9jb2RlIjogInRlc3Rwcm92aWRlcjEuc3dhc3RobW9ja0Bzd2FzdGgtaGN4LXN0YWdpbmciLCAieC1oY3gtcmVjaXBpZW50X2NvZGUiOiAidGVzdHBheW9yMS5zd2FzdGhtb2NrQHN3YXN0aC1oY3gtc3RhZ2luZyIsICJ4LWhjeC1jb3JyZWxhdGlvbl9pZCI6ICIyNzdmYjZkMC1hNjNlLTQ4ODUtYThkMC02NzY3MmY2ZWU2ZTkifQ.B_KdeKcU9-U9lunN9U85BMHRgS4J42kjGxCiX0iD69NJDaZLtCmOn_nqmEt_q7C1qJzuoDzOa9zzGUD7rVXh4aKahzdqiERBNoTVdeSHepalSS-e47HZ4me3-3A9kBaoyQGzS6qx7nplobWxfv8PlBNCzKQKMuXEZaGS88UAVCUAIBXHgExJYJgogRPpw56PsNXweV5xluV1JX51XJeHIAdOXghsLithY52gmnvYCNrjuTL_gO-XVl5wCltSlM9sIBRoCXBQinmcYJICpIOdhnTKcEmYtqUB70GSReUC4LTxnISWgQ0GeGFYqxvo97bX3dpZjIVG1fTvuIrUXPeE1w.WcoC73KvODQghMRX.tHWRPLtfhF_1U2tBqMOxyM3IHVTX5ZQAyiCwUxoeY597OpT9nI5x7Hd3yPubbg48li7i3N7pVbtAE-qa34vZwQ8q7eOJQmJgePH52yc_l7rKVEy3tH2ztYQoT0KyFg6ZgkETp7i9Vis6-6JwXvjH-Ha7qGGyzFM1R2xYreSzMEFlYhThSrBV0cEKoE-036ddpbI8fznF7DBFslrQzpOFl2I9GkpkmDDyXE2L5IUGgnXNuE8aSsCfqGJIJPUq1ja3D3DBgNZqYbTH3mgXCQrrpeGXr75uybhWjcBvjQbpX5oUH0vmnGMsmwu8bjAbDSFGX-YQgAojLirIk555dExMERjUnWIQ6rI2jcBFF7qf6y0IC9G6qcqfReHrUkJkrxcg8ncRS-MBornOg-7iuZyn6l_n6gDwA_M5za7OlE1itLJ9Jg2ML17cdsnJ2AM8xnSuGSfueBcXcx7ZWwq9aV5vbPnCJbnIJKgwpsCAlyqE_9RosfQajsEnF9UxWeg3vLmyLWdv9_E23QZBF-uDTwYSbbYvePD4NmMxf1eIR4Y7iBegol56K1Jm4qY-BWiTeh0g-Q8fWqXVbez9yMYukkL3EMGwlIbhtQGxmgG4-ltMLP_JQBSBZy4pF9fonBaRcY2uQsYxIpFKlgGPyGeXexd4xCYhhhBq-Y6BBNZPiCFBJGu3XX8-_adjSVq_Dwpjh30jbEiAyzwpXaIOj2G0Y_kMuufJ3t01FSKGl0kPjtIHjk2jOtXpkqwUg9ySwvjx4nmc01Fm9DhvVse203VtE1O0lVbY4BWpTCTcCtnVZ0NKDjcbEepiIqThizOGReOptG6r1EWTJCQrMkptNqIIbriZSsPVQ5_dyX6f5Env_OuWNiKZtS6EmQ-zY_qSNhepdODkEhjfKe5lIwAqwL0ad_TdfYLC4URhQobwSQwWezxMkYXtNW47_0jEt-bQgImOLJ9Ag8i2iyMbJkF4px20558GDqLdBQpLXdwOF3sYdDEU6EDT2foXA4dh0xBo_1Of__d-dSoqrFX1KeAvmdah5Sn7r2FXQYYj7liWt3pEONNIXpeN7jJfxGIFVdBWyxtxqadCqxd0pKyiae2VqFDxKnLRl4A1sFb0E5HjxEvbUL5AsufzplVgap5xwbkQDue2VyJ6uKeRmenhZNqhOSRarfmPf-pgw52zQmvBJXfYvvXU0HgJ3nvRRs0F0ftWuPocKE4vtJNA1u9n-fwmOZ44vvY1xWc3ts3Fa0xi9uKDn-vGf0bWFMpb2uZmTDwlffTLHbWLpJcRVKM2BZLiGMPw9KHV77xw5bEUljiJ1elyjezhkJQbaib-nLYsRwKTXN5xp-1mIAlXgyAbIVwuG-_xYEY7_OpYU6WEPOxntpvfDXd8PCYvnqFApcl5yxwbEm5wP8S3e6x0qLN-aCf5tajwx_eIKF4MLjSZivH4MCJ9UvhlLg7Xnqdgc1NiApCbDxbeUe6YvjdbykQWvgUvC9quO35wTC_ybAL3AM-SVq4mrmz2wGzXiXyMvQpju09hph03Q4_J6scR85fdnE4BzxjXiVNC2kQZie9XzIAlRg3Q_WY4ZyaAyqXsrodvdd11t2j1Izv7whx9ha_VWiLD2lSPQGHmRHL641NogbMhK-4YXrCi-k6Pnero86NkkboXS425_Dh6FU6eEx47BjXToxKTfvI2Hn8apIoGJGAld_8s07k2KswIqEltZBpCbSePFQVxFd60HSyoQg3Sp7Fk872cTZIE6AGNBOAsB-6Jn2DIpic4sIswq-KBLOKqCPBPdosSqCF16JqkjsUGDq2E0IQhLt0IpOhBM5OdCRASi2Bd8cxgGvQjpaZSTO8e_3L4d6VEve4FqW7evwoj36kIKnRWPUdXA-cgMML1MncmLgk4T60H40bBLw6nCeKOGLp4GYmYDUj_UhCR_c5VHvbzU0d_rrTR0_ChrRcW6nPfjev_O1hL0nl36GDmZ7pmIlQ54jBxxVe5QX2Y3ib3EeW8LGVdLiLDLKBB_fFG8FZhA_APm14BwkKuMUUnX8YZc9ZF2Dmt2ydJWfmL3tyk_Cla7qQz05d0mM1yursQRm0BS6khOwJTwVcFSbCeQperXJ1nfdImfSfdI9tjVB6SWaOM8KJ11boWXhmuW_KRpl7jfL_HlglcxLyn08x8r_115pPSDBqa1CafEe3tcrQt_GXi0LuJ9i8mR_Vw0GQ65ZSSsEAynTTLlPau2nFi4AlRDqk7MWO7gs-RxTmPPQO2xsA-HMbAViGaIpqZxTohcLn8bAiB3nk_OMjuC-lTp8lrSvC_cJKgkH1DxlCLJrIzRbNWVv4F2eFWGRDnNgwuIts38y0P9G91WxIl5UExWN5W1Y4ezhp3DDLvT9oTr_TuxL0CeVenS-qyuuOZUAyNtutBtnBGI8KXQ4zmB7HLriPL6RDdAGwF6BbsPO9mJQAi_SF3BrPH0iszw4AUpSRZ03UNWTbo2Eix7-d6XMH50ABXmiv7eojxl5eKoFwwiNTRRGR15otHW1BZLWc0pZGmpGeAIQwarOaY5KP5ld59e-bpd1m9KjfkmkRfyDmz06C7F872woKpxhYiAE-r260rnARhV4G5J_NYDDwlSN913fbSGFGy0DAZPaHTXtXMB7UaxktYFcahQIsw3VQg_YW3PC79O3U6qtJnzUnM2fnLtlJvqP7L2iGQC0aptiVRP9chOHpjZKYikalgagUTXn3YH2uSO0-rPIxRT-P7PdPcDMR7cwoFr5pSkoN1JZ_Zr06v-A_UzM6iJhnWfkFaikTigOJGEERmh0DUmvd8aqZNWL32ovgR-eO62TKPGFtqM2eWFaZC3jvMZb8m1z_wMqQGctIVKZxcHLMoo6R9-AdtgeIAh53LUG6EWA55uLjDxJ6w6qABtIA3fBx_7es0xy7j_F-Ry9dT_il2K_i8kCU7d-Y0bwTx17SPdifcMm6UsUC6O9pAJ7a5Dz-m2SzJBLyk6EAa2OvjDEobeu9rN6j4xeErpgrawmPgQVhGW2OITnOs1P2Ea5RRrCKVlUqtpR6T5H7nFCF5pWneDsLgVuAMAbfxqAEd9NK-ESF7etp2IY0kKQ3WxOsw9Dj152RcSpsDrwinnZn8dlqOfsVo46Gk5s6L7yHNS9-czAhqcK1xTPOM3OYYgWHoDE5-st1ojFzaJu_5szqoC71Sjv9r8VLzzYsDuQiCDY_njtgWC177gnwwiXCfu7346CGtVO9GohTCXiDwdrhxTOO3mqRCWmS8yZDAkh0B9ZjgKDNbUXTeI5eRz8uftN9dQWqhJF3ng0lV8uo6pRmlmdOJQebUscEoXQxgHF9h54xZK9mxTgo3Yjh_KW64XIWtfcem14rTN6XSnGss9ZVtBPHtoMVOHAFBmz7_BA-12hHx5IggaV2YbJbqgqZjKP9uk_gHk35sLepmu-fdYvx6HL4d2IHQubn-R2M7LjH3YgylnnIYRtmQNqWarJCi5rFry-TmfEXfmRv51uiukfV6qrOxuIBuPFu22zPYzHjODpaJOpH9xhwNVshpEY9n14aVh_1SxaJzedi0spx-sOM9UXbQPCgyDipTnjOx8DAdhOdeALC3yvlViqpSWl3oEriYx6f6X7pjdDueynswyQ0fEx_ECThOWf2CCXqVVaDsdxO3gSDCLjIqEy4kk6OQDmRsiqudmXsJjWkiIcwuqt5v-YpqYxdNPk3iVMgifuv0uVTgq_GTjtFfl7vuOF8eU4yUEyTeGuZfwuDqTNTUJ7zxpWqOhCwgQIj0f9R1yECIjOEXk0gRO2MgFcrgRCrk66Ztlt58eYkxND-7c9LYzfVVZ31yVFjc2UMb5F5WF4hjbl8PdHvWYvG3Xb-fGq4DKwdIibd8shxiB9KNULeBb6jrTqx1o81h2jeUcnPLdkHvGZ2NCL2oGlHWBH_wMcPfRT1U3nR-0XgJKaCZLxby1BHQWkMv8HwPH1HRGDRAQTR23WbW8dKeSTzlaqhacxk3YfmF0ULmEJ8rv9opDCGfIgJ8gXg4-0noo4JZKn-k-IFE39q18CaD7leaEHz4838cqikBKCBTZSKU_qVgdzTmOgv6TPlXgeVQZrXtIs4-41I8-W1KSnkpvDUxQJh2Yc5vuR98y5cdUI-kWh-QC-eebKuEXznG7bSnJtkt70Hq3C15IUQBmVc111RzQhuRt0TX7bMn6XRI8zScA6p0WgYvbK7l-QGWipeXM6ekKaAs2FtkCkSo0LrDtVQnl5ntcuv8V2RNScoCEHytQZUXodsPs3YcZyBmCERNq74qJEWc_6yF3zdWiRKp7iBV_C2aFhnOahIVccXWbLEnrnHiks60E1nIKvyGhWI0t1SXB6QXlngGZsvZd8mAgrTRPhHNs827TWvmfUqm8x-kC2DGfv4ni76kPQFgDJLdetiZuNxDoSInqz6xtxyz6rxwnvoJm0KsULB4K988ICFKGFSx8xUM_NHzY6D3ak_m8HvYn_8yR0OltWgT8ld3Zx_jbqBvPMpPxMJ7wXDYTMUBSSmwDhddvDFmaDi1IDHBBJIeGUKuQ7vHlkJbQAbBRSgK_BGP-UJuK78bUn_GiJbRBmHG-hoAGndNNyFS5xCOTq7K8JQfdM57o6bqO1-e-IcHxrmbnHnPAwEBs35SbWFPQxE4NjuyuA5Rz-N9-7Mh4btni6DGVgbattmngDGCeVt6CgpaLRAA47_LzQqRIKKGDETTkqGljUCJWoAubHefn_Ijnqry78vFfHa-t2FH_oTSjFJg3PtLUiEJ-F9gPyGlZgyEq2zExGaiigm2sKFy6phMN_MHybLgguOT-aRoEHxEYGO1BIgknFi-_Fx9LMM2qmhYtkdbjP_IpJNUKtD4kM4Hnoqm3TxckDfNCwKO7W_Y-4yrIPYe_h3RlxypOixnhkzj_H06eP-3PhGXNG0dYMc0Ofww3zjWqL9u1LARE_goNCILAJKzTW42Lx1KIEt2EwJvlSaF01XO-ekUVgXJjsbA73TZl-bu7EyyeklXe8ufamhwWRgB4jeTuq__EjYgempltwFSV_zzWnyo7hG3VyYuiyHWvw4vr7TGVHBDcIp5PufKUTpj4-cRLU9LIcGes7riMy_nDHo61a834SxbijKF6tUGZo4VvVhWsRvlltK7YizTvxOhjLrD3wFWLLoXtXlmHrq9zUWnJR4iQIIt9q0bQq-O9hudrYIUhBMMBiimaHhpHrKf6on55NODr2XcVqc4qjgKwypmuGdvTwqo263rZAua93_KbdfoKN2StcKjNPZ98vfYhD3VUlw2ILgSqMS370ELy4HlbNi9i8_8INvtXEG8m3xUAITV2HHQjWy1gShx_gA9H4g963wbrydXt0OWE1jWTkTUMf-HGYdG0MoZZQ9kf-4_epAPubCKplzR6wIdwpAsTegparag5LfqpVJI2ZWJCHnAhFoxf-0jOu7ywiZPC0go1D1Osi49IrrgYTaES-rgJ97ToWnnmkwWgbiLjQLrH9093Pw7lyWq1fsRkKIl_JyMOPEPLljG_S5sLwUYwrDt8--p63loGp6l2aftEmxQXb0AEkk3tMNH0QV1R-yU3KtGMlD72jRrYNPAgXx9U4H-h7PPWSUJEMxdI07mQVFs4jQ4O1N88blQbRgIMp6qvIK6m-uDQSi3HzNMbU61onbsMPFMw4ANiV3YU1IEZAQtq3UB8mnzMBq5ZK7Mgvz8qOxePYp8uSBWlbCSK0vOCdUhh3I6rZetiuM467yakXF2SSuHe5lQfitRM3WwPsFg6y7_7zKdAqCltAuYmjKsTHtM8u47svias2XwrdWFs-VSpQURgWIGTfAl94DZnTwcYoaLe3blCRN5StjlQoAXrAzM5jRtQXiEky-VkNEpcH4GbP6ijzYjZ9XYpIVt2HyEYpTU9U89r7L-PhQbf9SDIOo_EJB-3lM3T3mH5pdLuYntvyfKTxnKIJHHMvdu6-uIdWBD-tYh_eaM7I0fJ57V2KW7cKzfaFU1dBU2RCwTERy0VjczX4gMCZvMAErgOGanWtwrAWDGXtgGqLKvHzDiV2UTP-kZA1WHTNhYaDL9dc_tJSORII7WGPhcbxZ_nrO_Oh9U-CfQYzz0CkLywWTP56wHmuzvKPeE05c4VhN8A2OugLDY8Wtiepfpqb3osHloolFkri83kiVDN3fIzVbRb7GgBeB7S4v7DLJB9SPYDKe9_DOqhqOxTNrUKDAuT56IxxLMGifiAw5-7jzXGcg6kHWfMkyW-ymQuGHJFSYcYDTqhsC3ZKhyHIXcf-ViXiEcVZM0IYPUMjx3B17WOugb6j4HjiFI6r0VtFRchkr2GDfDqEr8iuMJ2TNfPr7IPdww_nARLCUz2WB1Ss-hXQJqmQA14HJ1lH7Uu1cDmnrS0Xv5CzmC7_hW4bI8VXIQAREqT7zwgvHhCSWSkIWgZ2AhmpmFQkRQ_2dss8OIZXMmYiP4K7JN5V3GpWNL_9P4RUyRwYZLe-nlbaFYcCjCVdGIIO0PfOXmum0o5DgSdgrjw30Rcjt2PUhHv-m4h55CKU2usW3qqKshbiV3CWHdNpXF_dVB01fI7-D7VIU0GkjrFzQWn5Yfmuv3eOwJ02xZOe0rlXpazJ7gwp4f_rxLzA9yRapwRKZ5s1BdmqYw_Uf5ownnzWdtQa9fThmi1Ws77uv4VqpS6WvxYM5YRGwbtATkhmoxyaBi5cRpO4G-iisBq8rYAR5CKHpdFzauem4HYPR4qOEUwiwbWtwZi_wzRyMjbMZk3idZb7C785oduZ9KxFvB5BNH8AWqdsslvMTEn-fJKpFVEgQxYHrf8-d6I3k5lJt5pCi03jcDEoSuhCEvZwbeFc31occk4IWLP_aGeguRVApz4bSisz-P-bv_MH2BVDtX17eu7BO8bua0UnO3vOkHZJHUtbOST5bRhmdA71PVoGVyOFAmdaIoVYaf5FkObWEolQN8uTzm-8iZ64Z7EWitcWznhPfnZn8AR-ab4beF1gOJSwrqELc1uFh1GUEYAx3H4_asQVOqDSKvLkRGIva7VFgIq2JccFJWGpZMcxdLq2j5RtIJOQgoBokP0wBn75vh-sQmX3jupUvhDkU6hbnKvJrOlSy4cQQgU9osmRb6q01b9tP1EHGI4-9RtJBnX67FSq5p35O6vBEzglQswcVMq-WUCzVzaV2eVKK9uoEKqXTFBgPNRNuoiBwmDjFiU6S5wsBgGvzMvVCNd7Q9EfICv3gXTNy0GDPdzQ6h_0EtMEIBB5O_5bOdB5_upzaVENjo2ROgyY_IWEid8_pGUgFGCa9Sx63tLHyPB-WlJrMjqU3Q3fdsszsCVE1Wy52Dk9kdJ9WdsdWHB6EfORpTmlORuZCXzZij_89wJ8DRa00BSpICq2iU1buFJ6G8q_szFIGW0eeZIcR3hwVWXbWAXX5yDmXsGJJzgIl-snVKh6pxMbCzyApW03SxWH09z65tN-ShLjUrfNDgW6QM4pdYjVwUjZfcQy-04tHi4d2n9qvdEOU5Qx23rUH9VMq5qKhbKkDTW9NWebRCPi-N0poEGy89ZomI7jkljWFqCnoIaFARsRhznUVcREndiW4E6dnfJtIQkwTV_ZR9uVQaF9KT7xIMwSBPhsw4oO6KOEqnCH3t5vhSWZeBYvsGvfS3li-nxDp5qX00iZi5UTh3qvEgPxsXj-DE9qEEqz0_s0DXbxXOo1WTOQVkpdVqVDa0DhQy7K8jkes4M1Ct2lapFkMGFx8UBH4llCBVoic_sOo_gf67GlLhIo441UcAEnYc5eF22-mtJmSMPTUgPFjbS8YPAzeO-LDj1nBzEbf7FeNOHAlc0Yk8t3PO-DvsM3RejlxN6IAnFKF_l9DTF7jil9T8Ej4XwR5d0qEErbOjwKfrLjaVvrwKpNSjqP2hps9mCtJcNkR1dsHQAlzEnNDmQ-zgtmZ4hAPyELuOZOro2tRyzIWpyhwitGmiukyy2BwdXLdmjVjFiokVokxSYC9An-TrBpU7XJaAXOYX50UU8TRXP0kHIDe7vdix6B13ajC9aZYXKfZj_kXSCOjQhE8PCqXIGpBInvOrBbo9FLL7waCUjeAuk_jK2gFABmDBGHY0xDrlN_YClxnthNRfvEhQ843zA-WSBJYRUDkfi273uglNI7KZB36BrrJVdElJzM-DwNqBXnKosB0GAxayTv8TAUrcbSagTARCjRYgSP8_Om8LOCU0dxtc0mHgjat1SrzYSmyMY2sLi0YpYScncgwqgdDxMblJDoT9avuwPI2rDb-T2PdLP6ePjixdTj8sBhHLlAxwmA1v4vPpsyep5EdJGpPiQDANZTrMkuFogiJg5B1umxDP1X3MMyEfL0KAGVr3A0V3Bqevm_8U4u2dLNOxiFuwpfqDLHvQGqyvQP1BZoSla6a8sRlnH8DBbidJgZRptQNk3TuaYvFOEnOlW3Px8NS5aW-Hi6biptDCT7zE6gjSvKddeBbEP1PKzRD4vrCZBytWozvhTywnSBnCGUWYb2mXrlFgiY7eMrF70EYgzpeEM5KtiMHeKwQL8dT4EZOosBIKYq9n7PVTxCz838uAJ5GGnF9jnLvSbmyE-xUridIlL2UIGII4O99MnlQ6hEtuJSF_Tkk-w7bICT0PUQnW8GzeIZ18uA-pkGHlCxLALsdWXRR9Fzw2HeQXq3F7Rprck7vjZu_L52CKIPGrJ1E8C4apwvnoNFAMijRht-nMJWnx60psXQUrq8DzKG9lT-Co4W3elK52x1QQ1sO6lpKittDPjlB9OvNUFFkMKQjqHsrDU6_kt1OJy9-d3_NeE2LkeUF7E892dDeLCs226R6ViPW-vIZEc3T0fit9t-AfVy5SUGWlD_DrU-7BwgoQvR311jAR3SYOXN-dDb4NVgZorBRaByjzhyn0vPZ3TqMZyYkg4PYdzEsHvQc3VF2145UXzd8Z0X07XlbggMLXp8gHfp5aXoPH9AT84Fvuow9ewqEa7aUIwAfCpnsqznsUJr31ESm_emxD0J2sWB7X4WNzfOrLORtihFUaneOQVIuPkBFL8RLY2M2fTMx-0KXDAEICrZXCLFv0e56fYbziU0Xub6DKOGwz5XdOL-3bxUkLil4age4NgAC5GmMxJ6E_nBGmT0HIWOpmvgs41iK6G7i1ZDCWwmFq6J1d-Eo.hscCVcxL5wYYFO-QNJ4W-w";

const responseOutgoing = await hcxIntegrator.processOutgoingRequest(fhirPayload, "hcxtest6.yopmail@swasth-hcx", operation);
const responseOutgoingCallback =  await hcxIntegrator2.processOutgoingCallback(fhirPayload, "hcxtest6.yopmail@swasth-hcx", operation2, actionJwe, "", "", "", "response.complete");
const responseIncoming = await hcxIntegrator.processIncoming(
  responseOutgoing.payload,
  operation
);

console.log(responseOutgoing);
console.log(responseIncoming);
console.log(responseOutgoingCallback)