using DataAccess;
using DomainClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity.Core;

namespace DataAccess.Tests
{
    [TestClass()]
    public class DataBaseExceptionsTest
    {
        private readonly Player _registeredPlayer1 = new Player()
        {
            Id = 1,
            Username = "Pale04",
            Email = "epalemolina@hotmail.com",
            Password = "040704",
            ProfileIcon = 1
        };

        [TestMethod()]
        public void RegisterUserExceptionTest()
        {
            try
            {
                UserDB.RegisterUser(_registeredPlayer1);
                Assert.Fail("RegisterUserExceptionTest");
            }
            catch (Exception error)
            {
                Assert.IsInstanceOfType(error, typeof(EntityException), "RegisterUserExceptionTest");
            }
        }

        [TestMethod()]
        public void LogInExceptionTest()
        {
            try
            {
                UserDB.LogIn(_registeredPlayer1.Username, _registeredPlayer1.Password);
                Assert.Fail("LogInExceptionTest");
            }
            catch (Exception error)
            {
                Assert.IsInstanceOfType(error, typeof(EntityException), "LogInExceptionTest");
            }
        }

        [TestMethod()]
        public void VerifyAccountExceptionTest()
        {
            try
            {
                UserDB.VerifyAccount(_registeredPlayer1.Username);
                Assert.Fail("VerifyAccountExceptionTest");
            }
            catch (Exception error)
            {
                Assert.IsInstanceOfType(error, typeof(EntityException), "VerifyAccountExceptionTest");
            }
        }

        [TestMethod()]
        public void GetPlayerByUsernameExceptionTest()
        {
            try
            {
                UserDB.GetPlayerByUsername(_registeredPlayer1.Username);
                Assert.Fail("GetPlayerByUsernameExceptionTest");
            }
            catch (Exception error)
            {
                Assert.IsInstanceOfType(error, typeof(EntityException), "GetPlayerByUsernameExceptionTest");
            }
        }

        [TestMethod()]
        public void GetPlayerByEmailExceptionTest()
        {
            try
            {
                UserDB.GetPlayerByEmail(_registeredPlayer1.Email);
                Assert.Fail("GetPlayerByEmailExceptionTest");
            }
            catch (Exception error)
            {
                Assert.IsInstanceOfType(error, typeof(EntityException), "GetPlayerByEmailExceptionTest");
            }
        }

        [TestMethod()]
        public void LogOutExceptionTest()
        {
            try
            {
                UserDB.LogOut(_registeredPlayer1.Username);
                Assert.Fail("LogOutExceptionTest");
            }
            catch (Exception error)
            {
                Assert.IsInstanceOfType(error, typeof(EntityException), "LogOutExceptionTest");
            }
        }

        [TestMethod()]
        public void UpdateUserStatusExceptionTest()
        {
            try
            {
                UserDB.UpdateUserStatus(_registeredPlayer1.Username, PlayerStatus.online);
                Assert.Fail("UpdateUserStatusExceptionTest");
            }
            catch (Exception error)
            {
                Assert.IsInstanceOfType(error, typeof(EntityException), "UpdateUserStatusExceptionTest");
            }
        }

        [TestMethod()]
        public void GetPlayerAchievementsExceptionTest()
        {
            try
            {
                UserDB.GetPlayerAchievements(_registeredPlayer1.Username);
                Assert.Fail("GetPlayerAchievementsExceptionTest");
            }
            catch (Exception error)
            {
                Assert.IsInstanceOfType(error, typeof(EntityException), "GetPlayerAchievementsExceptionTest");
            }
        }

        [TestMethod()]
        public void GetPlayerStatusExceptionTest()
        {
            try
            {
                UserDB.GetPlayerStatus(_registeredPlayer1.Username);
                Assert.Fail("GetPlayerStatusExceptionTest");
            }
            catch (Exception error)
            {
                Assert.IsInstanceOfType(error, typeof(EntityException), "GetPlayerStatusExceptionTest");
            }
        }

        [TestMethod()]
        public void UpdateApplicationSettingsExceptionTest()
        {
            try
            {
                UserDB.UpdateApplicationSettings(_registeredPlayer1.Username, new ApplicationSettings());
                Assert.Fail("UpdateApplicationSettingsExceptionTest");
            }
            catch (Exception error)
            {
                Assert.IsInstanceOfType(error, typeof(EntityException), "UpdateApplicationSettingsExceptionTest");
            }
        }

        [TestMethod()]
        public void GetApplicationSettingsExceptionTest()
        {
            try
            {
                UserDB.GetApplicationSettings(_registeredPlayer1.Username);
                Assert.Fail("GetApplicationSettingsExceptionTest");
            }
            catch (Exception error)
            {
                Assert.IsInstanceOfType(error, typeof(EntityException), "GetApplicationSettingsExceptionTest");
            }
        }

        [TestMethod()]
        public void UpdateProfileIconExceptionTest()
        {
            try
            {
                UserDB.UpdateProfileIcon(_registeredPlayer1.Username, 2);
                Assert.Fail("UpdateProfileIconExceptionTest");
            }
            catch (Exception error)
            {
                Assert.IsInstanceOfType(error, typeof(EntityException), "UpdateProfileIconExceptionTest");
            }
        }

        [TestMethod()]
        public void UpdatePasswordExceptionTest()
        {
            try
            {
                UserDB.UpdatePassword(_registeredPlayer1.Username, _registeredPlayer1.Password, "123456");
                Assert.Fail("UpdatePasswordExceptionTest");
            }
            catch (Exception error)
            {
                Assert.IsInstanceOfType(error, typeof(EntityException), "UpdatePasswordExceptionTest");
            }
        }

        [TestMethod()]
        public void UpdatePasswordWithoutVerificationExceptionTest()
        {
            try
            {
                UserDB.UpdatePassword(_registeredPlayer1.Email, "1234678");
                Assert.Fail("UpdatePasswordWithoutVerificationExceptionTest");
            }
            catch (Exception error)
            {
                Assert.IsInstanceOfType(error, typeof(EntityException), "UpdatePasswordWithoutVerificationExceptionTest");
            }
        }


    }
}
