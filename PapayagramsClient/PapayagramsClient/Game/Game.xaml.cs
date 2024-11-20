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
                    WPFGameBoardPieceSpot spot = new WPFControls.WPFGameBoardPieceSpot();
                    Grid.SetColumn(spot, i);
                    Grid.SetRow(spot, j);
                    BoardGrid.Children.Add(spot);
                }
            }
        }

        public void RefreshGameRoom(string roomCode)
        {
            throw new NotImplementedException();
        }

        private void LeaveGame()
        {
            if ((bool)new SelectionPopUpWindow(Properties.Resources.gameLeaveGameTitle, Properties.Resources.gameLeaveGame, 4).ShowDialog())
            {
                _host.LeaveGame(CurrentGame.RoomCode, CurrentPlayer.Player.Username);
                NavigationService.Navigate(new MainMenu());
            }
        }

        private void DumpSeed(object sender, RoutedEventArgs e)
        {
            //if (CurrentGame.GameData.PilePieces < CurrentGame.PlayersInRoom.Count)
            //{
                // show message of cant dump
                //return;
            //}

            WPFGamePiece piece = (WPFGamePiece)sender;
            Console.WriteLine("piece " + piece.PieceLetter.Text + " dumped.");
            _host.DumpPiece(piece.PieceLetter.Text);
            PiecesPanel.Children.Remove(piece);
        }

        private void PlaySeed(object sender, RoutedEventArgs e)
        {
            WPFGamePiece piece = (WPFGamePiece)sender;
            PiecesPanel.Children.Remove(piece);
        }

        public void ReceiveStartingHand(char[] initialPieces)
        {
            foreach (var letter in initialPieces)
            {
                PiecesPanel.Children.Add(new WPFGamePiece(letter.ToString()));
            }
        }

        public void AddDumpSeedsToHand(string[] pieces)
        {
            foreach (var letter in pieces)
            {
                PiecesPanel.Children.Add(new WPFGamePiece(letter));
            }
        }

        public void AddSeedToHand(string piece)
        {
            PiecesPanel.Children.Add(new WPFGamePiece(piece));
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
    }
}
