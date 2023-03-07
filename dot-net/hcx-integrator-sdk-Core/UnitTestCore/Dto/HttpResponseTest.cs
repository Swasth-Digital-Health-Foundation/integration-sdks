using Io.HcxProtocol.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestCore.Dto
{
    [TestClass]
    public class HttpResponseTest
    {
        [TestMethod]
        public void HttpResponseUnitTest()
        {
            HttpResponse httpResponse = new HttpResponse(200, "body");
            Assert.AreEqual(200, httpResponse.Status);
            Assert.AreEqual("body", httpResponse.Body);
        }

    }
}
