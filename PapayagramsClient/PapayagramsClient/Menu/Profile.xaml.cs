using PapayagramsClient.PapayagramsService;
using System;
using System.IO;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace PapayagramsClient.Menu
{
    public partial class Profile : Page
    {
        private PlayerStatsDC _userStats;

        public Profile(PlayerStatsDC userStats)
        {
            InitializeComponent();
            _userStats = userStats;
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
            ShowUserImage();
        }

        private void ShowUserImage()
        {
            ProfilePictureImage.Source = new BitmapImage(new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\PlayerImages\\image" + CurrentPlayer.Player.ProfileIcon + ".jpg"));
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ShowAvailableImages(object sender, RoutedEventArgs e)
        {
            ImagesViewPanel.Visibility = Visibility.Visible;
            ImagesViewPanel.IsEnabled = true;
        }

        private void CloseImages(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CloseImages();
        }

        private void CloseImages()
        {
            ImagesViewPanel.Visibility = Visibility.Hidden;
            ImagesViewPanel.IsEnabled = false;
        }

        private void SetImage1(object sender, RoutedEventArgs e)
        {
            ApplicationSettingsServiceClient host = new ApplicationSettingsServiceClient();

            try
            {
                host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                NavigationService.GoBack();
                return;
            }

            host.UpdateProfileIcon(CurrentPlayer.Player.Username, 1);
            host.Close();
            CurrentPlayer.Player.ProfileIcon = 1;
            ShowUserImage();
            CloseImages();
        }

        private void SetImage2(object sender, RoutedEventArgs e)
        {
            ApplicationSettingsServiceClient host = new ApplicationSettingsServiceClient();

            try
            {
                host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                NavigationService.GoBack();
                return;
            }

            host.UpdateProfileIcon(CurrentPlayer.Player.Username, 2);
            host.Close();
            CurrentPlayer.Player.ProfileIcon = 2;
            ShowUserImage();
            CloseImages();
        }

        private void SetImage3(object sender, RoutedEventArgs e)
        {
            ApplicationSettingsServiceClient host = new ApplicationSettingsServiceClient();

            try
            {
                host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                NavigationService.GoBack();
                return;
            }

            host.UpdateProfileIcon(CurrentPlayer.Player.Username, 3);
            host.Close();

            CurrentPlayer.Player.ProfileIcon = 3;
            ShowUserImage();
            CloseImages();
        }
    }
}
