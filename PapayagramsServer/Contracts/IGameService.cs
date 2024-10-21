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
        [OperationContract]
        void RefreshGameRoom(string roomCode);
    }
}
