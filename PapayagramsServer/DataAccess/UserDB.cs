using DomainClasses;
using System.Data.SqlClient;
using System.Linq;
using LanguageExt;
using System.Data;
using System.Data.Entity.Core;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;

namespace DataAccess
{
    public static class UserDB
    {
        /// <summary>
        /// Register a new player in the database
        /// </summary>
        /// <param name="player">Player object with his information</param>
        /// <returns>6 if the registration was successful</returns>
        /// <exception cref="EntityCommandExecutionException">When the Player has not initialized his attributes</exception>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static int RegisterUser(Player player)
        {
            int result = 0;
            using (var context = new papayagramsEntities())
            {
                result = context.register_user(player.Username, player.Email, player.Password);
            }
            return result;
        }

        /// <summary>
        /// Log in a user in Papayagrams and update his status to online
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <param name="password">Account password of the user</param>
        /// <returns>0 if the login was succesful, 1 if the account is pending to verify, -1 if the account does not exist and -2 if the password is incorrect</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static int LogIn(string username, string password)
        {
            int result;
            using (var context = new papayagramsEntities())
            {
                //this approach is used because the stored procedure log_in returns four different values
                SqlParameter returnValue = new SqlParameter
                {
                    ParameterName = "@ReturnValue",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                SqlParameter usernameParam = new SqlParameter("@username", username);
                SqlParameter passwordParam = new SqlParameter("@password", password);
                context.Database.ExecuteSqlCommand("EXEC @ReturnValue = log_in @username, @password", returnValue, usernameParam, passwordParam);
                result = (int)returnValue.Value;
            }
            return result;
        }

        /// <summary>
        /// Change the account status of the player to verified
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>1 if the change was succesful, 0 otherwise</returns>
        public static int VerifyAccount(string username)
        {
            int result = 0;
            using (var context = new papayagramsEntities())
            {
                var player = context.User.FirstOrDefault(p => p.username == username);
                if (player != null)
                {
                    player.accountStatus = "verified";
                    result = context.SaveChanges();
                }
            }
            return result;
        }

        /// <summary>
        /// Search for a user in the database
        /// </summary>
        /// <param name="username">Username of the user's account</param>
        /// <returns>Option object with the Player and his id, username, email and profile icon</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static Option<Player> GetPlayerByUsername(string username)
        {
            Option<Player> optionPlayer;

            using (var context = new papayagramsEntities())
            {
                var playerResult = context.User.Where(p => p.username == username).Include(p => p.UserConfiguration).ToList();

                optionPlayer = playerResult.Count == 0 ?
                    Option<Player>.None :
                    Option<Player>.Some(new Player
                    {
                        Id = playerResult[0].id,
                        Username = playerResult[0].username,
                        Email = playerResult[0].email,
                        ProfileIcon = playerResult[0].UserConfiguration.icon,
                    });
            }

            return optionPlayer;
        }

        /// <summary>
        /// Search for a player in the database by his email
        /// </summary>
        /// <param name="email">Email of the user's account</param>
        /// <returns>Option object with the Player</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static Option<Player> GetPlayerByEmail(string email)
        {
            Option<Player> optionPlayer;

            using (var context = new papayagramsEntities())
            {
                var playerResult = context.User.Where(p => p.email == email).Include(p => p.UserConfiguration).ToList();
                optionPlayer = playerResult.Count == 0 ?
                    Option<Player>.None :
                    Option<Player>.Some(new Player
                    {
                        Id = playerResult[0].id,
                        Username = playerResult[0].username,
                        Email = playerResult[0].email,
                        ProfileIcon = playerResult[0].UserConfiguration.icon,
                    });
            }

            return optionPlayer;
        }

        /// <summary>
        /// Change the player status to offline
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>1 if the operation was succesful, 0 otherwise</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static int LogOut(string username)
        {
            int result;
            using (var context = new papayagramsEntities())
            {
                result = context.log_out(username);
            }
            return result;
        }

        /// <summary>
        /// Update the connection status of the player
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <param name="status">Status of the player</param>
        /// <returns>1 if the update was successful, 0 otherwise </returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static int UpdateUserStatus (string username, PlayerStatus status)
        {
            int result = 0;
            using (var context = new papayagramsEntities())
            {
                var player = context.User.Where(p => p.username == username).Include(p => p.UserStatus);
                if (player.Any()) 
                {
                    player.First().UserStatus.status = status.ToString();
                    player.First().UserStatus.since = DateTime.Now;
                    result = context.SaveChanges();
                }
            }
            return result;
        }

        /// <summary>
        /// Obtains all existing achievements and every one indicates if the player has achieved it
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>A list with all the achievements if the operation was successful, an empty list otherwise.</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static List<DomainClasses.Achievement> GetPlayerAchievements (string username)
        {
            List<DomainClasses.Achievement> achievementsList = new List<DomainClasses.Achievement>();
            using (var context = new papayagramsEntities())
            {
                var achievementsResult = context.Achievement;
                if (achievementsResult.Any())
                {
                    foreach (var achievement in achievementsResult)
                    {
                        achievementsList.Add(new DomainClasses.Achievement
                        {
                            Id = achievement.id,
                            Description = achievement.description
                        });
                    }
                }

                var userResult = context.User.Where(p => p.username == username).Include(p => p.UserAchieved);
                if (userResult.Any())
                {
                    foreach (var userAchievement in userResult.First().UserAchieved)
                    {
                        DomainClasses.Achievement achievementFound = achievementsList.Find(a => a.Id == userAchievement.achievementId);
                        if (achievementFound != null)
                        {
                            achievementFound.IsAchieved = true;
                        }
                    }
                }
            }
            return achievementsList;
        }

        /// <summary>
        /// Retrieves the player's status
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>Status of the player, offline if the player does'nt exist</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static PlayerStatus GetPlayerStatus(string username)
        {
            PlayerStatus status = PlayerStatus.offline;
            using (var context = new papayagramsEntities())
            {
                var player = context.User.Where(p => p.username == username).Include(p => p.UserStatus);
                if (player.Any())
                {
                    status = (PlayerStatus)Enum.Parse(typeof(PlayerStatus), player.First().UserStatus.status);
                }
            }
            return status;
        }

        /// <summary>
        /// Update the application settings of the player
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <param name="configuration">Updated configurations</param>
        /// <returns>1 if the operation was successful, 0 otherwise</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static int UpdateApplicationSettings(string username, ApplicationSettings configuration)
        {
            int result = 0;
            using (var context = new papayagramsEntities())
            {
                var player = context.User.Where(p => p.username == username).Include(p => p.UserConfiguration).FirstOrDefault();
                if (player != null)
                {
                    player.UserConfiguration.pieceColor = configuration.PieceColor;
                    player.UserConfiguration.selectedLanguage = configuration.SelectedLanguage.ToString();
                    player.UserConfiguration.cursorDesign = configuration.Cursor;
                    result = context.SaveChanges();
                }
            }
            return result;
        }

        /// <summary>
        /// Retrieve the application settings of a player
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>Option object with the Settings object, an empty option object if the user does´nt exist</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static Option<ApplicationSettings> GetApplicationSettings(string username)
        {
            Option<ApplicationSettings> wrappedSettings;
            using (var context = new papayagramsEntities())
            {
                var player = context.User.Where(p => p.username == username).Include(p => p.UserConfiguration).FirstOrDefault();
                wrappedSettings = player == null ? Option<ApplicationSettings>.None : Option<ApplicationSettings>.Some(new ApplicationSettings()
                {
                    PieceColor = (int)player.UserConfiguration.pieceColor,
                    SelectedLanguage = (ApplicationLanguage)Enum.Parse(typeof(ApplicationLanguage), player.UserConfiguration.selectedLanguage),
                    Cursor = (int)player.UserConfiguration.cursorDesign
                });
            }
            return wrappedSettings;
        }

        /// <summary>
        /// Update the profile icon of the player in the database
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <param name="icon">Icon id to update</param>
        /// <returns>1 if the operation was successfull, 0 if the icon is the same ,-1 if the user does´nt exist</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static int UpdateProfileIcon(string username, int icon)
        {
            int result = 0;
            using (var context = new papayagramsEntities())
            {
                var player = context.User.Where(p => p.username == username).Include(p => p.UserConfiguration).FirstOrDefault();
                if (player != null)
                {
                    player.UserConfiguration.icon = icon;
                    result = context.SaveChanges();
                }
                else
                {
                    result = -1;
                }
            }
            return result;
        }

        /// <summary>
        /// Update the password of a player account in the database
        /// </summary>
        /// <param name="username">Userame of the player changing his password</param>
        /// <param name="currentPassword">Currente password of the account</param>
        /// <param name="newPassword">New password of the account</param>
        /// <returns>0 if the operation was successful, -1 if the currentPassword is wrong</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static int UpdatePassword(string username, string currentPassword, string newPassword)
        {
            int result;
            using (var context = new papayagramsEntities())
            {
                SqlParameter usernameParameter = new SqlParameter("@username", username);
                SqlParameter currentPasswordParameter = new SqlParameter("@currentPassword", currentPassword);
                SqlParameter newPasswordParameter = new SqlParameter("@newPassword", newPassword);
                SqlParameter returnParameter = new SqlParameter
                {
                    ParameterName = "@ReturnValue",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                context.Database.ExecuteSqlCommand("EXEC @ReturnValue = update_password @username, @currentPassword, @newPassword", returnParameter, usernameParameter, currentPasswordParameter, newPasswordParameter);
                result = (int)returnParameter.Value;
            }
            return result;
        }

        /// <summary>
        /// Update the password of a player account in the database, without checking the current password
        /// </summary>
        /// <param name="email">email of the player's account</param>
        /// <param name="newPassword">New password for the player's account</param>
        /// <returns>1 if the operation was successful, 0 otherwise</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static int UpdatePassword(string email, string newPassword)
        {
            int result;
            using (var context = new papayagramsEntities())
            {
                var player = context.User.FirstOrDefault(p => p.email == email);
                SqlParameter usernameParameter = new SqlParameter("@username", player.username);
                SqlParameter newPasswordParameter = new SqlParameter("@newPassword", newPassword);
                result = context.Database.ExecuteSqlCommand("EXEC update_password_no_verification @username, @newPassword", usernameParameter, newPasswordParameter);
            }
            return result;
        }
    }
}
