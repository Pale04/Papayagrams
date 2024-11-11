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
                _logger.Fatal("Error while trying to update user status", error);
                return (102, null);
            }

            GameRoom gameRoom = new GameRoom
            {
                State = GameRoomState.Waiting,
                GameConfiguration = new GameConfiguration
                {
                    GameMode = (GameMode)Enum.Parse(typeof(GameMode), gameConfiguration.GameMode.ToString()),
                    InitialPieces = gameConfiguration.InitialPieces,
                    MaxPlayers = gameConfiguration.MaxPlayers,
                    WordsLanguage = (Language)Enum.Parse(typeof(Language), gameConfiguration.WordsLanguage.ToString()),
                    TimeLimitMinutes = gameConfiguration.TimeLimitMinutes
                }
            };
            gameRoom.Players.Add(PlayersOnlinePool.GetPlayer(username));
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

        public (int returnCode, GameRoomDC joinedGameRoom) JoinGame(string username, string roomCode)
        {
            int code = 0;
            GameRoomDC serializedGameRoom = null;
            GameRoom room = GameRoomsPool.GetGameRoom(roomCode);

            if (room != null && room.State.Equals(GameRoomState.Waiting) && room.Players.Count < room.GameConfiguration.MaxPlayers)
            {
                try
                {
                    UserDB.UpdateUserStatus(username, PlayerStatus.in_game);
                }
                catch (EntityException error)
                {
                    _logger.Fatal("Error while trying to update user status", error);
                    return (102, null);
                }

                CallbacksPool.PlayerArrivesToPregame(username, OperationContext.Current.GetCallbackChannel<IPregameServiceCallback>());
                serializedGameRoom = GameRoomDC.ConvertToGameRoomDC(room);
                BroadcastRefreshLobby(serializedGameRoom);
                room.Players.Add(PlayersOnlinePool.GetPlayer(username));
            }
            else
            {
                code = 401;
            }

            return (code, serializedGameRoom);
        }

        public int LeaveLobby(string username, string code)
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
            GameRoomsPool.GetGameRoom(roomCode).State = GameRoomState.InGame;
            GamesInProgressPool.PrepareGame(roomCode);
        }

        public void BroadcastRefreshLobby (GameRoomDC gameRoom)
        {
            if (gameRoom != null)
            {
                List<PlayerDC> players = gameRoom.Players;
                foreach (PlayerDC p in players)
                {
                    var callbackChannel = (IPregameServiceCallback)CallbacksPool.GetPregameCallbackChannel(p.Username);
                    callbackChannel?.RefreshLobby(gameRoom);
                }
            }
        }

        public void ReturnToLobby(string username)
        {
            CallbacksPool.PlayerArrivesToPregame(username, OperationContext.Current.GetCallbackChannel<IPregameServiceCallback>());
            CallbacksPool.RemoveMainMenuCallbackChannel(username);
        }

        /// <summary>
        /// Verify if the game room exists and if has available slots for players
        /// </summary>
        /// <param name="gameCode">Game code of the game room</param>
        /// <returns>true if it's available, false otherwise</returns>
        public bool VerifyGameRoom(string gameCode)
        {
            GameRoom room = GameRoomsPool.GetGameRoom(gameCode);
            return room != null && room.State.Equals(GameRoomState.Waiting) && room.Players.Count < room.GameConfiguration.MaxPlayers;
        }
    }
}
