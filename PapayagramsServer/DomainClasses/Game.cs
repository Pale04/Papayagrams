using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainClasses
{
    public class Game
    {
        private Dictionary<string, int> _playersScores = new Dictionary<string, int>();
        private readonly Stack<char> _piecesPile = new Stack<char>();
        private readonly List<Player> _connectedPlayers = new List<Player>();

        public List<Player> ConnectedPlayers { get { return _connectedPlayers; } }
        public Stack<char> PiecesPile { get { return _piecesPile; } }
        public Dictionary<string, int> PlayersScores { get { return _playersScores; } }

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
        /// Retrieve pieces for only one player at the beginning of the game.
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

        /// <summary>
        /// Retrieve a list with one piece from the pile
        /// </summary>
        /// <returns>List with the piece</returns>
        public List<char> TakeSeed()
        {
            List<char> piece = new List<char>
            {
                _piecesPile.Pop()
            };
            return piece;
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

        public bool ThereAreLessPiecesThanPlayers()
        {
            return _piecesPile.Count < _connectedPlayers.Count;
        }

        public void AddScore(string username, int score)
        {
            _playersScores.Add(username,score);
        }

        public int GetScore(string username)
        {
            return _playersScores[username];
        }

        public string GetWinner()
        {
            return _playersScores.OrderByDescending(x => x.Value).First().Key;
        }
    }
}
