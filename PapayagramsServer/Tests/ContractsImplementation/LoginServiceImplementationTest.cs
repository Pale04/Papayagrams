using DataAccess;
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
            using (var context = new papayagramsEntities())
            {
                context.Database.ExecuteSqlCommand("delete from [TimeAtackHistory]");
                context.Database.ExecuteSqlCommand("delete from [SuddenDeathHistory]");
                context.Database.ExecuteSqlCommand("delete from [OriginalGameHistory]");
                context.Database.ExecuteSqlCommand("delete from [UserConfiguration]");
                context.Database.ExecuteSqlCommand("delete from [UserStatus]");
                context.Database.ExecuteSqlCommand("delete from [User]");
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('User', RESEED, 0)");
            }
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
            try
            {
                _serviceImplementation.RegisterUser(new PlayerDC());
                Assert.Fail("RegisterUserEmptyTest");
            }
            catch (FaultException<ServerException> error)
            {
                Assert.AreEqual(1, error.Detail.ErrorCode, "RegisterUserEmptyTest");
            }
        }

        [TestMethod()]
        public void RegisterUserUsernameExistsTest()
        {
            try
            {
                _serviceImplementation.RegisterUser(_registeredPlayer);
                Assert.Fail("RegisterUserUsernameExistsTest");
            }
            catch (FaultException<ServerException> error)
            {
                Assert.AreEqual(101, error.Detail.ErrorCode, "RegisterUserUsernameExistsTest");
            }
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

            try
            {
                _serviceImplementation.RegisterUser(newPlayer);
                Assert.Fail("RegisterUserEmailExistsTest");
            }
            catch (FaultException<ServerException> error)
            {
                Assert.AreEqual(102, error.Detail.ErrorCode, "RegisterUserEmailExistsTest");
            }
        }

        [TestMethod()]
        public void LogInSuccesfulTest()
        {
            PlayerDC result = _serviceImplementation.Login(_registeredPlayer.Username, _registeredPlayer.Password);
            Assert.AreEqual(_registeredPlayer, result, "LogInSuccesfulTest");
        }

        [TestMethod]
        public void LogInEmptyUsernameTest()
        {
            try
            {
                _serviceImplementation.Login("", "123");
                Assert.Fail("LogInEmptyTest");
            }
            catch (FaultException<ServerException> error)
            {
                Assert.AreEqual(103, error.Detail.ErrorCode, "LogInEmptyTest");
            }
        }

        [TestMethod]
        public void LogInEmptyPasswordTest()
        {
            try
            {
                _serviceImplementation.Login("Pale04", "");
                Assert.Fail("LogInEmptyPasswordTest");
            }
            catch (FaultException<ServerException> error)
            {
                Assert.AreEqual(104, error.Detail.ErrorCode, "LogInEmptyPasswordTest");
            }
        }

        [TestMethod()]
        public void LogInUserNonExistentTest()
        {
            try
            {
                _serviceImplementation.Login("Pale", "123");
                Assert.Fail("LogInUserNonExistentTest");
            }
            catch (FaultException<ServerException> error)
            {
                Assert.AreEqual(105, error.Detail.ErrorCode, "LogInUserNonExistentTest");
            }
        }

        [TestMethod()]
        public void LogInIncorrectPasswordTest()
        {
            try
            {
                _serviceImplementation.Login(_registeredPlayer.Username, "123");
                Assert.Fail("LogInIncorrectPasswordTest");
            }
            catch (FaultException<ServerException> error)
            {
                Assert.AreEqual(106, error.Detail.ErrorCode, "LogInIncorrectPasswordTest");
            }
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
            try
            {
                _serviceImplementation.Logout("");
                Assert.Fail("LogOutEmptyUsernameTest");
            }
            catch (FaultException<ServerException> error)
            {
                Assert.AreEqual(1, error.Detail.ErrorCode, "LogOutEmptyUsernameTest");
            }
        }

        [TestMethod()]
        public void LogOutNonExistUsernameTest()
        {
            try
            {
                _serviceImplementation.Logout("Pale");
                Assert.Fail("LogOutNonExistUsernameTest");
            }
            catch (FaultException<ServerException> error)
            {
                Assert.AreEqual(105, error.Detail.ErrorCode, "LogOutNonExistUsernameTest");
            }
        }
    }
}