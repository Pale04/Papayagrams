using DomainClasses;
using LanguageExt;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace DataAccess
{
    public class UserRelationshipDB
    {
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
        /// <returns>0 if the sending was successful, -1 if the sender has already sent a request before, -2 if the receiver has already sent a request to sender before, -3 if they are friends and -4 if the relationship is blocked</returns>
        /// <exception cref="EntityException">When it cannot establish connection with the database server</exception>
        public static int SendFriendRequest(string senderUsername, string receiverUsername)
        {
            int result;
            using (var context = new papayagramsEntities())
            {
                //this approach is used because the stored procedure send_friend_request returns five different values
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
