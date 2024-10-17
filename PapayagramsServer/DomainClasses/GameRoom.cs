using System.Collections;

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
