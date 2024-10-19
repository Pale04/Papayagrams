using System.Collections.Generic;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IMainMenuServiceCallback))]
    public interface IMainMenuService
    {
        [OperationContract]
        [FaultContract(typeof(ServerException))]
        List<FriendDC> GetFriends(string username);

        [OperationContract]
        [FaultContract(typeof(ServerException))]
        int RemoveFriend(string username, string friendUsername);

        [OperationContract]
        [FaultContract(typeof(ServerException))]
        int BlockFriend(string username, string friendUsername);

        [OperationContract]
        [FaultContract(typeof(ServerException))]
        List<FriendDC> GetFriendRequests(string username);

        [OperationContract]
        [FaultContract(typeof(ServerException))]
        int AcceptFriendRequest(string username, string friendUsername);

        [OperationContract]
        [FaultContract(typeof(ServerException))]
        int RejectFriendRequest(string username, string friendUsername);

        [OperationContract]
        [FaultContract(typeof(ServerException))]
        List<PlayerDC> SearchPlayers(string username);

        [OperationContract]
        [FaultContract(typeof(ServerException))]
        int SendFriendRequest(string username, string friendUsername);

        //TODO: implementar un objeto serializado para recuperar el perfil completo del usuario
        [OperationContract]
        [FaultContract(typeof(ServerException))]
        int GetPlayerProfile(string username);

        [OperationContract]
        [FaultContract(typeof(ServerException))]
        List<AchievementDC> GetAchievements(string username);

        //TODO: implementar un objeto serializado para recuperar el leaderboard
        [OperationContract]
        [FaultContract(typeof(ServerException))]
        int GetLeaderboard(PlayerDC player);
    }

    [ServiceContract]
    public interface IMainMenuServiceCallback
    {
        [OperationContract]
        void ReceiveFriendRequest(string username);
        [OperationContract]
        void ReceiveGameInvitation(string username);
    }
}
