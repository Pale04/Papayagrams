using DataAccess;
using Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;

namespace Contracts.Tests
{
    [TestClass()]
    public class LoginServiceImplementationTest
    {
        private readonly ServiceImplementation _serviceImplementation = new ServiceImplementation();
        private readonly PlayerDC _registeredPlayer = new PlayerDC()
        {
            Id = 1,
            Username = "Pale04",
            Email = "epalemolina@hotmail.com",
            Password = "040704"
        };

        [TestInitialize()]
        public void SetUp()
        {
            _serviceImplementation.RegisterUser(_registeredPlayer);
        }

        [TestCleanup()]
        public void CleanUp()
        {
            DataBaseOperation.RebootDataBase();
        }

        [TestMethod()]
        public void RegisterUserSuccesfulTest()
        {
            PlayerDC newPlayer = new PlayerDC()
            {
                Username = "Pale",
                Email = "epalemolina@gmail.com",
                Password = "asdfas´461ds+"
            };

            int expected = 0;
            int result = _serviceImplementation.RegisterUser(newPlayer);
            Assert.AreEqual(expected, result, "RegisterUserSuccesfulTest");
        }

        [TestMethod]
        public void RegisterUserEmptyTest()
        {
            int expected = 101;
            int result = _serviceImplementation.RegisterUser(new PlayerDC());
            Assert.AreEqual(expected, result, "RegisterUserEmptyTest");
        }

        [TestMethod()]
        public void RegisterUserUsernameExistsTest()
        {
            int expected = 201;
            int result = _serviceImplementation.RegisterUser(_registeredPlayer);
            Assert.AreEqual(expected, result, "RegisterUserUsernameExistsTest");
        }

        [TestMethod()]
        public void RegisterUserEmailExistsTest()
        {
            PlayerDC newPlayer = new PlayerDC()
            {
                Username = "David",
                Email = _registeredPlayer.Email,
                Password = "asdfas´461ds+"
            };

            int expected = 202;
            int result = _serviceImplementation.RegisterUser(newPlayer);
            Assert.AreEqual(expected, result, "RegisterUserEmailExistsTest");
        }

        [TestMethod()]
        public void LogInSuccesfulTest()
        {
            (int code,PlayerDC result) = _serviceImplementation.Login(_registeredPlayer.Username, _registeredPlayer.Password);
            Assert.AreEqual(_registeredPlayer, result, "LogInSuccesfulTest");
        }

        [TestMethod]
        public void LogInEmptyUsernameTest()
        {
            //TODO: Implement
        }

        [TestMethod]
        public void LogInEmptyPasswordTest()
        {
            //TODO
        }

        //The first time might not pass
        [TestMethod()]
        public void LogInUserNonExistentTest()
        {
            //TODO
        }

        [TestMethod()]
        public void LogInIncorrectPasswordTest()
        {
            //TODO
        }

        [TestMethod]
        public void LogOutSuccesfulTest()
        {
            int expected = 0;
            int result = _serviceImplementation.Logout(_registeredPlayer.Username);
            Assert.AreEqual(expected, result, "LogOutSuccesfulTest");
        }

        [TestMethod]
        public void LogOutEmptyUsernameTest()
        {
            int expected = 101;
            int result = _serviceImplementation.Logout("");
            Assert.AreEqual(expected, result, "LogOutEmptyUsernameTest");
        }

        [TestMethod()]
        public void LogOutNonExistUsernameTest()
        {
            int expected = 205;
            int result = _serviceImplementation.Logout("Pale");
            Assert.AreEqual(expected, result, "LogOutNonExistUsernameTest");
        }
    }
}