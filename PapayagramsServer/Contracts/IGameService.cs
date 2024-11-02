using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IGameServiceCallback))]
    public interface IGameService
    {
        [OperationContract]
        void ReachServer(string username);

        [OperationContract]
        void LeaveGame(string username);
    }

    [ServiceContract]
    public interface IGameServiceCallback
    {
        [OperationContract]
        void RefreshGameRoom(string roomCode);
    }
}
