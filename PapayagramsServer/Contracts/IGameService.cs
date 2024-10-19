using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IGameServiceCallback))]
    public interface IGameService
    {
        [OperationContract]
        int ReachServer();

        [OperationContract]
        int LeaveGame();

    }

    [ServiceContract]
    public interface IGameServiceCallback
    {
        /*TODO
        [OperationContract]
        void RefreshGameRoom(GameState roomCode);
        */
    }
}
