using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PapayagramsClient.Menu
{
    public partial class Configuration : Page
    {
        public Configuration()
        {
            InitializeComponent();
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void GoToChangePassword(object sender, RoutedEventArgs e)
        {

        }
    }
}
