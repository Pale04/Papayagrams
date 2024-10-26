using DomainClasses;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Contracts
{
    [DataContract]
    public class PlayerDC
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public int ProfileIcon { get; set; }

        public override bool Equals(object obj)
        {
            bool isEqual = false;

            if (obj != null && GetType() == obj.GetType())
            {
                PlayerDC player = (PlayerDC)obj;
                isEqual = Id == player.Id && Username == player.Username && Email == player.Email;
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Username.GetHashCode() ^ Email.GetHashCode() ^ Password.GetHashCode() ^ ProfileIcon.GetHashCode();
        }

        public static PlayerDC ConvertToPlayerDC(Player player)
        {
            return new PlayerDC
            {
                Id = player.Id,
                Username = player.Username,
                Email = player.Email,
                Password = player.Password,
                ProfileIcon = player.ProfileIcon
            };
        }
    }

    [DataContract]
    public class AchievementDC
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsAchieved { get; set; }

        public override bool Equals(object obj)
        {
            bool isEqual = false;

            if (obj != null && GetType() == obj.GetType())
            {
                AchievementDC achievement = (AchievementDC)obj;
                isEqual = Id == achievement.Id && Description == achievement.Description && IsAchieved == achievement.IsAchieved;
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Description.GetHashCode() ^ IsAchieved.GetHashCode();
        }
    }

    [DataContract]
    public class FriendDC
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Status { get; set; }

        public override bool Equals(object obj)
        {
            bool isEqual = false;

            if (obj != null && GetType() == obj.GetType())
            {
                FriendDC friend = (FriendDC)obj;
                isEqual = Username == friend.Username && Status == friend.Status;
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            return Username.GetHashCode() ^ Status.GetHashCode();
        }
    }

    [DataContract]
    public class Message
    {
        [DataMember]
        public string AuthorUsername;
        [DataMember]
        public string GameRoomCode;
        [DataMember]
        public DateTime Time;
        [DataMember]
        public string Content;
    }

    [DataContract]
    public class GameInvitationDC
    {
        [DataMember]
        public string GameRoomCode;
        [DataMember]
        public string PlayerUsername;
    }

    [DataContract]
    public class GameRoomDC
    {
        [DataMember]
        public string RoomCode { get; set; }

        [DataMember]
        public List<PlayerDC> Players;

        public static GameRoomDC ConvertToGameRoomDC(GameRoom room)
        {
            List<PlayerDC> players = room.Players.ConvertAll(PlayerDC.ConvertToPlayerDC);

            return new GameRoomDC
            {
                RoomCode = room.RoomCode,
                Players = players
            };
        }
    }
}
