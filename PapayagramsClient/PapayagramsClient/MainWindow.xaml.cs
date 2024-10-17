using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
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

namespace PapayagramsClient
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, PapayagramsService.IPregameServiceCallback
    {
        private string _roomCode;

        public MainWindow()
        {
            InitializeComponent();
            BtJoinGame.IsEnabled = false;
            BtLeaveGame.IsEnabled = false;
        }

        public void JoinGameResponse(string roomCodeResponse)
        {
            _roomCode = roomCodeResponse;
            LbGameRoom.Content = "In room " + _roomCode;
            BtJoinGame.IsEnabled = false;
            BtLeaveGame.IsEnabled = true;
        }

        public void ReceiveMessage(PapayagramsService.Message message)
        {
            LbLastMessage.Content = message.Content;
        }

        private void LogIntoServer(object sender, RoutedEventArgs e)
        {
            PapayagramsService.LoginServiceClient loginService = new PapayagramsService.LoginServiceClient();
            if (loginService.Login("pale", "123") == 0)
            {
                LbLogin.Content = "Logged in";
                BtJoinGame.IsEnabled = true;
                BtLogin.IsEnabled = false;
            }
            else
            {
                LbLogin.Content = "Error logging in";
            }
        }

        private void SendJoinGameRequest(object sender, RoutedEventArgs e)
        {
            System.ServiceModel.InstanceContext context = new System.ServiceModel.InstanceContext(this);

            PapayagramsService.PregameServiceClient gameService = new PapayagramsService.PregameServiceClient(context);
            gameService.CreateGame("pale");
        }

        private void SendLeaveGameRequest(object sender, RoutedEventArgs e)
        {
            System.ServiceModel.InstanceContext context = new System.ServiceModel.InstanceContext(this);

            PapayagramsService.PregameServiceClient gameService = new PapayagramsService.PregameServiceClient(context);
            if (gameService.LeaveGame("pale", _roomCode) == 0)
            {
                LbGameRoom.Content = "Not in game room";
                BtJoinGame.IsEnabled = true;
                BtLeaveGame.IsEnabled = false;
            }
        }

        private void SendMessageToRoom(object sender, RoutedEventArgs e)
        {
            System.ServiceModel.InstanceContext context = new System.ServiceModel.InstanceContext(this);

            PapayagramsService.PregameServiceClient chat = new PapayagramsService.PregameServiceClient(context);
            PapayagramsService.Message message = new PapayagramsService.Message();
            message.Content = TMessage.Text;
            message.AuthorUsername = "pale";
            message.GameRoomCode = _roomCode;
            message.Time = DateTime.Now;

            chat.SendMessage(message);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PapayagramsService.UserServiceClient userServiceClient = new PapayagramsService.UserServiceClient();
            PapayagramsService.PlayerDC newPlayer = new PapayagramsService.PlayerDC
            {
                Username = "Pale",
                Email = "epalemolina@hotmail.com",
                Password = "1234"
            };
            Console.WriteLine(userServiceClient.RegisterUser(newPlayer));
        }
    }
}
