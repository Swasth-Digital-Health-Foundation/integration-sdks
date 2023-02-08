using Io.HcxProtocol.Init;
using Io.HcxProtocol.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Init
{
    [TestClass]
    public class HCXIntegratorTest
    {
        [TestMethod]
        public void TestInitializeConfigObj()
        {
            Config configObj = new Config();
            configObj.ProtocolBasePath = "http://localhost:8095";
            configObj.ParticipantCode = "participant@01";
            configObj.AuthBasePath = "http://localhost:8080";
            configObj.UserName = "participant@gmail.com";
            configObj.Password = "12345";
            configObj.EncryptionPrivateKey = "Mz-VPPyU4RlcuYv1IwIvzw";
            configObj.IgUrl = "http://localhost:8090";

            HCXIntegrator.initConfig(configObj);

            Assert.IsNotNull(HCXIntegrator.config.ProtocolBasePath,"Pass");
            Assert.IsNotNull(HCXIntegrator.config.ParticipantCode, "Pass");
            Assert.IsNotNull(HCXIntegrator.config.AuthBasePath, "Pass");
            Assert.IsNotNull(HCXIntegrator.config.UserName, "Pass");
            Assert.IsNotNull(HCXIntegrator.config.Password, "Pass");
            Assert.IsNotNull(HCXIntegrator.config.EncryptionPrivateKey, "Pass");
            Assert.IsNotNull(HCXIntegrator.config.IgUrl, "Pass");
        }

        [TestMethod]
        public void TestInitializeConfigString()
        {
            string configStr = "{\"Password\":\"12345\",\"ProtocolBasePath\":\"http://localhost:8095\",\"IgUrl\":\"http://localhost:8090\",\"AuthBasePath\":\"http://localhost:8080\",\"EncryptionPrivateKey\":\"Mz-VPPyU4RlcuYv1IwIvzw\",\"ParticipantCode\":\"participant@01\",\"UserName\":\"participant@gmail.com\"}";

            Config configObj = new Config();
            configObj=JSONUtils.Deserialize<Config>(configStr);

            HCXIntegrator.initConfig(configObj);

            Assert.IsNotNull(HCXIntegrator.config.ProtocolBasePath, "Pass");
            Assert.IsNotNull(HCXIntegrator.config.ParticipantCode, "Pass");
            Assert.IsNotNull(HCXIntegrator.config.AuthBasePath, "Pass");
            Assert.IsNotNull(HCXIntegrator.config.UserName, "Pass");
            Assert.IsNotNull(HCXIntegrator.config.Password, "Pass");
            Assert.IsNotNull(HCXIntegrator.config.EncryptionPrivateKey, "Pass");
            Assert.IsNotNull(HCXIntegrator.config.IgUrl, "Pass");

        }

    }
}
