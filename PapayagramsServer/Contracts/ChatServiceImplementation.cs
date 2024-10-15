using BussinessLogic;
using DomainClasses;
using System;
using System.ServiceModel;

namespace Contracts
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class ServiceImplementation : IChatService
    {
        public void SendMessage(string message, string roomCode)
        {
            BroadcastMessage(message, GameData.GetGameRoom(roomCode));
        }

        private void BroadcastMessage(string message, GameRoom room)
        {
            Console.Write(message);
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
