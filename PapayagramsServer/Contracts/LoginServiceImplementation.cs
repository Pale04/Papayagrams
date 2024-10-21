using BussinessLogic;
using DataAccess;
using DomainClasses;
using LanguageExt;
using System;
using System.ServiceModel;
using System.Data.Entity.Core;
using LanguageExt.Common;

namespace Contracts
{
    public partial class ServiceImplementation : ILoginService
    {
        /// <summary>
        /// Create an account for a new user
        /// </summary>
        /// <param name="player">PlayerDC object with the user's data</param>
        /// <returns>6 if the registration was successful</returns>
        /// <exception cref="FaultException">Thrown when the parameters are invalid, the username or email already exists or happens a database connection failure</exception>
        public int RegisterUser(PlayerDC player)
        {
            Player newPlayer = new Player();
            try
            {
                newPlayer.Username = player.Username;
                newPlayer.Email = player.Email;
                newPlayer.Password = player.Password;
            }
            catch (ArgumentException error)
            {
                throw new FaultException<ServerException>(new ServerException(1, error.StackTrace));
            }

            try
            {
                if (UserDB.GetPlayerByUsername(newPlayer.Username).IsSome)
                {
                    throw new FaultException<ServerException>(new ServerException(101));
                }
                else if (UserDB.GetPlayerByEmail(newPlayer.Email).IsSome)
                {
                    throw new FaultException<ServerException>(new ServerException(102));
                }
                return UserDB.RegisterUser(newPlayer);
            }
            catch (EntityException error)
            {
                throw new FaultException<ServerException>(new ServerException(2, error.StackTrace));
            }
        }

        /// <summary>
        /// Log in the Papayagrams application
        /// </summary>
        /// <param name="username">Username of the account</param>
        /// <param name="password">Password of the account</param>
        /// <returns>0 if the log in was succesful, -1 if the account does not exist or -2 if the password is incorrect</returns>
        /// <exception cref="FaultException">Thrown when the parameters are invalid or happens, the account is not foun, the password is incorrect or happens a database connection failure</exception>
        public int Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new FaultException<ServerException>(new ServerException(103));
            }
            else if (string.IsNullOrEmpty(password))
            {
                throw new FaultException<ServerException>(new ServerException(104));
            }

            int loginResult;
            try
            {
                loginResult = UserDB.LogIn(username, password);
            }
            catch (EntityException error)
            {
                throw new FaultException<ServerException>(new ServerException(2, error.StackTrace));
            }

            if (loginResult == -1)
            {
                throw new FaultException<ServerException>(new ServerException(105));
            }
            else if (loginResult == -2)
            {
                throw new FaultException<ServerException>(new ServerException(106));
            }
            return loginResult;
        }

        public int Logout(string username)
        {
            PlayerData.RemovePlayer(username);
            Console.WriteLine("User " + username + " logged out");
            return 0;
        }

        private PlayerDC ConvertPlayerToDataContract(Player player)
        {
            return new PlayerDC()
            {
                Id = player.Id,
                Username = player.Username,
                Email = player.Email,
                Password = player.Password
            };
        }
    }
}
