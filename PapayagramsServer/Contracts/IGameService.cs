using System.Collections.Generic;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IGameServiceCallback))]
    public interface IGameService
    {
        [OperationContract(IsOneWay = true)]
        void ReachServer(string username, string gameRoomCode);

        [OperationContract(IsOneWay = true)]
        void DumpPiece(string gameRoomCode, string username, char piece);

        [OperationContract(IsOneWay = true)]
        void TakeSeed(string gameRoomCode);

        [OperationContract(IsOneWay = true)]
        void ShoutPapaya(string gameRoomCode);

        [OperationContract(IsOneWay = true)]
        void CalculateWinner(string gameRoomCode, string username, int score);

        [OperationContract]
        void LeaveGame(string gameRoomCode, string username, bool gameEnded);
    }

    [ServiceContract]
    public interface IGameServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void RefreshGameRoom(int piecesNumber, List<PlayerDC> connectedPlayers);

        [OperationContract(IsOneWay = true)]
        void RefreshTimer(int remainingMinutes);

        [OperationContract(IsOneWay = true)]
        void AddSeedsToHand(List<char> initalPieces);

        [OperationContract(IsOneWay = true)]
        void NotifyEndOfGame();

        [OperationContract(IsOneWay = true)]
        void EndGame(string winnerUsername, int score);
    }
}
