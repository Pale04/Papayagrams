using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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
        string JoinGame(string username, string roomCode);

        /// <summary>
        /// Remove a player from the game room of the specified code
        /// </summary>
        /// <param name="roomCode">The game room code to remove the player from</param>
        /// <returns>0 if the player was removed successfully</returns>
        [OperationContract]
        int LeaveGame(string username,string roomCode);

        [OperationContract(IsOneWay = true)]
        void SendMessage(Message message);
    }

    [ServiceContract]
    public interface IPregameServiceCallback 
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(Message message);
    }

    [DataContract]
    public class Message
    {
        [DataMember]
        public string AuthorUsername;
        [DataMember]
        public string GameRoomCode;
        [DataMember]
        public DateTime Time;
        [DataMember]
        public string Content;
    }
}
