namespace Io.HcxProtocol.Init
{
    /**
     * Library  : Io.Hcx.Protocol.Core
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// The methods and variables to access the configuration.
    /// </summary>
    public static class HCXIntegrator
    {
        public static Config config;

        /// <summary>
        /// This method is to initialize config factory by passing the configuration as object.
        /// </summary>
        /// <param name="_config">A object that contains configuration variables and its values.</param>
        /// <exception cref="System.Exception"></exception>
        public static void initConfig(Config _config)
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
            else if (string.IsNullOrEmpty(_config.AuthBasePath))
            {
                prop = "AuthBasePath";
            }
            else if (string.IsNullOrEmpty(_config.UserName))
            {
                prop = "UserName";
            }
            else if (string.IsNullOrEmpty(_config.Password))
            {
                prop = "Password";
            }
            else if (string.IsNullOrEmpty(_config.EncryptionPrivateKey))
            {
                prop = "EncryptionPrivateKey";
            }
            else if (string.IsNullOrEmpty(_config.IgUrl))
            {
                prop = "IgUrl";
            }

            if (prop != null)
                throw new System.Exception(prop + " is missing or has empty value, please add to the configuration.");
            else
                config = _config;
        }
    }
}
