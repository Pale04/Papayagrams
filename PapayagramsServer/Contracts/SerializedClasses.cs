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
                ProfileIcon = player.ProfileIcon
            };
        }
    }

    [DataContract]
    public class PlayerStatsDC
    {
        [DataMember]
        public int OriginalGamesPlayed { get; set; }
        [DataMember]
        public int TimeAttackGamesPlayed { get; set; }
        [DataMember]
        public int SuddenDeathGamesPlayed { get; set; }
        [DataMember]
        public int OriginalGamesWon { get; set; }
        [DataMember]
        public int TimeAttackGamesWon { get; set; }
        [DataMember]
        public int SuddenDeathGamesWon { get; set; }
        [DataMember]
        public int FriendsAmount { get; set; }

        public override bool Equals(object obj)
        {
            bool isEqual = false;

            if (obj != null && GetType() == obj.GetType())
            {
                PlayerStatsDC playerStats = (PlayerStatsDC)obj;
                isEqual = OriginalGamesPlayed == playerStats.OriginalGamesPlayed &&
                          TimeAttackGamesPlayed == playerStats.TimeAttackGamesPlayed &&
                          SuddenDeathGamesPlayed == playerStats.SuddenDeathGamesPlayed &&
                          OriginalGamesWon == playerStats.OriginalGamesWon &&
                          TimeAttackGamesWon == playerStats.TimeAttackGamesWon &&
                          SuddenDeathGamesWon == playerStats.SuddenDeathGamesWon &&
                          FriendsAmount == playerStats.FriendsAmount;
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            return OriginalGamesPlayed.GetHashCode() ^ TimeAttackGamesPlayed.GetHashCode() ^ SuddenDeathGamesPlayed.GetHashCode() ^OriginalGamesWon.GetHashCode() ^ TimeAttackGamesWon.GetHashCode() ^ SuddenDeathGamesWon.GetHashCode() ^ FriendsAmount.GetHashCode();
        }

        public static PlayerStatsDC ConvertToPlayerStatsDC(PlayerStats playerStats)
        {
            return new PlayerStatsDC
            {
                OriginalGamesPlayed = playerStats.OriginalGamesPlayed,
                TimeAttackGamesPlayed = playerStats.TimeAttackGamesPlayed,
                SuddenDeathGamesPlayed = playerStats.SuddenDeathGamesPlayed,
                OriginalGamesWon = playerStats.OriginalGamesWon,
                TimeAttackGamesWon = playerStats.TimeAttackGamesWon,
                SuddenDeathGamesWon = playerStats.SuddenDeathGamesWon,
                FriendsAmount = playerStats.FriendsAmount
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
                isEqual = Id == achievement.Id && IsAchieved == achievement.IsAchieved;
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ IsAchieved.GetHashCode();
        }

        public static AchievementDC ConvertToAchievementDC(Achievement achievement)
        {
            return new AchievementDC
            {
                Id = achievement.Id,
                Description = achievement.Description,
                IsAchieved = achievement.IsAchieved
            };
        }
    }

    [DataContract]
    public enum RelationStateDC
    {
        [EnumMember]
        Friend,
        [EnumMember]
        Blocked,
        [EnumMember]
        Pending
    }

    [DataContract]
    public class FriendDC
    {
        [DataMember] 
        public int Id { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public RelationStateDC RelationState { get; set; }

        public static FriendDC ConvertToFriendDC(Friend friend)
        {
            return new FriendDC
            {
                Id = friend.Id,
                Username = friend.Username,
                RelationState = (RelationStateDC)Enum.Parse(typeof(RelationStateDC), friend.RelationState.ToString())
            };
        }

        public override bool Equals(object obj)
        {
            bool isEqual = false;

            if (obj != null && GetType() == obj.GetType())
            {
                FriendDC friend = (FriendDC)obj;
                isEqual = Id == friend.Id && Username.Equals(friend.Username) && RelationState.Equals(friend.RelationState);
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Username.GetHashCode() ^ RelationState.GetHashCode();
        }
    }

    [DataContract]
    public class Message
    {
        [DataMember]
        public string AuthorUsername { get; set; }
        [DataMember]
        public string GameRoomCode { get; set; }
        [DataMember]
        public DateTime Time { get; set; }
        [DataMember]
        public string Content { get; set; }
    }

    [DataContract]
    public class GameInvitationDC
    {
        [DataMember]
        public string GameRoomCode { get; set; }
        [DataMember]
        public string SenderUsername { get; set; }
    }

    [DataContract]
    public enum GameModeDC
    {
        [EnumMember]
        Original,
        [EnumMember]
        SuddenDeath,
        [EnumMember]
        TimeAttack
    }

    [DataContract]
    public enum LanguageDC
    {
        [EnumMember]
        English,
        [EnumMember]
        Spanish
    }

    [DataContract]
    public class GameConfigurationDC
    {
        [DataMember]
        public GameModeDC GameMode { get; set; }

        /// <summary>
        /// Number of pieces that every player will have at the beginning of the game
        /// </summary>
        [DataMember]
        public int InitialPieces { get; set; }

        [DataMember]
        public int MaxPlayers { get; set; }

        [DataMember]
        public LanguageDC WordsLanguage { get; set; }

        [DataMember]
        public int TimeLimitMinutes { get; set; }

        public override bool Equals(object obj)
        {
            bool isEqual = false;
            if (obj != null && GetType() == obj.GetType())
            {
                GameConfigurationDC gameConfiguration = (GameConfigurationDC)obj;
                isEqual = GameMode == gameConfiguration.GameMode &&
                          InitialPieces == gameConfiguration.InitialPieces &&
                          MaxPlayers == gameConfiguration.MaxPlayers &&
                          WordsLanguage == gameConfiguration.WordsLanguage &&
                          TimeLimitMinutes == gameConfiguration.TimeLimitMinutes;
            }
            return isEqual;
        }

        public override int GetHashCode()
        {
            int hashCode = -1943627975;
            hashCode = hashCode * -1521134295 + GameMode.GetHashCode();
            hashCode = hashCode * -1521134295 + InitialPieces.GetHashCode();
            hashCode = hashCode * -1521134295 + MaxPlayers.GetHashCode();
            hashCode = hashCode * -1521134295 + WordsLanguage.GetHashCode();
            hashCode = hashCode * -1521134295 + TimeLimitMinutes.GetHashCode();
            return hashCode;
        }

        public static GameConfigurationDC ConvertToGameConfigurationDC (GameConfiguration gameConfiguration)
        {
            return new GameConfigurationDC
            {
                GameMode = (GameModeDC)Enum.Parse(typeof(GameModeDC), gameConfiguration.GameMode.ToString()),
                InitialPieces = gameConfiguration.InitialPieces,
                MaxPlayers = gameConfiguration.MaxPlayers,
                WordsLanguage = (LanguageDC)Enum.Parse(typeof(LanguageDC), gameConfiguration.WordsLanguage.ToString()),
                TimeLimitMinutes = gameConfiguration.TimeLimitMinutes
            };
        }
    }
    
    [DataContract]
    public class GameRoomDC
    {
        [DataMember]
        public string RoomCode { get; set; }

        [DataMember]
        public List<PlayerDC> Players { get; set; }

        [DataMember]
        public GameConfigurationDC GameConfiguration { get; set; }

        public static GameRoomDC ConvertToGameRoomDC(GameRoom room)
        {
            List<PlayerDC> players = room.Players.ConvertAll(PlayerDC.ConvertToPlayerDC);

            return new GameRoomDC
            {
                RoomCode = room.RoomCode,
                Players = players,
                GameConfiguration = GameConfigurationDC.ConvertToGameConfigurationDC(room.GameConfiguration)
            };
        }
    }

    [DataContract]
    public class LeaderboardStatsDC
    {
        [DataMember]
        public string PlayerUsername { get; set; }

        [DataMember]
        public int TotalGames { get; set; }

        [DataMember]
        public int GamesWon { get; set; }

        [DataMember]
        public int GamesLost { get; set; }

        public static LeaderboardStatsDC ConvertToLeaderboardStatsDC(LeaderboardStats leaderboardStats)
        {
            return new LeaderboardStatsDC
            {
                PlayerUsername = leaderboardStats.PlayerUsername,
                TotalGames = leaderboardStats.TotalGames,
                GamesWon = leaderboardStats.GamesWon,
                GamesLost = leaderboardStats.GamesLost
            };
        }

        public override bool Equals(object obj)
        {
            bool isEqual = false;
            if (obj != null && GetType() == obj.GetType())
            {
                LeaderboardStatsDC stats = (LeaderboardStatsDC)obj;
                isEqual = PlayerUsername.Equals(stats.PlayerUsername) && TotalGames == stats.TotalGames && GamesWon == stats.GamesWon && GamesLost == stats.GamesLost;
            }
            return isEqual;
        }

        public override int GetHashCode()
        {
            return PlayerUsername.GetHashCode() ^ TotalGames ^ GamesWon ^ GamesLost;
        }
    }

    [DataContract]
    public enum ApplicationLanguageDC
    {
        [EnumMember]
        english,
        [EnumMember]
        spanish,
        [EnumMember]
        auto
    }

    [DataContract]
    public class ApplicationSettingsDC
    {
        [DataMember]
        public int PieceColor { get; set; }

        [DataMember]
        public ApplicationLanguageDC SelectedLanguage { get; set; }

        [DataMember]
        public int Cursor { get; set; }

        public static ApplicationSettingsDC ConvertToApplicationSettingsDC(ApplicationSettings applicationSettings)
        {
            return new ApplicationSettingsDC
            {
                PieceColor = applicationSettings.PieceColor,
                SelectedLanguage = (ApplicationLanguageDC)Enum.Parse(typeof(ApplicationLanguageDC), applicationSettings.SelectedLanguage.ToString()),
                Cursor = applicationSettings.Cursor
            };
        }

        public override bool Equals(object obj)
        {
            bool isEqual = false;
            if (obj != null && GetType() == obj.GetType())
            {
                ApplicationSettingsDC settings = (ApplicationSettingsDC)obj;
                isEqual = PieceColor == settings.PieceColor && SelectedLanguage.Equals(settings.SelectedLanguage) && Cursor == settings.Cursor;
            }
            return isEqual;
        }

        public override int GetHashCode()
        {
            return PieceColor.GetHashCode() ^ SelectedLanguage.GetHashCode() ^ Cursor.GetHashCode();
        }
    }
}
