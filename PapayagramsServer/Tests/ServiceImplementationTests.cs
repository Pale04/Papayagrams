using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Contracts.Tests
{
    [TestClass()]
    public class ServiceImplementationTests
    {
        private ServiceImplementation ServiceImplementation = new ServiceImplementation();

        //TODO: Implement set up
        //TODO: Implement tear down

        [TestMethod()]
        public void RegisterUserSuccesfulTest()
        {
            PlayerDC player = new PlayerDC()
            {
                Username = "Pale04",
                Email = "epalemolina@hotmail.com",
                Password = "asdfas´461ds+"
            };

            int expected = 1;
            int result = ServiceImplementation.RegisterUser(player);
            Assert.AreEqual(expected, result, "RegisterUserSuccesfulTest");
        }

        [TestMethod()]
        public void LogInSuccesfulTest()
        {
            //TODO: Implement set up method to insert a player in the database

            PlayerDC expected = new PlayerDC()
            {
                Id = 1,
                Username = "Pale04",
                Email = "epalemolina@hotmail.com",
                Password = "asdfas´461ds+"
            };

            PlayerDC result = ServiceImplementation.Login("Pale04", "asdfas´461ds+");
            Assert.AreEqual(expected, result, "LogInSuccesfulTest");
        }

        [TestMethod()]
        public void LogInUserInexistentTest()
        {
            //TODO: Implement set up method to insert a player in the database
            try
            {
                PlayerDC result = ServiceImplementation.Login("Pale", "asdfas´461ds+");
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
            try
            {
                PlayerDC result = ServiceImplementation.Login("Pale04", "asdfas´461ds");
                Assert.Fail("LogInIncorrectPasswordTest");
            }
            catch (Exception error)
            {
                Assert.AreEqual("Incorrect password", error.Message, "LogInIncorrectPasswordTest");
            }
        }
    }
}