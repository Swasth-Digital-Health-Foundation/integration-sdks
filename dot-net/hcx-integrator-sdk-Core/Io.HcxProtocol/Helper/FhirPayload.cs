using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Validation;
using Io.HcxProtocol.Exceptions;
using Io.HcxProtocol.Utils;
using Io.HcxProtocol.Validation;
using System;
using System.Collections.Generic;

namespace Io.HcxProtocol.Helper
{
    /// <summary>
    /// Implementation of FHIR validation using HCX FHIR IG.
    /// </summary>
    public abstract class FhirPayload
    {
        public bool ValidatePayload(string fhirPayloadJson, Operations operation, Dictionary<string, object> error)
        {
            Resource resource;
            try
            {
                // parse our fhirPayloadJson into resource object
                FhirJsonParser jsonParser = new FhirJsonParser();
                resource = jsonParser.Parse<Resource>(fhirPayloadJson);
            }
            catch (Exception ex)
            {
                error.Add(ErrorCodes.ERR_WRONG_DOMAIN_PAYLOAD.ToString(), ex.Message.ToString());
                return false;
            }

            try
            {
                // check resource type in JSON payload & operation type should be same
                string resourceType = resource.TypeName;
                if (operation.getFhirResourceType() != resourceType)
                {
                    error.Add(ErrorCodes.ERR_WRONG_DOMAIN_PAYLOAD.ToString(), "Incorrect eObject is sent as the domain payload");
                    return false;
                }

                // create a resource validator
                Validator validator = HCXFhirValidator.GetFhirValidator();
                OperationOutcome outcome = validator.Validate(resource);

                // check outcome success
                if (!outcome.Success)
                {
                    List<string> errors = new List<string>();
                    foreach (var issue in outcome.Issue)
                    {
                        errors.Add(issue.Details.Text);
                    }
                    error.Add(ErrorCodes.ERR_INVALID_DOMAIN_PAYLOAD.ToString(), errors);
                    return false;
                }
            }
            catch (Exception ex)
            {
                error.Add(ErrorCodes.ERR_INVALID_DOMAIN_PAYLOAD.ToString(), ex.Message.ToString());
                return false;
            }

            return true;
        }
    }
}
