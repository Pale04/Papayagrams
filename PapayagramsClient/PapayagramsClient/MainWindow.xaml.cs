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
    public partial class MainWindow : Window, PapayagramsService.IChatServiceCallback, PapayagramsService.IGameServiceCallback
    {
        private string _roomCode;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void GameResponse(string roomCodeResponse)
        {
            //L1.Content = roomCodeResponse;
            _roomCode = roomCodeResponse;
            System.ServiceModel.InstanceContext context = new System.ServiceModel.InstanceContext(this);
            PapayagramsService.ChatServiceClient chatService = new PapayagramsService.ChatServiceClient(context);
            chatService.SendMessage("Hello from the client",roomCodeResponse);

            Console.WriteLine("Room code: " + roomCodeResponse);

            PapayagramsService.LoginServiceClient loginService = new PapayagramsService.LoginServiceClient();
            loginService.Logout();
        }

        public void ReceiveMessage(string message)
        {
            L1.Content = message;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.ServiceModel.InstanceContext context = new System.ServiceModel.InstanceContext(this);

            PapayagramsService.LoginServiceClient loginService = new PapayagramsService.LoginServiceClient();
            loginService.Login("pale", "123");

            PapayagramsService.GameServiceClient gameService = new PapayagramsService.GameServiceClient(context);
            gameService.CreateGame();

            
        }
    }
}
