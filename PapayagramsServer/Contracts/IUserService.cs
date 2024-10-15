using DomainClasses;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface IUserService
    {
        /// <summary>
        /// Create an account for a new user
        /// </summary>
        /// <param name="player">Player object with the user's data</param>
        /// <returns>1 if the registration was successful, 0 otherwise</returns>
        [OperationContract]
        int RegisterUser(Player player);
    }
}
