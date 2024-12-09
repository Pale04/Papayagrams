using BussinessLogic;
using DataAccess;
using DomainClasses;
using System.Collections.Generic;
using System.Data.Entity.Core;
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
                await Task.Delay(5000);
                CheckGameCallbackChannelState(gameRoomCode);
                if (GamesInProgressPool.GetGame(gameRoomCode) != null)
                {
                    PlayGame(gameRoomCode);
                }
            }
        }

        /// <summary>
        /// Send the initial pieces to every player and start the timer to start to playing
        /// </summary>
        /// <param name="gameRoomCode">Code of the game room</param>
        private void PlayGame(string gameRoomCode)
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
                StartGameTimer(gameRoomCode, timeLimitMinutes);
            }
        }

        private void StartGameTimer(string gameRoomCode, int timeLimitMinutes)
        {
            System.Threading.Thread timerThread = new System.Threading.Thread(() => {
                System.Threading.Thread.Sleep(timeLimitMinutes * 60000);
                BroadcastEndGameNotification(gameRoomCode);
            });
            timerThread.Start();
        }

        public void DumpPiece(string gameRoomCode, string username, char piece)
        {
            CheckGameCallbackChannelState(gameRoomCode);
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
            CheckGameCallbackChannelState(gameRoomCode);
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
            CheckGameCallbackChannelState(gameRoomCode);
            Game game = GamesInProgressPool.GetGame(gameRoomCode);
            game.PlayersScores.Add(username, score);

            while (game.ConnectedPlayers.Count != game.PlayersScores.Count)
            {
                await Task.Delay(500);
            }
            
            string winnerUsername = game.GetWinner();

            if (!PlayersOnlinePool.IsGuest(username))
            {
                try
                {
                    GameHistoryDB.UpdateGameHistory(username, username.Equals(winnerUsername), GameRoomsPool.GetGameRoom(gameRoomCode).GameConfiguration.GameMode);
                }
                catch (EntityException error)
                {
                    _logger.Fatal($"Database connection failed. Game history not updated in data base (username: {username}, winner: {username.Equals(winnerUsername)})", error);
                }
            }

            var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(username);
            channel.EndGame(winnerUsername, game.GetScore(winnerUsername));
        }

        public void LeaveGame(string gameRoomCode, string username, bool gameEnded)
        {
            if (string.IsNullOrEmpty(gameRoomCode) || string.IsNullOrEmpty(username))
            {
                _logger.WarnFormat("LeaveGame method called with null or empty parameters (gameRoomCode: {0}, username: {1})", gameRoomCode, username);
                return;
            }

            CheckGameCallbackChannelState(gameRoomCode);
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
                    _logger.Fatal($"Database connection failed. User status not updated in data base (username: {username}, to status: {PlayerStatus.online})", error);
                }
            }
        }

        private void BroadcastEndGameNotification(string gameRoomCode)
        {
            CheckGameCallbackChannelState(gameRoomCode);
            foreach (Player player in GamesInProgressPool.GetGame(gameRoomCode).ConnectedPlayers)
            {
                var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(player.Username);
                channel.NotifyEndOfGame();
            }
        }

        private void CheckGameCallbackChannelState(string gameRoomCode)
        {
            Game game = GamesInProgressPool.GetGame(gameRoomCode);
            if (game == null)
            {
                return;
            }

            List<Player> players = new List<Player>(game.ConnectedPlayers);
            foreach (Player player in players)
            {
                var callbackChannel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(player.Username);
                if (((ICommunicationObject)callbackChannel).State == CommunicationState.Closed)
                {
                    _logger.InfoFormat("GameCallback channel disposed (Game room: {0}, Username with callback disposed: {1})", gameRoomCode, player.Username);
                    GamesInProgressPool.ExitGame(gameRoomCode, player.Username);
                    GameRoomsPool.RemovePlayerFromGameRoom(player.Username, gameRoomCode);
                    ManageCallbackDispose(player.Username, gameRoomCode);
                }
            }
        }
    }
}
