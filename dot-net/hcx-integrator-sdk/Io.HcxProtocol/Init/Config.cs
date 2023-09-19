namespace Io.HcxProtocol.Init
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// A class that hold HCX configuration variables and its values.
    /// </summary>
    public class Config
    {
        public string ProtocolBasePath { get; set; }
        public string ParticipantCode { get; set; }
        public string AuthBasePath { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EncryptionPrivateKey { get; set; }
        public string HcxIGBasePath { get; set; }
        public string NrcesIGBasePath { get; set; }
        public string IncomingRequestClass { get; set; }
        public string OutgoingRequestClass { get; set; }
        public bool FhirValidationEnabled { get; set; }  
        public string Secret { get; set; }
        public string ParticipantGenerateToken { get; set; }
        public string signingPrivateKey { get; set; }
        public string LogFilePath { get; set; }  
        public string LogFileName { get; set; }

        //public  string getString(string path)
        //{
        //    ConfigValue v = find(path, ConfigValueType.STRING);
        //    return (string)v.unwrapped();
        //}



    }
}
