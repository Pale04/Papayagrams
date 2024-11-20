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

        //Cuando un jugador termina todas sus fichas y el server le manda una a cada jugador.
        [OperationContract(IsOneWay = true)]
        void TakeSeed(string gameRoomCode);

        //Un jugador terminó todas sus fichas y hay menos fichas en la pila que jugadores.
        [OperationContract(IsOneWay = true)]
        void ShoutPapaya(string gameRoomCode, string username);

        [OperationContract]
        void LeaveGame(string gameRoomCode, string username);

        //TODO: refinar
        [OperationContract]
        void CalculateWinner();
    }

    [ServiceContract]
    public interface IGameServiceCallback
    {
        //Se llama cada vez que se actualiza la pila, cuando alguien se sale de la partida.
        [OperationContract(IsOneWay = true)]
        void RefreshGameRoom(Stack<char> piecesPile, List<PlayerDC> connectedPlayers);

        [OperationContract(IsOneWay = true)]
        void RefreshTimer(int remainingMinutes);

        //Manda fichas iniciales, fichas del dump y una ficha cuando alguien se acaba las suyas
        [OperationContract(IsOneWay = true)]
        void AddSeedsToHand(List<char> initalPieces);

        [OperationContract(IsOneWay = true)]
        void EndGame();
    }
}
