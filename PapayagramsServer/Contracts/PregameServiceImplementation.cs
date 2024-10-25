using BussinessLogic;
using DataAccess;
using DomainClasses;
using LanguageExt;
using System;
using System.ServiceModel;
using System.Data.Entity.Core;

namespace Contracts
{
    public partial class ServiceImplementation : IPregameService
    {
        public (int, string) CreateGame(string username)
        {
            int resultCode = 0;
            string roomCode = null;
            Option<Player> player = Option<Player>.None;

            try
            {
                player = UserDB.GetPlayerByUsername(username);
            }
            catch (EntityException error)
            {
                //TODO: handle
                resultCode = 102;
            }

            if (player.IsSome)
            {
                GameRoom gameRoom = new GameRoom();
                gameRoom.State = GameRoomState.Waiting;
                gameRoom.Players.Add((Player)player.Case);
                roomCode = GameRoomsPool.AddGameRoom(gameRoom);

                CallbacksPool.PlayerArrivedToPregame(username,OperationContext.Current.GetCallbackChannel<IPregameServiceCallback>());
            }

            Console.WriteLine("sala de juego creada: " + roomCode);
            return (resultCode, roomCode);
        }

        public void InviteFriend(string username)
        {
            //TODO
            throw new NotImplementedException();
        }

        public int JoinGame(string username, string roomCode)
        {
            int resultCode = 0;
            GameRoom room = GameRoomsPool.GetGameRoom(roomCode);

            if (room != null && room.State.Equals(GameRoomState.Waiting))
            {
                Option<Player> player = Option<Player>.None;
                try
                {
                    player = UserDB.GetPlayerByUsername(username);
                }
                catch (EntityException error)
                {
                    //TODO: handle
                    resultCode = 102;
                }

                if (player.IsSome)
                {
                    room.Players.Add((Player)player.Case);
                    var callbackChannel = (IPregameServiceCallback)CallbacksPool.GetPregameCallbackChannel(username);
                    callbackChannel.RefreshLobby(GameRoomDC.ConvertToGameRoomDC(room));
                }
            }
            else
            {
                resultCode = 401;
            }

            return resultCode;
        }

        public int LeaveLobby(string username, string code)
        {
            GameRoomsPool.RemovePlayerFromGameRoom(username, code);
            Console.WriteLine(username + " leaved the game");
            return 0;
        }

        public void SendMessage(Message message)
        {
            Console.Write("sending: " + message.Content + " from: " + message.AuthorUsername);
            var callbackChannel = (IPregameServiceCallback)CallbacksPool.GetPregameCallbackChannel(message.AuthorUsername);
            callbackChannel.ReceiveMessage(message);
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
