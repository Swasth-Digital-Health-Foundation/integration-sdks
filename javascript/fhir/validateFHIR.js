import fhir from "fhir";
import fs from 'fs';

const { ParseConformance, Versions, Fhir } = fhir;
const definitionsPath = '../../FHIR Structure Definitions/HCX/definitions.json';
let hcxDefinitions;

try {
    hcxDefinitions = JSON.parse(fs.readFileSync(definitionsPath, 'utf-8'));
} catch (error) {
    console.error("Error reading or parsing HCX FHIR Definitions:", error);
    throw error; // If you don't want to proceed in case of an error
}

const parser = new ParseConformance(false, Versions.R4);
parser.parseBundle(hcxDefinitions);
const fhir = new Fhir(parser);

export const validateResource = (resource) => {
    const xmlResource = fhir.objToXml(resource);
    const validationResults = fhir.validate(xmlResource, {});
    console.log(validationResults);
    return validationResults;
}
