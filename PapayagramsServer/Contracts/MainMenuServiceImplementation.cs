using BussinessLogic;
using DataAccess;
using DomainClasses;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.ServiceModel;

namespace Contracts
{
    public partial class ServiceImplementation : IMainMenuService
    {
        /// <summary>
        /// Retrieve friends, pending friend requests and blocked players of a player
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>0 and a list with Friend objects if the operation was success, an error code and an empty list otherwie</returns>
        /// <remarks>Error codes that can be returned: 102</remarks>
        public (int returnCode, List<FriendDC> relationshipsList) GetAllRelationships(string username)
        {
            List<Friend> relationshipsList;

            try
            {
                relationshipsList = UserRelationshipDB.GetFriends(username);
                relationshipsList = relationshipsList.Concat(UserRelationshipDB.GetPendingFriendRequests(username)).ToList();
                relationshipsList = relationshipsList.Concat(UserRelationshipDB.GetBlockedPlayers(username)).ToList();
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                return (102, null);
            }

            return (0, relationshipsList.ConvertAll(FriendDC.ConvertToFriendDC));
        }

        /// <summary>
        /// Search a player who does'nt belong to friend list and blocked list of the searcher
        /// </summary>
        /// <param name="searcherUsername">Username ot the player who is searching</param>
        /// <param name="searchedUsername">Username of the player who needs to be found</param>
        /// <returns>PlayerDC object with its id, username and email if is found, an error code otherwise</returns>
        /// <remarks>Error code that can be returned: 102, 103</remarks>
        public (int returnCode, PlayerDC foundPlayer) SearchNoFriendPlayer(string searcherUsername, string searchedUsername)
        {
            Option<Player> wrappedPlayer;

            try
            {
                wrappedPlayer = UserRelationshipDB.SearchNoFriendPlayer(searcherUsername, searchedUsername);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                return (102, null);
            }

            return wrappedPlayer.IsSome ? (0, PlayerDC.ConvertToPlayerDC((Player)wrappedPlayer.Case)) : (103, null);
        }

        /// <summary>
        /// Send a friend request to another player
        /// </summary>
        /// <param name="senderUsername">Username of the player who sends the friend request</param>
        /// <param name="receiverUsername">Username of the player who receives the friend request</param>
        /// <returns>0 if the operation was successful, an error code otherwise </returns>
        /// <remarks> Error codes that can be returned: 101, 102, 301, 302, 303, 304 </remarks>
        public int SendFriendRequest(string senderUsername, string receiverUsername)
        {
            if (string.IsNullOrEmpty(senderUsername))
            {
                return 101;
            }

            int result;
            try
            {
                result = UserRelationshipDB.SendFriendRequest(senderUsername, receiverUsername);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                return 102;
            }

            switch (result)
            {
                case -1:
                    return 301;
                case -2:
                    return 302;
                case -3:
                    return 303;
                case -4:
                    return 304;
                default:
                    return result;
            }
        }

        /// <summary>
        /// Respond to a friend request
        /// </summary>
        /// <param name="respondentUsername">Username of the player who is responding the request</param>
        /// <param name="requesterUsername">Username of the player who sent the friend request</param>
        /// <param name="response">Response about accept or reject the request</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 102, 305, 306, 307</remarks>
        public int RespondFriendRequest(string respondentUsername, string requesterUsername, bool response)
        {
            if (string.IsNullOrEmpty(respondentUsername) || string.IsNullOrEmpty(requesterUsername))
            {
                return 101;
            }

            int result;
            try
            {
                result = UserRelationshipDB.RespondFriendRequest(respondentUsername, requesterUsername, response);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                return 102;
            }

            switch (result)
            {
                case 0:
                    return 305;
                case -1:
                    return 306;
                case -2:
                    return 307;
                default:
                    return 0;
            }
        }

        public int RemoveFriend(string username, string friendUsername)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Block a player
        /// </summary>
        /// <param name="username">Username of the player blocking</param>
        /// <param name="friendUsername">Username of the player beeing blocked</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 102, 308</remarks>
        public int BlockPlayer(string username, string friendUsername)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(friendUsername))
            {
                return 101;
            }

            int result;
            try
            {
                result = UserRelationshipDB.BlockPlayer(username, friendUsername);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                return 102;
            }

            if (result != 1)
            {
                //_logger.InfoFormat("Player block failed (Blocker username: {username}, Blocked username: {friendUsername}, Return code: {result})", username, friendUsername, result);
            }
            return result == 1? 0 : 308;
        }

        public int UnblockFriend(string username, string friendUsername)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return all achievements of the game an if the player has achieved them
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>0 and the list with AchievementDC objects, an error code and an empty list otherwise</returns>
        /// <remarks>Error codes that can be returned: 102</remarks>
        public (int, List<AchievementDC>) GetAchievements(string username)
        {
            List<DomainClasses.Achievement> achievementsList;

            try
            {
                achievementsList = UserDB.GetPlayerAchievements(username);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connnection failed", error);
                return (102, null);
            }

            return (0, achievementsList.ConvertAll(AchievementDC.ConvertToAchievementDC));
        }

        public int GetLeaderboard(PlayerDC player)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return the statistics of a player like the number of games played, won, lost, etc.
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>0 and a PlayerStatsDC object with the statistics, an error code and null otherwise</returns>
        /// <remarks>Error codes that can be returned: 102, 205</remarks>
        public (int returnCode, PlayerStatsDC playerStats) GetPlayerProfile(string username)
        {
            Option<PlayerStats> playerStats;

            try
            {
                playerStats = UserDB.GetPlayerStats(username); 
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                return (102, null);
            }

            return playerStats.IsSome? (0, PlayerStatsDC.ConvertToPlayerStatsDC((PlayerStats)playerStats.Case)) : (205, null);
        }

        /// <summary>
        /// Add the callback channel of the player to the callbacks pool
        /// </summary>
        /// <param name="username">Username of the player</param>
        public int ReportToServer(string username)
        {
            try
            {
                UserDB.UpdateUserStatus(username, PlayerStatus.online);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                return 102;
            }

            CallbacksPool.PlayerArrivesToMainMenu(username,OperationContext.Current.GetCallbackChannel<IMainMenuServiceCallback>());
            return 0;
        }
    }
}
