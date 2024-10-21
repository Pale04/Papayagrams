using System;
using System.Collections.Generic;

namespace Contracts
{
    public partial class ServiceImplementation : IMainMenuService
    {
        public int AcceptFriendRequest(string username, string friendUsername)
        {
            throw new NotImplementedException();
        }

        public int BlockFriend(string username, string friendUsername)
        {
            throw new NotImplementedException();
        }

        public List<AchievementDC> GetAchievements(string username)
        {
            throw new NotImplementedException();
        }

        public List<FriendDC> GetFriendRequests(string username)
        {
            throw new NotImplementedException();
        }

        public List<FriendDC> GetFriends(string username)
        {
            throw new NotImplementedException();
        }

        public int GetLeaderboard(PlayerDC player)
        {
            throw new NotImplementedException();
        }

        public int GetPlayerProfile(string username)
        {
            throw new NotImplementedException();
        }

        public int RejectFriendRequest(string username, string friendUsername)
        {
            throw new NotImplementedException();
        }

        public int RemoveFriend(string username, string friendUsername)
        {
            throw new NotImplementedException();
        }

        public void ReportToServer(string username)
        {
            throw new NotImplementedException();
        }

        public List<PlayerDC> SearchPlayers(string username)
        {
            throw new NotImplementedException();
        }

        public int SendFriendRequest(string username, string friendUsername)
        {
            throw new NotImplementedException();
        }
    }
}
