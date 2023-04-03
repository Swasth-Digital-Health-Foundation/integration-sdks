package io.hcxprotocol.helper;

import ca.uhn.fhir.context.FhirContext;
import ca.uhn.fhir.parser.IParser;
import ca.uhn.fhir.validation.FhirValidator;
import ca.uhn.fhir.validation.ResultSeverityEnum;
import ca.uhn.fhir.validation.SingleValidationMessage;
import ca.uhn.fhir.validation.ValidationResult;
import io.hcxprotocol.dto.ResponseError;
import io.hcxprotocol.validator.HCXFHIRValidator;
import org.apache.maven.artifact.versioning.ComparableVersion;
import org.hl7.fhir.r4.model.*;

import java.io.IOException;
import java.net.URL;
import java.util.*;

public class HCXFHIRUtils {

    //Initializing the FHIR parser
    static IParser p = FhirContext.forR4().newJsonParser().setPrettyPrint(true);

    public static boolean validateVersion(String bundleURL) throws IOException {
        //replace .html extension with .json
        bundleURL = bundleURL.replace(".html",".json");
        StructureDefinition bundleDef = (StructureDefinition) p.parseResource(new URL(bundleURL).openStream());
        System.out.println("bundledef" + bundleDef);
        ComparableVersion versionBundle = new ComparableVersion(bundleDef.getVersion());
        ComparableVersion versionBase = new ComparableVersion("0.7");
        return versionBundle.compareTo(versionBase) > 0;
    }

    public static Bundle resourceToBundle(DomainResource res, List<DomainResource> referencedResource, Bundle.BundleType type, String bundleURL, String hcxIGBasePath, String nrcesIGBasePath) throws Exception {

        //checking for bundle version. We support bundle version v0.7.1 and above
        ResponseError err = new ResponseError();
        try{
            if(!validateVersion(bundleURL)){
                throw new Exception("IG version should be greater than 0.7.0");
            }
        }catch (Exception e){
            throw new Exception("Could not read the Structure Definition for the Bundle");
        }

        DomainResource resource = res.copy();
        Bundle bundle = new Bundle();
        bundle.setId(UUID.randomUUID().toString());
        Meta meta = new Meta();
        meta.getProfile().add(new CanonicalType(bundleURL));
        meta.setLastUpdated(new Date());
        bundle.setMeta(meta);
        bundle.setIdentifier(new Identifier().setSystem( "https://www.tmh.in/bundle").setValue(UUID.randomUUID().toString()));
        bundle.setType((type));
        bundle.setTimestamp(new Date());
        //adding the main resource to the bundle entry
        resource.getContained().clear();
        bundle.getEntry().add(new Bundle.BundleEntryComponent().setFullUrl(resource.getResourceType() + "/" + resource.getId().toString().replace("#","")).setResource(resource));
        for (Resource refResource : referencedResource) {
            String id = refResource.getId().toString().replace("#","");
            refResource.setId(id);
            bundle.getEntry().add(new Bundle.BundleEntryComponent().setFullUrl(refResource.getResourceType() + "/" + id).setResource(refResource));
        }

        //validating the bundle
        FhirValidator validator = HCXFHIRValidator.getValidator(hcxIGBasePath, nrcesIGBasePath);
        ValidationResult result = validator.validateWithResult(bundle);
        List<SingleValidationMessage> messages = result.getMessages();
        List<String> errMessages = new ArrayList<>();
        for (SingleValidationMessage message : messages) {
            if (message.getSeverity() == ResultSeverityEnum.ERROR) {
                errMessages.add(message.getMessage());
            }
        }
        if (!errMessages.isEmpty()){
            throw new Exception(String.valueOf(errMessages));
        }
        return bundle;
    }


    public static DomainResource getPrimaryResource(Bundle resource, String resourceURL) throws Exception {
        Bundle newBundle = resource.copy();
        for(int i=0; i<newBundle.getEntry().size(); i++){
            Bundle.BundleEntryComponent par = newBundle.getEntry().get(i);
            DomainResource dm = (DomainResource) par.getResource();
            if (Objects.equals(dm.getMeta().getProfile().get(0).getValue(), resourceURL)){
                return dm;
            }
        }
        throw new Exception("No resource with the given URL found");
    }

    public static DomainResource getPrimaryResource(Bundle resource){
        Bundle newBundle = resource.copy();
        Bundle.BundleEntryComponent par = newBundle.getEntry().get(0);
        return (DomainResource) par.getResource();
    }


    public static List<DomainResource> getReferencedResource(Bundle resource, String resourceURL){
        Bundle newBundle = resource.copy();
        List<DomainResource> dmList = new ArrayList<>();
        for(int i=0; i<newBundle.getEntry().size(); i++){
            Bundle.BundleEntryComponent par = newBundle.getEntry().get(i);
            DomainResource dm = (DomainResource) par.getResource();
            if (!Objects.equals(dm.getMeta().getProfile().get(0).getValue(), resourceURL)){
                dmList.add(dm);
            }
        }
        return dmList;
    }

    public static List<DomainResource> getReferencedResource(Bundle resource){
        Bundle newBundle = resource.copy();
        List<DomainResource> dmList = new ArrayList<>();
        for(int i=1; i < newBundle.getEntry().size(); i++){
            Bundle.BundleEntryComponent par = newBundle.getEntry().get(i);
            DomainResource dm = (DomainResource) par.getResource();
            dmList.add(dm);
        }
        return dmList;
    }
}
