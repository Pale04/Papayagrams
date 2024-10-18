using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
    /// Lógica de interacción para MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void CreateNewGame(object sender, RoutedEventArgs e)
        {
            InstanceContext context = new InstanceContext(this);
            PapayagramsService.IMainMenuClient host = new PapayagramsService.IMainMenuClient(context);
            host.Open();

            host.CreateGame(CurrentPlayer.Player);

            host.Close();
        }
    }
}
