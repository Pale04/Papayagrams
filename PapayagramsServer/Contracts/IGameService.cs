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
        void DumpPiece(string piece);

        //Cuando un jugador termina todas sus fichas y el server le manda una a cada jugador.
        [OperationContract(IsOneWay = true)]
        void TakeSeed();

        //Un jugador terminó todas sus fichas y hay menos fichas en la pila que jugadores.
        [OperationContract]
        void ShoutPapaya(string username);

        [OperationContract]
        void LeaveGame(string gameRoomCode, string username);
    }

    [ServiceContract]
    public interface IGameServiceCallback
    {
        //Se llama cada vez que se actualiza la pila, cuando alguien se sale de la partida.
        [OperationContract]
        void RefreshGameRoom(string roomCode);

        [OperationContract]
        void ReceiveStartingHand(List<char> initalPieces);

        //El server manda tres fichas a un jugador después de utilizar el dump
        [OperationContract]
        void AddDumpSeedsToHand(List<string> pieces);

        //El server manda una ficha a cada jugador cuando un jugador termina todas sus fichas.
        [OperationContract]
        void AddSeedToHand(string piece);

        //Hay menos fichas en la pila que jugadores, entonces los clientes deben deshabilitar el dump.
        //También puede suceder que hay dos jugadores y dos fichas, entonces ya no hay suficientes fichas para regresar.
        [OperationContract]
        void RestrictDump();

        [OperationContract]
        void EndGame(string winner);
    }
}
