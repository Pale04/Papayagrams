using System.Collections.Generic;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IMainMenuServiceCallback))]
    public interface IMainMenuService
    {
        [OperationContract]
        (int returnCode, List<FriendDC> relationshipsList) GetAllRelationships(string username);

        [OperationContract]
        (int returnCode, PlayerDC foundPlayer) SearchNoFriendPlayer(string searcherUsername, string searchedUsername);

        [OperationContract]
        int SendFriendRequest(string senderUsername, string receiverUsername);

        [OperationContract]
        int RespondFriendRequest(string respondentUsername, string requesterUsername, bool response);

        [OperationContract]
        int RemoveFriend(string username, string friendUsername);

        [OperationContract]
        int BlockPlayer(string username, string friendUsername);

        [OperationContract]
        int UnblockPlayer(string username, string friendUsername);

        [OperationContract]
        (int returnCode, PlayerStatsDC playerStats) GetPlayerProfile(string username);

        [OperationContract]
        (int returnCode, List<AchievementDC> achievementsList) GetAchievements(string username);

        [OperationContract]
        List<LeaderboardStatsDC> GetGlobalLeaderboard();

        [OperationContract]
        int ReportToServer(string username);
    }

    [ServiceContract]
    public interface IMainMenuServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveFriendRequest(PlayerDC player);

        [OperationContract(IsOneWay = true)]
        void ReceiveGameInvitation(GameInvitationDC invitation);
    }
}
