using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IPregameServiceCallback))]
    public interface IPregameService
    {
        /// <summary>
        /// Create a game room and add the player to it
        /// </summary>
        [OperationContract]
        (int, GameRoomDC) CreateGame(string username, GameConfigurationDC gameConfiguration);

        /// <summary>
        /// Add a player to the game room of the specified code
        /// </summary>
        /// <param name="roomCode">The game room code to add the player to</param>
        [OperationContract]
        (int, GameRoomDC) JoinGame(string username, string roomCode);

        /// <summary>
        /// Remove a player from the game room of the specified code
        /// </summary>
        /// <param name="roomCode">The game room code to remove the player from</param>
        /// <returns>0 if the player was removed successfully</returns>
        [OperationContract]
        void LeaveLobby(string username,string roomCode);

        [OperationContract(IsOneWay = true)]
        void SendMessage(Message message);

        [OperationContract(IsOneWay = true)]
        void StartGame(string roomCode);

        [OperationContract]
        void InviteFriend(string username);

        [OperationContract]
        void ReturnToLobby(string username);
    }

    [ServiceContract]
    public interface IPregameServiceCallback 
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(Message message);

        [OperationContract(IsOneWay = true)]
        void StartGameResponse();

        [OperationContract(IsOneWay = true)]
        void RefreshLobby(GameRoomDC gameRoom);
    }
}
