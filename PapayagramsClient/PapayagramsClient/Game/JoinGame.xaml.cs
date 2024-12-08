using log4net;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PapayagramsClient.Game
{
    public partial class JoinGame : Page
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(JoinGame));

        public JoinGame()
        {
            InitializeComponent();
        }

        private void JoinGameRoom(object sender, RoutedEventArgs e)
        {
            string gameRoomCode = CodeTextbox.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(gameRoomCode))
            {
                return;
            }

            bool roomAvailable = false;

            try
            {
                roomAvailable = new PapayagramsService.GameCodeVerificationServiceClient().VerifyGameRoom(gameRoomCode);
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                _logger.Fatal("Couldn't connect to server for checking room availability");
                NavigationService.GoBack();
                return;
            }

            if (roomAvailable)
            {
                NavigationService.Navigate(new Lobby(gameRoomCode));
            }
            else
            {
                new SelectionPopUpWindow(Properties.Resources.joinGameCantJoinTitle, Properties.Resources.joinGameCantJoin, 2).ShowDialog();
            }
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
