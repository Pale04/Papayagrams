using log4net.Config;
using PapayagramsClient.PapayagramsService;
using System;
using System.Windows;

namespace PapayagramsClient
{
    public partial class MainWindow : Window, IMainMenuServiceCallback
    {
        public MainWindow()
        {
            var logRepository = log4net.LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new System.IO.FileInfo("log4net.config"));

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
