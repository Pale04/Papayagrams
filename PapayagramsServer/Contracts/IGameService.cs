using System.Collections.Generic;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IGameServiceCallback))]
    public interface IGameService
    {
        //Crear un objeto juego con metodo isEveryoneReady()
        //se crea la pila de fichas
        //Se guarda callback y hasta que estén todos dentro del juego entonces se llama StartGame
        [OperationContract(IsOneWay = true)]
        void ReachServer(string username);

        [OperationContract(IsOneWay = true)]
        void DumpPiece(string piece);

        //Cuando un jugador termina todas sus fichas y el server le manda una a cada jugador.
        [OperationContract(IsOneWay = true)]
        void TakeSeed();

        //Un jugador terminó todas sus fichas y hay menos fichas en la pila que jugadores.
        [OperationContract]
        void ShoutPapaya(string username);

        [OperationContract]
        void LeaveGame(string username);
    }

    [ServiceContract]
    public interface IGameServiceCallback
    {
        //Se llama cada vez que se actualiza la pila, cuando alguien se sale de la partida.
        [OperationContract]
        void RefreshGameRoom(string roomCode);

        //El server manda las fichas iniciales después de que TODOS reportan al server.
        [OperationContract]
        void ReceiveStartingHand(List<string> initalPieces);

        //El server manda tres fichas a un jugador después de utilizar el dump
        [OperationContract]
        void AddDumpSeedsToHand(List<string> pieces);

        //El server manda una ficha a cada jugador cuando un jugador termina todas sus fichas.
        [OperationContract]
        void AddSeedToHand(string piece);

        //Hay menos fichas en la pila que jugadores, entonces los clientes deben deshabilitar el dump.
        [OperationContract]
        void RestrictDump();

        [OperationContract]
        void EndGame(string winner);
    }
}
