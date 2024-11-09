using System;
using System.Collections.Generic;

namespace BussinessLogic
{
    public class GamesInProgressPool
    {
        private static Dictionary<string, Stack<char>> _piecesPiles = new Dictionary<string, Stack<char>>();

        /// <summary>
        /// Generate the pieces pile with the specified amount of pieces for the game room
        /// </summary>
        /// <param name="gameRoomCode">Code of the game room that starts the game</param>
        /// <returns> 0 if the pile was generated successfuly, 1 if the pile already exists</returns>
        public static int GeneratePiecesPile(string gameRoomCode)
        {
            int returnCode = 1;
            if (!_piecesPiles.ContainsKey(gameRoomCode))
            {
                Stack<char> piecesPile = new Stack<char>();
                Random random = new Random();
                int piecesAmount = GameRoomsPool.GetGameRoom(gameRoomCode).Players.Count;

                for (int i = 0; i < piecesAmount; i++)
                {
                    piecesPile.Push((char)random.Next(65, 90));
                }

                _piecesPiles.Add(gameRoomCode, piecesPile);
                returnCode = 0;
            }
            return returnCode;
        }

        /// <summary>
        /// Put a piece in the dump and return three pieces from the pile
        /// </summary>
        /// <param name="gameRoomCode">Code of the game room</param>
        /// <param name="piece">Piece that player puts in the dump</param>
        /// <returns>A list with the returned pieces or an empty List if there are less pieces than players in game</returns>
        public static List<char> PutInDump(string gameRoomCode, char piece)
        {
            List<char> returnPieces = new List<char>();

            if (!ThereAreLessPiecesThanPlayers(gameRoomCode))
            {
                for (int i = 1; i <= 3; i++)
                {
                    returnPieces.Add(_piecesPiles[gameRoomCode].Pop());
                }
                returnPieces.Add(piece);
            }

            return returnPieces;
        }

        /// <summary>
        /// Get pieces equal to the amount of players in the game room. Use it when a player use up all his pieces
        /// </summary>
        /// <param name="gameRoomCode">Code of the game room</param>
        /// <returns>A List with pieces for every player or an empty List if there are less pieces than player in game</returns>
        public static List<char> DistributePieceToEveryPlayer(string gameRoomCode)
        {
            List<char> returnPieces = new List<char>();

            if (!ThereAreLessPiecesThanPlayers(gameRoomCode))
            {
                for (int i = 1; i <= GameRoomsPool.GetGameRoom(gameRoomCode).Players.Count; i++)
                {
                    returnPieces.Add(_piecesPiles[gameRoomCode].Pop());
                }
            }

            return returnPieces;
        }

        private static bool ThereAreLessPiecesThanPlayers(string gameRoomCode)
        {
            return _piecesPiles[gameRoomCode].Count < GameRoomsPool.GetGameRoom(gameRoomCode).Players.Count;
        }
    }
}
