using PapayagramsClient.ClientData;
using PapayagramsClient.PapayagramsService;
using System.ServiceModel;
using System.Windows.Controls;
using NHunspell;
using System;

namespace PapayagramsClient.Game
{
    public partial class Game : Page, IGameServiceCallback
    {
        private string _spellCheckDict;
        private string _spellCheckAff;
        private GameServiceClient _host;

        public Game()
        {
            Console.WriteLine("Entering game...");
            ChooseLanguageDictionary();

            if (_spellCheckAff == null)
            {
                return;
            }

            InitializeComponent();
            FillGameGrids();

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
        }

        ~Game()
        {
            _host.Close();
        }

        private void ChooseLanguageDictionary()
        {
            if (CurrentGame.GameConfig.WordsLanguage.Equals(LanguageDC.Spanish))
            {
                _spellCheckAff = "../Resources/Dictionaries/es_MX.aff";
                _spellCheckDict = "../Resources/Dictionaries/es_MX.dic";
            }
            else if (CurrentGame.GameConfig.WordsLanguage.Equals(LanguageDC.English))
            {
                _spellCheckAff = "../Resources/Dictionaries/en_US.aff";
                _spellCheckDict = "../Resources/Dictionaries/en_US.dic";
            }
        }

        private void FillGameGrids()
        {
            for (int i = 0; i < 25; i++)
            {
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
                BoardGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    WPFControls.WPFGameBoardPieceSpot spot = new WPFControls.WPFGameBoardPieceSpot();
                    Grid.SetColumn(spot, i);
                    Grid.SetRow(spot, j);
                    BoardGrid.Children.Add(spot);
                }
            }
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

        private void PlaySeed(string piece)
        {
            // Poner una pieza
            throw new System.NotImplementedException();
        }

        private bool VerifyWord(string word)
        {
            using (var hunspell = new Hunspell(_spellCheckAff, _spellCheckDict))
            {
                hunspell.Spell(word);
                // TODO
            }

            throw new System.NotImplementedException();
        }

        public void ReceiveStartingHand(char[] initialPieces)
        {
            Console.WriteLine("Received pieces......");
            foreach (var letter in initialPieces)
            {
                PiecesPanel.Children.Add(new WPFControls.WPFGamePiece(letter.ToString()) { Width = 50, Height = 60 });
            }
        }

        public void AddDumpSeedsToHand(string[] pieces)
        {
            foreach (var letter in pieces)
            {
                PiecesPanel.Children.Add(new WPFControls.WPFGamePiece(letter) { Width = 50 });
            }
        }

        public void AddSeedToHand(string piece)
        {
            PiecesPanel.Children.Add(new WPFControls.WPFGamePiece(piece) { Width = 50 });
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
