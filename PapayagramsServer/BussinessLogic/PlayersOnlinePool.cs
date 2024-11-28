using DomainClasses;
using System.Collections.Generic;

namespace BussinessLogic
{
    public class PlayersOnlinePool
    {
        private static Dictionary<string, Player> _players = new Dictionary<string, Player>();
        private static Dictionary<string, Player> _guests = new Dictionary<string, Player>();

        public static void AddPlayer(Player player)
        {
            _players.Remove(player.Username);
            _players.Add(player.Username, player);
        }

        public static Player GetPlayer(string username)
        {
            return _players[username];
        }

        public static void RemovePlayer(string username) {
            _players.Remove(username);
        }

        /// <summary>
        /// Create a guest profile and add it to the guests pool.
        /// </summary>
        /// <returns>Player with the username and profile icon</returns>
        public static Player CreateGuestProfile()
        {
            string username;

            do
            {
                username = $"Guest_{CodeGenerator.GenerateCode()}";
            } while (_guests.ContainsKey(username));

            Player guest = new Player()
            {
                Username = username,
                ProfileIcon = 1
            };

            _guests.Add(username, guest);
            return guest;
        }

        public static bool IsGuest(string username)
        {
            return _guests.ContainsKey(username);
        }
    }
}
