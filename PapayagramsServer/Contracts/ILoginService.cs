using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface ILoginService
    {
        [OperationContract]
        int RegisterUser(PlayerDC player);

        [OperationContract]
        (int errorCode, PlayerDC loggedPlayer) Login(string username, string password);

        [OperationContract]
        int Logout(string username);

        [OperationContract]
        int VerifyAccount(string username, string code);

        [OperationContract]
        int SendAccountVerificationCode(string username);

        [OperationContract]
        PlayerDC AccessAsGuest();

        [OperationContract]
        int SendPasswordRecoveryPIN(string email);

        [OperationContract]
        int RecoverPassword(string pin, string email, string newPassword);
    }
}
