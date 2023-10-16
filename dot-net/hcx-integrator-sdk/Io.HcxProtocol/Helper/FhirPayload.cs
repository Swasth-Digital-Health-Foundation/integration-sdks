using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Validation;
using Io.HcxProtocol.Exceptions;
using Io.HcxProtocol.Init;
using Io.HcxProtocol.Utils;
using Io.HcxProtocol.Validation;
using System;
using System.Collections.Generic;

namespace Io.HcxProtocol.Helper
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// Implementation of FHIR validation using HCX FHIR IG.
    /// </summary>
    public abstract class FhirPayload
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ValidatePayload method used to validate the FHIR Json object using HCX FHIR IG.
        /// </summary>
        /// <param name="fhirPayloadJson">FHIR Payload Json String</param>
        /// <param name="operation">Which operation is being processed</param>
        /// <param name="error">Holds any validation errors</param>
        /// <returns>Return true if validation is successful.</returns>

        public bool ValidateFHIR(string fhirPayload, Operations operation, Dictionary<string, object> error, Config config)
        {
            Resource resource;
            try
            {
                // parse our fhirPayloadJson into resource object
                FhirJsonParser jsonParser = new FhirJsonParser();
                resource = jsonParser.Parse<Resource>(fhirPayload);
                _logger.Info("FHIR Payload is validated successfully");
            }
            catch (Exception ex)
            {
                error.Add(ErrorCodes.ERR_WRONG_DOMAIN_PAYLOAD.ToString(), "[Incorrect eObject is sent as the domain payload] " + ex.Message.ToString());
                _logger.Error("[Incorrect eObject is sent as the domain payload] " + ex.Message.ToString());
                return false;
            }

            try
            {
                // check resource type in JSON payload & operation type should be same
                string resourceType = resource.TypeName;
                if (operation.getFhirResourceType() != resourceType)
                {
                    error.Add(ErrorCodes.ERR_WRONG_DOMAIN_PAYLOAD.ToString(), "Incorrect eObject is sent as the domain payload");
                    _logger.Error(ErrorCodes.ERR_WRONG_DOMAIN_PAYLOAD.ToString(), "Incorrect eObject is sent as the domain payload");
                    return false;
                }

                // create a resource validator
                Validator validator = HCXFhirValidator.GetFhirValidator(config);
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
                    _logger.Error(ErrorCodes.ERR_INVALID_DOMAIN_PAYLOAD.ToString(), errors);
                    return false;
                }
            }
            catch (Exception ex)
            {
                error.Add(ErrorCodes.ERR_INVALID_DOMAIN_PAYLOAD.ToString(), ex.ToString());
                _logger.Error(ErrorCodes.ERR_INVALID_DOMAIN_PAYLOAD.ToString(), ex.ToString());
                return false;
            }

            return true;
        }
    }
}
