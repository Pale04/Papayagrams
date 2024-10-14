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
                PlayerData.GetPlayerByContext(OperationContext.Current)
            };
            gameRoom.RoomCode = GameData.AddGameRoom(gameRoom);

            OperationContext.Current.GetCallbackChannel<IGameServiceCallback>().JoinGame(gameRoom.RoomCode);
        }

        public void JoinGame(string roomCode)
        {
            throw new NotImplementedException();
        }

        public int LeaveGame(string code)
        {
            GameData.RemovePlayerFromGameRoom(OperationContext.Current, code);
            return 0;
        }
    }
}
