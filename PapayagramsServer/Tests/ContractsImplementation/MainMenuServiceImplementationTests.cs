using DataAccess;
using DomainClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Tests;

namespace Contracts.Tests
{
    [TestClass()]
    public class MainMenuServiceImplementationTests
    {
        private readonly ServiceImplementation _serviceImplementation = new ServiceImplementation();
        private readonly PlayerDC _registeredPlayer1 = new PlayerDC()
        {
            Id = 1,
            Username = "Pale04",
            Email = "epalemolina@hotmail.com",
            Password = "040704"
        };
        private readonly PlayerDC _registeredPlayer2 = new PlayerDC()
        {
            Id = 2,
            Username = "David04",
            Email = "david@gmail.com",
            Password = "040704"
        };

        private readonly AchievementDC _achievement1 = new AchievementDC()
        {
            Id = 1,
            Description = "Description1",
        };
        private readonly AchievementDC _achievement2 = new AchievementDC()
        {
            Id = 2,
            Description = "Description2"
        };

        [TestInitialize()]
        public void SetUp()
        {
            UserDB.RegisterUser(new Player
            {
                Username = _registeredPlayer1.Username,
                Email = _registeredPlayer1.Email,
                Password = _registeredPlayer1.Password
            });
            UserDB.RegisterUser(new Player
            {
                Username = _registeredPlayer2.Username,
                Email = _registeredPlayer2.Email,
                Password = _registeredPlayer2.Password
            });
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
        public void SearchPlayerSuccessfulTest()
        {
            (_, PlayerDC result) = _serviceImplementation.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(_registeredPlayer2, result, "SearchPlayerSuccessfulTest");
        }

        //It is the same case when the username is empty
        [TestMethod()]
        public void SearchPlayerNonExistentTest()
        {
            (int code, _) = _serviceImplementation.SearchNoFriendPlayer(_registeredPlayer1.Username, "juan");
            Assert.AreEqual(103, code, "SearchPlayerNonExistentTest");
        }

        [TestMethod()]
        public void SearchPlayerNullUsernameTest()
        {
            (int code, _) = _serviceImplementation.SearchNoFriendPlayer(_registeredPlayer1.Username, null);
            Assert.AreEqual(103, code, "SearchPlayerNonExistentTest");
        }

        [TestMethod()]
        public void SearchPlayerRequestPendingTest()
        {
            UserDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            (_, PlayerDC result) = _serviceImplementation.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(_registeredPlayer2, result, "SearchPlayerRequestPendingTest");
        }

        [TestMethod()]
        public void SearchPlayerFriendTest()
        {
            UserDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);

            Assert.Fail("Incomplete test");
            //TODO: Implement the method to accept the friend request

            (int code, _) = _serviceImplementation.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(103, code, "SearchPlayerFriendTest");
        }

        [TestMethod()]
        public void SearchPlayerSameUserTest()
        {
            (int code, _) = _serviceImplementation.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer1.Username);
            Assert.AreEqual(103, code, "SearchPlayerSameUserTest");
        }

        [TestMethod()]
        public void SearchPlayerBlockedTest()
        {
            //TODO: Implement the method to block a player
            (_, PlayerDC result) = _serviceImplementation.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(_registeredPlayer2, result, "SearchPlayerBlockedTest");
        }

        [TestMethod()]
        public void SendFriendRequestSuccessfulTest()
        {
            int expected = 0;
            int result = _serviceImplementation.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestSuccessfulTest");
        }

        [TestMethod()]
        public void SendFriendRequestSenderRequestedBefore()
        {
            int expected = 301;
            _serviceImplementation.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            int result = _serviceImplementation.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestSenderRequestedBefore");
        }

        [TestMethod()]
        public void SendFriendRequestReceiverRequestedBefore()
        {
            int expected = 302;
            _serviceImplementation.SendFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username);
            int result = _serviceImplementation.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestReceiverRequestedBefore");
        }

        [TestMethod()]
        public void SendFriendRequestAlreadyFriendsTest()
        {
            int expected = -3;
            _serviceImplementation.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);

            Assert.Fail("Incomplet test");
            //TODO Add the method to accept the friend request

            int result = _serviceImplementation.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestAlreadyFriendsTest");
        }

        [TestMethod()]
        public void SendFriendRequestBlockedRelationTest()
        {
            int expected = 304;

            Assert.Fail("Incomplet test");
            //TODO Add the method to block a player. No matter if they are friends or not

            int result = _serviceImplementation.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestBlockedRelationTest");
        }

        [TestMethod()]
        public void GetPlayerProfileSuccessfulTest()
        {
            PlayerStatsDC expected = new PlayerStatsDC()
            {
                OriginalGamesPlayed = 60,
                TimeAttackGamesPlayed = 30,
                SuddenDeathGamesPlayed = 103,
                OriginalGamesWon = 10,
                TimeAttackGamesWon = 5,
                SuddenDeathGamesWon = 3,
                FriendsAmount = 0
            };

            (int _, PlayerStatsDC result) = _serviceImplementation.GetPlayerProfile(_registeredPlayer1.Username);
            Assert.AreEqual(expected, result, "GetPlayerProfileSuccessfulTest");
        }

        [TestMethod()]
        public void GetPlayerProfileNonExistentUserTest()
        {
            (int code, _) = _serviceImplementation.GetPlayerProfile("juan");
            Assert.AreEqual(205, code, "GetPlayerProfileNonExistentTest");
        }

        [TestMethod()]
        public void GetPlayerProfileNullUsernameTest()
        {
            (int code, _) = _serviceImplementation.GetPlayerProfile(null);
            Assert.AreEqual(205, code, "GetPlayerProfileNullUsernameTest");
        }

        [TestMethod()]
        public void ConvertGameConfigurationDC ()
        {
            GameConfigurationDC esperado = new GameConfigurationDC
            {
                GameMode = GameModeDC.Original,
                InitialPieces = 10,
                MaxPlayers = 4,
                WordsLanguage = LanguageDC.English,
                TimeLimitMinutes = 5
            };

            GameConfigurationDC resultado = GameConfigurationDC.ConvertToGameConfigurationDC(new GameConfiguration
            {
                GameMode = GameMode.Original,
                InitialPieces = 10,
                MaxPlayers = 4,
                WordsLanguage = Language.English,
                TimeLimitMinutes = 5
            });
            Assert.AreEqual(esperado, resultado, "ConvertGameConfigurationDC");
        }

        [TestMethod()]
        public void GetAchievementsSuccessfulTest()
        {
            _achievement1.IsAchieved = true;
            List<AchievementDC> expected = new List<AchievementDC>()
            {
                _achievement1,
                _achievement2
            };
            (int _, List<AchievementDC> result) = _serviceImplementation.GetAchievements(_registeredPlayer1.Username);
            Assert.IsTrue(expected.SequenceEqual(result), "GetAchievementsSuccessfulTest");
        }

        [TestMethod()]
        public void GetAchievementsNonExistentUserTest()
        {
            _achievement1.IsAchieved = false;
            List<AchievementDC> expected = new List<AchievementDC>()
            {
                _achievement1,
                _achievement2
            };
            (int _, List<AchievementDC> result) = _serviceImplementation.GetAchievements("juan");
            Assert.IsTrue(expected.SequenceEqual(result), "GetAchievementsNonExistentUserTest");
        }

        [TestMethod()]
        public void GetAchievementsNullUsernameTest()
        {
            _achievement1.IsAchieved = false;
            List<AchievementDC> expected = new List<AchievementDC>()
            {
                _achievement1,
                _achievement2
            };
            (int _, List<AchievementDC> result) = _serviceImplementation.GetAchievements(null);
            Assert.IsTrue(expected.SequenceEqual(result), "GetAchievementsNullUsernameTest");
        }
    }
}