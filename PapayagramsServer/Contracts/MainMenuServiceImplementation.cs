using BussinessLogic;
using DataAccess;
using DomainClasses;
using LanguageExt;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.ServiceModel;

namespace Contracts
{
    public partial class ServiceImplementation : IMainMenuService
    {
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
                case 0:
                    return 0;
                case -1:
                    return 301;
                case -2:
                    return 302;
                case -3:
                    return 303;
                case -4:
                    return 304;
                default:
                    return -1;
            }
        }

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
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(friendUsername))
            {
                return 101;
            }
            
            int result;
            try
            {
                result = UserRelationshipDB.RemoveFriend(username, friendUsername);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                return 102;
            }

            if (result != 1)
            {
                _logger.WarnFormat("Friend removal failed (Username: {0}, Friend username: {1}, Return code: {2})", username, friendUsername, result);
                return 309;
            }
            return 0;
        }

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

            if (result == 0)
            {
                _logger.WarnFormat("Player block failed (Blocker username: {0}, Blocked username: {1})", username, friendUsername);
            }
            return result > 0? 0 : 308;
        }

        public int UnblockPlayer(string username, string friendUsername)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(friendUsername))
            {
                return 101;
            }

            int result;
            try
            {
                result = UserRelationshipDB.UnblockPlayer(username, friendUsername);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                return 102;
            }

            if (result != 1)
            {
                _logger.WarnFormat("Unblocking player failed (Player unblocking: {0}, Blocked username: {1}, return code: {2})", username, friendUsername, result);
            }
            return result == 1 ? 0 : 310;
        }

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

        public List<LeaderboardStatsDC> GetGlobalLeaderboard()
        {
            List<LeaderboardStats> globalLeaderboardStats = new List<LeaderboardStats>();
            try
            {
                globalLeaderboardStats = GameHistoryDB.GetGlobalLeaderboard();
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
            }
            return globalLeaderboardStats.ConvertAll(LeaderboardStatsDC.ConvertToLeaderboardStatsDC);
        }

        public (int returnCode, PlayerStatsDC playerStats) GetPlayerProfile(string username)
        {
            Option<PlayerStats> playerStats;

            try
            {
                playerStats = GameHistoryDB.GetPlayerStats(username); 
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                return (102, null);
            }

            return playerStats.IsSome? (0, PlayerStatsDC.ConvertToPlayerStatsDC((PlayerStats)playerStats.Case)) : (205, null);
        }

        public int ReportToServer(string username)
        {
            try
            {
                UserDB.UpdateUserStatus(username, PlayerStatus.online);
            }
            catch (EntityException error)
            {
                _logger.Fatal("Database connection failed", error);
                _logger.WarnFormat("Player status update failed (Username: {0}, To status: {1})", username, PlayerStatus.online.ToString());
                return 102;
            }

            CallbacksPool.PlayerArrivesToMainMenu(username,OperationContext.Current.GetCallbackChannel<IMainMenuServiceCallback>());
            return 0;
        }
    }
}
