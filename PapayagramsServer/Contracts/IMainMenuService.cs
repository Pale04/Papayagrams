using System.Collections.Generic;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IMainMenuServiceCallback))]
    public interface IMainMenuService
    {
        [OperationContract]
        (int returnCode, List<FriendDC> friendsList) GetFriends(string username);

        [OperationContract]
        int RemoveFriend(string username, string friendUsername);

        [OperationContract]
        int BlockFriend(string username, string friendUsername);

        [OperationContract]
        (int returnCode, List<FriendDC> friendRequestsList) GetFriendRequests(string username);

        [OperationContract]
        int AcceptFriendRequest(string username, string friendUsername);

        [OperationContract]
        int RejectFriendRequest(string username, string friendUsername);

        [OperationContract]
        (int returnCode, PlayerDC playersList) SearchNoFriendPlayer(string searcherUsername, string searchedUsername);

        [OperationContract]
        int SendFriendRequest(string senderUsername, string receiverUsername);

        [OperationContract]
        (int returnCode, PlayerStatsDC playerStats) GetPlayerProfile(string username);

        [OperationContract]
        (int returnCode, List<AchievementDC> achievementsList) GetAchievements(string username);

        //TODO: crear una clase serializada para recuperar el leaderboard
        [OperationContract]
        int GetLeaderboard(PlayerDC player);

        [OperationContract]
        int ReportToServer(string username);
    }

    [ServiceContract]
    public interface IMainMenuServiceCallback
    {
        [OperationContract]
        void ReceiveFriendRequest(PlayerDC player);
        [OperationContract(IsOneWay = true)]
        void ReceiveGameInvitation(GameInvitationDC invitation);
    }
}
