using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainClasses;
using LanguageExt;
using System;
using System.Data.Entity.Core;

namespace DataAccess.Tests
{
    [TestClass()]
    public class UserDBTests
    {
        private readonly Player _registeredPlayer = new Player()
        {
            Id = 1,
            Username = "Pale04",
            Email = "epalemolina@hotmail.com",
            Password = "040704"
        };

        [TestInitialize()]
        public void SetUp()
        {
            UserDB.RegisterUser(_registeredPlayer);
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
            Player newPlayer = new Player()
            {
                Username = "Pale47",
                Email = "epalemolina@gmail.com",
                Password = "asdfl_´.468*-"
            };

            int expected = 6;
            int result = UserDB.RegisterUser(newPlayer);

            Assert.AreEqual(expected, result, "RegisterUserSuccesfulTest");
        }

        [TestMethod()]
        public void RegisterUserEmptyTest()
        {
            try
            {
                UserDB.RegisterUser(new Player());
                Assert.Fail("RegisterUserEmptyTest");
            }
            catch (Exception error)
            {
                Assert.IsInstanceOfType(error, typeof(EntityCommandExecutionException), "RegisterUserEmptyTest");
            }
        }

        [TestMethod()]
        public void LogInSuccessfulTest()
        {
            int expected = 0;
            int result = UserDB.LogIn(_registeredPlayer.Username, _registeredPlayer.Password);
            Assert.AreEqual(expected, result, "LogInSuccessfulTest");
        }

        //It is the same case when the username is null
        [TestMethod()]
        public void LogInNonExistentAccountTest()
        {
            int expected = -1;
            int result = UserDB.LogIn("Pale", "1");
            Assert.AreEqual(expected, result, "LogInNonExistentAccountTest");
        }

        //It is the same case when the password is null
        [TestMethod()]
        public void LogInPasswordIncorrectTest()
        {
            int expected = -2;
            int result = UserDB.LogIn(_registeredPlayer.Username, "1");
            Assert.AreEqual(expected, result, "LogInPasswordIncorrectTest");
        }

        [TestMethod()]
        public void GetPlayerByUsernameSuccessfulTest()
        {
            Option<Player> result = UserDB.GetPlayerByUsername(_registeredPlayer.Username);
            Assert.AreEqual(_registeredPlayer, result.Case, "GetPlayerByUsernameSuccessfulTest");
        }

        //It it the same case when the username is null
        [TestMethod()]
        public void GetPlayerByNonExistentTest()
        {
            Option<Player> result = UserDB.GetPlayerByUsername("Pale");
            Assert.IsTrue(result.IsNone, "GetPlayerByNonExistentTest");
        }

        [TestMethod()]
        public void GetPlayerByEmailSuccessfulTest()
        {
            Option<Player> result = UserDB.GetPlayerByEmail(_registeredPlayer.Email);
            Assert.AreEqual(_registeredPlayer, result.Case, "GetPlayerByEmailSuccessfulTest");
        }

        //It is the same case when the email is null
        [TestMethod()]
        public void GetPlayerByEmailInexistentTest()
        {
            Option<Player> result = UserDB.GetPlayerByEmail("pale@gmail.com");
            Assert.IsTrue(result.IsNone, "GetPlayerByEmailInexistentTest");
        }

        [TestMethod()]
        public void LogOutSuccessfulTest()
        {
            int expected = 1;
            int result = UserDB.LogOut(_registeredPlayer.Username);
            Assert.AreEqual(expected, result, "LogOutSuccessfulTest");
        }

        [TestMethod()]
        public void LogOutNonExistentAccountTest()
        {
            int expected = 0;
            int result = UserDB.LogOut("Pale");
            Assert.AreEqual(expected, result, "LogOutNonExistentAccountTest");
        }

        [TestMethod()]
        public void LogOutNullUsernameTest()
        {
            int expected = 0;
            int result = UserDB.LogOut(null);
            Assert.AreEqual(expected, result, "LogOutNullUsernameTest");
        }
    }
}