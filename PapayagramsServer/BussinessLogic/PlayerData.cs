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
        /// Get a player instance based on its username
        /// </summary>
        /// <param name="username">The username of the player</param>
        /// <returns>The instance of the player with that username</returns>
        public static Player GetPlayerByUsername(string username)
        {
            return Players[username] as Player;
        }

        /// <summary>
        /// Add a player and its username to the list of connected players
        /// </summary>
        /// <param name="player">The instance of the player</param>
        /// <param name="username">The username of the player</param>
        public static void AddPlayer(Player player, string username)
        {
            Players.Add(username, player);
        }

        public static void RemovePlayer(string username)
        {
            Players.Remove(username);
        }
    }
}
