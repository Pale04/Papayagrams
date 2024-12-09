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
                _logger.Fatal($"Database connection failed. User status not updated in data base (username: {username}, to status: {PlayerStatus.in_game})", error);
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

        public void InviteFriend(string username, string invitedFriend, string gameRoomCode)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(invitedFriend) || string.IsNullOrEmpty(gameRoomCode))
            {
                _logger.WarnFormat("Invalid parameters for InviteFriend method (username: {0}, guestUsername: {1}, gameRoomCode: {2})", username, invitedFriend, gameRoomCode);
                return;
            }

            PlayerStatus invitedFriendStatus;
            try
            {
                invitedFriendStatus = UserDB.GetPlayerStatus(invitedFriend);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed. Get player status attempt", error);
                return;
            }

            if (invitedFriendStatus.Equals(PlayerStatus.online) && CheckMainMenuCallbackState(invitedFriend))
            {
                GameInvitationDC invitation = new GameInvitationDC
                {
                    GameRoomCode = gameRoomCode,
                    SenderUsername = username
                };

                var callbackChannel = (IMainMenuServiceCallback)CallbacksPool.GetMainMenuCallbackChannel(invitedFriend);
                callbackChannel.ReceiveGameInvitation(invitation);
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
                        _logger.Fatal("Database connection failed. User status update failed (username: {username}, to status: {PlayerStatus.in_game})", error);
                    }
                }
                
                CallbacksPool.PlayerArrivesToPregame(username, OperationContext.Current.GetCallbackChannel<IPregameServiceCallback>());
                room.Players.Add(PlayersOnlinePool.GetPlayer(username));
                
                CheckPregameCallbackChannelState(roomCode);
                for (int i = 0; i < room.Players.Count - 1; i++)
                {
                    var callbackChannel = (IPregameServiceCallback)CallbacksPool.GetPregameCallbackChannel(room.Players[i].Username);
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
            CheckPregameCallbackChannelState(roomCode);
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
                    _logger.Fatal($"Database connection failed. User status not updated in data base (username: {username}, to status: {PlayerStatus.online})", error);
                }
            }
        }

        public void SendMessage(Message message)
        {
            CheckPregameCallbackChannelState(message.GameRoomCode);
            List<Player> players = new List<Player>(GameRoomsPool.GetGameRoom(message.GameRoomCode).Players);

            foreach (Player p in players)
            {
                var callbackChannel = (IPregameServiceCallback)CallbacksPool.GetPregameCallbackChannel(p.Username);
                callbackChannel.ReceiveMessage(message);
            }
        }

        public void StartGame(string roomCode)
        {
            GameRoom gameRoom = GameRoomsPool.GetGameRoom(roomCode);
            gameRoom.State = GameRoomState.InGame;
            List<Player> players = new List<Player>(gameRoom.Players);
            GamesInProgressPool.PrepareGame(roomCode);

            //Redirige a todos los jugadores al tablero, menos al administrador de la sala (el primero en la lista)
            for (int i = 1; i < players.Count; i++)
            {
                var callbackChannel = (IPregameServiceCallback)CallbacksPool.GetPregameCallbackChannel(players[i].Username);
                if (((ICommunicationObject)callbackChannel).State == CommunicationState.Closed)
                {
                    _logger.InfoFormat("PregameCallback channel disposed (Game room: {0}, Username with callback disposed: {1})", roomCode, players[i].Username);
                    GameRoomsPool.RemovePlayerFromGameRoom(players[i].Username, roomCode);
                    ManageCallbackDispose(players[i].Username, roomCode);
                }
                else
                {
                    callbackChannel.CarryInsideGame();
                }
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

        private void ManageCallbackDispose(string username, string gameRoomCode)
        {
            if (!PlayersOnlinePool.IsGuest(username))
            {
                Logout(username);
            }
            else
            {
                PlayersOnlinePool.RemoveGuest(username);
            }
        }

        private void CheckPregameCallbackChannelState(string gameRoomCode)
        {
            List<Player> players = new List<Player>(GameRoomsPool.GetGameRoom(gameRoomCode).Players);
            foreach (Player player in players)
            {
                var callbackChannel = (IPregameServiceCallback)CallbacksPool.GetPregameCallbackChannel(player.Username);
                if (((ICommunicationObject)callbackChannel).State == CommunicationState.Closed)
                {
                    _logger.InfoFormat("PregameCallback channel disposed (Game room: {0}, Username with callback disposed: {1})", gameRoomCode, player.Username);
                    GameRoomsPool.RemovePlayerFromGameRoom(player.Username, gameRoomCode);
                    ManageCallbackDispose(player.Username, gameRoomCode);
                }
            }
        }
    }
}
