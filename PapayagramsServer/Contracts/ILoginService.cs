using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface ILoginService
    {
        /// <summary>
        /// Create an account for a new user
        /// </summary>
        /// <param name="player">Player object with the user's data</param>
        /// <returns>1 if the registration was successful, 0 otherwise</returns>
        [OperationContract]
        [FaultContract(typeof(ServerException))]
        int RegisterUser(PlayerDC user);

        /// <summary>
        /// Log in the Papayagrams application
        /// </summary>
        /// <param name="username">Username of the account</param>
        /// <param name="password">Password of the account</param>
        /// <returns>0 if the log in operation was succesful, -1 if the password is wrong</returns>
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
