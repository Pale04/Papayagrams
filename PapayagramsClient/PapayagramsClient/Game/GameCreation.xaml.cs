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
    public partial class GameCreation : Page, PapayagramsService.IPregameServiceCallback
    {
        public GameCreation()
        {
            InitializeComponent();

            NavigationService.Navigate(new Lobby());
        }

        public void ReceiveMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public void RefreshLobby()
        {
            throw new NotImplementedException();
        }

        public void StartGameResponse()
        {
            throw new NotImplementedException();
        }
    }
}
