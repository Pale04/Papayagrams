using DataAccess;
using Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Email = "epalemolina@gmail.com",
            Password = "040704"
        };

        [TestInitialize()]
        public void SetUp()
        {
            UserDB.RegisterUser(new DomainClasses.Player
            {
                Username = _registeredPlayer.Username,
                Email = _registeredPlayer.Email,
                Password = _registeredPlayer.Password
            });
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
                Email = "epalemolina@hotmail.com",
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
            UserDB.VerifyAccount(_registeredPlayer.Username);
            (int code,PlayerDC result) = _serviceImplementation.Login(_registeredPlayer.Username, _registeredPlayer.Password);
            Assert.AreEqual(_registeredPlayer, result, "LogInSuccesfulTest");
        }

        [TestMethod]
        public void LogInEmptyUsernameTest()
        {
            (int code, PlayerDC player) = _serviceImplementation.Login("", _registeredPlayer.Password);
            Assert.AreEqual(203, code, "LogInEmptyUsernameTest");
        }

        [TestMethod]
        public void LogInEmptyPasswordTest()
        {
            (int code, PlayerDC player) = _serviceImplementation.Login(_registeredPlayer.Username, "");
            Assert.AreEqual(204, code, "LogInEmptyPasswordTest");
        }

        //The first time might not pass
        [TestMethod()]
        public void LogInUserNonExistentTest()
        {
            (int code, PlayerDC player) = _serviceImplementation.Login("Pale", "1");
            Assert.AreEqual(205, code, "LogInUserNonExistentTest");
        }

        [TestMethod()]
        public void LogInIncorrectPasswordTest()
        {
            (int code, PlayerDC player) = _serviceImplementation.Login(_registeredPlayer.Username, "1");
            Assert.AreEqual(206, code, "LogInIncorrectPasswordTest");
        }

        [TestMethod()]
        public void LogInPendingAccountTest()
        {
            (int code, PlayerDC player) = _serviceImplementation.Login(_registeredPlayer.Username, _registeredPlayer.Password);
            Assert.AreEqual(207, code, "LogInPendingAccountTest");
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

        [TestMethod()]
        public void VerifyAccountIncorrectCodeTest()
        {
            int expected = 208;
            int result = _serviceImplementation.VerifyAccount(_registeredPlayer.Username, "123");
            Assert.AreEqual(expected, result, "VerifyAccountIncorrectCodeTest");
        }

        [TestMethod()]
        public void VerifyAccountEmptyParametersTest()
        {
            int expected = 101;
            int result = _serviceImplementation.VerifyAccount("", "");
            Assert.AreEqual(expected, result, "VerifyAccountEmptyParametersTest");
        }

        [TestMethod()]
        public void VerifyAccountNonExistentUsernameTest()
        {
            int expected = 208;
            int result = _serviceImplementation.VerifyAccount("Pale", "123");
            Assert.AreEqual(expected, result, "VerifyAccountNonExistentUsernameTest");
        }

        [TestMethod()]
        public void SendAccountVerificationCodeSuccessfulTest()
        {
            int expected = 0;
            int result = _serviceImplementation.SendAccountVerificationCode(_registeredPlayer.Username);
            Assert.AreEqual(expected, result, "SendAccountVerificationCodeSuccessfulTest");
        }

        //It is the same case when the parameter is null
        [TestMethod()]
        public void SendAccountVerificationCodeEmptyUsernameTest()
        {
            int expected = 205;
            int result = _serviceImplementation.SendAccountVerificationCode("");
            Assert.AreEqual(expected, result, "SendAccountVerificationCodeEmptyUsernameTest");
        }
    }
}