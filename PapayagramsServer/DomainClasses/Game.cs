using System;
using System.Collections.Generic;

namespace DomainClasses
{
    public class Game
    {
        private Stack<char> _piecesPile = new Stack<char>();
        private List<Player> _connectedPlayers = new List<Player>();

        public List<Player> ConnectedPlayers { get { return _connectedPlayers; } }

        /// <summary>
        /// Generate the pieces pile with the specified amount of pieces for the game room
        /// </summary>
        /// <param name="piecesAmount">Number of pieces that generates</param>
        /// <returns> 0 if the pile was generated successfuly, 1 if the pile already exists</returns>
        public int GeneratePiecesPile(int piecesAmount)
        {
            int returnCode = 1;
            if (_piecesPile.Count == 0)
            {
                Random random = new Random();
                for (int i = 1; i <= piecesAmount; i++)
                {
                    _piecesPile.Push((char)random.Next(65, 90));
                }
                returnCode = 0;
            }
            return returnCode;
        }

        /// <summary>
        /// Retrieve pieces for one player at the beginning of the game.
        /// </summary>
        /// <param name="initialPieces">Number of pieces that must be distributed to every player</param>
        /// <returns>List with the pieces</returns>
        public List<char> GetInitialPieces(int initialPieces)
        {
            List<char> pieces = new List<char>();
            for (int i = 1; i <= initialPieces; i++)
            {
                pieces.Add(_piecesPile.Pop());
            }
            return pieces;
        }

        public List<char> GetPieceForEveryone()
        {
            List<char> pieces = new List<char>();
            if (!ThereAreLessPiecesThanPlayers())
            {
                foreach (Player _ in ConnectedPlayers)
                {
                    pieces.Add(_piecesPile.Pop());
                }
            }
            return pieces;
        }

        /// <summary>
        /// Put a piece in the dump and return three pieces from the pile
        /// </summary>
        /// <param name="piece">Piece that player puts in the dump</param>
        /// <returns>A list with the returned pieces or an empty List if there are less pieces than 3</returns>
        public List<char> PutInDump(char piece)
        {
            List<char> returnPieces = new List<char>();

            if (_piecesPile.Count >= 3)
            {
                for (int i = 1; i <= 3; i++)
                {
                    returnPieces.Add(_piecesPile.Pop());
                }
                _piecesPile.Push(piece);
            }

            return returnPieces;
        }

        private bool ThereAreLessPiecesThanPlayers()
        {
            return _piecesPile.Count < _connectedPlayers.Count;
        }
    }
}
