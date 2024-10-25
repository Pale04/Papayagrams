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

        public (int, List<AchievementDC>) GetAchievements(string username)
        {
            throw new NotImplementedException();
        }

        public (int, List<FriendDC>) GetFriendRequests(string username)
        {
            throw new NotImplementedException();
        }

        public (int, List<FriendDC>) GetFriends(string username)
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
        public (int, PlayerDC) SearchNoFriendPlayer(string searcherUsername, string searchedUsername)
        {
            PlayerDC player = new PlayerDC();
            Option<Player> playerOption =Option<Player>.None;

            try
            {
                playerOption = UserDB.SearchNoFriendPlayer(searcherUsername, searchedUsername);
            }
            catch (EntityException error)
            {
                

            }
            //TODO: mostrar todos los jugadores menos amigos.

            if (playerOption.IsNone)
            {
                return (205, null);
            }

            return (0, null);
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
                //TOOD: handle
                return 102;
            }

            if (result == -1)
            {
                return 301;
            }
            else if(result == -2)
            {
                return 302;
            }
            else if (result == -3)
            {
                return 303;
            }

            return result;
        }
    }
}
