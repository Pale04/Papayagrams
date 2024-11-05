using BussinessLogic;
using DomainClasses;
using System;
using System.ServiceModel;
using System.Collections.Generic;
using DataAccess;
using System.Data;

namespace Contracts
{
    public partial class ServiceImplementation : IPregameService, IGameCodeVerificationService
    {
        public (int, GameRoomDC) CreateGame(string username, GameConfigurationDC gameConfiguration)
        {
            int resultCode = 0;

            try
            {
                UserDB.UpdateUserStatus(username, PlayerStatus.in_game);
            }
            catch (EntityException error)
            {
                _logger.Error("Error while trying to update user status", error);
                return (102, null);
            }

            GameRoom gameRoom = new GameRoom
            {
                State = GameRoomState.Waiting
                //TODO: set the configuration
            };
            gameRoom.Players.Add(PlayersPool.GetPlayer(username));
            GameRoomsPool.AddGameRoom(gameRoom);
            CallbacksPool.PlayerArrivesToPregame(username, OperationContext.Current.GetCallbackChannel<IPregameServiceCallback>());
            Console.WriteLine($"sala de juego creada: {gameRoom.RoomCode}");

            return (resultCode, GameRoomDC.ConvertToGameRoomDC(gameRoom));
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
                try
                {
                    UserDB.UpdateUserStatus(username, PlayerStatus.in_game);
                }
                catch (EntityException error)
                {
                    _logger.Error("Error while trying to update user status", error);
                    return (102, null);
                }

                CallbacksPool.PlayerArrivesToPregame(username, OperationContext.Current.GetCallbackChannel<IPregameServiceCallback>());
                BroadcastRefreshLobby(roomCode);
                room.Players.Add(PlayersPool.GetPlayer(username));
                serializedGameRoom = GameRoomDC.ConvertToGameRoomDC(room);
            }
            else
            {
                resultCode = 401;
            }

            return (resultCode, serializedGameRoom);
        }

        public void LeaveLobby(string username, string code)
        {
            GameRoomsPool.RemovePlayerFromGameRoom(username, code);
            CallbacksPool.RemovePregameCallbackChannel(username);

            try
            {
                UserDB.UpdateUserStatus(username, PlayerStatus.online);
            }
            catch (EntityException error)
            {
                _logger.Error("Error while trying to update user status", error);
                return 102;
            }

            return 0;
        }

        public void SendMessage(Message message)
        {
            GameRoom room = GameRoomsPool.GetGameRoom(message.GameRoomCode);
            List<Player> players = room.Players;

            foreach (Player p in players)
            {
                var callbackChannel = (IPregameServiceCallback)CallbacksPool.GetPregameCallbackChannel(p.Username);
                if (callbackChannel != null)
                {
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
                    callbackChannel?.RefreshLobby(GameRoomDC.ConvertToGameRoomDC(room));
                }
            }
        }

        public void ReturnToLobby(string username)
        {
            CallbacksPool.PlayerArrivesToPregame(username, OperationContext.Current.GetCallbackChannel<IPregameServiceCallback>());
            CallbacksPool.RemoveMainMenuCallbackChannel(username);
        }

        public bool VerifyGameRoom(string gameCode)
        {
            GameRoomDC serializedGameRoom = null;
            GameRoom room = GameRoomsPool.GetGameRoom(gameCode);
            
            if (room != null)
            {
                //todo
                return true;
            }

            return false;
        }
    }
}
