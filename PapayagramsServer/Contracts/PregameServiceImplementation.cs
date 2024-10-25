using BussinessLogic;
using DataAccess;
using DomainClasses;
using LanguageExt;
using System;
using System.ServiceModel;
using System.Data.Entity.Core;
using System.Collections.Generic;

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
                Console.WriteLine($"The pregame callback of {username} is there");
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
                    CallbacksPool.PlayerArrivedToPregame(username, OperationContext.Current.GetCallbackChannel<IPregameServiceCallback>());
                    room.Players.Add((Player)player.Case);
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
            GameRoom room = GameRoomsPool.GetGameRoom(message.GameRoomCode);
            List<Player> players = room.Players;

            foreach (Player p in players)
            {
                var callbackChannel = (IPregameServiceCallback)CallbacksPool.GetPregameCallbackChannel(p.Username);
                if (callbackChannel != null)
                {
                    Console.WriteLine($"Turn from {p.Username}");
                    callbackChannel.ReceiveMessage(message);
                }    
            }
        }

        public void StartGame(string roomCode)
        {
            //TODO
            throw new NotImplementedException();
        }

        public int NotifyServer(PlayerDC player)
        {
            //CallbacksPool.PlayerArrivedToPregame(player.Username, OperationContext.Current.GetCallbackChannel<IPregameServiceCallback>());
            //BroadcastRefreshLobby(GameRoomDC.ConvertToGameRoomDC(room));
            return 0;
        }

        private static void BroadcastRefreshLobby(GameRoomDC room)
        {
            List<PlayerDC> players = room.Players;
            foreach (PlayerDC p in players)
            {
                var callbackChannel = (IPregameServiceCallback)CallbacksPool.GetPregameCallbackChannel(p.Username);
                if (callbackChannel != null)
                {
                    callbackChannel.RefreshLobby(room);
                    Console.WriteLine("Refresh lobby sent to: " + p.Username);
                }
            }
        }
    }
}
