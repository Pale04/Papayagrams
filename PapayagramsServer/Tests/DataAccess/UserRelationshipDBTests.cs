using DomainClasses;
using LanguageExt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Tests;

namespace DataAccess.Tests
{
    [TestClass()]
    public class UserRelationshipDBTests
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
        private readonly Player _registeredPlayer3 = new Player()
        {
            Id = 3,
            Username = "Zaid04",
            Email = "zaid@gmail.com",
            Password = "040704",
            ProfileIcon = 1
        };

        [TestInitialize()]
        public void SetUp()
        {
            UserDB.RegisterUser(_registeredPlayer1);
            UserDB.RegisterUser(_registeredPlayer2);
            UserDB.RegisterUser(_registeredPlayer3);
        }

        [TestCleanup()]
        public void CleanUp()
        {
            DataBaseOperation.RebootDataBase();
        }

        [TestMethod()]
        public void SearchNoFriendPlayerSuccessfulTest()
        {
            Option<Player> result = UserRelationshipDB.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(_registeredPlayer2, (Player)result.Case, "SearchNoFriendPlayerSuccessfulTest");
        }

        [TestMethod()]
        public void SearchNoFriendPlayerPendingRequestTest()
        {
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Option<Player> result = UserRelationshipDB.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(_registeredPlayer2, (Player)result.Case, "SearchNoFriendPlayerPendingRequestTest");
        }

        [TestMethod()]
        public void SearchNoFriendPlayerBlockedTargetTest()
        {
            UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Option<Player> result = UserRelationshipDB.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.IsTrue(result.IsNone, "SearchNoFriendPlayerBlockedTest");
        }

        [TestMethod()]
        public void SearchNoFriendPlayerBlockedSearcherTest()
        {
            UserRelationshipDB.BlockPlayer(_registeredPlayer2.Username, _registeredPlayer1.Username);
            Option<Player> result = UserRelationshipDB.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.IsTrue(result.IsNone, "SearchNoFriendPlayerBlockedSearcherTest");
        }

        [TestMethod()]
        public void SearchNoFriendPlayerHimSelfTest()
        {
            Option<Player> result = UserRelationshipDB.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer1.Username);
            Assert.IsTrue(result.IsNone, "SearchNoFriendPlayerHimSelfTest");
        }

        [TestMethod()]
        public void SearchNoFriendPlayerAlreadyFriendsTest()
        {
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            UserRelationshipDB.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, true);
            Option<Player> result = UserRelationshipDB.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.IsTrue(result.IsNone, "SearchNo FriendPlayerAlreadyFriendsTest");
        }

        [TestMethod()]
        public void SearchNoFriendPlayerNonExistentTest()
        {
            Option<Player> result = UserRelationshipDB.SearchNoFriendPlayer(_registeredPlayer1.Username, "deivid");
            Assert.IsTrue(result.IsNone, "SearchNoFriendPlayerNonExistentTest");
        }

        [TestMethod()]
        public void SendFriendRequestSuccessfulTest()
        {
            int expected = 0;
            int result = UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestSuccessfulTest");
        }

        [TestMethod()]
        public void SendFriendRequestSenderRequestedBefore()
        {
            int expected = -1;
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            int result = UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestSenderRequestedBefore");
        }

        [TestMethod()]
        public void SendFriendRequestReceiverRequestedBefore()
        {
            int expected = -2;
            UserRelationshipDB.SendFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username);
            int result = UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestReceiverRequestedBefore");
        }

        [TestMethod()]
        public void SendFriendRequestAlreadyFriendsTest()
        {
            int expected = -3;
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            UserRelationshipDB.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, true);
            int result = UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestAlreadyFriendsTest");
        }

        [TestMethod()]
        public void SendFriendRequestBlockedRelationTest()
        {
            int expected = -4;
            UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            int result = UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestBlockedRelationTest");
        }

        [TestMethod()]
        public void RespondFriendRequestAcceptSuccessfulTest()
        {
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            int expected = 1;
            int result = UserRelationshipDB.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, true);
            Assert.AreEqual(expected, result, "RespondFriendRequestAcceptSuccessfulTest");
        }

        [TestMethod()]
        public void RespondFriendRequestRejectSuccessfulTest()
        {
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            int expected = 1;
            int result = UserRelationshipDB.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, false);
            Assert.AreEqual(expected, result, "RespondFriendRequestRejectSuccessfulTest");
        }

        [TestMethod()]
        public void RespondFriendRequestNonExistentRequestTest()
        {
            int expected = 0;
            int result = UserRelationshipDB.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, true);
            Assert.AreEqual(expected, result, "RespondFriendRequestNonExistentRelationTest");
        }

        [TestMethod()]
        public void RespondFriendRequestNonExistentEvaluatorTest()
        {
            int expected = -1;
            int result = UserRelationshipDB.RespondFriendRequest("deivid", _registeredPlayer1.Username, true);
            Assert.AreEqual(expected, result, "RespondFriendRequestNonExistentEvaluatorTest");
        }

        [TestMethod()]
        public void RespondFriendRequestNonExistentRequesterTest()
        {
            int expected = -2;
            int result = UserRelationshipDB.RespondFriendRequest(_registeredPlayer2.Username, "deivid", true);
            Assert.AreEqual(expected, result, "RespondFriendRequestNonExistentRequesterTest");
        }

        [TestMethod()]
        public void GetFriendsBothWaysSuccessfulTest()
        {
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            UserRelationshipDB.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, true);
            UserRelationshipDB.SendFriendRequest(_registeredPlayer3.Username, _registeredPlayer1.Username);
            UserRelationshipDB.RespondFriendRequest(_registeredPlayer1.Username, _registeredPlayer3.Username, true);

            List<Friend> expected = new List<Friend>
            {
                new Friend
                {
                    Id = _registeredPlayer2.Id,
                    Username = _registeredPlayer2.Username,
                    RelationState = RelationState.Friend,
                    ProfileIcon = 1
                },
                new Friend
                {
                    Id = _registeredPlayer3.Id,
                    Username = _registeredPlayer3.Username,
                    RelationState = RelationState.Friend,
                    ProfileIcon = 1
                }
            };
            List<Friend> result = UserRelationshipDB.GetFriends(_registeredPlayer1.Username);
            Assert.IsTrue(expected.SequenceEqual(result), "GetFriendsBothWaysSuccessfulTest");
        }

        [TestMethod()]
        public void GetFriendsWithPendingRequestTest()
        {
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            UserRelationshipDB.SendFriendRequest(_registeredPlayer3.Username, _registeredPlayer1.Username);

            List<Friend> result = UserRelationshipDB.GetFriends(_registeredPlayer1.Username);
            Assert.IsTrue(result.Count == 0, "GetFriendsWithPendingRequestTest");
        }

        [TestMethod()]
        public void GetFriendsWithBlockedPlayersTest()
        {
            UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, _registeredPlayer3.Username);

            List<Friend> result = UserRelationshipDB.GetFriends(_registeredPlayer1.Username);
            Assert.IsTrue(result.Count == 0, "GetFriendsWithBlockedPlayersTest");
        }

        [TestMethod()]
        public void GetFriendsNoFriendsTest()
        {
            List<Friend> result = UserRelationshipDB.GetFriends(_registeredPlayer1.Username);
            Assert.IsTrue(result.Count == 0, "GetFriendsNoFriendsTest");
        }

        [TestMethod()]
        public void GetFriendsNonExistentPlayerTest()
        {
            List<Friend> result = UserRelationshipDB.GetFriends("deivid");
            Assert.IsTrue(result.Count == 0, "GetFriendsNonExistentPlayerTest");
        }

        [TestMethod()]
        public void GetPendingRequestsBothWaysTest()
        {
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            UserRelationshipDB.SendFriendRequest(_registeredPlayer3.Username, _registeredPlayer1.Username);

            List<Friend> expected = new List<Friend>
            {
                new Friend
                {
                    Id = _registeredPlayer3.Id,
                    Username = _registeredPlayer3.Username,
                    RelationState = RelationState.Pending
                }
            };
            List<Friend> result = UserRelationshipDB.GetPendingFriendRequests(_registeredPlayer1.Username);
            Assert.IsTrue(expected.SequenceEqual(result), "GetFriendsBothWaysSuccessfulTest");
        }

        [TestMethod()]
        public void GetPendingRequestsWithFriendsTest()
        {
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            UserRelationshipDB.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, true);
            UserRelationshipDB.SendFriendRequest(_registeredPlayer3.Username, _registeredPlayer1.Username);

            List<Friend> expected = new List<Friend>
            {
                new Friend
                {
                    Id = _registeredPlayer3.Id,
                    Username = _registeredPlayer3.Username,
                    RelationState = RelationState.Pending
                }
            };

            List<Friend> result = UserRelationshipDB.GetPendingFriendRequests(_registeredPlayer1.Username);
            Assert.IsTrue(expected.SequenceEqual(result), "GetPendingRequestsWithFriendsTest");
        }

        [TestMethod()]
        public void GetPendingRequestsWithBlockedPlayersTest()
        {
            UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            UserRelationshipDB.SendFriendRequest(_registeredPlayer3.Username, _registeredPlayer1.Username);

            List<Friend> expected = new List<Friend>
            {
                new Friend
                {
                    Id = _registeredPlayer3.Id,
                    Username = _registeredPlayer3.Username,
                    RelationState = RelationState.Pending
                }
            };

            List<Friend> result = UserRelationshipDB.GetPendingFriendRequests(_registeredPlayer1.Username);
            Assert.IsTrue(expected.SequenceEqual(result), "GetPendingRequestsWithBlockedPlayersTest");
        }

        [TestMethod()]
        public void GetPendingRequestsNoPendingRequestsTest()
        {
            List<Friend> result = UserRelationshipDB.GetPendingFriendRequests(_registeredPlayer1.Username);
            Assert.IsTrue(result.Count == 0, "GetPendingRequestsNoPendingRequestsTest");
        }

        [TestMethod()]
        public void GetPendingRequestsNonExistentPlayerTest()
        {
            List<Friend> result = UserRelationshipDB.GetPendingFriendRequests("deivid");
            Assert.IsTrue(result.Count == 0, "GetPendingRequestsNonExistentPlayerTest");
        }

        [TestMethod()]
        public void GetBlockedPlayersBothWaysTest()
        {
            UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            UserRelationshipDB.BlockPlayer(_registeredPlayer3.Username, _registeredPlayer1.Username);

            List<Friend> expected = new List<Friend>()
            {
                new Friend
                {
                    Id = _registeredPlayer2.Id,
                    Username = _registeredPlayer2.Username,
                    RelationState = RelationState.Blocked
                }
            };

            List<Friend> result = UserRelationshipDB.GetBlockedPlayers(_registeredPlayer1.Username);
            Assert.IsTrue(expected.SequenceEqual(result), "GetBlockedPlayersBothWaysTest");
        }

        [TestMethod()]
        public void GetBlockedPlayersWithFriendsTest()
        {
            UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer3.Username);
            UserRelationshipDB.RespondFriendRequest(_registeredPlayer3.Username, _registeredPlayer1.Username, true);

            List<Friend> expected = new List<Friend>()
            {
                new Friend
                {
                    Id = _registeredPlayer2.Id,
                    Username = _registeredPlayer2.Username,
                    RelationState = RelationState.Blocked
                }
            };

            List<Friend> result = UserRelationshipDB.GetBlockedPlayers(_registeredPlayer1.Username);
            Assert.IsTrue(expected.SequenceEqual(result), "GetBlockedPlayersWithFriendsTest");
        }

        [TestMethod()]
        public void GetBlockedPlayersWithPendingRequestsTest()
        {
            UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer3.Username);

            List<Friend> expected = new List<Friend>()
            {
                new Friend
                {
                    Id = _registeredPlayer2.Id,
                    Username = _registeredPlayer2.Username,
                    RelationState = RelationState.Blocked
                }
            };

            List<Friend> result = UserRelationshipDB.GetBlockedPlayers(_registeredPlayer1.Username);
            Assert.IsTrue(expected.SequenceEqual(result), "GetBlockedPlayersWithPendingRequestsTest");
        }

        [TestMethod()]
        public void GetBlockedPlayersNoBlockedPlayersTest()
        {
            List<Friend> result = UserRelationshipDB.GetBlockedPlayers(_registeredPlayer1.Username);
            Assert.IsTrue(result.Count == 0, "GetBlockedPlayersNoBlockedPlayersTest");
        }

        [TestMethod()]
        public void GetBlockedPlayersNonExistentPlayerTest()
        {
            List<Friend> result = UserRelationshipDB.GetBlockedPlayers("deivid");
            Assert.IsTrue(result.Count == 0, "GetBlockedPlayersNonExistentPlayerTest");
        }

        [TestMethod()]
        public void BlockPlayerSuccessfulTest()
        {
            int expected = 1;
            int result = UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "BlockPlayerSuccessfulTest");
        }

        [TestMethod()]
        public void BlockPlayerFriendSenderTest()
        {
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            UserRelationshipDB.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, true);
            int expected = 2;
            int result = UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "BlockPlayerFriendReceiveTest");
        }

        [TestMethod()]
        public void BlockPlayerFriendReceiverTest()
        {
            UserRelationshipDB.SendFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username);
            UserRelationshipDB.RespondFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username, true);
            int expected = 2;
            int result = UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "BlockPlayerFriendSenderTest");
        }

        [TestMethod()]
        public void BlockPlayerPendingRequestSenderTest()
        {
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            int expected = 2;
            int result = UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "BlockPlayerPendingRequestSenderTest");
        }

        [TestMethod()]
        public void BlockPlayerPendingReceiverTest()
        {
            UserRelationshipDB.SendFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username);
            int expected = 2;
            int result = UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "BlockPlayerPendingReceiverTest");
        }

        [TestMethod()]
        public void BlockPlayerBlockedAgainTest()
        {
            UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            int expected = 2;
            int result = UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "BlockPlayerBlockedSenderTest");
        }

        [TestMethod()]
        public void BlockPlayerNonExistentBlockerTest()
        {
            int expected = 0;
            int result = UserRelationshipDB.BlockPlayer("deivid", _registeredPlayer1.Username);
            Assert.AreEqual(expected, result, "BlockPlayerNonExistentBlockerTest");
        }

        [TestMethod()]
        public void BlockPlayerNonExistentBlockedTest()
        {
            int expected = 0;
            int result = UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, "deivid");
            Assert.AreEqual(expected, result, "BlockPlayerNonExistentBlockedTest");
        }

        [TestMethod()]
        public void RemoveFriendReceiverSuccessfulTest()
        {
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            UserRelationshipDB.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, true);
            int expected = 1;
            int result = UserRelationshipDB.RemoveFriend(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "RemoveFriendSuccesfulTest");
        }

        [TestMethod()]
        public void RemoveFriendSenderSuccessfulTest()
        {
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            UserRelationshipDB.RespondFriendRequest(_registeredPlayer2.Username, _registeredPlayer1.Username, true);
            int expected = 1;
            int result = UserRelationshipDB.RemoveFriend(_registeredPlayer2.Username, _registeredPlayer1.Username);
            Assert.AreEqual(expected, result, "RemoveFriendSuccesfulTest");
        }

        [TestMethod()]
        public void RemoveFriendNonExistentRelationTest()
        {
            int expected = 0;
            int result = UserRelationshipDB.RemoveFriend(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "RemoveFriendNonExistentRelationTest");
        }

        [TestMethod()]
        public void RemoveFriendNonExistentFriendTest()
        {
            int expected = -2;
            int result = UserRelationshipDB.RemoveFriend(_registeredPlayer1.Username, "deivid");
            Assert.AreEqual(expected, result, "RemoveFriendNonExistentFriendTest");
        }

        [TestMethod()]
        public void RemoveFriendNonExistentPlayerTest()
        {
            int expected = -1;
            int result = UserRelationshipDB.RemoveFriend("deivid", _registeredPlayer1.Username);
            Assert.AreEqual(expected, result, "RemoveFriendNonExistentPlayerTest");
        }

        [TestMethod()]
        public void UblockPlayerSuccessfulTest()
        {
            UserRelationshipDB.BlockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            int expected = 1;
            int result = UserRelationshipDB.UnblockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "UnblockPlayerSuccessfulTest");
        }

        [TestMethod()]
        public void UnblockPlayerNonExistentRelationTest()
        {
            int expected = 0;
            int result = UserRelationshipDB.UnblockPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "UnblockPlayerNonExistentRelationTest");
        }

        [TestMethod()]
        public void UnblockPlayerNonExistentBlockerTest()
        {
            int expected = -1;
            int result = UserRelationshipDB.UnblockPlayer("deivid", _registeredPlayer1.Username);
            Assert.AreEqual(expected, result, "UnblockPlayerNonExistentBlockerTest");
        }

        [TestMethod()]
        public void UnblockPlayerNonExistentBlockedTest()
        {
            int expected = -2;
            int result = UserRelationshipDB.UnblockPlayer(_registeredPlayer1.Username, "deivid");
            Assert.AreEqual(expected, result, "UnblockPlayerNonExistentBlockedTest");
        }
    }
}
