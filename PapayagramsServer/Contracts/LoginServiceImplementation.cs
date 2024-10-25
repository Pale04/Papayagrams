using DataAccess;
using DomainClasses;
using System;
using System.ServiceModel;
using System.Data.Entity.Core;
using LanguageExt;
using LanguageExt.Common;

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
            catch (ArgumentException)
            {
                return 101;
            }

            try
            {
                if (UserDB.GetPlayerByUsername(newPlayer.Username).IsSome)
                {
                    return 201;
                }
                else if (UserDB.GetPlayerByEmail(newPlayer.Email).IsSome)
                {
                    return 202;
                }

                UserDB.RegisterUser(newPlayer);
                return 0;
            }
            catch (EntityException error)
            {
                //TODO: Log the error
                return 102;
            }
        }

        /// <summary>
        /// Log in the Papayagrams application
        /// </summary>
        /// <param name="username">Username of the account</param>
        /// <param name="password">Password of the account</param>
        /// <returns>0 if the log in was succesful</returns>
        /// <exception cref="FaultException{ServerException}">Thrown when the parameters are invalid or happens, the account is not foun, the password is incorrect or happens a database connection failure</exception>
        public (int, PlayerDC) Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                return (203, null);
            }
            else if (string.IsNullOrEmpty(password))
            {
                return (204, null);
            }

            int loginResult;
            try
            {
                loginResult = UserDB.LogIn(username, password);
            }
            catch (EntityException error)
            {
                //TODO: Log the error
                return (102, null);
            }

            if (loginResult == -1)
            {
                return (205, null);
            }
            else if (loginResult == -2)
            {
                return (206, null);
            }

            Option<Player> playerLogged = UserDB.GetPlayerByUsername(username);
            Console.WriteLine("User " + username + " logged in");
            return (0,PlayerDC.ConvertToPlayerDC((Player)playerLogged.Case));
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
                return 101;
            }

            int logoutResult;
            try
            {
                logoutResult = UserDB.LogOut(username);
            }
            catch (EntityException error)
            {
                return 102;
            }

            if (logoutResult == 0)
            {
                return 205;
            }

            //TODO: Remover al jugador y todos sus callback channels del hashtable
            Console.WriteLine("User " + username + " logged out");
            return 0;
        }
    }
}
