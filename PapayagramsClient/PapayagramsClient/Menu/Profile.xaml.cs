using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PapayagramsClient.Menu
{
    /// <summary>
    /// Lógica de interacción para Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        public Profile()
        {
            InitializeComponent();
            SetPlayerData();
            BackImage.SetImage("../Resources/Icons/back-svgrepo-com.svg");
        }

        private void SetPlayerData()
        {
            throw new NotImplementedException();
            // TODO: get player stats from server
            PlayedGamesLabel.Content = Properties.Resources.profilePlayedGames + "";
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
