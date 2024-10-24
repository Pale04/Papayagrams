using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface ILoginService
    {
        [OperationContract]
        int RegisterUser(PlayerDC player);

        [OperationContract]
        (int, PlayerDC) Login(string username, string password);

        [OperationContract]
        int Logout(string username);
    }
}
