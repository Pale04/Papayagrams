using DataAccess;
using DomainClasses;
using System;
using System.ServiceModel;
using System.Data.Entity.Core;
using LanguageExt;
using BussinessLogic;
using log4net;
using MailKit.Net.Smtp;

namespace Contracts
{
    public partial class ServiceImplementation : ILoginService
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ServiceImplementation));
        private static readonly MailService.MailService _mailService = new MailService.MailService();

        /// <summary>
        /// Create an account for a new user and send an email with the verification code
        /// </summary>
        /// <param name="player">PlayerDC object with the user's data</param>
        /// <returns>0 if the registration was successful, a error code otherwise</returns>
        public int RegisterUser(PlayerDC player)
        {
            int codeResult = 0;
            Player newPlayer = new Player();

            try
            {
                newPlayer.Username = player.Username;
                newPlayer.Email = player.Email;
                newPlayer.Password = player.Password;
            }
            catch (ArgumentException)
            {
                codeResult = 101;
            }

            if (codeResult == 0)
            {
                try
                {
                    if (UserDB.GetPlayerByUsername(newPlayer.Username).IsSome)
                    {
                        codeResult = 201;
                    }
                    else if (UserDB.GetPlayerByEmail(newPlayer.Email).IsSome)
                    {
                        codeResult = 202;
                    }
                    else
                    {
                        UserDB.RegisterUser(newPlayer);
                        SendAccountVerificationCode(newPlayer.Username);
                    }
                }
                catch (EntityException error)
                {
                    _logger.Error("Error while trying to register a new user", error);
                    return 102;
                }
            }

            return codeResult;
        }

        /// <summary>
        /// Log in the Papayagrams application
        /// </summary>
        /// <param name="username">Username of the account</param>
        /// <param name="password">Password of the account</param>
        /// <returns>(0,Player) if the log in was succesful, (errorCode, null) otherwise</returns>
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
            else if (loginResult == 1)
            {
                return (207, null);
            }

            Option<Player> playerLogged = UserDB.GetPlayerByUsername(username);
            PlayersPool.AddPlayer((Player)playerLogged.Case);
            Console.WriteLine("User " + username + " logged in");

            return (0, PlayerDC.ConvertToPlayerDC((Player)playerLogged.Case));
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
                //TODO: Log the error
                return 102;
            }

            if (logoutResult == 0)
            {
                return 205;
            }

            CallbacksPool.RemoveAllCallbacksChannels(username);
            PlayersPool.RemovePlayer(username);
            Console.WriteLine("User " + username + " logged out. His callbacks channels have been removed");

            return 0;
        }

        public int VerifyAccount(string code, string username)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(username))
            {
                return 101;
            }

            if (!PlayersPool.AccountVerificationCodeCorrect(username, code))
            {
                return 208;
            }

            int codeResult;
            try
            {
                codeResult = UserDB.VerifyAccount(username);
            }
            catch (EntityException error)
            {
                //TODO: Log the error
                return 102;
            }

            if (codeResult == 1)
            {
                PlayersPool.RemoveAccountVerificationCode(username);
                return 0;
            }
            else
            {
                return 209;
            }
        }

        public int SendAccountVerificationCode(string username)
        {
            Option<Player> player;

            try
            {
                player = UserDB.GetPlayerByUsername(username);
            }
            catch (EntityException error)
            {
                //TODO: Log the error
                return 102;
            }

            if (player.IsNone)
            {
                return 205;
            }

            Player playerChecking = (Player)player.Case;
            string code = PlayersPool.GenerateAccountVerificationCode(username);

            try
            {
                return _mailService.SendMail(playerChecking.Email, "Account verification code", $"Your account verification code is: {code}");
            }
            catch (SmtpCommandException error)
            {
                _logger.Error("Error while trying to send the account verification code", error);
                return 104;
            }
        }
    }
}
