using log4net;
using PapayagramsClient.ClientData;
using PapayagramsClient.PapayagramsService;
using PapayagramsClient.WPFControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace PapayagramsClient.Game
{
    public partial class Lobby : Page, IPregameServiceCallback
    {
        private PregameServiceClient _host;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Lobby));


        // Create game room with configuration x
        public Lobby(GameConfigurationDC gameConfig)
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
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                _logger.Fatal("Couldn't connect to server to create lobby");
                return;
            }

            (int returnCode, GameRoomDC gameRoom) = _host.CreateGame(CurrentPlayer.Player.Username, gameConfig);

            switch (returnCode)
            {
                case 102:
                    new SelectionPopUpWindow(Properties.Resources.errorDatabaseConnection, Properties.Resources.errorDatabaseConnection, 3).ShowDialog();
                    break ;

                case 0:
                    break;
            }

            GameRoomCodeText.Content = Properties.Resources.joinGameCode + ": " + gameRoom.RoomCode;
            CurrentGame.RoomCode = gameRoom.RoomCode;
            CurrentGame.State = CurrentGame.GameState.InLobby;
            CurrentGame.PlayersInRoom = gameRoom.Players.ToList();
            CurrentGame.GameConfig = gameConfig;
            RefreshLobby(gameRoom);

            SetFriendsOverlay();
        }

        // Join to game room with code x
        public Lobby(string gameRoomCode)
        {
            if (string.IsNullOrEmpty(gameRoomCode))
            {
                return;
            }

            InstanceContext context = new InstanceContext(this);
            _host = new PregameServiceClient(context);

            try
            {
                _host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                _logger.Fatal("Couldn't connect to server to join lobby");
                return;
            }

            (int returnCode, GameRoomDC gameRoom) = _host.JoinGame(CurrentPlayer.Player.Username, gameRoomCode);

            switch (returnCode)
            {
                case 0:
                    InitializeComponent();
                    CreateGameButton.Visibility = Visibility.Hidden;
                    CreateGameButton.IsEnabled = false;
                    GameRoomCodeText.Content = Properties.Resources.joinGameCode + ": " + gameRoomCode;
                    CurrentGame.RoomCode = gameRoom.RoomCode;
                    CurrentGame.State = CurrentGame.GameState.InLobby;
                    CurrentGame.PlayersInRoom = gameRoom.Players.ToList();
                    CurrentGame.GameConfig = gameRoom.GameConfiguration;
                    RefreshLobby(new GameRoomDC
                    {
                        Players = CurrentGame.PlayersInRoom.ToArray(),
                    });
                    SetFriendsOverlay();
                    return;

                case 102:
                    new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorDatabaseConnection, 3).ShowDialog();
                    break;

                case 401:
                    new SelectionPopUpWindow(Properties.Resources.lobbyRoomNotFoundTitle, Properties.Resources.lobbyRoomNotFound, 2).ShowDialog();
                    break;
            }
        }

        private void SetFriendsOverlay()
        {
            FriendsOverlay.AddFriendButton.Visibility = Visibility.Hidden;
            FriendsOverlay.AddFriendButton.IsEnabled = false;
            FriendsOverlay.NewFriendUsernameTextBox.IsEnabled = false;
            FriendsOverlay.NewFriendUsernameTextBox.Visibility = Visibility.Hidden;
            FriendsOverlay.SwitchViewButton.IsEnabled = false;
            FriendsOverlay.SwitchViewButton.Visibility = Visibility.Hidden;
            FriendsOverlay.FillFriendsListForInvitations();
        }

        ~Lobby()
        {
            try
            {
                _host.Close();
            }
            catch (CommunicationObjectFaultedException)
            {
                _logger.Fatal("Couldn't close server connection at lobby");
            }
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

            if (CurrentPlayer.Player.Username == CurrentGame.PlayersInRoom[0].Username)
            {
                CreateGameButton.Visibility = Visibility.Visible;
                CreateGameButton.IsEnabled = true;
            }
            else
            {
                CreateGameButton.Visibility = Visibility.Hidden;
                CreateGameButton.IsEnabled = false;
            }

            PlayersStackPanel.Children.Clear();

            foreach (PlayerDC player in CurrentGame.PlayersInRoom)
            {
                Grid playerGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition(),
                        new ColumnDefinition(),
                    }
                };

                Label usernameLabel = new Label
                {
                    Content = player.Username,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                playerGrid.Children.Add(usernameLabel);
                Grid.SetColumn(usernameLabel, 1);

                Image playerImage = new Image 
                { 
                    Source = ImagesService.GetImageFromId(player.ProfileIcon), 
                    Height = 30, 
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                playerGrid.Children.Add(playerImage);
                Grid.SetColumn(playerImage, 0);

                PlayersStackPanel.Children.Add(playerGrid);
            }
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            try
            {
                _host.LeaveLobby(CurrentPlayer.Player.Username, CurrentGame.RoomCode);
            }
            catch (CommunicationObjectFaultedException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                _logger.Fatal("Couldn't connect to server to leave game");
                NavigationService.Navigate(new Login.Login());
                return;
            }

            CurrentGame.RoomCode = "";
            CurrentGame.PlayersInRoom = new List<PlayerDC>();
            if (CurrentPlayer.IsGuest)
            {
                NavigationService.Navigate(new Login.Login());
            }
            else
            {
                NavigationService.Navigate(new MainMenu());
            }
        }

        private void SendMessage()
        {
            if (string.IsNullOrEmpty(MessageTextbox.Text))
            {
                return;
            }

            Message message = new Message
            {
                AuthorUsername = CurrentPlayer.Player.Username,
                Content = MessageTextbox.Text.Trim(),
                GameRoomCode = CurrentGame.RoomCode
            };

            MessageTextbox.Text = "";

            try
            {
                _host.SendMessage(message);
            }
            catch (CommunicationObjectFaultedException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                _logger.Fatal("Couldn't connect to server to join lobby");
                NavigationService.Navigate(new Login.Login());
                return;
            }
        }

        private void CreateGame(object sender, RoutedEventArgs e)
        {
            if (CurrentGame.PlayersInRoom.Count < 1)
            {
                return;
            }

            if (CurrentGame.PlayersInRoom[0].Username == CurrentPlayer.Player.Username)
            {
                try
                {
                    _host.StartGame(CurrentGame.RoomCode);
                }
                catch (CommunicationObjectFaultedException)
                {
                    new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                    _logger.Fatal("Couldn't connect to server to start game");
                    NavigationService.Navigate(new Login.Login());
                    return;
                }

                NavigationService.Navigate(new Game());
            }
        }

        public void CarryInsideGame()
        {
            NavigationService.Navigate(new Game());
        }

        private void SendMessage(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                SendMessage();
            }
        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void OpenFriendOverlay(object sender, RoutedEventArgs e)
        {
            FriendsOverlay.Visibility = Visibility.Visible;
            FriendsOverlay.IsEnabled = true;
        }

        private void InviteFriendToGame(object sender, RoutedEventArgs e)
        {
            FriendInfoPanel friendPanel = (FriendInfoPanel)sender;

            foreach(PlayerDC player in CurrentGame.PlayersInRoom)
            {
                if (player.Username == friendPanel.UsernameLabel.Text)
                {
                    return;
                }
            }

            try
            {
                _host.InviteFriend(CurrentPlayer.Player.Username, friendPanel.UsernameLabel.Text, CurrentGame.RoomCode);
            }
            catch (CommunicationObjectFaultedException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                _logger.Fatal("Couldn't connect to server to invite friend");
                NavigationService.Navigate(new Login.Login());
                return;
            }

            new SelectionPopUpWindow(Properties.Resources.lobbyFriendInvited, Properties.Resources.lobbyFriendInvited, 0).ShowDialog();
        }

        private void CloseFriendsOverlay(object sender, RoutedEventArgs e)
        {
            FriendsOverlay.Visibility = Visibility.Hidden;
            FriendsOverlay.IsEnabled = false;
        }
    }
}
