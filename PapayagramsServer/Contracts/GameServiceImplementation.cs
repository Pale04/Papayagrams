using BussinessLogic;
using DataAccess;
using DomainClasses;
using System.Data;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Contracts
{
    public partial class ServiceImplementation : IGameService
    {
        public async void ReachServer(string username, string gameRoomCode)
        {
            CallbacksPool.PlayerArrivesToGame(username, OperationContext.Current.GetCallbackChannel<IGameServiceCallback>());
            CallbacksPool.RemovePregameCallbackChannel(username);
            GamesInProgressPool.ConnectToGame(gameRoomCode, username);

            // Permite que solamente el primero que llegó al servidor sea el que comience el juego
            if (GamesInProgressPool.GetGame(gameRoomCode).ConnectedPlayers[0].Username.Equals(username))
            {
                await Task.Delay(10000);
                PlayGame(gameRoomCode);
            }
        }

        /// <summary>
        /// Send the initial pieces to every player and start the timer to start to playing
        /// </summary>
        /// <param name="gameRoomCode">Code of the game room</param>
        private static void PlayGame(string gameRoomCode)
        {
            GameRoom gameRoom = GameRoomsPool.GetGameRoom(gameRoomCode);
            Game game = GamesInProgressPool.GetGame(gameRoomCode);

            foreach (Player player in game.ConnectedPlayers)
            {
                var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(player.Username);
                channel.AddSeedsToHand(game.GetInitialPieces(gameRoom.GameConfiguration.InitialPieces));
            }

            foreach (Player player in game.ConnectedPlayers)
            {
                var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(player.Username);
                channel.RefreshGameRoom(game.PiecesPile.Count, game.ConnectedPlayers.ConvertAll(PlayerDC.ConvertToPlayerDC));
            }

            int timeLimitMinutes = gameRoom.GameConfiguration.TimeLimitMinutes;
            if (timeLimitMinutes > 0)
            {
                System.Threading.Thread timerThread = new System.Threading.Thread(() => {
                    System.Threading.Thread.Sleep(timeLimitMinutes * 60000);
                    BroadcastEndGameNotification(gameRoomCode);
                });
                timerThread.Start();
            }
        }

        public void DumpPiece(string gameRoomCode, string username, char piece)
        {
            Game game = GamesInProgressPool.GetGame(gameRoomCode);
            var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(username);
            channel.AddSeedsToHand(game.PutInDump(piece));

            foreach (Player player in game.ConnectedPlayers)
            {
                var playerChannel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(player.Username);
                playerChannel.RefreshGameRoom(game.PiecesPile.Count, game.ConnectedPlayers.ConvertAll(PlayerDC.ConvertToPlayerDC));
            }
        }

        public void TakeSeed(string gameRoomCode)
        {
            Game game = GamesInProgressPool.GetGame(gameRoomCode);

            if (!game.ThereAreLessPiecesThanPlayers())
            {
                foreach (Player player in game.ConnectedPlayers)
                {
                    var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(player.Username);
                    channel.AddSeedsToHand(game.TakeSeed());
                }
            }

            foreach (Player player in game.ConnectedPlayers)
            {
                var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(player.Username);
                channel.RefreshGameRoom(game.PiecesPile.Count, game.ConnectedPlayers.ConvertAll(PlayerDC.ConvertToPlayerDC));
            }
        }

        public void ShoutPapaya(string gameRoomCode)
        {
            BroadcastEndGameNotification(gameRoomCode);
        }

        public async void CalculateWinner(string gameRoomCode, string username, int score)
        {
            Game game = GamesInProgressPool.GetGame(gameRoomCode);
            game.PlayersScores.Add(username, score);

            while (game.ConnectedPlayers.Count != game.PlayersScores.Count)
            {
                await Task.Delay(500);
            }
            
            string winnerUsername = game.GetWinner();

            try
            {
                GameHistoryDB.UpdateGameHistory(username, username.Equals(winnerUsername), GameRoomsPool.GetGameRoom(gameRoomCode).GameConfiguration.GameMode);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                _logger.WarnFormat("Game history not updated in data base (username: {0}, winner: {1})",username, username.Equals(winnerUsername));
            }

            var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(username);
            channel.EndGame(winnerUsername, game.GetScore(winnerUsername));
        }

        public void LeaveGame(string gameRoomCode, string username, bool gameEnded)
        {
            if (string.IsNullOrEmpty(gameRoomCode) || string.IsNullOrEmpty(username))
            {
                _logger.WarnFormat("LeaveGame method called with null or empty parameters (gameRoomCode: {0}, username: {1})", gameRoomCode, username);
            }

            CallbacksPool.RemoveGameCallbackChannel(username);
            GamesInProgressPool.ExitGame(gameRoomCode, username);
            GameRoomsPool.RemovePlayerFromGameRoom(username, gameRoomCode);

            if (!gameEnded)
            {
                if (GamesInProgressPool.GameExists(gameRoomCode))
                {
                    Game game = GamesInProgressPool.GetGame(gameRoomCode);
                    foreach (Player player in game.ConnectedPlayers)
                    {
                        var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(player.Username);
                        channel.RefreshGameRoom(game.PiecesPile.Count, game.ConnectedPlayers.ConvertAll(PlayerDC.ConvertToPlayerDC));
                    }
                }
            }
            
            if (!PlayersOnlinePool.IsGuest(username))
            {
                try
                {
                    UserDB.UpdateUserStatus(username, PlayerStatus.online);
                }
                catch (EntityException error)
                {
                    _logger.Fatal("Database connection failed", error);
                    _logger.WarnFormat("User status not updated in data base (username: {0}, to status: {1})", username, PlayerStatus.online);
                }
            }
        }

        private static void BroadcastEndGameNotification(string gameRoomCode)
        {
            foreach (Player player in GamesInProgressPool.GetGame(gameRoomCode).ConnectedPlayers)
            {
                var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(player.Username);
                channel.NotifyEndOfGame();
            }
        }
    }
}
