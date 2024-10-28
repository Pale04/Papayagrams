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
        public (int, GameRoomDC) CreateGame(string username)
        {
            int resultCode = 0;
            GameRoomDC serializedGameRoom = null;
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
                GameRoom gameRoom = new GameRoom
                {
                    State = GameRoomState.Waiting
                };
                gameRoom.Players.Add((Player)player.Case);
                string roomCode = GameRoomsPool.AddGameRoom(gameRoom);

                serializedGameRoom = new GameRoomDC
                {
                    RoomCode = roomCode,
                    Players = gameRoom.Players.ConvertAll(PlayerDC.ConvertToPlayerDC)
                };

                CallbacksPool.PlayerArrivedToPregame(username,OperationContext.Current.GetCallbackChannel<IPregameServiceCallback>());
                Console.WriteLine("sala de juego creada: " + serializedGameRoom.RoomCode);
            }

            return (resultCode, serializedGameRoom);
        }

        public void InviteFriend(string username)
        {
            //TODO
            throw new NotImplementedException();
        }

        public (int, GameRoomDC) JoinGame(string username, string roomCode)
        {
            int resultCode = 0;
            GameRoomDC serializedGameRoom = null;
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
                    BroadcastRefreshLobby(roomCode);
                    serializedGameRoom = new GameRoomDC
                    {
                        RoomCode = roomCode,
                        Players = room.Players.ConvertAll(PlayerDC.ConvertToPlayerDC)
                    };
                }
            }
            else
            {
                resultCode = 401;
            }

            return (resultCode, serializedGameRoom);
        }

        public int LeaveLobby(string username, string code)
        {
            GameRoomsPool.RemovePlayerFromGameRoom(username, code);
            //TODO: remove player from callbacks pool
            Console.WriteLine($"{username} leaved the game room {code}");
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

        public void BroadcastRefreshLobby (string roomCode)
        {
            GameRoom room = GameRoomsPool.GetGameRoom(roomCode);

            if (room != null)
            {
                List<Player> players = room.Players;
                foreach (Player p in players)
                {
                    var callbackChannel = (IPregameServiceCallback)CallbacksPool.GetPregameCallbackChannel(p.Username);
                    if (callbackChannel != null)
                    {
                        callbackChannel.RefreshLobby(GameRoomDC.ConvertToGameRoomDC(room));
                    }
                }
            }
        }
    }
}
