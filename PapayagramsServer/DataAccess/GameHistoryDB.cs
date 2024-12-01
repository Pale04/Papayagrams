using System;
using System.Linq;
using System.Data.Entity.Core;
using System.Data.Entity;
using DomainClasses;

namespace DataAccess
{
    public class GameHistoryDB
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
                case GameMode.SuddenDeath:
                    return UpdateSuddenDeathHistory(username, gameWon);
                case GameMode.TimeAttack:
                    return UpdateTimeAtackHistory(username, gameWon);
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

        private static int UpdateSuddenDeathHistory(string username, bool gameWon)
        {
            //todo
            throw new NotImplementedException();
        }

        private static int UpdateTimeAtackHistory(string username, bool gameWon)
        {
            //todo
            throw new NotImplementedException();
        }
    }
}
