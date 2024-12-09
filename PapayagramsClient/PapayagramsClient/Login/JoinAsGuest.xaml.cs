using log4net;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PapayagramsClient.Login
{
    public partial class JoinAsGuest : Page
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(JoinAsGuest));

        public JoinAsGuest()
        {
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
            string gameRoomCode = CodeTextbox.Text.Trim().ToUpper();
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
