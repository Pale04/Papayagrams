using DomainClasses;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using LanguageExt;
using System.Data;
using System.Data.Entity.Core;

namespace DataAccess
{
    public class UserDB
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
        /// <returns>0 if the login was succesful, -1 if the account does not exist and -2 if the password is incorrect</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
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
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
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
    }
}
