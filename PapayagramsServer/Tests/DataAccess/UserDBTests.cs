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

        private readonly DomainClasses.Achievement _achievement1 = new DomainClasses.Achievement()
        {
            Id = 1,
            Description = "Description1",
        };
        private readonly DomainClasses.Achievement _achievement2 = new DomainClasses.Achievement()
        {
            Id = 2,
            Description = "Description2"
        };

        [TestInitialize()]
        public void SetUp()
        {
            UserDB.RegisterUser(_registeredPlayer1);
            UserDB.RegisterUser(_registeredPlayer2);
            DataBaseOperation.CreateGameHistoryPlayer(_registeredPlayer1.Id);
            DataBaseOperation.RegisterAchievements(_achievement1.Description, _achievement2.Description);
            DataBaseOperation.RegisterUserAchievement(_registeredPlayer1.Id, _achievement1.Id);
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
        public void SearchNoFriendPlayerSuccessfulTest()
        {
            Option<Player> result = UserDB.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(_registeredPlayer2, (Player)result.Case, "SearchNoFriendPlayerSuccessfulTest");
        }

        [TestMethod()]
        public void SearchNoFriendPlayerPendingRequestTest()
        {
            UserDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Option<Player> result = UserDB.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(_registeredPlayer2, (Player)result.Case, "SearchNoFriendPlayerPendingRequestTest");
        }

        [TestMethod()]
        public void SearchNoFriendPlayerBlockedTest()
        {
            UserDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);

            //TODO: hacer método para bloquear a un jugador

            Option<Player> result = UserDB.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(_registeredPlayer2, (Player)result.Case, "SearchNoFriendPlayerBlockedTest");
        }

        [TestMethod()]
        public void SearchNoFriendPlayerHimSelfTest()
        {
            Option<Player> result = UserDB.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer1.Username);
            Assert.IsTrue(result.IsNone, "SearchNoFriendPlayerHimSelfTest");
        }

        [TestMethod()]
        public void SearchNoFriendPlayerAlreadyFriendsTest()
        {
            UserDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);

            Assert.Fail("Incomplet test");
            //TODO Add the method to accept the friend request

            Option<Player> result = UserDB.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.IsTrue(result.IsNone, "SearchNo FriendPlayerAlreadyFriendsTest");
        }

        [TestMethod()]
        public void SearchNoFriendPlayerNonExistentTest()
        {
            Option<Player> result = UserDB.SearchNoFriendPlayer(_registeredPlayer1.Username, "Pale");
            Assert.IsTrue(result.IsNone, "SearchNoFriendPlayerNonExistentTest");
        }

        [TestMethod()]
        public void SendFriendRequestSuccessfulTest()
        {
            int expected = 0;
            int result = UserDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestSuccessfulTest");
        }

        [TestMethod()]
        public void SendFriendRequestSenderRequestedBefore()
        {
            int expected = -1;
            UserDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            int result = UserDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestSenderRequestedBefore");
        }

        [TestMethod()]
        public void SendFriendRequestReceiverRequestedBefore()
        {
            int expected = -2;
            UserDB.SendFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username);
            int result = UserDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestReceiverRequestedBefore");
        }

        [TestMethod()]
        public void SendFriendRequestAlreadyFriendsTest()
        {
            int expected = -3;
            UserDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);

            Assert.Fail("Incomplete test");
            //TODO Add the method to accept the friend request

            int result = UserDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestAlreadyFriendsTest");
        }

        [TestMethod()]
        public void SendFriendRequestBlockedRelationTest()
        {
            int expected = -4;

            Assert.Fail("Incomplete test");
            //TODO Add the method to block a player. No matter if they are friends or not

            int result = UserDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestBlockedRelationTest");
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
            _achievement1.IsAchieved = true;
            List<DomainClasses.Achievement> expected = new List<DomainClasses.Achievement>()
            {
                _achievement1,
                _achievement2
            };
            List<DomainClasses.Achievement> result = UserDB.GetPlayerAchievements(_registeredPlayer1.Username);

            Assert.IsTrue(expected.SequenceEqual(result), "GetPlayerAchievementsSuccessfulTest");
        }

        [TestMethod()]
        public void GetPlayerAchievementsWithoutAnyAchieved()
        {
            _achievement1.IsAchieved = false;
            List<DomainClasses.Achievement> expected = new List<DomainClasses.Achievement>()
            {
                _achievement1,
                _achievement2
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
        public void UpdateOriginalGameHistoryWongGameTest()
        {
            int expected = 1;
            int result = UserDB.UpdateOriginalGameHistory(_registeredPlayer1.Username, true);
            Assert.AreEqual(expected, result, "UpdateOriginalGameHistorySuccessfulTest");
        }

        [TestMethod()]
        public void UpdateOriginalGameHistoryLostGameTest()
        {
            int expected = 1;
            int result = UserDB.UpdateOriginalGameHistory(_registeredPlayer1.Username, false);
            Assert.AreEqual(expected, result, "UpdateOriginalGameHistoryNonExistentUserTest");
        }

        [TestMethod()]
        public void 
    }
}