using BussinessLogic;
using DataAccess;
using DomainClasses;
using System;
using System.ServiceModel;
using System.Data.Entity.Core;
using LanguageExt;

namespace Contracts
{
    public partial class ServiceImplementation : ILoginService
    {
        /// <summary>
        /// Create an account for a new user
        /// </summary>
        /// <param name="player">PlayerDC object with the user's data</param>
        /// <returns>0 if the registration was successful</returns>
        /// <exception cref="FaultException{ServerException}">Thrown when the parameters are invalid, the username or email already exists or happens a database connection failure</exception>
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

                UserDB.RegisterUser(newPlayer);
                return 0;
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
        /// <returns>0 if the log in was succesful</returns>
        /// <exception cref="FaultException{ServerException}">Thrown when the parameters are invalid or happens, the account is not foun, the password is incorrect or happens a database connection failure</exception>
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

            Option<Player> playerLogged = UserDB.GetPlayerByUsername(username);
            PlayerData.AddPlayer((Player)playerLogged.Case, username);
            Console.WriteLine("User " + username + " logged in");
            return 0;
        }

        /// <summary>
        /// Take out the player from the application
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>0 if the logut was succesfull</returns>
        /// <exception cref="FaultException{ServerException}">thrown when the parameter is invalid, hapens a database connection failure or the username does not exist</exception>
        public int Logout(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new FaultException<ServerException>(new ServerException(1));
            }

            int logoutResult;
            try
            {
                logoutResult = UserDB.LogOut(username);
            }
            catch (EntityException error)
            {
                throw new FaultException<ServerException>(new ServerException(2, error.StackTrace));
            }

            if (logoutResult == 0)
            {
                throw new FaultException<ServerException>(new ServerException(105));
            }

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
