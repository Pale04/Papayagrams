using PapayagramsClient.ClientData;
using PapayagramsClient.PapayagramsService;
using System;
using System.ServiceModel;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PapayagramsClient.Game
{
    /// <summary>
    /// Lógica de interacción para Lobby.xaml
    /// </summary>
    public partial class Lobby : Page, IPregameServiceCallback
    {
        private string _gameRoomCode;
        private PregameServiceClient _host;

        public Lobby()
        {
            InitializeComponent();
            InstanceContext context = new InstanceContext(this);
            _host = new PregameServiceClient(context);

            _host.Open();
            (int code, string gameRoomCode) = _host.CreateGame(CurrentPlayer.Player.Username);
            GameRoomCodeText.Content = gameRoomCode;
            _gameRoomCode = gameRoomCode;


            //int connectionResult = host.NotifyServer(CurrentPlayer.Player);

     

            /*if (connectionResult != 0)
            {
                // TODO
                // add message when connecting to server returns an error code
                new PopUpWindow("","",1).ShowDialog();
                NavigationService.GoBack();
            }*/
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

        private void SendMessage(object sender, System.Windows.RoutedEventArgs e)
        {
            InstanceContext context = new InstanceContext(this);
            PregameServiceClient host = new PregameServiceClient(context);
            Message message = new Message
            {
                AuthorUsername = CurrentPlayer.Player.Username,
                Content = MessageTextbox.Text,
                GameRoomCode = _gameRoomCode
            };
            host.SendMessage(message);
            
        }
    }
}
