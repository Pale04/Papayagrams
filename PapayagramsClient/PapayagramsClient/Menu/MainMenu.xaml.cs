using PapayagramsClient.Game;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using PapayagramsClient.PapayagramsService;
using PapayagramsClient.Menu;

namespace PapayagramsClient
{
    public partial class MainMenu : Page, IMainMenuServiceCallback
    {
        public MainMenu()
        {
            InitializeComponent();

            InstanceContext context = new InstanceContext(this);
            MainMenuServiceClient host = new MainMenuServiceClient(context);
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

            host.ReportToServer(CurrentPlayer.Player.Username);
            host.Close();
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

        public void AddIcons()
        {
            FriendImage.SetImage("../Resources/Icons/friend-svgrepo-com.svg");
        }

        public void ReceiveGameInvitation(GameInvitationDC invitation)
        {
            throw new NotImplementedException();
        }
    }
}
