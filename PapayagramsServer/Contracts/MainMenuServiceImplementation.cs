using DataAccess;
using DomainClasses;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.ServiceModel;

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

        /// <summary>
        /// Search a player by username
        /// </summary>
        /// <param name="username">PlayerDC object with its id, username and email.</param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServerException}">When the consult is empty or happens a database error</exception>
        public PlayerDC SearchPlayer(string username)
        {
            PlayerDC player = new PlayerDC();
            Option<Player> playerOption;

            try
            {
                playerOption = UserDB.GetPlayerByUsername(username);
            }
            catch (EntityException error)
            {
                throw new FaultException<ServerException>(new ServerException(2, error.StackTrace));
            }

            if (playerOption.IsNone)
            {
                throw new FaultException<ServerException>(new ServerException(205));
            }

            return ConvertPlayerToDataContract((Player)playerOption.Case);
        }

        public int SendFriendRequest(string username, string friendUsername)
        {
            throw new NotImplementedException();
        }
    }
}
