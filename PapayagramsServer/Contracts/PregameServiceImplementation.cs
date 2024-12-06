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
        /// <summary>
        /// Create a new game room.
        /// </summary>
        /// <param name="username">Username of the player creating the room</param>
        /// <param name="gameConfiguration">Configuration of the game room</param>
        /// <returns>0 and the game room created, an error code and null if an error occurs</returns>
        /// <remarks>Error codes that can be returned: 102</remarks>
        public (int, GameRoomDC) CreateGame(string username, GameConfigurationDC gameConfiguration)
        {
            try
            {
                UserDB.UpdateUserStatus(username, PlayerStatus.in_game);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
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

        /// <summary>
        /// Send an invitation to a friend to join a game room.
        /// </summary>
        /// <param name="username">Username of the player sending the invitation</param>
        /// <param name="guestUsername">Username of the player receiving the invitation</param>
        /// <param name="gameRoomCode">Code of the game room</param>
        public void InviteFriend(string username, string guestUsername, string gameRoomCode)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(guestUsername) || string.IsNullOrEmpty(gameRoomCode))
            {
                _logger.WarnFormat("Invalid parameters for InviteFriend method (username: {0}, guestUsername: {1}, gameRoomCode: {2})", username, guestUsername, gameRoomCode);
                return;
            }

            PlayerStatus status;
            try
            {
                status = UserDB.GetPlayerStatus(username);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                return;
            }

            if (status == PlayerStatus.in_game)
            {
                GameInvitationDC invitation = new GameInvitationDC
                {
                    GameRoomCode = gameRoomCode,
                    SenderUsername = username
                };

                var callbackChannel = (IMainMenuServiceCallback)CallbacksPool.GetPregameCallbackChannel(guestUsername);
                callbackChannel.ReceiveGameInvitation(invitation);
            }
        }

        /// <summary>
        /// Put a player into a game room, if it´s in waiting state and has available slots.
        /// </summary>
        /// <param name="username">username of the player joining into the room</param>
        /// <param name="roomCode">code of the game room</param>
        /// <returns>0 and the joined game room object, an error code and null if an error occurss</returns>
        /// <remarks>Error codes that can be returned: 102, 401</remarks>
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
                        _logger.WarnFormat("User status not updated in data base (username: {0}, to status: {1})", username, PlayerStatus.in_game);
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

        /// <summary>
        /// Leave the game room in waiting state and notify to other players
        /// </summary>
        /// <param name="username">Username of the player leaving the room</param>
        /// <param name="roomCode">code of the game room</param>
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
                    _logger.WarnFormat("User status not updated in data base (username: {0}, to status: {1})", username, PlayerStatus.online);
                }
            }
        }

        /// <summary>
        /// Send a message through the chat to all players in the game room
        /// </summary>
        /// <param name="message">Message to be sent</param>
        public void SendMessage(Message message)
        {
            GameRoom room = GameRoomsPool.GetGameRoom(message.GameRoomCode);
            List<Player> players = room.Players;

            foreach (Player p in players)
            {
                var callbackChannel = (IPregameServiceCallback)CallbacksPool.GetPregameCallbackChannel(p.Username);
                callbackChannel.ReceiveMessage(message);    
            }
        }

        /// <summary>
        /// Prepare the game and bring into every player except the admin.
        /// </summary>
        /// <param name="roomCode">Code of the game room</param>
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

        /// <summary>
        /// Notify to server that someone has returned to the lobby after a game ended
        /// </summary>
        /// <param name="gameRoomCode">Code of the game room</param>
        /// <param name="username">Username who has returned</param>
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
    }
}
