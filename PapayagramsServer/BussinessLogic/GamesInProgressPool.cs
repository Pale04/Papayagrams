using DomainClasses;
using System.Collections.Generic;

namespace BussinessLogic
{
    public class GamesInProgressPool
    {
        private static Dictionary<string,Game> _gamesInProgress = new Dictionary<string,Game>();

        public static void PrepareGame (string gameRoomCode)
        {
            if (!_gamesInProgress.ContainsKey(gameRoomCode))
            {
                Game game = new Game();
                game.GeneratePiecesPile(144);
                _gamesInProgress.Add(gameRoomCode, game);
            }
        }

        public static void ConnectToGame (string gameRoomCode, string username)
        {
            _gamesInProgress[gameRoomCode].ConnectedPlayers.Add(PlayersOnlinePool.GetPlayer(username));
        }

        public static Game GetGame(string gameRoomCode)
        {
            return _gamesInProgress[gameRoomCode];
        }

        /// <summary>
        /// Remove a player from a game. 
        /// </summary>
        /// <param name="gameRoomCode">Code of the game room</param>
        /// <param name="username">Username of the player</param>
        public static void ExitGame(string gameRoomCode, string username)
        {
            Player player = PlayersOnlinePool.GetPlayer(username);
            Game game = _gamesInProgress[gameRoomCode];
            game.ConnectedPlayers.Remove(player);
        }

        public static void RemoveGame(string gameRoomCode)
        {
            _gamesInProgress.Remove(gameRoomCode);
        }
    }
}
