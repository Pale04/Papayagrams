using PapayagramsClient.ClientData;
using PapayagramsClient.PapayagramsService;
using System;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PapayagramsClient.Game
{
    /// <summary>
    /// Lógica de interacción para Lobby.xaml
    /// </summary>
    public partial class Lobby : Page, IPregameServiceCallback
    {
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

            (int err, GameRoomDC gameRoom) = _host.CreateGame(CurrentPlayer.Player.Username);

            if (err != 0)
            {
                NavigationService.GoBack();
                return;
            }

            GameRoomCodeText.Content = gameRoom.RoomCode ;
            CurrentGame.RoomCode = gameRoom.RoomCode;
            CurrentGame.State = CurrentGame.GameState.InLobby;
            CurrentGame.PlayersInRoom = gameRoom.Players.ToList();
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

            (int err, GameRoomDC gameRoom) = _host.JoinGame(CurrentPlayer.Player.Username, gameRoomCode);

            switch (err)
            {
                case 0:
                    GameRoomCodeText.Content = gameRoomCode;
                    CurrentGame.RoomCode = gameRoom.RoomCode;
                    CurrentGame.State = CurrentGame.GameState.InLobby;
                    CurrentGame.PlayersInRoom = gameRoom.Players.ToList();
                    return;

                case 102:
                    new PopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorDatabaseConnection, 3).ShowDialog();
                    break;

                case 401:
                    new PopUpWindow(Properties.Resources.lobbyRoomNotFoundTitle, Properties.Resources.lobbyRoomNotFound, 2).ShowDialog();
                    break;
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

        public void RefreshLobby(GameRoomDC gameRoom)
        {
            CurrentGame.PlayersInRoom = gameRoom.Players.ToList();

            PlayersStackPanel.Children.Clear();

            foreach (PlayerDC player in CurrentGame.PlayersInRoom)
            {
                Grid playerGrid = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition(),
                        new RowDefinition(),
                    },
                    ColumnDefinitions =
                    {
                        new ColumnDefinition(),
                    }
                };

                Label usernameLabel = new Label
                {
                    Content = player.Username
                };
                playerGrid.Children.Add(usernameLabel);
                Grid.SetColumn(usernameLabel, 0);
                Grid.SetRow(usernameLabel, 1);

                Image playerImage = new Image();
                playerGrid.Children.Add(playerImage);
                Grid.SetColumn(playerImage, 0);
                Grid.SetRow(playerImage, 0);

                PlayersStackPanel.Children.Add(playerGrid);
            }
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
                GameRoomCode = CurrentGame.RoomCode
            };
            host.SendMessage(message);
            
        }
    }
}
