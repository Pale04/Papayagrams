using DomainClasses;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IUserServiceCallback))]
    public interface IUserService
    {
        [OperationContract(IsOneWay = true)]
        void RegisterUser(Player player);
    }

    [ServiceContract]
    public interface IUserServiceCallback
    {
        [OperationContract]
        void RegisterUserResponse(bool success);

    }
}
