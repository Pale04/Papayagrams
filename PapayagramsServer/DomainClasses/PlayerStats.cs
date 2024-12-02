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

        public override bool Equals(object obj)
        {
            bool isEqual = false;

            if (obj != null && GetType() == obj.GetType())
            {
                PlayerStats player = (PlayerStats)obj;
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
