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
        private List<Player> _players = new List<Player>();
        public List<Player> Players
        {
            get => _players;
            set => _players = value;
        }

        public GameRoomState state { get; set; }
    }
}
