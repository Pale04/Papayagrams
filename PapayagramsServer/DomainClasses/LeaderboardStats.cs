
namespace DomainClasses
{
    public class LeaderboardStats
    {
        private string _playerUsername;
        private int _totalGames;
        private int _gamesWon;
        private int _gamesLost;

        public string PlayerUsername { get { return _playerUsername; } }
        public int TotalGames { get { return _totalGames; } }
        public int GamesWon { get { return _gamesWon; } }
        public int GamesLost { get { return _gamesLost; } }

        public LeaderboardStats (string playerUsername, PlayerStats playerStats)
        {
            _playerUsername = playerUsername;
            _totalGames = playerStats.OriginalGamesPlayed + playerStats.TimeAttackGamesPlayed + playerStats.SuddenDeathGamesPlayed;
            _gamesWon = playerStats.OriginalGamesWon + playerStats.TimeAttackGamesWon + playerStats.SuddenDeathGamesWon;
            _gamesLost = _totalGames - _gamesWon;
        }

        public override bool Equals(object obj)
        {
            bool isEqual = false;
            if (obj != null && GetType() == obj.GetType())
            {
                LeaderboardStats stats = (LeaderboardStats)obj;
                isEqual = _playerUsername.Equals(stats.PlayerUsername) && _totalGames == stats.TotalGames && _gamesWon == stats.GamesWon && _gamesLost == stats.GamesLost;
            }
            return isEqual;
        }

        public override int GetHashCode()
        {
            return _playerUsername.GetHashCode() ^ _totalGames ^ _gamesWon ^ _gamesLost;
        }
    }
}
