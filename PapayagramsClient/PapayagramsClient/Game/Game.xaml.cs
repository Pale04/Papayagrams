using PapayagramsClient.PapayagramsService;
using System.ServiceModel;
using System.Windows.Controls;

namespace PapayagramsClient.Game
{
    public partial class Game : Page, IGameServiceCallback
    {
        private GameServiceClient _host;
        public Game()
        {
            InstanceContext context = new InstanceContext(this);
            _host = new GameServiceClient(context);

            try
            {
                _host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new PopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                NavigationService.GoBack();
                return;
            }

            _host.ReachServer(CurrentPlayer.Player.Username);

            InitializeComponent();
        }

        ~Game()
        {
            _host.Close();
        }

        public void RefreshGameRoom(string roomCode)
        {
            throw new System.NotImplementedException();
        }

        private void LeaveGame()
        {
            throw new System.NotImplementedException();
        }

        private void PutPiece(string letter)
        {
            throw new System.NotImplementedException();
        }

        private void DumpPiece(string piece)
        {
            throw new System.NotImplementedException();
        }

        private void TakeSeed()
        {
            throw new System.NotImplementedException();
        }
    }
}
