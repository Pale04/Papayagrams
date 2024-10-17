﻿using System.ServiceModel;

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
        int RegisterUser(PlayerDC user);

        /// <summary>
        /// Log in the Papayagrams application
        /// </summary>
        /// <param name="username">Username of the account</param>
        /// <param name="password">Password of the account</param>
        /// <returns>The PLayerDC object with the user's information</returns>
        [OperationContract]
        PlayerDC LogIn(string username, string password);
    }
}
