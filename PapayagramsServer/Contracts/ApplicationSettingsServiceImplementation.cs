using DataAccess;
using DomainClasses;
using LanguageExt;
using System;
using System.Data;

namespace Contracts
{
    public partial class ServiceImplementation : IApplicationSettingsService
    {
        /// <summary>
        /// Retrieve the application settings of a player
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>0 and the Settings object with details, an error code and null otherwise</returns>
        /// <remarks>Error codes that can be returned: 102, 205</remarks>
        public (int returnCode, ApplicationSettingsDC configuration) GetApplicationSettings(string username)
        {
            Option<ApplicationSettings> wrappedConfiguration;
            try
            {
                wrappedConfiguration = UserDB.GetApplicationSettings(username);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                return (102, null);
            }

            if (wrappedConfiguration.IsSome)
            {
                return (0, ApplicationSettingsDC.ConvertToApplicationSettingsDC((ApplicationSettings)wrappedConfiguration.Case));
            }
            else
            {
                _logger.WarnFormat("Application settings retrieval failed (Username: {0})", username);
                return (205, null);
            }
        }

        /// <summary>
        /// Update the application settings of a player
        /// </summary>
        /// <param name="username">Username of the player updating settings</param>
        /// <param name="updatedConfiguration">Updated configuration of the application</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 102, 501</remarks>
        public int UpdateAplicationSettings(string username, ApplicationSettingsDC updatedConfiguration)
        {
            if (string.IsNullOrEmpty(username) || updatedConfiguration == null)
            {
                _logger.WarnFormat("UpdateAplicationSettings method called with invalid parameters (Username: {0})", username);
                return 101;
            }

            int operationResult;
            try
            {
                operationResult = UserDB.UpdateApplicationSettings(username, new ApplicationSettings()
                {
                    PieceColor = updatedConfiguration.PieceColor,
                    SelectedLanguage = (ApplicationLanguage)Enum.Parse(typeof(ApplicationLanguage), updatedConfiguration.SelectedLanguage.ToString()),
                    Cursor = updatedConfiguration.Cursor
                });
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                return 102;
            }

            if (operationResult == 0)
            {
                _logger.WarnFormat("Application settings update failed (Username: {0})", username);
                return 501;
            }

            return 0;
        }

        /// <summary>
        /// Update the passward of a player in the database
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <param name="currentPassword">Current password of the player</param>
        /// <param name="newPasswordord">New password for the player´s account</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 102, 503</remarks>
        public int UpdatePassword(string username, string currentPassword, string newPasswordord)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPasswordord))
            {
                return 101;
            }

            int operationResult;
            try
            {
                operationResult = UserDB.UpdatePassword(username, currentPassword, newPasswordord);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                return 102;
            }

            if (operationResult == -1)
            {
                _logger.InfoFormat("Password update failed (Username: {0})", username);
                return 503;
            }
            return 0;
        }

        /// <summary>
        /// Change the profile icon of a player in the database
        /// </summary>
        /// <param name="username">Username of the player updating</param>
        /// <param name="profileIcon">Icon id to update</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 102, 502</remarks>
        public int UpdateProfileIcon(string username, int profileIcon)
        {
            if (string.IsNullOrEmpty(username) || profileIcon < 1)
            {
                _logger.WarnFormat("UpdateProfileIcon method called with invalid parameters (Username: {0}, ProfileIcon: {1})", username, profileIcon);
                return 101;
            }

            int operationResult;
            try
            {
                operationResult = UserDB.UpdateProfileIcon(username, profileIcon);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                return 102;
            }

            if (operationResult == -1)
            {
                _logger.WarnFormat("Profile icon update failed (Username: {0}, ProfileIcon: {1})", username, profileIcon);
                return 502;
            }

            return 0;
        }
    }
}
