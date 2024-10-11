using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<Player> Players { get; set; }
        public GameRoomState state { get; set; }
    }
}
