using System.Collections.Generic;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IMainMenuServiceCallback))]
    public interface IMainMenuService
    {
        [OperationContract]
        (int, List<FriendDC>) GetFriends(string username);

        [OperationContract]
        int RemoveFriend(string username, string friendUsername);

        [OperationContract]
        int BlockFriend(string username, string friendUsername);

        [OperationContract]
        (int, List<FriendDC>) GetFriendRequests(string username);

        [OperationContract]
        int AcceptFriendRequest(string username, string friendUsername);

        [OperationContract]
        int RejectFriendRequest(string username, string friendUsername);

        [OperationContract]
        (int, PlayerDC) SearchNoFriendPlayer(string searcherUsername, string searchedUsername);

        [OperationContract]
        int SendFriendRequest(string senderUsername, string receiverUsername);

        //TODO: implementar un objeto serializado para recuperar el perfil completo del usuario
        [OperationContract]
        int GetPlayerProfile(string username);

        [OperationContract]
        (int, List<AchievementDC>) GetAchievements(string username);

        //TODO: implementar un objeto serializado para recuperar el leaderboard
        [OperationContract]
        int GetLeaderboard(PlayerDC player);

        [OperationContract]
        void ReportToServer(string username);
    }

    [ServiceContract]
    public interface IMainMenuServiceCallback
    {
        [OperationContract]
        void ReceiveFriendRequest(PlayerDC player);
        [OperationContract]
        void ReceiveGameInvitation(GameInvitationDC invitation);
    }
}
