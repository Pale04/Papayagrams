using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface ILoginService
    {
        [OperationContract]
        [FaultContract(typeof(ServerException))]
        int RegisterUser(PlayerDC player);

        [OperationContract]
        (int, PlayerDC) Login(string username, string password);

        [OperationContract]
        [FaultContract(typeof(ServerException))]
        int Logout(string username);
    }
}
