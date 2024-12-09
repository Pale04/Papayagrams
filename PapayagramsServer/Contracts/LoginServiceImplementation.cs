using DataAccess;
using DomainClasses;
using System;
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
                        _logger.InfoFormat("Attempt to register with email in use(email: {0})", newPlayer.Email);
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
                    _logger.Fatal("Database connection failed", error);
                    return 102;
                }
            }

            return codeResult;
        }

        public (int errorCode, PlayerDC loggedPlayer) Login(string username, string password)
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
                _logger.Fatal("Database connection failed", error);
                return (102, null);
            }

            if (loginResult == -1)
            {
                return (205, null);
            }
            else if (loginResult == -2)
            {
                _logger.InfoFormat("Login attempt failed (username id: {0})",username);
                return (206, null);
            }

            Option<Player> playerLogged = UserDB.GetPlayerByUsername(username);
            int code = 0;
            if (loginResult == 1)
            {
                code = 207;
            }

            PlayersOnlinePool.AddPlayer((Player)playerLogged.Case);
            _logger.InfoFormat("Login successful (username id: {0})", username);

            return (code, PlayerDC.ConvertToPlayerDC((Player)playerLogged.Case));
        }

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
                _logger.Fatal($"Database connection failed. User status not updated in data base (username: {username}, to status: {PlayerStatus.offline})", error);
                return 102;
            }

            if (logoutResult == 0)
            {
                return 205;
            }

            CallbacksPool.RemoveAllCallbacksChannels(username);
            PlayersOnlinePool.RemovePlayer(username);
            return 0;
        }

        public int VerifyAccount(string username, string code)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(username))
            {
                return 101;
            }

            if (!VerificationCodesPool.AccountVerificationCodeCorrect(username, code))
            {
                _logger.InfoFormat("Account verification attempt failed (username id: {0})", username);
                return 208;
            }

            int codeResult;
            try
            {
                codeResult = UserDB.VerifyAccount(username);
            }
            catch (EntityException error)
            {
                _logger.Fatal($"Database connection failed. Account verification update (Username: {username})", error);
                return 102;
            }

            if (codeResult == 1)
            {
                VerificationCodesPool.RemoveAccountVerificationCode(username);
                _logger.InfoFormat("Account verification successful (username id: {0})",username);
                return 0;
            }
            else
            {
                _logger.WarnFormat("Account verification failed. Unknown return code (username: {0}, return code: {1})", username, codeResult);
                return 209;
            }
        }

        public int SendAccountVerificationCode(string username)
        {
            Option<Player> wrappedPlayer;

            try
            {
                wrappedPlayer = UserDB.GetPlayerByUsername(username);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed",error);
                return 102;
            }

            if (wrappedPlayer.IsNone)
            {
                return 205;
            }

            Player player = (Player)wrappedPlayer.Case;
            string code = VerificationCodesPool.GenerateAccountVerificationCode(username);

            try
            {
                return MailService.MailService.SendMail(player.Email, "Account verification code", $"Your account verification code is: {code}");
            }
            catch (SmtpCommandException error)
            {
                _logger.Warn($"Verificaton email sending failed (username: {username})", error);
                return 104;
            }
        }

        public PlayerDC AccessAsGuest()
        {
            return PlayerDC.ConvertToPlayerDC(PlayersOnlinePool.CreateGuestProfile());
        }

        public int SendPasswordRecoveryPIN(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return 101;
            }

            string pin = VerificationCodesPool.GeneratePasswordRecoveryPIN(email);
            try
            {
                return MailService.MailService.SendMail(email, "Password recovery PIN", $"Your password recovery PIN is: {pin}");
            }
            catch (SmtpCommandException error)
            {
                _logger.Warn($"Password recovery email sending failed (Account email: {email})", error);
                return 104;
            }
        }

        public int RecoverPassword(string pin, string email, string newPassword)
        {
            if (string.IsNullOrEmpty(pin) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(newPassword))
            {
                return 101;
            }

            if (!VerificationCodesPool.PasswordRecoveryPINCorrect(email, pin))
            {
                _logger.InfoFormat("Password recovery attempt failed (Account email: {0})", email);
                return 210;
            }

            int codeResult;
            try
            {
                codeResult = UserDB.UpdatePassword(email, newPassword);
            }
            catch (EntityException error)
            {
                _logger.Fatal($"Database connection failed. Password update (Account email: {email})", error);
                return 102;
            }

            if (codeResult == 0)
            {
                _logger.WarnFormat("Password recovery failed (Account email: {0})", email);
                return 211;
            }

            VerificationCodesPool.RemovePasswordRecoveryPIN(email);
            return 0;
        }
    }
}
