using System.Collections.Generic;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IMainMenuServiceCallback))]
    public interface IMainMenuService
    {
        /// <summary>
        /// Retrieve friends, pending friend requests and blocked players of a player
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>0 and a list with Friend objects if the operation was success, an error code and an empty list otherwie</returns>
        /// <remarks>Error codes that can be returned: 102</remarks>
        [OperationContract]
        (int returnCode, List<FriendDC> relationshipsList) GetAllRelationships(string username);

        /// <summary>
        /// Search a player who does'nt belong to friend list, blocked list or is him self
        /// </summary>
        /// <param name="searcherUsername">Username ot the player who is searching</param>
        /// <param name="searchedUsername">Username of the player who needs to be found</param>
        /// <returns>PlayerDC object with its id, username and email if is found, an error code otherwise</returns>
        /// <remarks>Error code that can be returned: 102, 103</remarks>
        [OperationContract]
        (int returnCode, PlayerDC foundPlayer) SearchNoFriendPlayer(string searcherUsername, string searchedUsername);

        /// <summary>
        /// Send a friend request to another player
        /// </summary>
        /// <param name="senderUsername">Username of the player who sends the friend request</param>
        /// <param name="receiverUsername">Username of the player who receives the friend request</param>
        /// <returns>0 if the operation was successful, an error code otherwise </returns>
        /// <remarks> Error codes that can be returned: 101, 102, 205, 301, 302, 303, 304, 311</remarks>
        [OperationContract]
        int SendFriendRequest(string senderUsername, string receiverUsername);

        /// <summary>
        /// Respond to a friend request
        /// </summary>
        /// <param name="respondentUsername">Username of the player who is responding the request</param>
        /// <param name="requesterUsername">Username of the player who sent the friend request</param>
        /// <param name="response">Response about accept or reject the request</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 102, 305, 306, 307</remarks>
        [OperationContract]
        int RespondFriendRequest(string respondentUsername, string requesterUsername, bool response);

        /// <summary>
        /// Remove a friend from the friend list of a player
        /// </summary>
        /// <param name="username">Username of the player removing</param>
        /// <param name="friendUsername">Username of the friend being removed</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 102, 309</remarks>
        [OperationContract]
        int RemoveFriend(string username, string friendUsername);

        /// <summary>
        /// Block a player
        /// </summary>
        /// <param name="username">Username of the player blocking</param>
        /// <param name="friendUsername">Username of the player beeing blocked</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 102, 308</remarks>
        [OperationContract]
        int BlockPlayer(string username, string friendUsername);

        /// <summary>
        /// Unblock a player of the blocked plyers list
        /// </summary>
        /// <param name="username">Username of the player unblocking the other one</param>
        /// <param name="friendUsername">Username of the player who will be unblocked</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 101, 102, 310</remarks>
        [OperationContract]
        int UnblockPlayer(string username, string friendUsername);

        /// <summary>
        /// Return the statistics of a player like the number of games played, won, lost, etc.
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>0 and a PlayerStatsDC object with the statistics, an error code and null otherwise</returns>
        /// <remarks>Error codes that can be returned: 102, 205</remarks>
        [OperationContract]
        (int returnCode, PlayerStatsDC playerStats) GetPlayerProfile(string username);

        /// <summary>
        /// Return all achievements of the game an if the player has achieved them
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>0 and the list with AchievementDC objects, an error code and an empty list otherwise</returns>
        /// <remarks>Error codes that can be returned: 102</remarks>
        [OperationContract]
        (int returnCode, List<AchievementDC> achievementsList) GetAchievements(string username);

        /// <summary>
        /// Retrive the global leaderboard of the game
        /// </summary>
        /// <returns>A list with every player stats of the games</returns>
        [OperationContract]
        List<LeaderboardStatsDC> GetGlobalLeaderboard();

        /// <summary>
        /// Notify the server that a player has arrived to the main menu after login.
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>0 if the operation was successful, an error code otherwise</returns>
        /// <remarks>Error codes that can be returned: 102</remarks>
        [OperationContract]
        int ReportToServer(string username);
    }

    [ServiceContract]
    public interface IMainMenuServiceCallback
    {
        /// <summary>
        /// Receive a game invitation from another player within a game room
        /// </summary>
        /// <param name="invitation">Object with the information of the invitation</param>
        [OperationContract(IsOneWay = true)]
        void ReceiveGameInvitation(GameInvitationDC invitation);
    }
}
