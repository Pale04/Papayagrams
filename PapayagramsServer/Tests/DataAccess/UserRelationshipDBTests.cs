using DomainClasses;
using LanguageExt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        [TestInitialize()]
        public void SetUp()
        {
            UserDB.RegisterUser(_registeredPlayer1);
            UserDB.RegisterUser(_registeredPlayer2);
            DataBaseOperation.CreateGameHistoryPlayer(_registeredPlayer1.Id);
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
        public void SearchNoFriendPlayerBlockedTest()
        {
            UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);

            //TODO: hacer método para bloquear a un jugador

            Option<Player> result = UserRelationshipDB.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(_registeredPlayer2, (Player)result.Case, "SearchNoFriendPlayerBlockedTest");
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

            Assert.Fail("Incomplet test");
            //TODO Add the method to accept the friend request

            Option<Player> result = UserRelationshipDB.SearchNoFriendPlayer(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.IsTrue(result.IsNone, "SearchNo FriendPlayerAlreadyFriendsTest");
        }

        [TestMethod()]
        public void SearchNoFriendPlayerNonExistentTest()
        {
            Option<Player> result = UserRelationshipDB.SearchNoFriendPlayer(_registeredPlayer1.Username, "Pale");
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

            Assert.Fail("Incomplete test");
            //TODO Add the method to accept the friend request

            int result = UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestAlreadyFriendsTest");
        }

        [TestMethod()]
        public void SendFriendRequestBlockedRelationTest()
        {
            int expected = -4;

            Assert.Fail("Incomplete test");
            //TODO Add the method to block a player. No matter if they are friends or not

            int result = UserRelationshipDB.SendFriendRequest(_registeredPlayer1.Username, _registeredPlayer2.Username);
            Assert.AreEqual(expected, result, "SendFriendRequestBlockedRelationTest");
        }
    }
}
