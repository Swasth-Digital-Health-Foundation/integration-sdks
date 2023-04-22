package io.hcxprotocol.validator;

import ca.uhn.fhir.context.FhirContext;
import ca.uhn.fhir.context.support.DefaultProfileValidationSupport;
import ca.uhn.fhir.parser.IParser;
import ca.uhn.fhir.validation.FhirValidator;
import io.hcxprotocol.createresource.HCXInsurancePlan;
import io.hcxprotocol.utils.JSONUtils;
import org.hl7.fhir.common.hapi.validation.support.*;
import org.hl7.fhir.common.hapi.validation.validator.FhirInstanceValidator;
import org.hl7.fhir.r4.model.*;

import java.io.*;
import java.net.URL;
import java.net.URLConnection;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Arrays;
import java.util.zip.ZipEntry;
import java.util.zip.ZipInputStream;

public class HCXFHIRValidator {

    private static HCXFHIRValidator instance = null;

    private FhirValidator validator = null;

    private String hcxIGBasePath;

    private String nrcesIGBasePath;

    private HCXFHIRValidator(String hcxIGBasePath, String nrcesIGBasePath) throws Exception {
        FhirContext fhirContext = FhirContext.forR4();
        fhirContext.setDefaultTypeForProfile(hcxIGBasePath + "StructureDefinition-HCXInsurancePlan.html", HCXInsurancePlan.class);
        //   Create a chain that will hold the validation modules
        System.out.println("we have started");
        ValidationSupportChain supportChain = new ValidationSupportChain();
        String currentDir = System.getProperty("user.dir") + "\\";

        Path newDirNRCES = Paths.get(currentDir, "nrces_definitions");
        Path newDirHCX = Paths.get(currentDir, "hcx_definitions");

        File nrcesFile = new File(newDirNRCES.toString());
        boolean isPresentNRCESPath = nrcesFile.exists() && nrcesFile.isDirectory();
        if (!isPresentNRCESPath) {
            Files.createDirectory(newDirNRCES);
        }

        File hcxFile = new File(newDirHCX.toString());
        boolean isPresentHCXPath = hcxFile.exists() && hcxFile.isDirectory();
        if (!isPresentHCXPath) {
            Files.createDirectory(newDirHCX);
        }

        downloadZip(new URL(hcxIGBasePath), currentDir + "hcx_definitions.zip");
        downloadZip(new URL(nrcesIGBasePath), currentDir + "nrces_definitions.zip");

       // extract zip files
        if (!isPresentNRCESPath) {
            decompressZIP(Paths.get(currentDir, "nrces_definitions.zip"), newDirNRCES);
        }
        if (!isPresentHCXPath) {
            decompressZIP(Paths.get(currentDir, "hcx_definitions.zip"), newDirHCX);
        }

        // DefaultProfileValidationSupport supplies base FHIR definitions. This is generally required
        // even if we are using custom profiles, since those profiles will derive from the base
        // definitions.
        DefaultProfileValidationSupport defaultSupport = new DefaultProfileValidationSupport(fhirContext);
        supportChain.addValidationSupport(defaultSupport);

        // This module supplies several code systems that are commonly used in validation
        supportChain.addValidationSupport(new CommonCodeSystemsTerminologyService(fhirContext));

        // This module implements terminology services for in-memory code validation
        supportChain.addValidationSupport(new InMemoryTerminologyServerValidationSupport(fhirContext));

        IParser parser = fhirContext.newJsonParser();

        // Create a PrePopulatedValidationSupport which can be used to load custom definitions.
        PrePopulatedValidationSupport prePopulatedSupport = new PrePopulatedValidationSupport(fhirContext);
        loadProfiles(prePopulatedSupport, parser, "nrces_definitions");
        loadProfiles(prePopulatedSupport, parser, "hcx_definitions");
        supportChain.addValidationSupport(prePopulatedSupport);
        CachingValidationSupport cache = new CachingValidationSupport(supportChain);
        System.out.println(prePopulatedSupport.fetchAllConformanceResources());

        // Create a validator using the FhirInstanceValidator module.
        FhirInstanceValidator validatorModule = new FhirInstanceValidator(cache);
        this.validator = fhirContext.newValidator().registerValidatorModule(validatorModule);
    }

    private static HCXFHIRValidator getInstance(String hcxIGBasePath, String nrcesIGBasePath) throws Exception {
        if (null == instance)
            instance = new HCXFHIRValidator(hcxIGBasePath, nrcesIGBasePath);

        return instance;
    }

    public static FhirValidator getValidator(String hcxIGBasePath, String nrcesIGBasePath) throws Exception {
        return getInstance(hcxIGBasePath, nrcesIGBasePath).validator;
    }

    public void downloadZip(URL url, String outputDir) throws IOException {
        URLConnection conn = url.openConnection();
        InputStream in = conn.getInputStream();
        FileOutputStream out = new FileOutputStream(outputDir);
        byte[] b = new byte[1024];
        int count;
        while ((count = in.read(b)) >= 0) {
            out.write(b, 0, count);
        }
        out.flush();
        out.close();
        in.close();
    }

    public void decompressZIP(Path zipFile, Path outputDir) throws IOException {
        try (ZipInputStream zipInputStream = new ZipInputStream(Files.newInputStream(zipFile))) {
            ZipEntry entry;
            while ((entry = zipInputStream.getNextEntry()) != null) {
                Path outputFile = outputDir.resolve(entry.getName());
                Files.copy(zipInputStream, outputFile);
            }
            zipInputStream.closeEntry();
        }
//        File file = new File(zipFile.toUri());
//        boolean deleted = file.delete();
    }

    public void loadProfiles(PrePopulatedValidationSupport prePopulatedSupport, IParser parser, String type) throws FileNotFoundException {
        File dir = new File(System.getProperty("user.dir") + "\\" + type);
        File[] directoryListing = dir.listFiles();
        if (directoryListing != null) {
            for (File file : directoryListing) {
                if (file.getName().startsWith("StructureDefinition")) {
                    prePopulatedSupport.addStructureDefinition(parser.parseResource(new FileReader(file)));
                } else if (file.getName().startsWith("ValueSet")) {
                    prePopulatedSupport.addValueSet(parser.parseResource(new FileReader(file)));
                }
            }
        }
    }
}