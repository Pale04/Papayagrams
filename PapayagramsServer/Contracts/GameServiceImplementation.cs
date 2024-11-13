using BussinessLogic;
using DomainClasses;
using System;
using System.Linq;
using System.ServiceModel;

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

        public void ReachServer(string username, string gameRoomCode)
        {
            CallbacksPool.PlayerArrivesToGame(username, OperationContext.Current.GetCallbackChannel<IGameServiceCallback>());
            CallbacksPool.RemovePregameCallbackChannel(username);
            GamesInProgressPool.ConnectToGame(gameRoomCode, username);

            while (!GamesInProgressPool.IsEveryoneReady(gameRoomCode))
            {
            }

            //Permite que solamente el primero que llegó a la partida sea el que la comience
            if (GamesInProgressPool.GetGame(gameRoomCode).ConnectedPlayers.First().Username.Equals(username))
            {
                StartGamePlay(gameRoomCode);
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
        private void StartGamePlay(string gameRoomCode)
        {
            Game game = GamesInProgressPool.GetGame(gameRoomCode);
            foreach (Player player in game.ConnectedPlayers)
            {
                var channel = (IGameServiceCallback)CallbacksPool.GetGameCallbackChannel(player.Username);
                channel.ReceiveStartingHand(game.GetInitialPieces(GameRoomsPool.GetGameRoom(gameRoomCode).GameConfiguration.InitialPieces));
            }

            //TODO: iniciar cronometro en un hilo, entonces estará llamando cada cierto tiempo a RefreshTimer
        }
    }
}
