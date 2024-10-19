using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Contracts.Tests
{
    [TestClass()]
    public class LoginServiceImplementationTest
    {
        private readonly ServiceImplementation _serviceImplementation = new ServiceImplementation();

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
            int result = _serviceImplementation.RegisterUser(player);
            Assert.AreEqual(expected, result, "RegisterUserSuccesfulTest");
        }

        [TestMethod()]
        public void RegisterUserUsernameExistsTest()
        {
            //TODO: Implement set up method to insert a player in the database
            try
            {
                PlayerDC newPlayer = new PlayerDC()
                {
                    Username = "Pale04",
                    Email = "epalemolina@hotmail.com",
                    Password = "asdfas´461ds+"
                };
                int result = _serviceImplementation.RegisterUser(newPlayer);
                Assert.Fail("RegisterUserUsernameExistsTest");
            }
            catch (Exception error)
            {
                Assert.AreEqual(error.Message, "An account with the same username exists", "RegisterUserUsernameExistsTest");
            }
        }

        [TestMethod()]
        public void RegisterUserEmailExistsTest()
        {
            //TODO: Implement set up method to insert a player in the database
            try
            {
                PlayerDC newPlayer = new PlayerDC()
                {
                    Username = "Pale",
                    Email = "epalemolina@hotmail.com",
                    Password = "asdfas´461ds+"
                };
                int result = _serviceImplementation.RegisterUser(newPlayer);
                Assert.Fail("RegisterUserEmailExistsTest");
            }
            catch (Exception error)
            {
                Assert.AreEqual(error.Message, "An account with the same email exists", "RegisterUserEmailExistsTest");
            }
        }

        [TestMethod()]
        public void LogInSuccesfulTest()
        {
            //TODO: Implement set up method to insert a player in the database

            int expected = 0;
            int result = _serviceImplementation.Login("Pale04", "asdfas´461ds+");
            Assert.AreEqual(expected, result, "LogInSuccesfulTest");
        }

        [TestMethod()]
        public void LogInUserInexistentTest()
        {
            //TODO: Implement set up method to insert a player in the database
            try
            {
                int result = _serviceImplementation.Login("Pale", "asdfas´461ds+");
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
            int result = _serviceImplementation.Login("Pale04", "asdfas´461ds+");
            Assert.AreEqual(expected, result, "LogInIncorrectPasswordTest");
        }
    }
}