using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface IApplicationSettingsService
    {
        [OperationContract]
        int UpdateAplicationSettings(string username, ApplicationSettingsDC updatedConfiguration);

        [OperationContract]
        (int returnCode, ApplicationSettingsDC configuration) GetApplicationSettings(string username);

        [OperationContract]
        int UpdateProfileIcon(string username, int profileIcon);

        [OperationContract]
        int UpdatePassword(string username, string currentPassword, string newPassword);
    }
}
