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
        public List<Player> Players = new List<Player>();
        public GameRoomState State { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            GameRoom gameRoom = (GameRoom)obj;
            return RoomCode == gameRoom.RoomCode;
        }
    }
}
