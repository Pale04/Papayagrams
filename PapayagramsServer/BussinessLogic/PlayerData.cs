using System;
using System.Collections;
using DomainClasses;

namespace BussinessLogic
{
    public class PlayerData
    {
        private static Hashtable _connectedPlayers = new Hashtable();

        /// <summary>
        /// Get a player instance based on its username
        /// </summary>
        /// <param name="username">The username of the player</param>
        /// <returns>The instance of the player with that username</returns>
        public static Player GetPlayerByUsername(string username)
        {
            return _connectedPlayers[username] as Player;
        }

        /// <summary>
        /// Add a player and its username to the list of connected players
        /// </summary>
        /// <param name="player">The instance of the player</param>
        /// <param name="username">The username of the player</param>
        public static void AddPlayer(Player player, string username)
        {
            _connectedPlayers.Add(username, player);
        }

        /// <summary>
        /// Remove a player from the list of connected players
        /// </summary>
        /// <param name="username"></param>
        public static void RemovePlayer(string username)
        {
            _connectedPlayers.Remove(username);
        }
    }
}
