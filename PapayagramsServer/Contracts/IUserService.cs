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
        /// <exception cref="ArgumentException">when the player's attributes are invalid</exception>
        /// <exception cref="Exception">when the username or email are already in use</exception>
        [OperationContract]
        int RegisterUser(PlayerDC user);
    }
}
