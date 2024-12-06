using DomainClasses;
using LanguageExt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
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
            DataBaseOperation.CreateGameHistoryPlayer(_registeredPlayer1.Id);
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

            Option<PlayerStats> result = GameHistoryDB.GetPlayerStats(_registeredPlayer1.Username);
            Assert.AreEqual(expected, (PlayerStats)result.Case, "GetPlayerStatsSuccessfulTest");
        }

        [TestMethod()]
        public void GetPlayerStatsNonExistentUserTest()
        {
            Option<PlayerStats> result = GameHistoryDB.GetPlayerStats("Pale");
            Assert.IsTrue(result.IsNone, "GetPlayerStatsNonExistentUserTest");
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
            List<LeaderboardStats> result = GameHistoryDB.GetGlobalLeaderboard();
            Assert.IsTrue(expected.SequenceEqual(result), "GetGlobalLeaderboardSuccessfulTest");
        }
    }
}
