using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainClasses;
using LanguageExt;
using System;
using System.Data.Entity.Core;
using Tests;
using System.Collections.Generic;
using System.Linq;
using Contracts;

namespace DataAccess.Tests
{
    [TestClass()]
    public class UserDBTests
    {
        private readonly Player _registeredPlayer1 = new Player()
        {
            Id = 1,
            Username = "Pale04",
            Email = "epalemolina@hotmail.com",
            Password = "040704",
            ProfileIcon = 1
        };
        private readonly Player _registeredPlayer2 = new Player()
        {
            Id = 2,
            Username = "David04",
            Email = "david@gmail.com",
            Password = "040704",
            ProfileIcon = 1
        };

        private readonly List<DomainClasses.Achievement> _achievements = new List<DomainClasses.Achievement>()
        {
            new DomainClasses.Achievement() { Id = 1},
            new DomainClasses.Achievement() { Id = 2},
            new DomainClasses.Achievement() { Id = 3},
            new DomainClasses.Achievement() { Id = 4},
            new DomainClasses.Achievement() { Id = 5},
            new DomainClasses.Achievement() { Id = 6},
            new DomainClasses.Achievement() { Id = 7},
            new DomainClasses.Achievement() { Id = 8},
            new DomainClasses.Achievement() { Id = 9},
            new DomainClasses.Achievement() { Id = 10},
            new DomainClasses.Achievement() { Id = 11},
            new DomainClasses.Achievement() { Id = 12},
            new DomainClasses.Achievement() { Id = 13},
            new DomainClasses.Achievement() { Id = 14},
            new DomainClasses.Achievement() { Id = 15},
            new DomainClasses.Achievement() { Id = 16}
        };

        [TestInitialize()]
        public void SetUp()
        {
            UserDB.RegisterUser(_registeredPlayer1);
            UserDB.RegisterUser(_registeredPlayer2);
            DataBaseOperation.RegisterUserAchievement(_registeredPlayer1.Id, _achievements[0].Id);
        }

        [TestCleanup()]
        public void CleanUp()
        {
            DataBaseOperation.RebootDataBase();
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
            UserDB.VerifyAccount(_registeredPlayer1.Username);
            int result = UserDB.LogIn(_registeredPlayer1.Username, _registeredPlayer1.Password);
            Assert.AreEqual(expected, result, "LogInSuccessfulTest");
        }

        [TestMethod()]
        public void LogInPendingAccountTest()
        {
            int expected = 1;
            int result = UserDB.LogIn(_registeredPlayer1.Username, _registeredPlayer1.Password);
            Assert.AreEqual(expected, result, "LogInPendingAccountTest");
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
            int result = UserDB.LogIn(_registeredPlayer1.Username, "1");
            Assert.AreEqual(expected, result, "LogInPasswordIncorrectTest");
        }

        [TestMethod]
        public void VerifyAccountSuccessfulTest()
        {
            int expected = 1;
            int result = UserDB.VerifyAccount(_registeredPlayer1.Username);
            Assert.AreEqual(expected, result, "VerifyAccountSuccessfulTest");
        }

        [TestMethod]
        public void VerifyAccountNonExistentTest()
        {
            int expected = 0;
            int result = UserDB.VerifyAccount("Pale");
            Assert.AreEqual(expected, result, "VerifyAccountNonExistentTest");
        }

        [TestMethod()]
        public void GetPlayerByUsernameSuccessfulTest()
        {
            Option<Player> result = UserDB.GetPlayerByUsername(_registeredPlayer1.Username);
            Assert.AreEqual(_registeredPlayer1, (Player)result.Case, "GetPlayerByUsernameSuccessfulTest");
        }

        //It it the same case when the username is null
        [TestMethod()]
        public void GetPlayerByUsernameNonExistentTest()
        {
            Option<Player> result = UserDB.GetPlayerByUsername("Pale");
            Assert.IsTrue(result.IsNone, "GetPlayerByUsernameNonExistentTest");
        }

        [TestMethod()]
        public void GetPlayerByEmailSuccessfulTest()
        {
            Option<Player> result = UserDB.GetPlayerByEmail(_registeredPlayer1.Email);
            Assert.AreEqual(_registeredPlayer1, (Player)result.Case, "GetPlayerByEmailSuccessfulTest");
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
            int result = UserDB.LogOut(_registeredPlayer1.Username);
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

        [TestMethod()]
        public void UpdateUserStatusSuccessfulTest()
        {
            int expected = 1;
            int result = UserDB.UpdateUserStatus(_registeredPlayer1.Username, PlayerStatus.in_game);
            Assert.AreEqual(expected, result, "UpdateUserStatusSuccessfulTest");
        }

        [TestMethod()]
        public void UpdateUserStatusNonExistentTest()
        {
            int expected = 0;
            int result = UserDB.UpdateUserStatus("Pale", PlayerStatus.in_game);
            Assert.AreEqual(expected, result, "UpdateUserStatusNonExistentTest");
        }

        [TestMethod()]
        public void GetPlayerAchievementsSuccessfulTest()
        {
            List<DomainClasses.Achievement> expected = new List<DomainClasses.Achievement>(_achievements);
            expected[0].IsAchieved = true;
            List<DomainClasses.Achievement> result = UserDB.GetPlayerAchievements(_registeredPlayer1.Username);
            Assert.IsTrue(expected.SequenceEqual(result), "GetPlayerAchievementsSuccessfulTest");
        }

        [TestMethod()]
        public void GetPlayerAchievementsWithoutAnyAchieved()
        {
            List<DomainClasses.Achievement> expected = new List<DomainClasses.Achievement>(_achievements);
            expected[0].IsAchieved = false;
            List<DomainClasses.Achievement> result = UserDB.GetPlayerAchievements(_registeredPlayer2.Username);
            Assert.IsTrue(expected.SequenceEqual(result), "GetPlayerAchievementsWithoutAnyAchieved");
        }

        [TestMethod()]
        public void GetPlayerAchievementsNonExistentUserTest()
        {
            List<DomainClasses.Achievement> result = UserDB.GetPlayerAchievements("Pale");
            Assert.IsTrue(result.Count == 16, "GetPlayerAchievementsNonExistentUserTest");
        }

        [TestMethod()]
        public void GetPlayerStatusSuccessfulTest()
        {
            UserDB.UpdateUserStatus(_registeredPlayer1.Username, PlayerStatus.online);
            PlayerStatus expected = PlayerStatus.online;
            PlayerStatus result = UserDB.GetPlayerStatus(_registeredPlayer1.Username);
            Assert.AreEqual(expected, result, "GetPlayerStatusSuccessfulTest");
        }

        [TestMethod()]
        public void UpdateApplicationSettingsSuccessfulTest()
        {
            ApplicationSettings settings = new ApplicationSettings()
            {
                PieceColor = 10,
                SelectedLanguage = ApplicationLanguage.auto,
                Cursor = 2
            };
            int expected = 1;
            int result = UserDB.UpdateApplicationSettings(_registeredPlayer1.Username, settings);
            Assert.AreEqual(expected, result, "UpdateApplicationSettingsSuccessfulTest");
        }

        [TestMethod()]
        public void UpdateApplicationSettingsNonExistentUserTest()
        {
            ApplicationSettings settings = new ApplicationSettings()
            {
                PieceColor = 1,
                SelectedLanguage = ApplicationLanguage.spanish,
                Cursor = 1
            };
            int expected = 0;
            int result = UserDB.UpdateApplicationSettings("deivid", settings);
            Assert.AreEqual(expected, result, "UpdateApplicationSettingsNonExistentUserTest");
        }

        [TestMethod()]
        public void GetApplicationSettingsSuccessfulTest()
        {
            ApplicationSettings expected = new ApplicationSettings()
            {
                PieceColor = 1,
                SelectedLanguage = ApplicationLanguage.auto,
                Cursor = 1
            };
            Option<ApplicationSettings> result = UserDB.GetApplicationSettings(_registeredPlayer1.Username);
            Assert.AreEqual(expected, (ApplicationSettings)result.Case, "GetApplicationSettingsSuccessfulTest");
        }

        [TestMethod()]
        public void GetApplicationSettingsNonExistentUserTest()
        {
            Option<ApplicationSettings> result = UserDB.GetApplicationSettings("Pale");
            Assert.IsTrue(result.IsNone, "GetApplicationSettingsNonExistentUserTest");
        }

        [TestMethod()]
        public void UpdateProfileIconSuccessfulTest()
        {
            int expected = 1;
            int result = UserDB.UpdateProfileIcon(_registeredPlayer1.Username, 2);
            Assert.AreEqual(expected, result, "UpdateProfileIconSuccessfulTest");
        }

        [TestMethod()]
        public void UpdateProfileIconSameIconTest()
        {
            int expected = 0;
            int result = UserDB.UpdateProfileIcon(_registeredPlayer1.Username, 1);
            Assert.AreEqual(expected, result, "UpdateProfileIconSameIconTest");
        }

        [TestMethod()]
        public void UpdateProfileIconNonExistentUserTest()
        {
            int expected = -1;
            int result = UserDB.UpdateProfileIcon("Pale", 2);
            Assert.AreEqual(expected, result, "UpdateProfileIconNonExistentUserTest");
        }

        [TestMethod()]
        public void UpdatePasswordSuccessfulTest()
        {
            int expected = 0;
            int result = UserDB.UpdatePassword(_registeredPlayer1.Username, _registeredPlayer1.Password, "123456");
            Assert.AreEqual(expected, result, "UpdatePasswordSuccessfulTest");
        }

        [TestMethod()]
        public void UpdatePasswordIncorrectCurrentPasswordTest()
        {
            int expected = -1;
            int result = UserDB.UpdatePassword(_registeredPlayer1.Username, "abc", "123456");
            Assert.AreEqual(expected, result, "UpdatePasswordIncorrectCurrentPasswordTest");
        }

        [TestMethod()]
        public void UpdatePasswordWithoutVerificationSuccessfulTest()
        {
            int expected = 1;
            int result = UserDB.UpdatePassword(_registeredPlayer1.Email, "123456");
            Assert.AreEqual(expected, result, "UpdatePasswordWithoutVerificationSuccessfulTest");
        }

        [TestMethod()]
        public void UpdatePasswordWithoutVerificationNonExistentPlayerTest()
        {
            int expected = 0;
            int result = UserDB.UpdatePassword("Deivid", "abc");
            Assert.AreEqual(expected, result, "UpdatePasswordWithoutVerificationNonExistentPlayerTest");
        }
    }
}