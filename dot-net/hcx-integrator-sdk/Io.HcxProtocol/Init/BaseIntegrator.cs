using Io.HcxProtocol.Impl;
using Io.HcxProtocol.Interfaces;
using Io.HcxProtocol.Utils;
using System;
using System.Collections.Generic;
using System.Text;

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
            //else if (string.IsNullOrEmpty(_config.AuthBasePath))
            //{
            //    prop = "AuthBasePath";
            //}
            else if(string.IsNullOrEmpty(_config.ParticipantGenerateToken))//cr 12
            {
                prop = "ParticipantGenerationToken";
            }
            else if (string.IsNullOrEmpty(_config.UserName))
            {
                prop = "UserName";
            }
            else if (string.IsNullOrEmpty(_config.Password))
            {
                prop = "Password";
            }
            //else if (string.IsNullOrEmpty(_config.Secrete)) //cr 12
            //{
            //    prop = "Secrete";
            //}

            else if (string.IsNullOrEmpty(_config.EncryptionPrivateKey))
            {
                prop = "EncryptionPrivateKey";
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

            if (prop != null)
                throw new System.Exception(prop + " is missing or has empty value, please add to the configuration.");
            else
                config = _config;
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

        private T GetProcessRequest<T>(string className, Type defaultClass) where T : class
        {
            if (!string.IsNullOrEmpty(className))
            {
                try
                {
                    Type classType = Type.GetType(className);
                    object instance = Activator.CreateInstance(classType);
                    // logger.Info($"Request class {className} provided in the config exists.");
                    return instance as T;
                }
                catch (Exception ex)
                {
                    // logger.Error($"Request class {className} provided in the config map does not exist, hence default {defaultClass} is used.");
                }
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
