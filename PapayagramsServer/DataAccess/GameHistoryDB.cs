using System;
using System.Linq;
using System.Data.Entity.Core;
using System.Data.Entity;
using DomainClasses;
using LanguageExt;
using System.Collections.Generic;

namespace DataAccess
{
    public static class GameHistoryDB
    {
        /// <summary>
        /// Update the game history specified of the player and add the victory or defeat
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <param name="gameWon">If the player won the game or not</param>
        /// <returns>1 if the update was successful, 0 otherwise</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>

        public static int UpdateGameHistory(string username, bool gameWon, GameMode gameMode)
        {
            switch (gameMode) {
                case GameMode.Original:
                    return UpdateOriginalGameHistory(username, gameWon);
                default:
                    return 0;
            }
        }
        

        private static int UpdateOriginalGameHistory(string username, bool gameWon)
        {
            int result = 0;
            using (var contex = new papayagramsEntities())
            {
                var player = contex.User.Where(p => p.username == username).Include(p => p.OriginalGameHistory);

                if (player.Any())
                {
                    var gameHistory = player.First().OriginalGameHistory.First();
                    if (gameWon)
                    {
                        gameHistory.wonGames++;
                    }
                    else
                    {
                        gameHistory.lostGames++;
                    }
                    result = contex.SaveChanges();
                }
            }
            return result;
        }

        /// <summary>
        /// Retrieves the statistics of a player like the amount of won and played games and amount of friends
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>An Option object with the statistics if the operation was succesful, an empty Option object if the user does not exist</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static Option<PlayerStats> GetPlayerStats(string username)
        {
            Option<PlayerStats> playerStatsResult;
            using (var context = new papayagramsEntities())
            {
                var result = context.User.Where(p => p.username == username)
                    .Include(p => p.OriginalGameHistory)
                    .Include(p => p.SuddenDeathHistory)
                    .Include(p => p.TimeAtackHistory)
                    .Include(p => p.UserRelationship);

                if (result.Any())
                {
                    PlayerStats playerStats = new PlayerStats
                    {
                        OriginalGamesPlayed = (int)result.First().OriginalGameHistory.First().wonGames + (int)result.First().OriginalGameHistory.First().lostGames,
                        TimeAttackGamesPlayed = (int)result.First().TimeAtackHistory.First().wonGames + (int)result.First().TimeAtackHistory.First().lostGames,
                        SuddenDeathGamesPlayed = (int)result.First().SuddenDeathHistory.First().wonGames + (int)result.First().SuddenDeathHistory.First().lostGames,
                        OriginalGamesWon = (int)result.First().OriginalGameHistory.First().wonGames,
                        TimeAttackGamesWon = (int)result.First().TimeAtackHistory.First().wonGames,
                        SuddenDeathGamesWon = (int)result.First().SuddenDeathHistory.First().wonGames,
                        FriendsAmount = result.First().UserRelationship.Count(relation => relation.relationState.Equals("friend"))
                    };
                    playerStatsResult = Option<PlayerStats>.Some(playerStats);
                }
                else
                {
                    playerStatsResult = Option<PlayerStats>.None;
                }
            }
            return playerStatsResult;
        }

        /// <summary>
        /// Obtains the statistics of all players in papayagrams
        /// </summary>
        /// <returns>A list with every player and his statistics</returns>
        public static List<LeaderboardStats> GetGlobalLeaderboard()
        {
            List<LeaderboardStats> leaderboardStats = new List<LeaderboardStats>();
            using (var context = new papayagramsEntities())
            {
                var allPlayers = context.User;
                foreach (var player in allPlayers)
                {
                    Option<PlayerStats> playerStats = GetPlayerStats(player.username);
                    leaderboardStats.Add(new LeaderboardStats(player.username, (PlayerStats)playerStats.Case));
                }
            }
            return leaderboardStats;
        }
    }
}
