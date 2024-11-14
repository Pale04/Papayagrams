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

        public static void StartGameTimer(string gameRoomCode)
        {
            int timeLimit = GameRoomsPool.GetGameRoom(gameRoomCode).GameConfiguration.TimeLimitMinutes;
            if (timeLimit != 0)
            {
                System.Timers.Timer gameTimer = new System.Timers.Timer(GameRoomsPool.GetGameRoom(gameRoomCode).GameConfiguration.TimeLimitMinutes * 1000);
                //TODO: iniciar cronometro en un hilo (creo que Timer ya crea un hilo: INVESTIGAR) que estará llamando cada cierto tiempo a RefreshTimer de cada jugador
            }            
        }

        /// <summary>
        /// Remove a player from a game. If no one are left, the game is removed and the game becomes in waiting state. 
        /// </summary>
        /// <param name="gameRoomCode">Code of the game room</param>
        /// <param name="username">Username of the player</param>
        public static void ExitGame(string gameRoomCode, string username)
        {
            Player player = PlayersOnlinePool.GetPlayer(username);
            Game game = _gamesInProgress[gameRoomCode];
            game.ConnectedPlayers.Remove(player);

            if (game.ConnectedPlayers.Count == 0)
            {
                _gamesInProgress.Remove(gameRoomCode);
                GameRoomsPool.GetGameRoom(gameRoomCode).State = GameRoomState.Waiting;
            }
        }

        private static void SendTimerUpdateToPlayers()
        {
            //TODO:llamar refreshtimer de cada jugador.
        }
    }
}
