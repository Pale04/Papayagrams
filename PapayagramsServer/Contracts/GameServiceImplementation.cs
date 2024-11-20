using BussinessLogic;
using DomainClasses;
using System;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Contracts
{
    public partial class ServiceImplementation : IGameService
    {
        public void DumpPiece(string gameRoomCode, string username, char piece)
        {
            Game game = GamesInProgressPool.GetGame(gameRoomCode);
            var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(username);
            channel.AddSeedsToHand(game.PutInDump(piece));

            foreach (Player player in game.ConnectedPlayers)
            {
                var playerChannel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(player.Username);
                playerChannel.RefreshGameRoom(game.PiecesPile, game.ConnectedPlayers.ConvertAll(PlayerDC.ConvertToPlayerDC));
            }
        }

        public void LeaveGame(string gameRoomCode, string username)
        {
            CallbacksPool.RemoveGameCallbackChannel(username);
            GamesInProgressPool.ExitGame(gameRoomCode, username);
            Game game = GamesInProgressPool.GetGame(gameRoomCode);

            if (game.ConnectedPlayers.Count == 0)
            {
                GamesInProgressPool.RemoveGame(gameRoomCode);
                GameRoomsPool.GetGameRoom(gameRoomCode).State = GameRoomState.Waiting;
            }
            else
            {
                foreach (Player player in game.ConnectedPlayers)
                {
                    var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(player.Username);
                    channel.RefreshGameRoom(game.PiecesPile, game.ConnectedPlayers.ConvertAll(PlayerDC.ConvertToPlayerDC));
                }
            }
        }

        public async void ReachServer(string username, string gameRoomCode)
        {
            CallbacksPool.PlayerArrivesToGame(username, OperationContext.Current.GetCallbackChannel<IGameServiceCallback>());
            CallbacksPool.RemovePregameCallbackChannel(username);
            GamesInProgressPool.ConnectToGame(gameRoomCode, username);

            // Permite que solamente el primero que llegó al servidor sea el que la comience el juego
            if (GamesInProgressPool.GetGame(gameRoomCode).ConnectedPlayers.First().Username.Equals(username))
            {
                await Task.Delay(10000);
                PlayGame(gameRoomCode);
            }
        }

        public void ShoutPapaya(string gameRoomCode, string username)
        {
            throw new NotImplementedException();
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
                channel.RefreshGameRoom(game.PiecesPile, game.ConnectedPlayers.ConvertAll(PlayerDC.ConvertToPlayerDC));
            }
        }

        public void CalculateWinner()
        {
            throw new NotImplementedException();
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
                channel.RefreshGameRoom(game.PiecesPile, game.ConnectedPlayers.ConvertAll(PlayerDC.ConvertToPlayerDC));
            }

            int timeLimitMinutes = gameRoom.GameConfiguration.TimeLimitMinutes;
            if (timeLimitMinutes != 0)
            {
                System.Threading.Thread timerThread = new System.Threading.Thread(() => {
                    System.Threading.Thread.Sleep(timeLimitMinutes * 60000);
                    SendEndGameNotification(gameRoomCode);
                });
                timerThread.Start();
            }
        }

        private static void SendEndGameNotification(string gameRoomCode)
        {
            GameRoom gameRoom = GameRoomsPool.GetGameRoom(gameRoomCode);
            foreach (Player player in gameRoom.Players)
            {
                var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(player.Username);
                channel.EndGame();
            }
        }
    }
}
