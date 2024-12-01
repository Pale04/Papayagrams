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

        /// <summary>
        /// Total minutes that will be during the game. 0 means that has not time limit.
        /// </summary>
        public int TimeLimitMinutes { get; set; }
    }

    public class GameRoom
    {
        private readonly List<Player> _players = new List<Player>();
        
        public string RoomCode { get; set; }
        public List<Player> Players { get { return _players; } }
        public GameRoomState State { get; set; }
        public GameConfiguration GameConfiguration { get; set; }

        public override bool Equals(object obj)
        {
            bool isEqual = false;

            if (obj != null && GetType() == obj.GetType())
            {
                GameRoom room = (GameRoom)obj;
                isEqual = RoomCode == room.RoomCode;
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            return RoomCode.GetHashCode();
        }
    }
}
