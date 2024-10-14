using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface ILoginService
    {
        /// <summary>
        /// Login as the specified user
        /// </summary>
        /// <param name="username">The username of the user to login</param>
        /// <param name="password">The password of the user</param>
        /// <returns>0 if the login was successful
        /// 1 if the password was incorrect</returns>
        [OperationContract]
        int Login(string username, string password);

        /// <summary>
        /// Logout the current user
        /// </summary>
        /// <returns>0 if logout successfully</returns>
        [OperationContract]
        int Logout();
    }

}
