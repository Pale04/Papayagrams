using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Contracts.Tests
{
    [TestClass()]
    public class LoginServiceImplementationTest
    {
        private ServiceImplementation ServiceImplementation = new ServiceImplementation();

        //TODO: Implement set up
        //TODO: Implement tear down

        [TestMethod()]
        public void LogInSuccesfulTest()
        {
            //TODO: Implement set up method to insert a player in the database

            int expected = 0;
            int result = ServiceImplementation.Login("Pale04", "asdfas´461ds+");
            Assert.AreEqual(expected, result, "LogInSuccesfulTest");
        }

        [TestMethod()]
        public void LogInUserInexistentTest()
        {
            //TODO: Implement set up method to insert a player in the database
            try
            {
                int result = ServiceImplementation.Login("Pale", "asdfas´461ds+");
                Assert.Fail("LogInInexistentTest");
            }
            catch (Exception error)
            {
                Assert.AreEqual("Player not found", error.Message, "LogInInexistentTest");
            }
        }

        [TestMethod()]
        public void LogInIncorrectPasswordTest()
        {
            //TODO: Implement set up method to insert a player in the database
            int expected = -1;
            int result = ServiceImplementation.Login("Pale04", "asdfas´461ds+");
            Assert.AreEqual(expected, result, "LogInIncorrectPasswordTest");
        }
    }
}