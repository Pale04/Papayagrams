using BussinessLogic;
using DomainClasses;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Contracts
{
    public partial class ServiceImplementation : IGameService
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
            Console.WriteLine("sala de juego creada" + gameRoom.RoomCode);
            OperationContext.Current.GetCallbackChannel<IGameServiceCallback>().GameResponse(gameRoom.RoomCode);
        }

        public void JoinGame(string roomCode)
        {
            Console.WriteLine("hello");
        }

        public int LeaveGame(string code)
        {
            GameData.RemovePlayerFromGameRoom(OperationContext.Current, code);
            return 0;
        }
    }
}
