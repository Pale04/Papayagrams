using PapayagramsClient.PapayagramsService;
using System;
using System.IO;
using System.ServiceModel;
using System.Windows.Controls;

namespace PapayagramsClient.Game
{
    /// <summary>
    /// Lógica de interacción para GameCreation.xaml
    /// </summary>
    public partial class GameCreation
    {
        public GameCreation()
        {
            InitializeComponent();
        }

        private void ReturnToMainMenu(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void CreateGame(object sender, System.Windows.RoutedEventArgs e)
        {
            //PapayagramsService.GameConfigurationDC gameConfig = new PapayagramsService.GameConfigurationDC();
        }
    }
}
