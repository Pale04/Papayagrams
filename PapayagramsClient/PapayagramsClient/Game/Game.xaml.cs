using PapayagramsClient.ClientData;
using PapayagramsClient.PapayagramsService;
using System.ServiceModel;
using System.Windows.Controls;
using System;
using System.Windows;
using PapayagramsClient.WPFControls;
using System.Collections.Generic;

namespace PapayagramsClient.Game
{
    public partial class Game : Page, IGameServiceCallback
    {
        private GameServiceClient _host;

        public Game()
        {
            Console.WriteLine("Entering game...");
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
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                NavigationService.GoBack();
                return;
            }

            _host.ReachServer(CurrentPlayer.Player.Username, CurrentGame.RoomCode);
            CurrentGame.GameData = new GameProgressData()
            {
                PilePieces = 144,
                Points = 0
            };
        }

        ~Game()
        {
            _host.Close();
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
                    WPFGameBoardPieceSpot spot = new WPFGameBoardPieceSpot();
                    Grid.SetColumn(spot, i);
                    Grid.SetRow(spot, j);
                    BoardGrid.Children.Add(spot);
                }
            }
        }

        private void LeaveGame(object sender, RoutedEventArgs e)
        {
            if ((bool)new SelectionPopUpWindow(Properties.Resources.gameLeaveGameTitle, Properties.Resources.gameLeaveGame, 4).ShowDialog())
            {
                _host.LeaveGame(CurrentGame.RoomCode, CurrentPlayer.Player.Username);
                NavigationService.Navigate(new MainMenu());
            }
        }

        private void DumpSeedFromHand(object sender, RoutedEventArgs e)
        {
            WPFGamePiece piece = (WPFGamePiece)sender;
            if (DumpSeed(piece.PieceLetter.Text))
            {
                PiecesPanel.Children.Remove(piece);
            }
        }

        private void DumpSeedFromBoard(object sender, RoutedEventArgs e)
        {
            WPFGameBoardPieceSpot piece = (WPFGameBoardPieceSpot)sender;
            if (DumpSeed((string)piece.LetterLabel.Content))
            {
                piece.LetterLabel.Content = "";
                piece.MainGrid.Background = null;
            }
        }

        private bool DumpSeed(string letter)
        {
            if (CurrentGame.GameData.PilePieces < CurrentGame.PlayersInRoom.Count)
            {
                // message of not enough pieces
                return false;
            }

            Console.WriteLine("piece " + letter + " dumped.");
            _host.DumpPiece(CurrentGame.RoomCode, CurrentPlayer.Player.Username, char.Parse(letter));

            return true;
        }

        private void PlaySeed(object sender, RoutedEventArgs e)
        {
            WPFGamePiece piece = (WPFGamePiece)sender;
            PiecesPanel.Children.Remove(piece);
        }

        private (int points, List<string> correctWords) EvaluateBoard()
        {
            return (0, null);
        }

        public void EndGame(string winner)
        {
            throw new NotImplementedException();
        }

        public void RefreshTimer(int remainingMinutes)
        {
            throw new NotImplementedException();
        }

        private void PickupSeed(object sender, RoutedEventArgs e)
        {
            WPFGameBoardPieceSpot piece = (WPFGameBoardPieceSpot)sender;
            if (string.IsNullOrEmpty((string)piece.LetterLabel.Content))
            {
                return;
            }

            Console.WriteLine("picked up piece: " +  piece.LetterLabel.Content);

            string pieceLetter = (string)piece.LetterLabel.Content;

            PiecesPanel.Children.Add(new WPFGamePiece(pieceLetter));

            piece.LetterLabel.Content = "";
            piece.MainGrid.Background = null;
        }

        private void MovePiece(object sender, RoutedEventArgs e)
        {
            WPFGameBoardPieceSpot piece = (WPFGameBoardPieceSpot)sender;
            Console.WriteLine("moved piece: " +  piece.LetterLabel.Content);
            piece.LetterLabel.Content = "";
            piece.MainGrid.Background = null;
        }

        public void RefreshGameRoom(Stack<char> piecesPile, PlayerDC[] connectedPlayers)
        {
            CurrentGame.GameData.PilePieces = piecesPile.Count;
        }

        public void AddSeedsToHand(char[] pieces)
        {
            foreach (var letter in pieces)
            {
                PiecesPanel.Children.Add(new WPFGamePiece(letter.ToString()));
            }
        }

        public void EndGame()
        {
            throw new NotImplementedException();
        }
    }
}
