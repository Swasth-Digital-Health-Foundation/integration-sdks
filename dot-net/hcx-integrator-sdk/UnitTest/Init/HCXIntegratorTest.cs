using Io.HcxProtocol.Init;
using Io.HcxProtocol.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Init
{
    [TestClass]
    public class HCXIntegratorTest
    {
        private Config configObj;

        [TestMethod]
        public void TestInitializeConfigObj()
        {
            configObj = new Config();
            configObj.ProtocolBasePath = "http://localhost:8095";
            configObj.ParticipantCode = "participant@01";
            configObj.AuthBasePath = "http://localhost:8080";
            configObj.UserName = "participant@gmail.com";
            configObj.Password = "12345";
            configObj.EncryptionPrivateKey = "Mz-VPPyU4RlcuYv1IwIvzw";
            configObj.HcxIGBasePath = "https://ig.hcxprotocol.io/v0.7/";

            HCXIntegrator hcxIntegrator = HCXIntegrator.GetInstance(configObj);

            Assert.AreEqual("http://localhost:8095", hcxIntegrator.GetHCXProtocolBasePath());
            Assert.AreEqual("participant@01", hcxIntegrator.GetParticipantCode());
            Assert.AreEqual("http://localhost:8080", hcxIntegrator.GetAuthBasePath());
            Assert.AreEqual("participant@gmail.com", hcxIntegrator.GetUsername());
            Assert.AreEqual("12345", hcxIntegrator.GetPassword());
            Assert.AreEqual("Mz-VPPyU4RlcuYv1IwIvzw", hcxIntegrator.GetPrivateKey());
            Assert.AreEqual("https://ig.hcxprotocol.io/v0.7/", hcxIntegrator.GetHCXIGBasePath());
            //System.out.println("password 1 " + hcxIntegrator.GetPassword());

            Config configObj1 = new Config();
            configObj1.ProtocolBasePath = "http://localhost:8095";
            configObj1.ParticipantCode = "participant@01";
            configObj1.AuthBasePath = "http://localhost:8080";
            configObj1.UserName = "participant@gmail.com";
            configObj1.Password = "67890";
            configObj1.EncryptionPrivateKey = "Mz-VPPyU4RlcuYv1IwIvzw";
            configObj1.HcxIGBasePath = "https://ig.hcxprotocol.io/v0.7/";
            HCXIntegrator hcxIntegrator1 = HCXIntegrator.GetInstance(configObj1);

            Assert.AreEqual("67890", hcxIntegrator1.GetPassword());
            //System.out.println("password 2 " + hcxIntegrator1.GetPassword());

            Assert.AreEqual("12345", hcxIntegrator.GetPassword());
            //System.out.println("password 1 " + hcxIntegrator.GetPassword());
        }

        [TestMethod]
        public void TestInitializeConfigString()
        {
            string configStr = "{\"Password\":\"12345\",\"ProtocolBasePath\":\"http://localhost:8095\",\"HcxIGBasePath\":\"https://ig.hcxprotocol.io/v0.7/\",\"AuthBasePath\":\"http://localhost:8080\",\"EncryptionPrivateKey\":\"Mz-VPPyU4RlcuYv1IwIvzw\",\"ParticipantCode\":\"participant@01\",\"UserName\":\"participant@gmail.com\"}";

            Config configObj = new Config();
            configObj = JSONUtils.Deserialize<Config>(configStr);

            HCXIntegrator hcxIntegrator = HCXIntegrator.GetInstance(configObj);

            Assert.IsNotNull(hcxIntegrator.GetHCXProtocolBasePath(), "Pass");
            Assert.IsNotNull(hcxIntegrator.GetParticipantCode(), "Pass");
            Assert.IsNotNull(hcxIntegrator.GetAuthBasePath(), "Pass");
            Assert.IsNotNull(hcxIntegrator.GetUsername(), "Pass");
            Assert.IsNotNull(hcxIntegrator.GetPassword(), "Pass");
            Assert.IsNotNull(hcxIntegrator.GetPrivateKey(), "Pass");
            Assert.IsNotNull(hcxIntegrator.GetHCXIGBasePath(), "Pass");
        }

    }
}
