using PapayagramsClient.PapayagramsService;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class MainWindow : Window, IMainMenuServiceCallback
    {
        public MainWindow()
        {
            InitializeComponent();
            Cursor = new Cursor(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Cursors\\papaya.cur");
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
