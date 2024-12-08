using log4net;
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

namespace PapayagramsClient.Login
{
    public partial class JoinAsGuest : Page
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(JoinAsGuest));

        public JoinAsGuest()
        {
            PapayagramsService.LoginServiceClient host = new PapayagramsService.LoginServiceClient();
            try
            {
                host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                _logger.Fatal("Couldn't connect to server for login as guest");
                return;
            }

            CurrentPlayer.Player = host.AccessAsGuest();
            CurrentPlayer.IsGuest = true;
            host.Close();

            InitializeComponent();
        }

        ~JoinAsGuest()
        {
            CurrentPlayer.Player = null;
        }

        private void ReturnToLogin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }

        private void JoinGameRoom(object sender, RoutedEventArgs e)
        {
            string gameRoomCode = CodeTextbox.Text.Trim();
            if (string.IsNullOrEmpty(gameRoomCode))
            {
                return;
            }

            bool roomAvailable = new PapayagramsService.GameCodeVerificationServiceClient().VerifyGameRoom(gameRoomCode);

            if (roomAvailable)
            {
                NavigationService.Navigate(new Game.Lobby(gameRoomCode));
            }
            else
            {
                new SelectionPopUpWindow(Properties.Resources.joinGameCantJoinTitle, Properties.Resources.joinGameCantJoin, 2).ShowDialog();
            }
        }
    }
}
