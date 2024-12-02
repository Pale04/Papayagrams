using DomainClasses;
using LanguageExt;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace DataAccess
{
    public static class UserRelationshipDB
    {
        /// <summary>
        /// Retrieve the player searched if exists, is not friend and is not blocked
        /// </summary>
        /// <param name="searcherUsername">Username of the player who is searching</param>
        /// <param name="searchedUsername">Username of the player who needs to be found</param>
        /// <returns>Option object with the player found, Option None object if does'nt exist, is friend, is himself or is blocked</returns>
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
                        Id = resultList[0].id,
                        Username = resultList[0].username,
                        Email = resultList[0].email,
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

        /// <summary>
        /// Accept or reject a friend request
        /// </summary>
        /// <param name="evaluatingPlayerUsername">username of the player who is responding the request</param>
        /// <param name="requestingPlayerUsername">username of the player who sent the request</param>
        /// <param name="acceptRequest">Response of the player</param>
        /// <returns>1 if the operation was successful, 0 if no request exists, -1 if evaluator not found an -2 if requester not found</returns>
        public static int RespondFriendRequest(string evaluatingPlayerUsername, string requestingPlayerUsername, bool acceptRequest)
        {
            int result = 0;
            using (var context = new papayagramsEntities())
            {
                var evaluatingPlayer = context.User.Where(p => p.username == evaluatingPlayerUsername).Include(p => p.UserRelationship).FirstOrDefault();
                var requestingPlayer = context.User.Where(p => p.username == requestingPlayerUsername).FirstOrDefault();

                if (evaluatingPlayer == null)
                {
                    return -1;
                }
                else if (requestingPlayer == null)
                {
                    return -2;
                }

                var request = evaluatingPlayer.UserRelationship.Where(r => r.senderId == requestingPlayer.id && r.relationState.Equals("request_pending")).FirstOrDefault();
                if (request != null)
                {
                    if (acceptRequest)
                    {
                        request.relationState = "friend";
                    }
                    else
                    {
                        context.UserRelationship.Remove(request);
                    }
                    result = context.SaveChanges();
                }
            }
            return result;
        }

        /// <summary>
        /// Return friends of a player
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>A list with Friend objects</returns>
        public static List<Friend> GetFriends(string username)
        {
            List<Friend> friends = new List<Friend>();
            using (var context = new papayagramsEntities())
            {
                var player = context.User.Where(p => p.username == username).Include(p => p.UserRelationship).Include(p => p.UserRelationship1).FirstOrDefault();
                if (player != null)
                {
                    var friendsAdded = player.UserRelationship1.Where(r => r.relationState.Equals("friend"));
                    var friendsAccepted = player.UserRelationship.Where(r => r.relationState.Equals("friend"));
                    foreach (var friend in friendsAdded.Concat(friendsAccepted))
                    {
                        int friendId = (int)(friend.receiverId == player.id ? friend.senderId : friend.receiverId);
                        var friendData = context.User.Where(p => p.id == friendId).First();
                        friends.Add(new Friend
                        {
                            Id = friendData.id,
                            Username = friendData.username,
                            RelationState = RelationState.Friend
                        });
                    }
                }
            }
            return friends;
        }

        /// <summary>
        /// Return pending friend requests of a player
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>A list with Friend objects</returns>
        public static List<Friend> GetPendingFriendRequests(string username)
        {
            List<Friend> pendingRequestsList = new List<Friend>();
            using (var context = new papayagramsEntities())
            {
                var player = context.User.Where(p => p.username == username).Include(p => p.UserRelationship).FirstOrDefault();
                if (player != null)
                {
                    var pendingRequests = player.UserRelationship.Where(r => r.relationState.Equals("request_pending"));
                    foreach (var request in pendingRequests)
                    {
                        var requester = context.User.Where(p => p.id == request.senderId).First();
                        pendingRequestsList.Add(new Friend
                        {
                            Id = requester.id,
                            Username = requester.username,
                            RelationState = RelationState.Pending
                        });
                    }
                }
            }
            return pendingRequestsList;
        }

        /// <summary>
        /// Return players who have been blocked by a player
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>A list with Friend objects</returns>
        public static List<Friend> GetBlockedPlayers(string username)
        {
            List<Friend> blockedPlayersList = new List<Friend>();
            using (var context = new papayagramsEntities())
            {
                var player = context.User.Where(p => p.username == username).Include(p => p.UserRelationship1).FirstOrDefault();
                if (player != null)
                {
                    var blockedPlayers = player.UserRelationship1.Where(r => r.relationState.Equals("blocked"));
                    foreach (var blockedPlayer in blockedPlayers)
                    {
                        var blockedPlayerData = context.User.Where(p => p.id == blockedPlayer.receiverId).First();
                        blockedPlayersList.Add(new Friend
                        {
                            Id = blockedPlayerData.id,
                            Username = blockedPlayerData.username,
                            RelationState = RelationState.Blocked
                        });
                    }
                }
            }
            return blockedPlayersList;
        }

        public static int BlockPlayer(string username, string blockedUsername)
        {
            //TODO: el que bloquea ahora se vuelve sender y el bloqueado se vuelve receiver. Todo
            // para que sirva el método "GetBlockedPlayers"
            return 0;
        }
    }
}
