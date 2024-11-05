using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PapayagramsClient.Game
{
    /// <summary>
    /// Lógica de interacción para JoinGame.xaml
    /// </summary>
    public partial class JoinGame : Page
    {
        public JoinGame()
        {
            InitializeComponent();
        }

        private void JoinGameRoom(object sender, RoutedEventArgs e)
        {
            string gameRoomCode = CodeTextbox.Text.Trim();
            Lobby lobby = new Lobby(gameRoomCode);

            if (lobby != null)
            {
                NavigationService.Navigate(lobby);
            }
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
