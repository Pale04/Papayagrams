using PapayagramsClient.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PapayagramsClient.PapayagramsService;

namespace PapayagramsClient
{
    /// <summary>
    /// Lógica de interacción para MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
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
                return;
            }

            host.ReportToServer(CurrentPlayer.Player.Username);
            host.Close();
        }

        private void CreateNewGame(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GameCreation());
        }

        private void JoinGameButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new JoinGame());
        }

        public void ReceiveFriendRequest(PlayerDC player)
        {
            throw new NotImplementedException();
        }

        public void ReceiveGameInvitation()
        {
            throw new NotImplementedException();
        }
    }
}
