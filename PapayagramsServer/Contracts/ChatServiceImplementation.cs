using DomainClasses;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;

namespace Contracts
{
    public class ChatServiceImplementation : IChatService
    {
        public static Hashtable GameRooms = new Hashtable();

        public void SendMessage(string message, int roomCode)
        {
            GameRoom currentGameRoom = (GameRoom)GameRooms[roomCode];

            if (currentGameRoom == null)
            {
                return;
            }

            BroadcastMessage(message, currentGameRoom);
        }

        private void BroadcastMessage(string message, GameRoom room)
        {
            foreach (Player player in room.Players)
            {
                player.Context.GetCallbackChannel<IChatServiceCallback>().ReceiveMessage(message);
            }
        }
    }
}
