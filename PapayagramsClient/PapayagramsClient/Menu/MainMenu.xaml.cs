using PapayagramsClient.Game;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using PapayagramsClient.PapayagramsService;
using PapayagramsClient.Menu;
using PapayagramsClient.WPFControls;

namespace PapayagramsClient
{
    public partial class MainMenu : Page, IMainMenuServiceCallback
    {
        private MainMenuServiceClient _host;
        private string _invitationGameCode;

        public MainMenu()
        {
            InitializeComponent();

            InstanceContext context = new InstanceContext(this);

            _host = new MainMenuServiceClient(context);

            try
            {
                _host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                NavigationService.GoBack();
                return;
            }

            int returnCode = _host.ReportToServer(CurrentPlayer.Player.Username);

            switch (returnCode)
            {
                case 0:
                    break;

                case 102:
                    new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorDatabaseConnection, 3).ShowDialog();
                    NavigationService.GoBack();
                    return;
            }
        }

        ~MainMenu()
        {
            _host.Close();
        }

        private void CreateNewGame(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GameCreation());
        }

        private void JoinGame(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new JoinGame());
        }

        private void GoToProfile(object sender, RoutedEventArgs e)
        {
            (int returnCode, PlayerStatsDC userStats) = _host.GetPlayerProfile(CurrentPlayer.Player.Username);
            switch (returnCode)
            {
                case 102:
                    new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorDatabaseConnection, 3).ShowDialog();
                    return;
                case 205:
                    new SelectionPopUpWindow(Properties.Resources.errorOccurredTitle, Properties.Resources.errorUnexpectedError, 3).ShowDialog();
                    return;
            }

            NavigationService.Navigate(new Profile(userStats));
        }

        public void ReceiveFriendRequest(PlayerDC player)
        {
            throw new NotImplementedException();
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            LoginServiceClient host = new LoginServiceClient();

            try
            {
                host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                return;
            }

            int returnCode = host.Logout(CurrentPlayer.Player.Username);
            host.Close();

            switch (returnCode)
            {
                case 0:
                    CurrentPlayer.Player = null;
                    NavigationService.Navigate(new Login.Login());
                    break;

                case 102:
                    new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorDatabaseConnection, 3).ShowDialog();
                    break;

                case 205:
                    break;
            }
        }

        private void GoToAchievements(object sender, RoutedEventArgs e)
        {
            (int returnCode, AchievementDC[] achievements) = _host.GetAchievements(CurrentPlayer.Player.Username);

            switch (returnCode)
            {
                case 0:
                    NavigationService.Navigate(new Achievements(achievements));
                    break;
            }
        }

        private void GoToConfiguration(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Configuration());
        }

        public void ReceiveGameInvitation(GameInvitationDC invitation)
        {
            GameInvitationPanel.Visibility = Visibility.Visible;
            GameInvitationPanel.IsEnabled = true;

            string friend = invitation.PlayerUsername;

            GameInvitationPanel.MessageLabel.Text = friend + " " + Properties.Resources.gameInvitationMessage;
            _invitationGameCode = invitation.GameRoomCode;
        }

        private void JoinInvitationGame(object sender, RoutedEventArgs e)
        {
            GameInvitationPanel.Visibility = Visibility.Hidden;
            GameInvitationPanel.IsEnabled = false;

            if (!string.IsNullOrWhiteSpace(_invitationGameCode))
            {
                bool roomAvailable = new GameCodeVerificationServiceClient().VerifyGameRoom(_invitationGameCode);

                if (roomAvailable)
                {
                    NavigationService.Navigate(new Lobby(_invitationGameCode));
                }
                else
                {
                    new SelectionPopUpWindow(Properties.Resources.joinGameCantJoinTitle, Properties.Resources.joinGameCantJoin, 2).ShowDialog();
                }
            }

            _invitationGameCode = "";
        }

        private void RejectInvitationGame(object sender, RoutedEventArgs e)
        {
            GameInvitationPanel.Visibility = Visibility.Hidden;
            GameInvitationPanel.IsEnabled = false;

            _invitationGameCode = "";
        }

        private void OpenFriendsOverlay(object sender, RoutedEventArgs e)
        {
            FriendsMenuPanel.Visibility = Visibility.Visible;
            FriendsMenuPanel.IsEnabled = true;
            (int returnCode, FriendDC[] relationships) = _host.GetAllRelationships(CurrentPlayer.Player.Username);
            switch (returnCode)
            {
                case 0:
                    FriendsMenuPanel.FillLists(relationships);
                    break;
            }
        }

        private void CloseFriendsOverlay(object sender, RoutedEventArgs e)
        {
            FriendsMenuPanel.Visibility = Visibility.Hidden;
            FriendsMenuPanel.IsEnabled = false;
        }

        private void AddNewFriend(object sender, RoutedEventArgs e)
        {
            string friendUsername = FriendsMenuPanel.NewFriendUsernameTextBox.Text;
            int returnCode = _host.SendFriendRequest(CurrentPlayer.Player.Username, friendUsername);

            switch (returnCode)
            {
                case 0:
                    new SelectionPopUpWindow(Properties.Resources.friendsRequestSentTitle, Properties.Resources.friendsRequestSent, 0).ShowDialog();
                    break;

                case 101:
                    // one of the usernames passed to the function was null
                    new SelectionPopUpWindow(Properties.Resources.errorOccurredTitle, Properties.Resources.errorUnexpectedError, 3).ShowDialog();
                    break;

                case 102:
                    new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorDatabaseConnection, 3).ShowDialog();
                    break;

                case 301:
                    break;

                case 302:
                    break;

                case 303:
                    break;

                case 304:
                    break;
            }
        }

        private void AcceptFriendRequest(object sender, RoutedEventArgs e)
        {
            FriendInfoPanel friendPanel = (FriendInfoPanel)sender;
            int returnCode = _host.RespondFriendRequest(CurrentPlayer.Player.Username, friendPanel.UsernameLabel.Text, true);

            switch (returnCode)
            {
                case 0:
                    break;

                case 101:
                    // one of the usernames passed to the function was null
                    new SelectionPopUpWindow(Properties.Resources.errorOccurredTitle, Properties.Resources.errorUnexpectedError, 3).ShowDialog();
                    break;

                case 102:
                    new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorDatabaseConnection, 3).ShowDialog();
                    break;

                case 305:
                    // No request exists for the specified users
                    new SelectionPopUpWindow(Properties.Resources.errorOccurredTitle, Properties.Resources.errorUnexpectedError, 3).ShowDialog();
                    break;

                case 306:
                    // Wrong username for respondent
                    new SelectionPopUpWindow(Properties.Resources.errorOccurredTitle, Properties.Resources.errorUnexpectedError, 3).ShowDialog();
                    break;

                case 307:
                    // Wrong username for requester
                    new SelectionPopUpWindow(Properties.Resources.errorOccurredTitle, Properties.Resources.errorUnexpectedError, 3).ShowDialog();
                    break;
            }
        }

        private void RejectFriendRequest(object sender, RoutedEventArgs e)
        {
            FriendInfoPanel friendPanel = (FriendInfoPanel)sender;
            int returnCode = _host.RespondFriendRequest(CurrentPlayer.Player.Username, friendPanel.UsernameLabel.Text, false);

            switch (returnCode)
            {
                case 0:
                    break;

                case 101:
                    // one of the usernames passed to the function was null
                    new SelectionPopUpWindow(Properties.Resources.errorOccurredTitle, Properties.Resources.errorUnexpectedError, 3).ShowDialog();
                    break;

                case 102:
                    new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorDatabaseConnection, 3).ShowDialog();
                    break;

                case 305:
                    // No request exists for the specified users
                    new SelectionPopUpWindow(Properties.Resources.errorOccurredTitle, Properties.Resources.errorUnexpectedError, 3).ShowDialog();
                    break;

                case 306:
                    // Wrong username for respondent
                    new SelectionPopUpWindow(Properties.Resources.errorOccurredTitle, Properties.Resources.errorUnexpectedError, 3).ShowDialog();
                    break;

                case 307:
                    // Wrong username for requester
                    new SelectionPopUpWindow(Properties.Resources.errorOccurredTitle, Properties.Resources.errorUnexpectedError, 3).ShowDialog();
                    break;
            }
        }

        private void BlockUser(object sender, RoutedEventArgs e)
        {
            FriendInfoPanel friendPanel = (FriendInfoPanel)sender;
            int returnCode = _host.BlockPlayer(CurrentPlayer.Player.Username, friendPanel.UsernameLabel.Text);
            // TODO: Catch error codes
        }

        private void UnblockUser(object sender, RoutedEventArgs e)
        {
            FriendInfoPanel friendPanel = (FriendInfoPanel)sender;
            int returnCode = _host.UnblockFriend(CurrentPlayer.Player.Username, friendPanel.UsernameLabel.Text);
             
        }

        private void RemoveFriend(object sender, RoutedEventArgs e)
        {
            FriendInfoPanel friendPanel = (FriendInfoPanel)sender;
            int returnCode = _host.RemoveFriend(CurrentPlayer.Player.Username, friendPanel.UsernameLabel.Text);
            // TODO: Catch error codes
        }
    }
}
