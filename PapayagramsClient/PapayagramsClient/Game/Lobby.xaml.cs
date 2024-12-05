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
                NavigationService.GoBack();
                return;
            }

            (int returnCode, GameRoomDC gameRoom) = _host.CreateGame(CurrentPlayer.Player.Username, gameConfig);

            if (returnCode != 0)
            {
                NavigationService.GoBack();
                return;
            }

            GameRoomCodeText.Content = Properties.Resources.joinGameCode + gameRoom.RoomCode;
            CurrentGame.RoomCode = gameRoom.RoomCode;
            CurrentGame.State = CurrentGame.GameState.InLobby;
            CurrentGame.PlayersInRoom = gameRoom.Players.ToList();
            CurrentGame.GameConfig = gameConfig;
            RefreshLobby(gameRoom);

            FriendsOverlay.AddFriendButton.Visibility = Visibility.Hidden;
            FriendsOverlay.AddFriendButton.IsEnabled = false;
            FriendsOverlay.NewFriendUsernameTextBox.IsEnabled = false;
            FriendsOverlay.NewFriendUsernameTextBox.Visibility = Visibility.Hidden;
            FriendsOverlay.SwitchViewButton.IsEnabled = false;
            FriendsOverlay.SwitchViewButton.Visibility = Visibility.Hidden;
            FriendsOverlay.FillFriendsListForInvitations();
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
                return;
            }

            (int returnCode, GameRoomDC gameRoom) = _host.JoinGame(CurrentPlayer.Player.Username, gameRoomCode);

            switch (returnCode)
            {
                case 0:
                    InitializeComponent();
                    CreateGameButton.Visibility = Visibility.Hidden;
                    CreateGameButton.IsEnabled = false;
                    GameRoomCodeText.Content = Properties.Resources.joinGameCode + gameRoomCode;
                    CurrentGame.RoomCode = gameRoom.RoomCode;
                    CurrentGame.State = CurrentGame.GameState.InLobby;
                    CurrentGame.PlayersInRoom = gameRoom.Players.ToList();
                    CurrentGame.GameConfig = gameRoom.GameConfiguration;
                    RefreshLobby(new GameRoomDC
                    {
                        Players = CurrentGame.PlayersInRoom.ToArray(),
                    });
                    FriendsOverlay.AddFriendButton.Visibility = Visibility.Hidden;
                    FriendsOverlay.AddFriendButton.IsEnabled = false;
                    FriendsOverlay.NewFriendUsernameTextBox.IsEnabled = false;
                    FriendsOverlay.NewFriendUsernameTextBox.Visibility = Visibility.Hidden;
                    FriendsOverlay.SwitchViewButton.IsEnabled = false;
                    FriendsOverlay.SwitchViewButton.Visibility = Visibility.Hidden;
                    FriendsOverlay.FillFriendsListForInvitations();
                    return;

                case 102:
                    new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorDatabaseConnection, 3).ShowDialog();
                    break;

                case 401:
                    new SelectionPopUpWindow(Properties.Resources.lobbyRoomNotFoundTitle, Properties.Resources.lobbyRoomNotFound, 2).ShowDialog();
                    break;
            }
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

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            _host.LeaveLobby(CurrentPlayer.Player.Username, CurrentGame.RoomCode);
            NavigationService.Navigate(new MainMenu());
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

            _host.SendMessage(message);
            MessageTextbox.Text = "";
        }

        private void CreateGame(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Players in game:");
            foreach (PlayerDC player in CurrentGame.PlayersInRoom)
            {
                Console.WriteLine(player.Username);
            }

            if (CurrentGame.PlayersInRoom.Count < 1)
            {
                return;
            }

            if (CurrentGame.PlayersInRoom[0].Username == CurrentPlayer.Player.Username)
            {
                _host.StartGame(CurrentGame.RoomCode);
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

            _host.InviteFriend(friendPanel.UsernameLabel.Text);
            new SelectionPopUpWindow(Properties.Resources.lobbyFriendInvited, Properties.Resources.lobbyFriendInvited, 0).ShowDialog();
        }

        private void CloseFriendsOverlay(object sender, RoutedEventArgs e)
        {
            FriendsOverlay.Visibility = Visibility.Hidden;
            FriendsOverlay.IsEnabled = false;
        }
    }
}
