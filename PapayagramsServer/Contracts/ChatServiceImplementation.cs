using BussinessLogic;
using DomainClasses;
using System.ServiceModel;

namespace Contracts
{
    public class ChatServiceImplementation : IChatService
    {
        public void SendMessage(string message, string roomCode)
        {
            BroadcastMessage(message, GameData.GetGameRoom(roomCode));
        }

        private void BroadcastMessage(string message, GameRoom room)
        {
            foreach (Player player in room.Players)
            {
                OperationContext playerContext = PlayerData.GetPlayerContext(player);

                if (playerContext != null)
                {
                    playerContext.GetCallbackChannel<IChatServiceCallback>().ReceiveMessage(message);
                }
            }
        }
    }
}
