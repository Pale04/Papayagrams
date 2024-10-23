using BussinessLogic;
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

        /// <summary>
        /// Add the callback channel of the player to the callbacks pool
        /// </summary>
        /// <param name="username">Username of the player</param>
        public void ReportToServer(string username)
        {
            CallbacksPool.PlayerArrivedToMainMenu(username,OperationContext.Current.GetCallbackChannel<IMainMenuServiceCallback>());
        }

        /// <summary>
        /// Search a player by username
        /// </summary>
        /// <param name="searcherUsername">Username ot the player who is searching</param>
        /// <param name="searchedUsername">Username of the player who needs to be found</param>
        /// <returns>PlayerDC object with its id, username and email.</returns>
        /// <exception cref="FaultException{ServerException}">When the consult is empty or happens a database error</exception>
        public PlayerDC SearchNoFriendPlayer(string searcherUsername, string searchedUsername)
        {
            PlayerDC player = new PlayerDC();
            Option<Player> playerOption =Option<Player>.None;

            //TODO: mostrar todos los jugadores menos amigos.

            if (playerOption.IsNone)
            {
                throw new FaultException<ServerException>(new ServerException{ ErrorCode = 205 });
            }

            return ConvertPlayerToDataContract((Player)playerOption.Case);
        }

        public int SendFriendRequest(string senderUsername, string receiverUsername)
        {
            int result;
            try
            {
                result = UserDB.SendFriendRequest(senderUsername, receiverUsername);
            }
            catch (EntityException error)
            {
                throw new FaultException<ServerException>(new ServerException{ ErrorCode = 102, StackTrace = error.StackTrace });
            }

            if (result == -1)
            {
                throw new FaultException<ServerException>(new ServerException{ ErrorCode = 301 });
            }
            else if(result == -2)
            {
                throw new FaultException<ServerException>(new ServerException { ErrorCode = 302 });
            }
            else if (result == -3)
            {
                throw new FaultException<ServerException>(new ServerException{ ErrorCode = 303 });
            }

            return result;
        }
    }
}
