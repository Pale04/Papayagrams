using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IPregameServiceCallback))]
    public interface IPregameService
    {
        /// <summary>
        /// Create a new game room.
        /// </summary>
        /// <param name="username">Username of the player creating the room</param>
        /// <param name="gameConfiguration">Configuration of the game room</param>
        /// <returns>0 and the game room created, an error code and null if an error occurs</returns>
        /// <remarks>Error codes that can be returned: 102</remarks>
        [OperationContract]
        (int returnCode, GameRoomDC createdGameRoom) CreateGame(string username, GameConfigurationDC gameConfiguration);

        /// <summary>
        /// Put a player into a game room, if it´s in waiting state and has available slots.
        /// </summary>
        /// <param name="username">username of the player joining into the room</param>
        /// <param name="roomCode">code of the game room</param>
        /// <returns>0 and the joined game room object, an error code and null if an error occurss</returns>
        /// <remarks>Error codes that can be returned: 102, 401</remarks>
        [OperationContract]
        (int returnCode, GameRoomDC joinedGameRoom) JoinGame(string username, string roomCode);

        /// <summary>
        /// Leave the game room in waiting state and notify to other players
        /// </summary>
        /// <param name="username">Username of the player leaving the room</param>
        /// <param name="roomCode">code of the game room</param>
        [OperationContract(IsOneWay = true)]
        void LeaveLobby(string username,string roomCode);

        /// <summary>
        /// Send a message through the chat to all players in the game room
        /// </summary>
        /// <param name="message">Message to be sent</param>
        [OperationContract(IsOneWay = true)]
        void SendMessage(Message message);

        /// <summary>
        /// Prepare the game and bring into every player except the admin.
        /// </summary>
        /// <param name="roomCode">Code of the game room</param>
        [OperationContract]
        void StartGame(string roomCode);

        /// <summary>
        /// Send an invitation to a friend to join a game room.
        /// </summary>
        /// <param name="username">Username of the player sending the invitation</param>
        /// <param name="invitedFriend">Username of the player receiving the invitation</param>
        /// <param name="gameRoomCode">Code of the game room</param>
        [OperationContract(IsOneWay = true)]
        void InviteFriend(string username, string invitedFriend, string gameRoomCode);

        /// <summary>
        /// Notify to server that someone has returned to the lobby after a game ended
        /// </summary>
        /// <param name="gameRoomCode">Code of the game room</param>
        /// <param name="username">Username who has returned</param>
        [OperationContract]
        void ReturnToLobby(string gameRoomCode, string username);
    }

    [ServiceContract]
    public interface IPregameServiceCallback 
    {
        /// <summary>
        /// Recieve a message from the chat in the game room.
        /// </summary>
        /// <param name="message">Message that was sent</param>
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(Message message);

        /// <summary>
        /// Notify to all the plyers in the game room that the administrator has started the game.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void CarryInsideGame();

        /// <summary>
        /// Refresh the game room in the lobby after the pieces pile or the players list has changed.
        /// </summary>
        /// <param name="gameRoom">Game room updated</param>
        [OperationContract(IsOneWay = true)]
        void RefreshLobby(GameRoomDC gameRoom);
    }

    [ServiceContract]
    public interface IGameCodeVerificationService
    {
        /// <summary>
        /// Verify if the game room exists and if has available slots for players
        /// </summary>
        /// <param name="gameCode">Game code of the game room</param>
        /// <returns>true if it's available, false otherwise</returns>
        [OperationContract]
        bool VerifyGameRoom(string gameCode);
    }
}
