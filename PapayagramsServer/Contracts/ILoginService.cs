using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface ILoginService
    {
        /// <summary>
        /// Log in the Papayagrams application
        /// </summary>
        /// <param name="username">Username of the account</param>
        /// <param name="password">Password of the account</param>
        /// <returns>0 if the log in operation was succesful, -1 if the password is wrong</returns>
        /// <exception cref="ArgumentException">when the username or password are empty</exception>"
        /// <exception cref="Exception">when the username or password are incorrect</exception>""
        [OperationContract]
        int Login(string username, string password);

        /// <summary>
        /// Logout the current user
        /// </summary>
        /// <returns>0 if logout successfully</returns>
        [OperationContract]
        int Logout(string username);
    }

}
