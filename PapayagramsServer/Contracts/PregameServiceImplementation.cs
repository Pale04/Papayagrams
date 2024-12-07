using BussinessLogic;
using DomainClasses;
using System;
using System.ServiceModel;
using System.Collections.Generic;
using DataAccess;
using System.Data.Entity.Core;

namespace Contracts
{
    public partial class ServiceImplementation : IPregameService, IGameCodeVerificationService
    {
        public (int, GameRoomDC) CreateGame(string username, GameConfigurationDC gameConfiguration)
        {
            try
            {
                UserDB.UpdateUserStatus(username, PlayerStatus.in_game);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                _logger.WarnFormat("User status not updated in data base (username: {0}, to status: {1})", username, PlayerStatus.in_game.ToString());
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

            return (0, GameRoomDC.ConvertToGameRoomDC(gameRoom));
        }

        public void InviteFriend(string username, string guestUsername, string gameRoomCode)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(guestUsername) || string.IsNullOrEmpty(gameRoomCode))
            {
                _logger.WarnFormat("Invalid parameters for InviteFriend method (username: {0}, guestUsername: {1}, gameRoomCode: {2})", username, guestUsername, gameRoomCode);
                return;
            }

            PlayerStatus guestStatus;
            try
            {
                guestStatus = UserDB.GetPlayerStatus(guestUsername);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                return;
            }

            if (guestStatus == PlayerStatus.online)
            {
                GameInvitationDC invitation = new GameInvitationDC
                {
                    GameRoomCode = gameRoomCode,
                    SenderUsername = username
                };

                var callbackChannel = (IMainMenuServiceCallback)CallbacksPool.GetMainMenuCallbackChannel(guestUsername);
                try
                {
                    callbackChannel.ReceiveGameInvitation(invitation);
                }
                catch (ObjectDisposedException error)
                {
                    _logger.Info($"Callback channel disposed (Game room: {gameRoomCode}, Username with callback disposed: {guestUsername})", error);
                    ManageCallbackDispose(guestUsername);
                }
            }
        }

        public (int returnCode, GameRoomDC joinedGameRoom) JoinGame(string username, string roomCode)
        {
            int returnCode = 0;
            GameRoom room = GameRoomsPool.GetGameRoom(roomCode);

            if (room != null && room.State.Equals(GameRoomState.Waiting) && room.Players.Count < room.GameConfiguration.MaxPlayers)
            {
                if (!PlayersOnlinePool.IsGuest(username))
                {
                    try
                    {
                        UserDB.UpdateUserStatus(username, PlayerStatus.in_game);
                    }
                    catch (EntityException error)
                    {
                        _logger.Fatal("Database connection failed", error);
                        _logger.WarnFormat("User status update failed (username: {0}, to status: {1})", username, PlayerStatus.in_game.ToString());
                        return (102, null);
                    }
                }
                
                CallbacksPool.PlayerArrivesToPregame(username, OperationContext.Current.GetCallbackChannel<IPregameServiceCallback>());
                room.Players.Add(PlayersOnlinePool.GetPlayer(username));

                List<Player> players = room.Players;
                for (int i = 0; i < players.Count - 1; i++)
                {
                    var callbackChannel = (IPregameServiceCallback)CallbacksPool.GetPregameCallbackChannel(players[i].Username);
                    callbackChannel.RefreshLobby(GameRoomDC.ConvertToGameRoomDC(room));
                }
            }
            else
            {
                returnCode = 401;
            }

            return (returnCode, GameRoomDC.ConvertToGameRoomDC(room));
        }

        public void LeaveLobby(string username, string roomCode)
        {
            GameRoomsPool.RemovePlayerFromGameRoom(username, roomCode);
            CallbacksPool.RemovePregameCallbackChannel(username);

            GameRoom room = GameRoomsPool.GetGameRoom(roomCode);
            if (room != null)
            {
                foreach (Player player in room.Players)
                {
                    var callbackChannel = (IPregameServiceCallback)CallbacksPool.GetPregameCallbackChannel(player.Username);
                    callbackChannel.RefreshLobby(GameRoomDC.ConvertToGameRoomDC(room));
                }
            }

            if (!PlayersOnlinePool.IsGuest(username))
            {
                try
                {
                    UserDB.UpdateUserStatus(username, PlayerStatus.online);
                }
                catch (EntityException error)
                {
                    _logger.Fatal("Database connection failed", error);
                    _logger.WarnFormat("User status not updated in data base (username: {0}, to status: {1})", username, PlayerStatus.online.ToString());
                }
            }
        }

        public void SendMessage(Message message)
        {
            GameRoom room = GameRoomsPool.GetGameRoom(message.GameRoomCode);
            List<Player> players = room.Players;

            foreach (Player p in players)
            {
                var callbackChannel = (IPregameServiceCallback)CallbacksPool.GetPregameCallbackChannel(p.Username);
                try
                {
                    callbackChannel.ReceiveMessage(message);
                }
                catch (ObjectDisposedException error)
                {
                    _logger.Info($"Callback channel disposed (Game room: {message.GameRoomCode}, Username with callback disposed: {p.Username})", error);
                    GameRoomsPool.RemovePlayerFromGameRoom(p.Username, message.GameRoomCode);
                    ManageCallbackDispose(p.Username);
                }
            }
        }

        public void StartGame(string roomCode)
        {
            GameRoom gameRoom = GameRoomsPool.GetGameRoom(roomCode);
            gameRoom.State = GameRoomState.InGame;
            GamesInProgressPool.PrepareGame(roomCode);

            //Redirige a todos los jugadores al tablero, menos al administrador de la sala (el primero en la lista)
            for (int i = 1; i < gameRoom.Players.Length(); i++)
            {
                var callbackChannel = (IPregameServiceCallback)CallbacksPool.GetPregameCallbackChannel(gameRoom.Players[i].Username);
                callbackChannel.CarryInsideGame();
            }
        }

        public bool VerifyGameRoom(string gameCode)
        {
            GameRoom room = GameRoomsPool.GetGameRoom(gameCode);
            return room != null && room.State.Equals(GameRoomState.Waiting) && room.Players.Count < room.GameConfiguration.MaxPlayers;
        }

        public void ReturnToLobby(string gameRoomCode, string username)
        {
            CallbacksPool.RemoveGameCallbackChannel(username);
            GamesInProgressPool.ExitGame(gameRoomCode, username);
            CallbacksPool.PlayerArrivesToPregame(username, OperationContext.Current.GetCallbackChannel<IPregameServiceCallback>());
            GameRoom room = GameRoomsPool.GetGameRoom(gameRoomCode);

            if (room.State.Equals(GameRoomState.InGame))
            {
                room.State = GameRoomState.Waiting;
            }
        }

        private void ManageCallbackDispose(string username)
        {
            try
            {
                UserDB.UpdateUserStatus(username, PlayerStatus.offline);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                _logger.WarnFormat("User status not updated in data base (username: {0}, to status: {1})", username, PlayerStatus.offline.ToString());

            }
            CallbacksPool.RemoveAllCallbacksChannels(username);
            PlayersOnlinePool.RemovePlayer(username);
        }
    }
}
