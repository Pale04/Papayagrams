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
        string CreateGame(string username);

        /// <summary>
        /// Add a player to the game room of the specified code
        /// </summary>
        /// <param name="roomCode">The game room code to add the player to</param>
        [OperationContract]
        int JoinGame(string username, string roomCode);

        /// <summary>
        /// Remove a player from the game room of the specified code
        /// </summary>
        /// <param name="roomCode">The game room code to remove the player from</param>
        /// <returns>0 if the player was removed successfully</returns>
        [OperationContract]
        int LeaveGame(string username,string roomCode);

        [OperationContract(IsOneWay = true)]
        void SendMessage(Message message);

        [OperationContract(IsOneWay = true)]
        void StartGame(string roomCode);

        [FaultContract(typeof(ServerException))]
        void InviteFriend(string username);
    }

    [ServiceContract]
    public interface IPregameServiceCallback 
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(Message message);
        [OperationContract(IsOneWay = true)]
        void StartGameResponse();
        [OperationContract(IsOneWay = true)]
        void RefreshGameRoom();
    }
}
