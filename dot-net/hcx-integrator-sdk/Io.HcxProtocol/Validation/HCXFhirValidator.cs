using Hl7.Fhir.Language.Debugging;
using Hl7.Fhir.Specification.Source;
using Hl7.Fhir.Validation;
using Io.HcxProtocol.Utils;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace Io.HcxProtocol.Validation
{
    public class HCXFhirValidator
    {
        private static HCXFhirValidator instance = null;
        private Validator validator = null;

        private HCXFhirValidator()
        {
            // directory path containing expanded profile packages files
            string rootDir = Directory.GetCurrentDirectory();
            string profileDirectory = Path.Combine(rootDir, "profiles");

            DownloadProfileDirectory(profileDirectory);

            // create a cached resolver for resource validation
            IResourceResolver resolver = new CachedResolver(
              new MultiResolver(
                // create the default FHIR specification resolver (specification.zip included in HL7.fhir.specification.r4)
                ZipSource.CreateValidationSource(),
                // create the directory source resolver, which points to our profiles directory
                new DirectorySource(profileDirectory, new DirectorySourceSettings()
                {
                    IncludeSubDirectories = true,
                })
              )
            );

            // create a resource validator, which uses our cached resolver
            validator = new Validator(new ValidationSettings()
            {
                ResourceResolver = resolver,
            });
        }

        private static HCXFhirValidator GetInstance()
        {
            if (instance == null)
                instance = new HCXFhirValidator();

            return instance;
        }

        public static Validator GetFhirValidator()
        {
            return GetInstance().validator;
        }

        private bool DownloadProfileDirectory(string profileDirPath)
        {
            string fileSourceUrl = "https://ig.hcxprotocol.io/v0.7/definitions.json.zip";
            string profileFileName = "definitions.json.zip";
            string fullPathToFile = Path.Combine(profileDirPath, profileFileName);
            try
            {
                if (!Directory.Exists(profileDirPath))
                {
                    Directory.CreateDirectory(profileDirPath);
                }
                ClearProfileDirectory(profileDirPath);

                HttpUtils.DownloadFile(fileSourceUrl, fullPathToFile);

                string zipFilePath = fullPathToFile;
                ZipFile.ExtractToDirectory(zipFilePath, profileDirPath);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Unable to download Profile Directory File." + "\n" + ex.Message.ToString());
            }

            return true;
        }

        private void ClearProfileDirectory(string profileDirPath)
        {
            string[] oldFiles = Directory.GetFiles(profileDirPath);
            try
            {
                foreach (string file in oldFiles)
                {
                    File.Delete(file);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Unable to Clear Profile Directory." + "\n" + ex.Message.ToString());
            }
        }
    }
}
