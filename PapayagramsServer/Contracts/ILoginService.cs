using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface ILoginService
    {
        /// <summary>
        /// Create an account for a new user and send an email with the verification code
        /// </summary>
        /// <param name="player">PlayerDC object with the user's data</param>
        /// <returns>0 if the registration was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 102, 201, 202</remarks>
        [OperationContract]
        int RegisterUser(PlayerDC player);

        /// <summary>
        /// Log in the Papayagrams application
        /// </summary>
        /// <param name="username">Username of the account</param>
        /// <param name="password">Password of the account</param>
        /// <returns>(0,Player) if the log in was succesful, (errorCode, null) otherwise</returns>
        /// <remarks>Error codes that can be returned: 102, 203, 204, 205, 206, 207</remarks>
        [OperationContract]
        (int errorCode, PlayerDC loggedPlayer) Login(string username, string password);

        /// <summary>
        /// Take out the player from the application
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>0 if the logut was succesfull, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 102, 205</remarks>
        [OperationContract]
        int Logout(string username);

        /// <summary>
        /// Change the status of an account to verified
        /// </summary>
        /// <param name="username">Username of the player veifyig his account</param>
        /// <param name="code">Verification code that was sent through email</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 102, 208, 209</remarks>
        [OperationContract]
        int VerifyAccount(string username, string code);

        /// <summary>
        /// Send an email with the account verification code
        /// </summary>
        /// <param name="username">Username of the player to send the code</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 102, 205, 104</remarks>
        [OperationContract]
        int SendAccountVerificationCode(string username);

        /// <summary>
        /// Create a temporary account for a guest
        /// </summary>
        /// <returns>A PlayerDC object with the temporary account</returns>
        [OperationContract]
        PlayerDC AccessAsGuest();

        /// <summary>
        /// Send an email with a password recovery PIN for recovering the password
        /// </summary>
        /// <param name="email">Email of the account to send the PIN</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 104</remarks>
        [OperationContract]
        int SendPasswordRecoveryPIN(string email);

        /// <summary>
        /// Change the password of an account if the pin is correct
        /// </summary>
        /// <param name="pin">PIN that was sent to palyer's email</param>
        /// <param name="email">email of the account</param>
        /// <param name="newPassword">new password for account</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 102, 210, 211</remarks>
        [OperationContract]
        int RecoverPassword(string pin, string email, string newPassword);
    }
}
