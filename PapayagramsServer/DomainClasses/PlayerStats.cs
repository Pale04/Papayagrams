namespace DomainClasses
{
    public class PlayerStats
    {
        public int OriginalGamesPlayed { get; set; }
        public int TimeAttackGamesPlayed { get; set; }
        public int SuddenDeathGamesPlayed { get; set; }
        public int OriginalGamesWon { get; set; }
        public int TimeAttackGamesWon { get; set; }
        public int SuddenDeathGamesWon { get; set; }
        public int FriendsAmount { get; set; }

        public override bool Equals(object other)
        {
            bool isEqual = false;

            if (other != null && GetType() == other.GetType())
            {
                PlayerStats player = (PlayerStats)other;
                isEqual = OriginalGamesPlayed == player.OriginalGamesPlayed &&
                          TimeAttackGamesPlayed == player.TimeAttackGamesPlayed &&
                          SuddenDeathGamesPlayed == player.SuddenDeathGamesPlayed &&
                          OriginalGamesWon == player.OriginalGamesWon &&
                          TimeAttackGamesWon == player.TimeAttackGamesWon &&
                          SuddenDeathGamesWon == player.SuddenDeathGamesWon &&
                          FriendsAmount == player.FriendsAmount;
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            return OriginalGamesPlayed.GetHashCode() ^
                   TimeAttackGamesPlayed.GetHashCode() ^
                   SuddenDeathGamesPlayed.GetHashCode() ^
                   OriginalGamesWon.GetHashCode() ^
                   TimeAttackGamesWon.GetHashCode() ^
                   SuddenDeathGamesWon.GetHashCode() ^
                   FriendsAmount.GetHashCode();
        }
    }
}
