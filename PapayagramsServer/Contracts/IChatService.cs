using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IChatServiceCallback))]
    public interface IChatService
    {
        //[OperationContract(IsOneWay = true)]
        [OperationContract]
        void SendMessage(string message);
    }

    [ServiceContract]
    public interface IChatServiceCallback
    {
        [OperationContract]
        void ReceiveMessage(string message);
    }
}
