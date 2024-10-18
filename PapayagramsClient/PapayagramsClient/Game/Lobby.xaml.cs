using PapayagramsClient.PapayagramsService;
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

namespace PapayagramsClient.Game
{
    /// <summary>
    /// Lógica de interacción para Lobby.xaml
    /// </summary>
    public partial class Lobby : Page, PapayagramsService.IPregameServiceCallback
    {
        private string _gameRoomCode;

        public Lobby(string gameRoomCode)
        {
            InitializeComponent();
            GameRoomCodeText.Content = gameRoomCode;

            InstanceContext context = new InstanceContext(this);
            PregameServiceClient host = new PregameServiceClient(context);
            host.Open();

            host.NotifyServer(CurrentPlayer.Player);

            host.Close();
        }

        public void ReceiveMessage(Message message)
        {
            Label formattedMessage = new Label();
            formattedMessage.Content = message.AuthorUsername + ": " + message.Content;
            ChatPanel.Children.Add(formattedMessage);
        }

        private void ReturnToMainMenu()
        {
            InstanceContext context = new InstanceContext(this);
            PregameServiceClient host = new PregameServiceClient(context);

            int result = host.LeaveGame(CurrentPlayer.Player.Username, _gameRoomCode);

            if (result == 0)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
