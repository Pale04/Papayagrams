using PapayagramsClient.PapayagramsService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapayagramsClient.ClientData
{
    public class GameProgressData
    {
        public int Points { get; set; }
        public List<List<char>> GameBoard {  get; set; }
        public int PiecesInHand { get; set; }
        public int PiecesInPile { get; set; }
    }

    public class CurrentGame
    {
        public enum GameState
        {
            InLobby,
            Started,
            Finished
        }
        
        public enum PlayerState
        {
            Connected,
            Disconected
        }


        public static GameState State { get; set; }
        public static Hashtable PlayersInRoom { get; set; }
        public static GameProgressData GameData { get; set; }
    }
}
