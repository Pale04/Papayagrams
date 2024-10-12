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
        public int RoomCode { get; set; }
        public List<Player> Players { get; set; }
        public GameRoomState state { get; set; }
    }
}
