using System.Collections;
using System.Collections.Generic;

namespace DomainClasses
{
    public enum GameRoomState
    {
        Waiting,
        InGame,
        Closed
    }

    public class GameRoom
    {
        public string RoomCode { get; set; }
        public Hashtable Players = new Hashtable();
        public GameRoomState state { get; set; }
    }
}
