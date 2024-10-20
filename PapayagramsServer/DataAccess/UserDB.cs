using DomainClasses;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using LanguageExt;
using System.Data;
using System;

namespace DataAccess
{
    public class UserDB
    {
        /// <summary>
        /// Register a new player in the database
        /// </summary>
        /// <param name="player">Player object with his information</param>
        /// <returns>2 if the registration was successful, EntityCommandExecutionException otherwise </returns>
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
        /// Log in a user in Papayagrams
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <param name="password">Account password of the user</param>
        /// <returns>0 if the login was succesful, 1 if the password is incorrect and -1 if the account does not exist</returns>
        public static int LogIn(string username, string password)
        {
            int result;
            using (var context = new papayagramsEntities())
            {
                //this approach is used because the stored procedure log_in returns three different values
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
        /// Search for a user in the database
        /// </summary>
        /// <param name="username">Username of the user's account</param>
        /// <returns>Option object with the Player</returns>
        public static Option<Player> GetPlayerByUsername(string username)
        {
            Option<Player> optionPlayer;

            using (var context = new papayagramsEntities())
            {
                var playerResult = context.get_player_by_username(username);
                List<get_player_by_username_Result> playerResultList = playerResult.ToList();

                optionPlayer = playerResultList.Count == 0 ?
                    Option<Player>.None :
                    Option<Player>.Some(new Player
                    {
                        Id = playerResultList.First().id,
                        Username = playerResultList.First().username,
                        Email = playerResultList.First().email,
                        Password = playerResultList.First().password
                    });
            }

            return optionPlayer;
        }

        public static Option<Player> GetPlayerByEmail(string email)
        {
            Option<Player> optionPlayer;

            using (var context = new papayagramsEntities())
            {
                var playerResult = context.get_player_by_email(email);
                List<get_player_by_email_Result> playerResultList = playerResult.ToList();

                optionPlayer = playerResultList.Count == 0 ?
                    Option<Player>.None :
                    Option<Player>.Some(new Player
                    {
                        Id = playerResultList.First().id,
                        Username = playerResultList.First().username,
                        Email = playerResultList.First().email,
                        Password = playerResultList.First().password
                    });
            }

            return optionPlayer;
        }

        public static int UpdateUserStatus(string username, PlayerStatus status)
        {
            int result;
            using (var context = new papayagramsEntities())
            {
                result = context.update_user_status(username, PlayerStatus.online.ToString(), DateTime.Now);
            }
            return result;
        }
    }
}
