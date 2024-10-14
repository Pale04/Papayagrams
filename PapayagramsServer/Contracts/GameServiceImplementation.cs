using BussinessLogic;
using DomainClasses;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Contracts
{
    public class GameServiceImplementation : IGameService
    {
        public void CreateGame()
        {
            GameRoom gameRoom = new GameRoom();
            gameRoom.state = GameRoomState.Waiting;
            gameRoom.Players = new List<Player>
            {
                ServerData.GetPlayerByContext(OperationContext.Current)
            };
            gameRoom.RoomCode = ServerData.AddGameRoom(gameRoom);

            OperationContext.Current.GetCallbackChannel<IGameServiceCallback>().JoinGame(gameRoom.RoomCode);
        }

        public void JoinGame(string roomCode)
        {
            throw new NotImplementedException();
        }
    }
}
