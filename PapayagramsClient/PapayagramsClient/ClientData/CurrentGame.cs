using PapayagramsClient.PapayagramsService;
using System.Collections.Generic;

namespace PapayagramsClient.ClientData
{
    public class GameProgressData
    {
        public int Points { get; set; }
        public List<List<char>> GameBoard {  get; set; }
        public LinkedList<string> PiecesInHand { get; set; }
    }

    public static class CurrentGame
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

        public static string RoomCode { get; set; }
        public static GameState State { get; set; }
        public static List<PlayerDC> PlayersInRoom { get; set; }
        public static GameProgressData GameData { get; set; }
        public static GameConfigurationDC GameConfig { get; set; }
    }
}
