using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface IApplicationSettingsService
    {
        /// <summary>
        /// Update the application settings of a player
        /// </summary>
        /// <param name="username">Username of the player updating settings</param>
        /// <param name="updatedConfiguration">Updated configuration of the application</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 102, 501</remarks>
        [OperationContract]
        int UpdateAplicationSettings(string username, ApplicationSettingsDC updatedConfiguration);

        /// <summary>
        /// Retrieve the application settings of a player
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>0 and the Settings object with details, an error code and null otherwise</returns>
        /// <remarks>Error codes that can be returned: 102, 205</remarks>
        [OperationContract]
        (int returnCode, ApplicationSettingsDC configuration) GetApplicationSettings(string username);

        /// <summary>
        /// Change the profile icon of a player in the database
        /// </summary>
        /// <param name="username">Username of the player updating</param>
        /// <param name="profileIcon">Icon id to update</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 102, 502</remarks>
        [OperationContract]
        int UpdateProfileIcon(string username, int profileIcon);

        /// <summary>
        /// Update the passward of a player in the database
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <param name="currentPassword">Current password of the player</param>
        /// <param name="newPasswordord">New password for the player´s account</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 102, 503</remarks>
        [OperationContract]
        int UpdatePassword(string username, string currentPassword, string newPassword);
    }
}
