using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PapayagramsClient.Menu
{
    public partial class Leaderboards : Page
    {
        public Leaderboards()
        {
            InitializeComponent();
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
