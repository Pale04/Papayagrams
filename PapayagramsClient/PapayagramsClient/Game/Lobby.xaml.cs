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

            try
            {
                _host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new PopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                NavigationService.GoBack();
                return;
            }

            (int err, string gameRoomCode) = _host.CreateGame(CurrentPlayer.Player.Username);

            if (err != 0)
            {
                NavigationService.GoBack();
                return;
            }

            GameRoomCodeText.Content = gameRoomCode;
            _gameRoomCode = gameRoomCode;
        }

        public Lobby(string gameRoomCode)
        {
            if (string.IsNullOrEmpty(gameRoomCode))
            {
                NavigationService.GoBack();
                return;
            }

            InitializeComponent();

            InstanceContext context = new InstanceContext(this);
            _host = new PregameServiceClient(context);

            try
            {
                _host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new PopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                NavigationService.GoBack();
                return;
            }

            int result = _host.JoinGame(CurrentPlayer.Player.Username, gameRoomCode);

            switch (result)
            {
                case 0:
                    GameRoomCodeText.Content = gameRoomCode;
                    _gameRoomCode = gameRoomCode;
                    return;
            }

            NavigationService.GoBack();
        }

        ~Lobby()
        {
            _host.Close();
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
