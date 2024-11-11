using PapayagramsClient.ClientData;
using PapayagramsClient.PapayagramsService;
using System.ServiceModel;
using System.Windows.Controls;
using NHunspell;

namespace PapayagramsClient.Game
{
    public partial class Game : Page, IGameServiceCallback
    {
        private string spellCheckDict;
        private string spellCheckAff;
        private GameServiceClient _host;
        public Game()
        {
            if (CurrentPlayer.Configuration.Language.Equals(LanguageDC.Spanish))
            {
                spellCheckAff = "../Resources/Dictionaries/es_MX.aff";
                spellCheckDict = "../Resources/Dictionaries/es_MX.dic";
            }
            else if (CurrentPlayer.Configuration.Language.Equals(LanguageDC.English))
            {
                spellCheckAff = "../Resources/Dictionaries/en_US.aff";
                spellCheckDict = "../Resources/Dictionaries/en_US.dic";
            }
            else
            {
                return;
            }

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

            _host.ReachServer(CurrentPlayer.Player.Username, CurrentGame.RoomCode);

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
            // TODO: Agregar popup de confirmación para salir
            _host.LeaveGame(CurrentGame.RoomCode, CurrentPlayer.Player.Username);
            NavigationService.Navigate(new MainMenu());
        }

        private void DumpPiece(string piece)
        {
            if (CurrentGame.GameData.PilePieces < CurrentGame.PlayersInRoom.Count)
            {
                // show message of cant dump
                return;
            }

            // tirar una pieza
            throw new System.NotImplementedException();
        }

        private void playSeed(string piece)
        {
            // Poner una pieza
            throw new System.NotImplementedException();
        }

        private bool verifyWord(string word)
        {
            using (var hunspell = new Hunspell(spellCheckAff, spellCheckDict))
            {
                hunspell.Spell(word);
                // TODO
            }

            throw new System.NotImplementedException();
        }

        private void renderPieces() { 
            throw new System.NotImplementedException(); 
        }

        public void ReceiveStartingHand(char[] initialPieces)
        {
            foreach (var piece in initialPieces)
            {
                CurrentGame.GameData.PiecesInHand.AddLast(piece.ToString());
            }
        }

        public void AddDumpSeedsToHand(string[] pieces)
        {
            foreach (var piece in pieces)
            {
                CurrentGame.GameData.PiecesInHand.AddLast(piece);
            }
        }

        public void AddSeedToHand(string piece)
        {
            CurrentGame.GameData.PiecesInHand.AddLast(piece);
        }

        public void RestrictDump()
        {
            throw new System.NotImplementedException();
        }

        public void EndGame(string winner)
        {
            throw new System.NotImplementedException();
        }
    }
}
