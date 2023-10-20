using Io.HcxProtocol.Impl;
using Io.HcxProtocol.Interfaces;
using System;

namespace Io.HcxProtocol.Init
{
    public class BaseIntegrator
    {
        private Config config;

        public void ValidateConfig(Config _config)
        {
            string prop = null;

            if (string.IsNullOrEmpty(_config.ProtocolBasePath))
            {
                prop = "ProtocolBasePath";
            }
            else if (string.IsNullOrEmpty(_config.ParticipantCode))
            {
                prop = "ParticipantCode";
            }
            else if (string.IsNullOrEmpty(_config.ParticipantGenerateToken))
            {
                prop = "ParticipantGenerationToken";
            }
            else if (string.IsNullOrEmpty(_config.UserName))
            {
                prop = "UserName";
            }
            else if (string.IsNullOrEmpty(_config.EncryptionPrivateKey))
            {
                prop = "EncryptionPrivateKey";
            }
            else if (string.IsNullOrEmpty(_config.SigningPrivateKey))
            {
                prop = "SigningPrivateKey";
            }

            if (string.IsNullOrEmpty(_config.HcxIGBasePath))
            {
                //Default Hcx IG Base Path 
                _config.HcxIGBasePath = "https://ig.hcxprotocol.io/v0.7.1/";
            }
            if (string.IsNullOrEmpty(_config.NrcesIGBasePath))
            {
                //Default Nrces IG Base Path 
                _config.NrcesIGBasePath = "https://nrces.in/ndhm/fhir/r4/";
            }
            if (!string.IsNullOrWhiteSpace(_config.LogType) && _config.LogType.ToLower() == "file")
            {
                if (string.IsNullOrEmpty(_config.LogFileName))
                {
                    prop = "LogFileName";
                }
                if (string.IsNullOrEmpty(_config.LogFilePath))
                {
                    prop = "LogiFilePath";
                }
            }

            if (prop != null)
                throw new System.Exception(prop + " is missing or has empty value, please add to the configuration.");
            else
                config = _config;

            ValidateOptionalFields(config);
        }

        private void ValidateOptionalFields(Config _config)
        {
            if (string.IsNullOrEmpty(_config.Password) && string.IsNullOrEmpty(_config.Secret))
            {
                throw new System.Exception("Access token generation failed. Please provide participant password or user secret in the config");
            }
        }

        protected void SetConfig(Config config)
        {
            ValidateConfig(config);
        }

        protected Config GetConfig()
        {
            return config;
        }

        protected IIncomingRequest GetIncomingRequest()
        {
            return GetProcessRequest<HCXIncomingRequest>(config.IncomingRequestClass, typeof(HCXIncomingRequest));
        }

        protected IOutgoingRequest GetOutgoingRequest()
        {
            return GetProcessRequest<HCXOutgoingRequest>(config.OutgoingRequestClass, typeof(HCXOutgoingRequest));
        }

        private T GetProcessRequest<T>(T classInstance, Type defaultClass) where T : class
        {
            if (classInstance != null)
            {
                return classInstance as T;
            }
            return Activator.CreateInstance(defaultClass) as T;
        }

        /**
        * This method is to get the hcx protocol base path.
        */
        public string GetHCXProtocolBasePath()
        {
            return config.ProtocolBasePath;
        }

        /**
        * This method is to get the participant code.
        */
        public string GetParticipantCode()
        {
            return config.ParticipantCode;
        }

        /**
         * This method is to get the authorization base path.
         */
        public string GetAuthBasePath()
        {
            return config.AuthBasePath;
        }

        /**
         * This method is to get the username.
         */
        public string GetUsername()
        {
            return config.UserName;
        }

        /**
        * This method is to get the password.
        */
        public string GetPassword()
        {
            return config.Password;
        }

        /**
        * This method is to get the encryption private key.
        */
        public string GetPrivateKey()
        {
            return config.EncryptionPrivateKey;
        }

        /**
        * This method is to get the HCX implementation guide base path.
        */
        public string GetHCXIGBasePath()
        {
            return config.HcxIGBasePath;
        }

        /**
         * This method is to get the NRCES implementation guide base path.
         */
        public string GetNRCESIGBasePath()
        {
            return config.NrcesIGBasePath;
        }
    }
}
