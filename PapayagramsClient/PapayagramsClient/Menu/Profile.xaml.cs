using log4net;
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
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Profile));

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
            ProfilePictureImage.Source = ImagesService.GetImageFromId(CurrentPlayer.Player.ProfileIcon);
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainMenu());
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
            UpdateUserImage(1);
        }

        private void SetImage2(object sender, RoutedEventArgs e)
        {
            UpdateUserImage(2);
        }

        private void SetImage3(object sender, RoutedEventArgs e)
        {
            UpdateUserImage(3);
        }

        private void UpdateUserImage(int imageId)
        {
            ApplicationSettingsServiceClient host = new ApplicationSettingsServiceClient();

            try
            {
                host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                _logger.Fatal("Couldn't reach server for updating profile image");
                NavigationService.GoBack();
                return;
            }

            host.UpdateProfileIcon(CurrentPlayer.Player.Username, imageId);
            host.Close();
            CurrentPlayer.Player.ProfileIcon = imageId;
            ShowUserImage();
            CloseImages();
        }
    }
}
