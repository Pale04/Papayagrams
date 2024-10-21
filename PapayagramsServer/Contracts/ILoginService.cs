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
        [FaultContract(typeof(ServerException))]
        int Login(string username, string password);

        /// <summary>
        /// Logout the current user
        /// </summary>
        /// <returns>0 if logout successfully</returns>
        [OperationContract]
        int Logout(string username);
    }
}
