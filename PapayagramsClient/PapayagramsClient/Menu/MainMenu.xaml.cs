using PapayagramsClient.Game;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using PapayagramsClient.PapayagramsService;
using PapayagramsClient.Menu;
using System.IO;

namespace PapayagramsClient
{
    public partial class MainMenu : Page, IMainMenuServiceCallback
    {
        private MainMenuServiceClient _host;

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
                new PopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                NavigationService.GoBack();
                return;
            }

            _host.ReportToServer(CurrentPlayer.Player.Username);
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
            NavigationService.Navigate(new Profile());
        }

        public void ReceiveFriendRequest(PlayerDC player)
        {
            throw new NotImplementedException();
        }

        public void ReceiveGameInvitation()
        {
            throw new NotImplementedException();
        }

        public void ReceiveGameInvitation(GameInvitationDC invitation)
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
                new PopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                NavigationService.GoBack();
                return;
            }

            int result = host.Logout(CurrentPlayer.Player.Username);
            host.Close();

            switch (result)
            {
                case 0:
                    CurrentPlayer.Player = null;
                    NavigationService.Navigate(new Login.Login());
                    break;

                case 102:
                    new PopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorDatabaseConnection, 3).ShowDialog();
                    break;

                case 205:
                    break;
            }
        }
    }
}
