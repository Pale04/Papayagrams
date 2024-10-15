using DomainClasses;
using System;
using System.Collections;
using System.ServiceModel;

namespace BussinessLogic
{
    public class PlayerData
    {
        private static Hashtable Players = new Hashtable();

        /// <summary>
        /// Get a player instance based on its operation context
        /// </summary>
        /// <param name="context">The operation context of the user</param>
        /// <returns>The instance of the player with that operation context</returns>
        public static Player GetPlayerByContext(OperationContext context)
        {
            return Players[context] as Player;
        }

        /// <summary>
        /// Get the operation context of a player based on its instance
        /// </summary>
        /// <param name="player">The instance of the player</param>
        /// <returns>The operation context of the specified player</returns>
        public static OperationContext GetPlayerContext(Player player)
        {
            foreach (OperationContext context in Players.Keys)
            {
                if (Players[context] == player)
                {
                    return context;
                }
            }

            return null;
        }

        /// <summary>
        /// Add a player and its operation context to the list of connected players
        /// </summary>
        /// <param name="player">The instance of the player</param>
        /// <param name="playerContext">The context of the player to add</param>
        public static void AddPlayer(Player player, OperationContext playerContext)
        {
            Players.Add(playerContext, player);
        }

        public static void RemovePlayer(OperationContext playerContext)
        {
            Players.Remove(playerContext);
        }
    }
}
