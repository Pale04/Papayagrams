using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace MailService.Tests
{
    [TestClass()]
    public class MailserviceTests
    {
        MailService mailService = new MailService();

        [TestMethod()]
        public void SendMailSuccesfulTest()
        {
            Task result = mailService.SendMail("epalemolina@gmail.com", "Welcome to Papayagrams", "Hello papayita");
            Assert.AreEqual(Task.CompletedTask, result, "SendMailSuccesfulTest");
        }
    }
}