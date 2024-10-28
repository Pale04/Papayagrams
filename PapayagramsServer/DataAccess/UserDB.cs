using DomainClasses;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using LanguageExt;
using System.Data;
using System.Data.Entity.Core;
using System.Data.Entity;

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
        /// <returns>Option object with the Player</returns>
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
                        Id = playerResult.First().id,
                        Username = playerResult.First().username,
                        Email = playerResult.First().email,
                        ProfileIcon = playerResult.First().UserConfiguration.icon,
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
                        Id = playerResult.First().id,
                        Username = playerResult.First().username,
                        Email = playerResult.First().email,
                        ProfileIcon = playerResult.First().UserConfiguration.icon,
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
        /// Retrieve the player searched if exists and they are not friends
        /// </summary>
        /// <param name="searcherUsername">Username of the player who is searching</param>
        /// <param name="searchedUsername">Username of the player who needs to be found</param>
        /// <returns>Option object with the player found or None if not exist, they are friends or is himself</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static Option<Player> SearchNoFriendPlayer(string searcherUsername, string searchedUsername)
        {
            Option<Player> foundPlayer;
            using (var context = new papayagramsEntities())
            {
                var result = context.search_no_friend_player(searcherUsername, searchedUsername);
                var resultList = result.ToList();

                foundPlayer = resultList.Count == 0 ?
                    Option<Player>.None :
                    Option<Player>.Some(new Player
                    {
                        Id = resultList.First().id,
                        Username = resultList.First().username,
                        Email = resultList.First().email,
                    });
            }
            return foundPlayer;
        }

        /// <summary>
        /// Send a friend request to another player
        /// </summary>
        /// <param name="senderUsername">Username of the player who sends the friend request</param>
        /// <param name="receiverUsername">Username of the player who receives the friend request</param>
        /// <returns>0 if the sending was successful, -1 if the sender has already sent a request before, -2 if the receiver has already sent a request to sender before or -3 if they are friends</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static int SendFriendRequest(string senderUsername, string receiverUsername)
        {
            int result;
            using (var context = new papayagramsEntities())
            {
                //this approach is used because the stored procedure send_friend_request returns four different values
                SqlParameter returnValue = new SqlParameter
                {
                    ParameterName = "@ReturnValue",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                SqlParameter senderParameter = new SqlParameter("@senderUsername", senderUsername);
                SqlParameter receiverParameter = new SqlParameter("@receiverUsername", receiverUsername);
                context.Database.ExecuteSqlCommand("EXEC @ReturnValue = send_friend_request @senderUsername, @receiverUsername", returnValue, senderParameter, receiverParameter);
                result = (int)returnValue.Value;
            }
            return result;
        }
    }
}
