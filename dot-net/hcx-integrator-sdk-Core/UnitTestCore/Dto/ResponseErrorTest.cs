using Io.HcxProtocol.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestCore.Dto
{
    [TestClass]
    public class ResponseErrorTest
    {
        [TestMethod]
        public void ResponseErrorUnitTest()
        {
            ResponseError responseError = new ResponseError("code", "message", "trace");

            Assert.AreEqual("code", responseError.Code);
            Assert.AreEqual("message", responseError.Message);
            Assert.AreEqual("trace", responseError.Trace);
        }
    }
}
