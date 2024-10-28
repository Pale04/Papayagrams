using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MailService.Tests
{
    [TestClass()]
    public class MailserviceTests
    {
        private static readonly MailService _mailService = new MailService();

        [TestMethod()]
        public void SendMailSuccesfulTest()
        {
            int result = _mailService.SendMail("epalemolina@gmail.com", "Welcome to Papayagrams", "Hello papayita");
            Assert.AreEqual(0, result, "SendMailSuccesfulTest");
        }
    }
}