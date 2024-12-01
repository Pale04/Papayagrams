using BussinessLogic;
using DataAccess;
using DomainClasses;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Contracts
{
    public partial class ServiceImplementation : IGameService
    {
        public void ReachServer(string username, string gameRoomCode)
        {
            CallbacksPool.PlayerArrivesToGame(username, OperationContext.Current.GetCallbackChannel<IGameServiceCallback>());
            CallbacksPool.RemovePregameCallbackChannel(username);
            GamesInProgressPool.ConnectToGame(gameRoomCode, username);

            // Permite que solamente el primero que llegó al servidor sea el que comience el juego
            if (GamesInProgressPool.GetGame(gameRoomCode).ConnectedPlayers.First().Username.Equals(username))
            {
                Task.Delay(10000);
                PlayGame(gameRoomCode);
            }
        }

        /// <summary>
        /// Send the initial pieces to every player and start the timer to start to playing
        /// </summary>
        /// <param name="gameRoomCode"></param>
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

        public void CalculateWinner(string gameRoomCode, string username, int score)
        {
            Game game = GamesInProgressPool.GetGame(gameRoomCode);

            while (game.ConnectedPlayers.Count != game.PlayersScores.Count)
            {
                Task.Delay(500);
            }
            
            string winnerUsername = game.GetWinner();

            foreach (Player player in game.ConnectedPlayers)
            {
                try
                {
                    GameHistoryDB.UpdateGameHistory(player.Username, player.Username.Equals(winnerUsername), GameRoomsPool.GetGameRoom(gameRoomCode).GameConfiguration.GameMode);
                }
                catch (EntityException error)
                {
                    _logger.Fatal("Database connection failed", error);
                }
            }
            
            var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(username);
            channel.EndGame(winnerUsername, game.GetScore(winnerUsername));
            GamesInProgressPool.ExitGame(gameRoomCode, username);
            CallbacksPool.RemoveGameCallbackChannel(username);
        }

        public void LeaveGame(string gameRoomCode, string username)
        {
            CallbacksPool.RemoveGameCallbackChannel(username);
            GamesInProgressPool.ExitGame(gameRoomCode, username);
            GameRoomsPool.RemovePlayerFromGameRoom(username, gameRoomCode);
            Game game = GamesInProgressPool.GetGame(gameRoomCode);

            if (game != null)
            {
                foreach (Player player in game.ConnectedPlayers)
                {
                    var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(player.Username);
                    channel.RefreshGameRoom(game.PiecesPile.Count, game.ConnectedPlayers.ConvertAll(PlayerDC.ConvertToPlayerDC));
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
