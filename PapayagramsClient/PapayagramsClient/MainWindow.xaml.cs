using PapayagramsClient.PapayagramsService;
using System;
using System.Windows;

namespace PapayagramsClient
{
    public partial class MainWindow : Window, IMainMenuServiceCallback
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Login.Login());
        }

        public void ReceiveFriendRequest(PlayerDC player)
        {
            throw new NotImplementedException();
        }

        public void ReceiveGameInvitation(GameInvitationDC invitation)
        {
            throw new NotImplementedException();
        }
    }
}
