using PapayagramsClient.PapayagramsService;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PapayagramsClient.Menu
{
    public partial class Profile : Page
    {
        private PlayerStatsDC _userStats;

        public Profile(PlayerStatsDC userStats)
        {
            InitializeComponent();
            SetPlayerData();
        }

        private void SetPlayerData()
        {
            UsernameLabel.Content = CurrentPlayer.Player.Username;
            EmailLabel.Content = CurrentPlayer.Player.Email;

            int totalGamesPlayed = _userStats.OriginalGamesPlayed + _userStats.TimeAttackGamesPlayed + _userStats.SuddenDeathGamesPlayed;
            PlayedGamesLabel.Content = Properties.Resources.profilePlayedGames + totalGamesPlayed;
            PlayedOriginalLabel.Content = Properties.Resources.profileOriginalGames + _userStats.OriginalGamesPlayed;
            PlayedSuddenDeathLabel.Content = Properties.Resources.profileSuddenDeathGames + _userStats.SuddenDeathGamesPlayed;
            PlayedTimeAttackLabel.Content = Properties.Resources.profileTimeAttackGames + _userStats.TimeAttackGamesPlayed;

            int totalGamesWon = _userStats.OriginalGamesWon + _userStats.TimeAttackGamesWon + _userStats.SuddenDeathGamesWon;
            WonGamesLabel.Content = Properties.Resources.profileGamesWon + totalGamesWon;
            WonOriginalLabel.Content = Properties.Resources.profileOriginalGamesWon + _userStats.OriginalGamesWon;
            WonTimeAttackLabel.Content = Properties.Resources.profileTimeAttackGamesWon + _userStats.TimeAttackGamesWon;
            WonSuddenDeathLabel.Content = Properties.Resources.profileSuddenDeathGamesWon + _userStats.SuddenDeathGamesWon;

            NuberFriendsLabel.Content = Properties.Resources.profileNumberFriends + _userStats.FriendsAmount;
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
