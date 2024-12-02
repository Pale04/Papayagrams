using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PapayagramsClient.Game
{
    public partial class JoinGame : Page
    {
        public JoinGame()
        {
            InitializeComponent();
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
