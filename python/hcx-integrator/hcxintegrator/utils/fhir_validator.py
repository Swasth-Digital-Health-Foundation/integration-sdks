import json
from fhir.resources import construct_fhir_element
json_dict = {"resourceType": "Claim",
    "id": "mmanu",
    "active": True,
    "name": "Acme Corporation",
    "address": [{"country": "Switzerland"}]
}

fhirPayload = {
  "resourceType": "Bundle",
  "id": "98aa81af-7a49-4159-a8ed-35e721d6ae74",
  "meta": {
    "lastUpdated": "2023-02-20T14:03:15.013+05:30",
    "profile": [ "https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-ClaimRequestBundle.html" ]
  },
  "identifier": {
    "system": "https://www.tmh.in/bundle",
    "value": "7ee7ee1a-fcad-49c3-8127-aa70c7a4dc0d"
  },
  "type": "collection",
  "timestamp": "2023-02-20T14:03:15.013+05:30",
  "entry": [ {
    "fullUrl": "Claim/bb1eea08-8739-4f14-b541-04622f18450c",
    "resource": {
      "resourceType": "Claim",
      "id": "bb1eea08-8739-4f14-b541-04622f18450c",
      "meta": {
        "lastUpdated": "2023-02-20T14:03:14.918+05:30",
        "profile": [ "https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-Claim.html" ]
      },
      "identifier": [ {
        "system": "http://identifiersystem.com",
        "value": "IdentifierValue"
      } ],
      "status": "active",
      "type": {
        "coding": [ {
          "system": "http://terminology.hl7.org/CodeSystem/claim-type",
          "code": "institutional"
        } ]
      },
      "use": "claim",
      "patient": {
        "reference": "Patient/RVH1003"
      },
      "created": "2023-02-20T14:03:14+05:30",
      "insurer": {
        "reference": "Organization/GICOFINDIA"
      },
      "provider": {
        "reference": "Organization/WeMeanWell01"
      },
      "priority": {
        "coding": [ {
          "system": "http://terminology.hl7.org/CodeSystem/processpriority",
          "code": "normal"
        } ]
      },
      "payee": {
        "type": {
          "coding": [ {
            "system": "http://terminology.hl7.org/CodeSystem/payeetype",
            "code": "provider"
          } ]
        },
        "party": {
          "reference": "Organization/WeMeanWell01"
        }
      },
      "careTeam": [ {
        "sequence": 4,
        "provider": {
          "reference": "Organization/WeMeanWell01"
        }
      } ],
      "diagnosis": [ {
        "sequence": 1,
        "diagnosisCodeableConcept": {
          "coding": [ {
            "system": "http://irdai.com",
            "code": "E906184",
            "display": "SINGLE INCISION LAPAROSCOPIC APPENDECTOMY"
          } ],
          "text": "SINGLE INCISION LAPAROSCOPIC APPENDECTOMY"
        },
        "type": [ {
          "coding": [ {
            "system": "http://terminology.hl7.org/CodeSystem/ex-diagnosistype",
            "code": "admitting",
            "display": "Admitting Diagnosis"
          } ]
        } ]
      } ],
      "insurance": [ {
        "sequence": 1,
        "focal": True,
        "coverage": {
          "reference": "Coverage/COVERAGE1"
        }
      } ],
      "item": [ {
        "sequence": 1,
        "productOrService": {
          "coding": [ {
            "system": "https://irdai.gov.in/package-code",
            "code": "E101021",
            "display": "Twin Sharing Ac"
          } ]
        },
        "unitPrice": {
          "value": 100000,
          "currency": "INR"
        }
      }, {
        "sequence": 1,
        "productOrService": {
          "coding": [ {
            "system": "https://irdai.gov.in/package-code",
            "code": "E924260",
            "display": "CLINICAL TOXICOLOGY SCREEN, BLOOD"
          } ]
        },
        "unitPrice": {
          "value": 2000,
          "currency": "INR"
        }
      }, {
        "sequence": 1,
        "productOrService": {
          "coding": [ {
            "system": "https://irdai.gov.in/package-code",
            "code": "E924261",
            "display": "CLINICAL TOXICOLOGY SCREEN,URINE"
          } ]
        },
        "unitPrice": {
          "value": 1000,
          "currency": "INR"
        }
      }, {
        "sequence": 1,
        "productOrService": {
          "coding": [ {
            "system": "https://irdai.gov.in/package-code",
            "code": "E507029",
            "display": "ECG"
          } ]
        },
        "unitPrice": {
          "value": 5000,
          "currency": "INR"
        }
      }, {
        "sequence": 1,
        "productOrService": {
          "coding": [ {
            "system": "https://irdai.gov.in/package-code",
            "code": "E6080377",
            "display": "UltraSound Abdomen"
          } ]
        },
        "unitPrice": {
          "value": 5000,
          "currency": "INR"
        }
      }, {
        "sequence": 1,
        "productOrService": {
          "coding": [ {
            "system": "https://irdai.gov.in/package-code",
            "code": "502001",
            "display": "Surgeons Charges"
          } ]
        },
        "unitPrice": {
          "value": 1000,
          "currency": "INR"
        }
      }, {
        "sequence": 1,
        "productOrService": {
          "coding": [ {
            "system": "https://irdai.gov.in/package-code",
            "code": "5020021",
            "display": "Anesthesiologist charges"
          } ]
        },
        "unitPrice": {
          "value": 1000,
          "currency": "INR"
        }
      }, {
        "sequence": 1,
        "productOrService": {
          "coding": [ {
            "system": "https://irdai.gov.in/package-code",
            "code": "E6080373",
            "display": "Physician charges"
          } ]
        },
        "unitPrice": {
          "value": 1000,
          "currency": "INR"
        }
      }, {
        "sequence": 1,
        "productOrService": {
          "coding": [ {
            "system": "https://irdai.gov.in/package-code",
            "code": "201008",
            "display": "Recovery Room"
          } ]
        },
        "unitPrice": {
          "value": 10000,
          "currency": "INR"
        }
      }, {
        "sequence": 1,
        "productOrService": {
          "coding": [ {
            "system": "https://irdai.gov.in/package-code",
            "code": "406003",
            "display": "intra -venous (iv) set"
          } ]
        },
        "unitPrice": {
          "value": 5000,
          "currency": "INR"
        }
      }, {
        "sequence": 1,
        "productOrService": {
          "coding": [ {
            "system": "https://irdai.gov.in/package-code",
            "code": "E507353",
            "display": "Oral Medication"
          } ]
        },
        "unitPrice": {
          "value": 5000,
          "currency": "INR"
        }
      }, {
        "sequence": 1,
        "productOrService": {
          "coding": [ {
            "system": "https://irdai.gov.in/package-code",
            "code": "E925171",
            "display": "Hospital charges"
          } ]
        },
        "unitPrice": {
          "value": 5000,
          "currency": "INR"
        }
      }, {
        "sequence": 1,
        "productOrService": {
          "coding": [ {
            "system": "https://irdai.gov.in/package-code",
            "code": "501001",
            "display": "Consultation Charges"
          } ]
        },
        "unitPrice": {
          "value": 5000,
          "currency": "INR"
        }
      } ],
      "total": {
        "value": 146000.0,
        "currency": "INR"
      }
    }
  }, {
    "fullUrl": "Organization/WeMeanWell01",
    "resource": {
      "resourceType": "Organization",
      "id": "WeMeanWell01",
      "meta": {
        "profile": [ "https://nrces.in/ndhm/fhir/r4/StructureDefinition/Organization" ]
      },
      "identifier": [ {
        "type": {
          "coding": [ {
            "system": "http://terminology.hl7.org/CodeSystem/v2-0203",
            "code": "AC",
            "display": "Narayana"
          } ]
        },
        "system": "http://abdm.gov.in/facilities",
        "value": "HFR-ID-FOR-TMH"
      } ],
      "name": "WeMeanWell Hospital",
      "address": [ {
        "text": " Bannerghatta Road, Bengaluru ",
        "city": "Bengaluru",
        "country": "India"
      } ]
    }
  }, {
    "fullUrl": "Organization/GICOFINDIA",
    "resource": {
      "resourceType": "Organization",
      "id": "GICOFINDIA",
      "meta": {
        "profile": [ "https://nrces.in/ndhm/fhir/r4/StructureDefinition/Organization" ]
      },
      "identifier": [ {
        "type": {
          "coding": [ {
            "system": "http://terminology.hl7.org/CodeSystem/v2-0203",
            "code": "AC",
            "display": "GOVOFINDIA"
          } ]
        },
        "system": "http://irdai.gov.in/insurers",
        "value": "GICOFINDIA"
      } ],
      "name": "GICOFINDIA"
    }
  }, {
    "fullUrl": "Patient/RVH1003",
    "resource": {
      "resourceType": "Patient",
      "id": "RVH1003",
      "meta": {
        "profile": [ "https://nrces.in/ndhm/fhir/r4/StructureDefinition/Patient" ]
      },
      "identifier": [ {
        "type": {
          "coding": [ {
            "system": "http://terminology.hl7.org/CodeSystem/v2-0203",
            "code": "SN",
            "display": "Subscriber Number"
          } ]
        },
        "system": "http://gicofIndia.com/beneficiaries",
        "value": "BEN-101"
      } ],
      "name": [ {
        "text": "Prasidh Dixit"
      } ],
      "gender": "male",
      "birthDate": "1960-09-26",
      "address": [ {
        "text": "#39 Kalena Agrahara, Kamanahalli, Bengaluru - 560056",
        "city": "Bengaluru",
        "state": "Karnataka",
        "postalCode": "560056",
        "country": "India"
      } ]
    }
  }, {
    "fullUrl": "Coverage/COVERAGE1",
    "resource": {
      "resourceType": "Coverage",
      "id": "COVERAGE1",
      "meta": {
        "profile": [ "https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-Coverage.html" ]
      },
      "identifier": [ {
        "system": "https://www.gicofIndia.in/policies",
        "value": "policy-RVH1003"
      } ],
      "status": "active",
      "subscriber": {
        "reference": "Patient/RVH1003"
      },
      "subscriberId": "2XX8971",
      "beneficiary": {
        "reference": "Patient/RVH1003"
      },
      "relationship": {
        "coding": [ {
          "system": "http://terminology.hl7.org/CodeSystem/subscriber-relationship",
          "code": "self"
        } ]
      },
      "payor": [ {
        "reference": "Organization/GICOFINDIA"
      } ]
    }
  } ]
}


json_dict = json.dumps(fhirPayload)
org = construct_fhir_element('Bundle', json_dict)
# org.address[0].country == "Switzerland"

# org.dict()['active'] is True
