using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MailService.Tests
{
    [TestClass()]
    public class MailserviceTests
    {
        [TestMethod()]
        public void SendMailSuccesfulTest()
        {
            int result = MailService.SendMail("epalemolina@gmail.com", "Welcome to Papayagrams", "Hello papayita");
            Assert.AreEqual(0, result, "SendMailSuccesfulTest");
        }
    }
}