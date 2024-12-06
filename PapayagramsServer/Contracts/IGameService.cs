using System.Collections.Generic;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IGameServiceCallback))]
    public interface IGameService
    {
        /// <summary>
        /// Notify the server that a player has arrived to the game after the host has started it
        /// </summary>
        /// <param name="username">Username of the player arriving</param>
        /// <param name="gameRoomCode">Code of the game room</param>
        [OperationContract(IsOneWay = true)]
        void ReachServer(string username, string gameRoomCode);

        /// <summary>
        /// Send 1 piece to the "dump" and get 3 pieces through the "AddSeedsToHand" callback method
        /// </summary>
        /// <param name="gameRoomCode">Code of the game room</param>
        /// <param name="username">Username of the player sending the pieces</param>
        /// <param name="piece">Piece sent to dump</param>
        [OperationContract(IsOneWay = true)]
        void DumpPiece(string gameRoomCode, string username, char piece);

        /// <summary>
        /// Send 1 piece to every player after a player finished his pieces
        /// </summary>
        /// <param name="gameRoomCode">Code of the game room</param>
        [OperationContract(IsOneWay = true)]
        void TakeSeed(string gameRoomCode);

        /// <summary>
        /// Notify to every player that someone has finished his pieces.
        /// </summary>
        /// <param name="gameRoomCode">Code of the game room</param>
        [OperationContract(IsOneWay = true)]
        void ShoutPapaya(string gameRoomCode);

        /// <summary>
        /// Calculate the winner of the game and notify the players, after someone shouted "Papaya"
        /// </summary>
        /// <param name="gameRoomCode">Code of the game room</param>
        /// <param name="username">Username of the player sending his score</param>
        /// <param name="score">Score of the player</param>
        [OperationContract(IsOneWay = true)]
        void CalculateWinner(string gameRoomCode, string username, int score);

        /// <summary>
        /// Leave the game room and if the game has not ended notify to other players 
        /// </summary>
        /// <param name="gameRoomCode">Code of the game room</param>
        /// <param name="username">Username of the player leaving the room</param>
        /// <param name="gameEnded">True if the game has ended, false otherwise</param>
        [OperationContract]
        void LeaveGame(string gameRoomCode, string username, bool gameEnded);
    }

    [ServiceContract]
    public interface IGameServiceCallback
    {
        /// <summary>
        /// Update the game room with the new number of pieces and the list of connected players
        /// </summary>
        /// <param name="piecesNumber">Number of pieces within the pile</param>
        /// <param name="connectedPlayers">Lista of players playing</param>
        [OperationContract(IsOneWay = true)]
        void RefreshGameRoom(int piecesNumber, List<PlayerDC> connectedPlayers);

        /// <summary>
        /// Refresh the timer of the game every determined time
        /// </summary>
        /// <param name="remainingMinutes">Minutes for end game</param>
        [OperationContract(IsOneWay = true)]
        void RefreshTimer(int remainingMinutes);

        /// <summary>
        /// Add pieces to the hand of the player when it is necessary
        /// </summary>
        /// <param name="initalPieces">pieces sent to player</param>
        [OperationContract(IsOneWay = true)]
        void AddSeedsToHand(List<char> initalPieces);

        /// <summary>
        /// Notify the end of the game to the players after someone has shouted "Papaya"
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void NotifyEndOfGame();

        /// <summary>
        /// Notify the game winner after he was calculated
        /// </summary>
        /// <param name="winnerUsername">username of the winner player</param>
        /// <param name="score">score of the winner player</param>
        [OperationContract(IsOneWay = true)]
        void EndGame(string winnerUsername, int score);
    }
}
