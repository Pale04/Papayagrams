using DomainClasses;
using System.Collections.Generic;

namespace BussinessLogic
{
    public class PlayersOnlinePool
    {
        private static Dictionary<string, Player> _players = new Dictionary<string, Player>();

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
    }
}
