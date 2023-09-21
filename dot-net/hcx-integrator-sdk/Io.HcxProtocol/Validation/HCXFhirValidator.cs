using Hl7.Fhir.Specification.Source;
using Hl7.Fhir.Validation;
using Io.HcxProtocol.Init;
using Io.HcxProtocol.Utils;
using System;
using System.IO;
using System.IO.Compression;

namespace Io.HcxProtocol.Validation
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// HCXFhirValidator provides an instance which is having the HCX IG specification definitions to validate the FHIR object.
    /// </summary>

    public class HCXFhirValidator
    {
        private static HCXFhirValidator instance = null;
        private Validator validator = null;

        private string hcxIGBasePath;
        private string nrcesIGBasePath;

        private static HCXFhirValidator GetInstance(Config config)
        {
            if (instance == null)
                instance = new HCXFhirValidator(config);

            return instance;
        }

        public static Validator GetFhirValidator(Config config)
        {
            return GetInstance(config).validator;
        }

        private HCXFhirValidator(Config config)
        {
            hcxIGBasePath = config.HcxIGBasePath;
            nrcesIGBasePath = config.NrcesIGBasePath;

            // directory path containing expanded profile packages files
            string rootDir = Directory.GetCurrentDirectory();
            string profileDirectory = Path.Combine(rootDir, "profiles");

            CopySpecifiationZipToBin();
            DownloadProfileDirectory(profileDirectory);
            DownloadNRCESProfileDirectory(profileDirectory);

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

        private bool DownloadProfileDirectory(string profileDirPath)
        {
            string fileSourceUrl = hcxIGBasePath + "definitions.json.zip";
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
                throw new System.Exception("Unable to download Profile Directory File." + "\n" + ex.ToString());
            }

            return true;
        }

        private bool DownloadNRCESProfileDirectory(string profileDirPath)
        {
            string profileDirPathNrces = Path.Combine(profileDirPath, "definitions_nrces");
            string fileSourceUrl = nrcesIGBasePath + "definitions.json.zip";
            string profileFileName = "definitions_nrces.json.zip";
            string fullPathToFile = Path.Combine(profileDirPathNrces, profileFileName);
            try
            {
                if (!Directory.Exists(profileDirPathNrces))
                {
                    Directory.CreateDirectory(profileDirPathNrces);
                }
                ClearProfileDirectory(profileDirPathNrces);

                HttpUtils.DownloadFile(fileSourceUrl, fullPathToFile);

                string zipFilePath = fullPathToFile;
                ZipFile.ExtractToDirectory(zipFilePath, profileDirPathNrces);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Unable to download NRCES Profile Directory File." + "\n" + ex.ToString());
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

        static void CopySpecifiationZipToBin()
        {
            try
            {

                //Used If specification.zip is not copied to local by hl7.fhir.specification.r4 itself.
                string specificationFile = "specification.zip";

                // Package Path from User Profile Directory
                string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string packagePathUserProfile = Path.Combine(userProfilePath, ".nuget\\packages\\hl7.fhir.specification.r4\\4.3.0\\contentFiles\\any\\any", specificationFile);
                string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, specificationFile);

                // Package Path from Project Directory
                string projectDirPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                string packagePathfromProject = Path.Combine(projectDirPath, "packages\\Hl7.Fhir.Specification.R4.4.3.0\\contentFiles\\any\\any", specificationFile);

                if (!File.Exists(basePath))
                {
                    if (File.Exists(packagePathUserProfile))
                    {
                        File.Copy(packagePathUserProfile, basePath, false);
                    }
                    else if (File.Exists(packagePathfromProject))
                    {
                        File.Copy(packagePathfromProject, basePath, false);
                    }
                }
            }
            catch (System.Exception)
            {
            }

        }
    }
}
