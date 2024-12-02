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
        private readonly PlayerDC _registeredPlayer3 = new PlayerDC()
        {
            Id = 3,
            Username = "Juan04",
            Email = "juan@gmail.com",
            Password = "040704"
        };
        private readonly PlayerDC _registeredPlayer4 = new PlayerDC()
        {
            Id = 4,
            Username = "Zaid04",
            Email = "zaid@gmail.com",
            Password = "040704"
        };

        private readonly List<DomainClasses.Achievement> _achievements = new List<DomainClasses.Achievement>()
        {
            new DomainClasses.Achievement() { Id = 11, Description = "1 game won in original game mode"},
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
            UserDB.RegisterUser(new Player
            {
                Username = _registeredPlayer3.Username,
                Email = _registeredPlayer3.Email,
                Password = _registeredPlayer3.Password
            });
            UserDB.RegisterUser(new Player
            {
                Username = _registeredPlayer4.Username,
                Email = _registeredPlayer4.Email,
                Password = _registeredPlayer4.Password
            });
            DataBaseOperation.CreateGameHistoryPlayer(_registeredPlayer1.Id);
            DataBaseOperation.RegisterUserAchievement(_registeredPlayer1.Id, _achievements[0].Id);
        }

        [TestCleanup()]
        public void CleanUp()
        {
            DataBaseOperation.RebootDataBase();
        }

        [TestMethod()]
        public void GetAllRelationshipsSuccessfulTest()
        {
            _serviceImplementation.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            _serviceImplementation.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, true);
            _serviceImplementation.SendFriendRequest(_registeredPlayer3.Username, _registeredPlayer1.Username);
            //TODO: bloquear al jugador 4

            Assert.Fail("Incomplete test");

            List<FriendDC> expected = new List<FriendDC>()
            {
                new FriendDC()
                {
                    Id = _registeredPlayer2.Id,
                    Username = _registeredPlayer2.Username,
                    RelationState = RelationStateDC.Friend
                },
                new FriendDC()
                {
                    Id = _registeredPlayer3.Id,
                    Username = _registeredPlayer3.Username,
                    RelationState = RelationStateDC.Pending
                },
                new FriendDC()
                {
                    Id = _registeredPlayer4.Id,
                    Username = _registeredPlayer4.Username,
                    RelationState = RelationStateDC.Blocked
                }   
            };
            (int _, List<FriendDC> result) = _serviceImplementation.GetAllRelationships(_registeredPlayer1.Username);
            Assert.IsTrue(expected.SequenceEqual(result), "GetAllRelationshipsSuccessfulTest");
        }

        [TestMethod()]
        public void GetAllRelationshipsFriendsBothWaysTest()
        {
            _serviceImplementation.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            _serviceImplementation.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, true);
            _serviceImplementation.SendFriendRequest(_registeredPlayer3.Username, _registeredPlayer1.Username);
            _serviceImplementation.RespondFriendRequest(_registeredPlayer1.Username, _registeredPlayer3.Username, true);

            List<FriendDC> expected = new List<FriendDC>()
            {
                new FriendDC()
                {
                    Id = _registeredPlayer2.Id,
                    Username = _registeredPlayer2.Username,
                    RelationState = RelationStateDC.Friend
                },
                new FriendDC()
                {
                    Id = _registeredPlayer3.Id,
                    Username = _registeredPlayer3.Username,
                    RelationState = RelationStateDC.Friend
                }
            };

            (_, List<FriendDC> result) = _serviceImplementation.GetAllRelationships(_registeredPlayer1.Username);
            Assert.IsTrue(expected.SequenceEqual(result), "GetAllRelationshipsFriendsBothWaysTest");
        }

        [TestMethod()]
        public void GetAllRelationshipsPendingBothWaysTest()
        {
            _serviceImplementation.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            _serviceImplementation.SendFriendRequest(_registeredPlayer3.Username, _registeredPlayer1.Username);

            List<FriendDC> expected = new List<FriendDC>()
            {
                new FriendDC()
                {
                    Id = _registeredPlayer3.Id,
                    Username = _registeredPlayer3.Username,
                    RelationState = RelationStateDC.Pending
                }
            };

            (_, List<FriendDC> result) = _serviceImplementation.GetAllRelationships(_registeredPlayer1.Username);
            Assert.IsTrue(expected.SequenceEqual(result), "GetAllRelationshipsPendingBothWaysTest");
        }

        [TestMethod()]
        public void GetAllRelationshiosBlockedBothWaysTest()
        {
            //TODO: bloquear dos relaciones en distintas direcciones
            Assert.Fail("Incomplete test");
        }

        [TestMethod()]
        public void GetAllRelationshipsEmptyTest()
        {
            (_, List<FriendDC> result) = _serviceImplementation.GetAllRelationships(_registeredPlayer1.Username);
            Assert.IsTrue(result.Count == 0, "GetAllRelationshipsEmptyTest");
        }

        [TestMethod()]
        public void GetAllRelationshipsNonExistentUserTest()
        {
            (_, List<FriendDC> result) = _serviceImplementation.GetAllRelationships("juan");
            Assert.IsTrue(result.Count == 0, "GetAllRelationshipsNonExistentUserTest");
        }

        [TestMethod()]
        public void SearchPlayerSuccessfulTest()
        {
            (_, PlayerDC result) = _serviceImplementation.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(_registeredPlayer2, result, "SearchPlayerSuccessfulTest");
        }

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
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            (_, PlayerDC result) = _serviceImplementation.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(_registeredPlayer2, result, "SearchPlayerRequestPendingTest");
        }

        [TestMethod()]
        public void SearchPlayerFriendTest()
        {
            _serviceImplementation.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            _serviceImplementation.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, true);
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
            Assert.Fail("Incomplete test");
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
        public void SendFriendRequestInvalidParameterTest()
        {
            int expected = 101;
            int result = _serviceImplementation.SendFriendRequest(null, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestInvalidParameterTest");
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
            _serviceImplementation.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            _serviceImplementation.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, true);
            int expected = 303;
            int result = _serviceImplementation.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestAlreadyFriendsTest");
        }

        [TestMethod()]
        public void SendFriendRequestBlockedRelationTest()
        {
            Assert.Fail("Incomplet test");
            //TODO Add the method to block a player. No matter if they are friends or not

            int expected = 304;
            int result = _serviceImplementation.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestBlockedRelationTest");
        }

        [TestMethod()]
        public void RespondFriendRequestAcceptSuccessfulTest()
        {
            _serviceImplementation.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            int expected = 0;
            int result = _serviceImplementation.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, true);
            Assert.AreEqual(expected, result, "RespondFriendRequestAcceptSuccessfulTest");
        }

        [TestMethod()]
        public void RespondFriendRequestRejectSuccessfulTest()
        {
            _serviceImplementation.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            int expected = 0;
            int result = _serviceImplementation.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, false);
            Assert.AreEqual(expected, result, "RespondFriendRequestRejectSuccessfulTest");
        }

        [TestMethod()]
        public void RespondFriendRequestInvalidParametersTest()
        {
            int expected = 101;
            int result = _serviceImplementation.RespondFriendRequest(null, "  ", true);
            Assert.AreEqual(expected, result, "RespondFriendRequestNonExistentRequestTest");
        }

        [TestMethod()]
        public void RespondFriendRequestNonExistentRequestTest()
        {
            int expected = 305;
            int result = _serviceImplementation.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, true);
            Assert.AreEqual(expected, result, "RespondFriendRequestNonExistentRequestTest");
        }

        [TestMethod()]
        public void RespondFriendRequestNonExistentRespondentTest()
        {
            int expected = 306;
            int result = _serviceImplementation.RespondFriendRequest("Deivid", _registeredPlayer2.Username, true);
            Assert.AreEqual(expected, result, "RespondFriendRequestNonExistentRequesterTest");
        }

        [TestMethod()]
        public void RespondFriendRequestNonExistentRequesterTest()
        {
            int expected = 307;
            int result = _serviceImplementation.RespondFriendRequest(_registeredPlayer2.Username, "Deivid", true);
            Assert.AreEqual(expected, result, "RespondFriendRequestNonExistentRequesterTest");
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
            _achievements[0].IsAchieved = true;
            List<AchievementDC> expected = new List<AchievementDC>()
            {
                AchievementDC.ConvertToAchievementDC(_achievements[0]),
            };
            (int _, List<AchievementDC> result) = _serviceImplementation.GetAchievements(_registeredPlayer1.Username);
            Assert.IsTrue(expected.SequenceEqual(result), "GetAchievementsSuccessfulTest");
        }

        [TestMethod()]
        public void GetAchievementsNonExistentUserTest()
        {
            _achievements[0].IsAchieved = false;
            List<AchievementDC> expected = new List<AchievementDC>()
            {
                AchievementDC.ConvertToAchievementDC(_achievements[0]),
            };
            (int _, List<AchievementDC> result) = _serviceImplementation.GetAchievements("juan");
            Assert.IsTrue(expected.SequenceEqual(result), "GetAchievementsNonExistentUserTest");
        }

        [TestMethod()]
        public void GetAchievementsNullUsernameTest()
        {
            _achievements[0].IsAchieved = false;
            List<AchievementDC> expected = new List<AchievementDC>()
            {
                AchievementDC.ConvertToAchievementDC(_achievements[0]),
            };
            (int _, List<AchievementDC> result) = _serviceImplementation.GetAchievements(null);
            Assert.IsTrue(expected.SequenceEqual(result), "GetAchievementsNullUsernameTest");
        }
    }
}