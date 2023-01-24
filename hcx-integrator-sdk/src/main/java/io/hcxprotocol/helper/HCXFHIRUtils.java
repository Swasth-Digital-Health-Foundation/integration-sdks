package io.hcxprotocol.helper;

import ca.uhn.fhir.context.FhirContext;
import ca.uhn.fhir.parser.IParser;
import org.hl7.fhir.r4.model.*;

import java.util.Date;
import java.util.UUID;

public class HCXFHIRUtils {

    //Initializing the FHIR parser
    static IParser p = FhirContext.forR4().newJsonParser().setPrettyPrint(true);

    public static Bundle resourceToBundle(DomainResource res, DomainResource[] referencedResource, Bundle.BundleType type, String bundleURL){
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
        return bundle;
    }


    public static DomainResource bundleToResource(Bundle resource){
        Bundle newBundle = resource.copy();
        Bundle.BundleEntryComponent par = newBundle.getEntry().get(0);
        DomainResource dm = (DomainResource) par.getResource();
        dm.addContained(newBundle.getEntry().get(0).getResource());
        for(int i=1; i<newBundle.getEntry().size(); i++){
            dm.addContained(newBundle.getEntry().get(i).getResource());
        }
        return dm;
    }
}
