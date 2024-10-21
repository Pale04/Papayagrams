using BussinessLogic;
using DomainClasses;
using System;
using System.Collections;
using System.ServiceModel;

namespace Contracts
{
    public partial class ServiceImplementation : IPregameService
    {
        public string CreateGame(string username)
        {
            GameRoom gameRoom = new GameRoom();
            gameRoom.state = GameRoomState.Waiting;
            gameRoom.Players.Add(username, OperationContext.Current.GetCallbackChannel<IPregameServiceCallback>());
            gameRoom.RoomCode = GameData.AddGameRoom(gameRoom);

            Console.WriteLine("sala de juego creada: " + gameRoom.RoomCode);

            return gameRoom.RoomCode;
        }

        public void InviteFriend(string username)
        {
            //TODO
            throw new NotImplementedException();
        }

        public int JoinGame(string username, string roomCode)
        {
            Player player = PlayerData.GetPlayerByUsername(username);
            GameRoom room = GameData.GetGameRoom(roomCode);
            room.Players.Add(username, OperationContext.Current.GetCallbackChannel<IPregameServiceCallback>());
            return 0;
        }

        public int LeaveLobby(string username, string code)
        {
            GameData.RemovePlayerFromGameRoom(username, code);
            Console.WriteLine(username + " leaved the game");
            return 0;
        }

        public void SendMessage(Message message)
        {
            Console.Write("sending: " + message.Content + " from: " + message.AuthorUsername);
            GameRoom room = GameData.GetGameRoom(message.GameRoomCode);

            foreach (DictionaryEntry player in room.Players)
            {
                Console.WriteLine("Sending message to " + player.Key);
                (player.Value as IPregameServiceCallback).ReceiveMessage(message);
            }
        }

        public void StartGame(string roomCode)
        {
            //TODO
            throw new NotImplementedException();
        }

        public int NotifyServer(PlayerDC player)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
