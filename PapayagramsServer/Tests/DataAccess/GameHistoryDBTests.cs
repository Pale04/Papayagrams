using DomainClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Tests;

namespace DataAccess.Tests
{
    [TestClass()]
    public class GameHistoryDBTests
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
        }

        [TestCleanup()]
        public void CleanUp()
        {
            DataBaseOperation.RebootDataBase();
        }

        [TestMethod()]
        public void UpdateOriginalGameHistoryWonGameTest()
        {
            int expected = 1;
            int result = GameHistoryDB.UpdateGameHistory(_registeredPlayer1.Username, true, GameMode.Original);
            Assert.AreEqual(expected, result, "UpdateOriginalGameHistoryWonGameTest");
        }

        [TestMethod()]
        public void UpdateOriginalGameHistoryLostGameTest()
        {
            int expected = 1;
            int result = GameHistoryDB.UpdateGameHistory(_registeredPlayer1.Username, false, GameMode.Original);
            Assert.AreEqual(expected, result, "UpdateOriginalGameHistoryLostGameTest");
        }

        [TestMethod()]
        public void UpdateOriginalGameHistoryNonExistentUserTest()
        {
            int expected = 0;
            int result = GameHistoryDB.UpdateGameHistory("ABCD", true, GameMode.Original);
            Assert.AreEqual(expected, result, "UpdateOriginalGameHistoryNonExistentUserTest");
        }
    }
}
