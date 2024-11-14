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
        public void DumpPiece(string piece)
        {
            throw new NotImplementedException();
        }

        public void LeaveGame(string gameRoomCode, string username)
        {
            CallbacksPool.RemoveGameCallbackChannel(username);
            GamesInProgressPool.ExitGame(gameRoomCode, username);
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

        public void ShoutPapaya(string username)
        {
            throw new NotImplementedException();
        }

        public void TakeSeed()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Send the initial pieces to every player and start the timer for start to playing
        /// </summary>
        /// <param name="gameRoomCode"></param>
        private static void PlayGame(string gameRoomCode)
        {
            Game game = GamesInProgressPool.GetGame(gameRoomCode);
            foreach (Player player in game.ConnectedPlayers)
            {
                var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(player.Username);
                channel.ReceiveStartingHand(game.GetInitialPieces(GameRoomsPool.GetGameRoom(gameRoomCode).GameConfiguration.InitialPieces));
            }
            GamesInProgressPool.StartGameTimer(gameRoomCode);
        }
    }
}
