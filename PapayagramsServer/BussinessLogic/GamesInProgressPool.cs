using DomainClasses;
using System.Collections.Generic;

namespace BussinessLogic
{
    public static class GamesInProgressPool
    {
        private static Dictionary<string,Game> _gamesInProgress = new Dictionary<string,Game>();

        public static void PrepareGame (string gameRoomCode)
        {
            if (!_gamesInProgress.ContainsKey(gameRoomCode))
            {
                Game game = new Game();
                game.GeneratePiecesPile(44);
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

        public static bool GameExists(string gameRoomCode)
        {
            return _gamesInProgress.ContainsKey(gameRoomCode);
        }

        /// <summary>
        /// Remove a player from a game, if no players are left in the game, remove the game in progress
        /// </summary>
        /// <param name="gameRoomCode">Code of the game room</param>
        /// <param name="username">Username of the player</param>
        public static void ExitGame(string gameRoomCode, string username)
        {
            Game game = _gamesInProgress[gameRoomCode];
            game.ConnectedPlayers.Remove(PlayersOnlinePool.GetPlayer(username));

            if (game.ConnectedPlayers.Count == 0)
            {
                _gamesInProgress.Remove(gameRoomCode);
                GameRoomsPool.GetGameRoom(gameRoomCode).State = GameRoomState.Waiting;
            }
        }
    }
}
