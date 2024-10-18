using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests
{
    [TestClass()]
    public class UserServiceImplementationTest
    {
        private ServiceImplementation ServiceImplementation = new ServiceImplementation();

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
                int result = ServiceImplementation.RegisterUser(newPlayer);
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
                int result = ServiceImplementation.RegisterUser(newPlayer);
                Assert.Fail("RegisterUserEmailExistsTest");
            }
            catch (Exception error)
            {
                Assert.AreEqual(error.Message, "An account with the same email exists", "RegisterUserEmailExistsTest");
            }
        }
    }
}
