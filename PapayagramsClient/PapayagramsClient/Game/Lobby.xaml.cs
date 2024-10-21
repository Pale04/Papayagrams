using PapayagramsClient.ClientData;
using PapayagramsClient.Login.Popups;
using PapayagramsClient.PapayagramsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    public partial class Lobby : Page, IPregameServiceCallback
    {
        private string _gameRoomCode;

        public Lobby(string gameRoomCode)
        {
            InitializeComponent();
            GameRoomCodeText.Content = gameRoomCode;

            InstanceContext context = new InstanceContext(this);
            PregameServiceClient host = new PregameServiceClient(context);
            host.Open();

            int connectionResult = host.NotifyServer(CurrentPlayer.Player);

            host.Close();

            if (connectionResult != 0)
            {
                // TODO
                // add message when connecting to server returns an error code
                new PopUpWindow("","",1).ShowDialog();
                NavigationService.GoBack();
            }
        }

        public void JoinGameResponse(string roomCode)
        {
            // TODO
            throw new NotImplementedException();
        }

        public void ReceiveMessage(Message message)
        {
            Label formattedMessage = new Label();
            formattedMessage.Content = message.AuthorUsername + ": " + message.Content;
            ChatPanel.Children.Add(formattedMessage);
        }

        public void RefreshLobby()
        {
            // TODO
            throw new NotImplementedException();
        }

        public void StartGameResponse()
        {
            // TODO
            throw new NotImplementedException();
        }

        private void ReturnToMainMenu()
        {
            InstanceContext context = new InstanceContext(this);
            PregameServiceClient host = new PregameServiceClient(context);

            int result = host.LeaveLobby(CurrentPlayer.Player.Username, CurrentGame.RoomCode);

            if (result == 0)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
