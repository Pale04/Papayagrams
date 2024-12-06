using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainClasses;
using LanguageExt;
using System;
using System.Data.Entity.Core;
using Tests;
using System.Collections.Generic;
using System.Linq;

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
            new DomainClasses.Achievement() { Id = 11, Description = "1 game won in original game mode"},
        };

        [TestInitialize()]
        public void SetUp()
        {
            UserDB.RegisterUser(_registeredPlayer1);
            UserDB.RegisterUser(_registeredPlayer2);
            DataBaseOperation.CreateGameHistoryPlayer(_registeredPlayer1.Id);
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
        public void GetPlayerStatsSuccessfulTest()
        {
            PlayerStats expected = new PlayerStats()
            {
                OriginalGamesPlayed = 60,
                TimeAttackGamesPlayed = 30,
                SuddenDeathGamesPlayed = 103,
                OriginalGamesWon = 10,
                TimeAttackGamesWon = 5,
                SuddenDeathGamesWon = 3,
                FriendsAmount = 0
            };

            Option<PlayerStats> result = UserDB.GetPlayerStats(_registeredPlayer1.Username);
            Assert.AreEqual(expected, (PlayerStats)result.Case, "GetPlayerStatsSuccessfulTest");
        }

        [TestMethod()]
        public void GetPlayerStatsNonExistentUserTest()
        {
            Option<PlayerStats> result = UserDB.GetPlayerStats("Pale");
            Assert.IsTrue(result.IsNone, "GetPlayerStatsNonExistentUserTest");
        }

        [TestMethod()]
        public void GetPlayerAchievementsSuccessfulTest()
        {
            _achievements[0].IsAchieved = true;
            List<DomainClasses.Achievement> expected = new List<DomainClasses.Achievement>()
            {
                _achievements[0]
            };
            List<DomainClasses.Achievement> result = UserDB.GetPlayerAchievements(_registeredPlayer1.Username);

            Assert.IsTrue(expected.SequenceEqual(result), "GetPlayerAchievementsSuccessfulTest");
        }

        [TestMethod()]
        public void GetPlayerAchievementsWithoutAnyAchieved()
        {
            _achievements[0].IsAchieved = false;
            List<DomainClasses.Achievement> expected = new List<DomainClasses.Achievement>()
            {
                _achievements[0]
            };
            List<DomainClasses.Achievement> result = UserDB.GetPlayerAchievements(_registeredPlayer2.Username);
            Assert.IsTrue(expected.SequenceEqual(result), "GetPlayerAchievementsWithoutAnyAchieved");
        }

        [TestMethod()]
        public void GetPlayerAchievementsNonExistentUserTest()
        {
            List<DomainClasses.Achievement> result = UserDB.GetPlayerAchievements("Pale");
            Assert.IsTrue(result.Count == 2, "GetPlayerAchievementsNonExistentUserTest");
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
        public void GetGlobalLeaderboardSuccessfulTest()
        {
            List<LeaderboardStats> expected = new List<LeaderboardStats>
            {
                new LeaderboardStats(_registeredPlayer1.Username, new PlayerStats()
                {
                    OriginalGamesPlayed = 60,
                    TimeAttackGamesPlayed = 30,
                    SuddenDeathGamesPlayed = 103,
                    OriginalGamesWon = 10,
                    TimeAttackGamesWon = 5,
                    SuddenDeathGamesWon = 3
                }),
                new LeaderboardStats(_registeredPlayer2.Username, new PlayerStats()
                {
                    OriginalGamesPlayed = 0,
                    TimeAttackGamesPlayed = 0,
                    SuddenDeathGamesPlayed = 0,
                    OriginalGamesWon = 0,
                    TimeAttackGamesWon = 0,
                    SuddenDeathGamesWon = 0
                })
            };
            List<LeaderboardStats> result = UserDB.GetGlobalLeaderboard();
            Assert.IsTrue(expected.SequenceEqual(result), "GetGlobalLeaderboardSuccessfulTest");
        }
    }
}