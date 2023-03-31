namespace Io.HcxProtocol.Init
{
    /**
     * Library  : Io.Hcx.Protocol.Core
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
        public string IgUrl { get; set; }
    }
}
