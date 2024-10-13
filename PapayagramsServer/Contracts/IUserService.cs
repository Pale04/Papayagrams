using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IUserServiceCallback))]
    public interface IUserService
    {
        [OperationContract(IsOneWay = true)]
        void RegisterUser(string username, string password);
    }

    [ServiceContract]
    public interface IUserServiceCallback
    {
        [OperationContract]
        void RegisterUserResponse(bool success);

    }
}
