using DomainClasses;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using LanguageExt;

namespace DataAccess
{
    public class UserDB
    {
        /// <summary>
        /// Register a new player in the database
        /// </summary>
        /// <param name="player">Player object with his information</param>
        /// <returns>1 if the registration was successful, 0 otherwise</returns>
        public static int RegisterUser(Player player)
        {
            int result = 0;
            using (var context = new papayagramsEntities())
            {
                //The stored procedure register_user that provide the PapayagramsModel does not return the number of rows affected and the ExecuteSqlCommand method does
                SqlParameter usernameParameter = new SqlParameter("@param1", player.Username);
                SqlParameter emailParameter = new SqlParameter("@param2", player.Email);
                SqlParameter passwordParameter = new SqlParameter("@param3", player.Password);
                result = context.Database.ExecuteSqlCommand("EXEC register_user @param1, @param2, @param3", usernameParameter, emailParameter, passwordParameter);
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

        public static void recordUserLogIn()
        {
            //TODO: Implement recordUserLogIn method
        }

        public static void recordUserLogOut()
        {
            //TODO: Implement recordUserLogOut method
        }
    }
}
