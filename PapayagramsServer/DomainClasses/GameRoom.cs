using System.Collections.Generic;

namespace DomainClasses
{
    public enum GameRoomState
    {
        Waiting,
        InGame,
        Closed
    }

    public enum GameMode
    {
        Original,
        SuddenDeath,
        TimeAttack
    }

    public enum Language
    {
        English,
        Spanish
    }

    public class GameConfiguration
    {
        public GameMode GameMode { get; set; }

        public int InitialPieces { get; set; }

        public int MaxPlayers { get; set; }

        public Language WordsLanguage { get; set; }

        public int TimeLimitMinutes { get; set; }
    }

    public class GameRoom
    {
        public string RoomCode { get; set; }
        
        public List<Player> Players = new List<Player>();

        public GameRoomState State { get; set; }

        public GameConfiguration GameConfiguration { get; set; }

        public override bool Equals(object other)
        {
            bool isEqual = false;

            if (other != null || GetType() == other.GetType())
            {
                GameRoom room = (GameRoom)other;
                isEqual = RoomCode == room.RoomCode;
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
