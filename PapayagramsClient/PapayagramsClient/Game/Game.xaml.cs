using PapayagramsClient.ClientData;
using PapayagramsClient.PapayagramsService;
using System.ServiceModel;
using System.Windows.Controls;
using System;
using System.Windows;
using PapayagramsClient.WPFControls;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace PapayagramsClient.Game
{
    public partial class Game : Page, IGameServiceCallback
    {
        private GameServiceClient _host;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Game));

        private List<string> _createdWords;

        private const double MID_WORD_MULTIPLIER = 1.2;
        private const double LARGE_WORD_MULTIPLIER = 1.5;
        private const int FINISH_PIECES_POINTS = 4;

        public Game()
        {
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
                _logger.Fatal("Couldn't reach server when joining game: " + CurrentGame.RoomCode);
                return;
            }

            _host.ReachServer(CurrentPlayer.Player.Username, CurrentGame.RoomCode);
            CurrentGame.GameData = new GameProgressData()
            {
                PilePieces = 144,
                Points = 0
            };

            ShowCurrentPlayers();
        }

        ~Game()
        {
            try
            {
                _host.Close();
            }
            catch (CommunicationObjectFaultedException)
            {
                _logger.Fatal("Couldn't close server connection at game");
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
                    WPFGameBoardPieceSpot spot = new WPFGameBoardPieceSpot();
                    Grid.SetColumn(spot, i);
                    Grid.SetRow(spot, j);
                    BoardGrid.Children.Add(spot);
                }
            }
        }

        private void ShowCurrentPlayers()
        {
            PlayersStackPanel.Children.Clear();

            foreach (PlayerDC player in CurrentGame.PlayersInRoom)
            {
                Label playerLabel = new Label();
                playerLabel.Content = player.Username;
                PlayersStackPanel.Children.Add(playerLabel);
            }
        }

        private void LeaveGame(object sender, RoutedEventArgs e)
        {
            if ((bool)new SelectionPopUpWindow(Properties.Resources.gameLeaveGameTitle, Properties.Resources.gameLeaveGame, 4).ShowDialog())
            {
                try
                {
                    _host.LeaveGame(CurrentGame.RoomCode, CurrentPlayer.Player.Username, false);
                }
                catch (CommunicationObjectFaultedException)
                {
                    _logger.Fatal("Couldn't connect to server to leave game");
                    NavigationService.Navigate(new Login.Login());
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                    return;
                }

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
            if (CurrentGame.GameData.PilePieces <= 3)
            {
                // message of not enough pieces
                return false;
            }

            Console.WriteLine("piece " + letter + " dumped.");
            try
            {
                _host.DumpPiece(CurrentGame.RoomCode, CurrentPlayer.Player.Username, char.Parse(letter));
            }
            catch (CommunicationObjectFaultedException)
            {
                _logger.Fatal("Couldn't connect to server to dump piece");
                NavigationService.Navigate(new Login.Login());
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                return false;
            }

            return true;
        }

        private void PlaySeed(object sender, RoutedEventArgs e)
        {
            WPFGamePiece piece = (WPFGamePiece)sender;
            PiecesPanel.Children.Remove(piece);

            if (PiecesPanel.Children.Count < 1)
            {
                if (CurrentGame.GameData.PilePieces < CurrentGame.PlayersInRoom.Count)
                {
                    _host.ShoutPapaya(CurrentGame.RoomCode);
                    return;
                }

                    try
                    {
                        _host.TakeSeed(CurrentGame.RoomCode);
                    }
                    catch (CommunicationObjectFaultedException)
                    {
                        _logger.Fatal("Couldn't connect to server to take seeds");
                        NavigationService.Navigate(new Login.Login());
                        new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                        return;
                    }

                    CurrentGame.GameData.Points += FINISH_PIECES_POINTS;
                }
        }

        private WPFGameBoardPieceSpot GetPieceAt(int column, int row)
        {
            return BoardGrid.Children.Cast<WPFGameBoardPieceSpot>().First(spot => Grid.GetColumn(spot) == column && Grid.GetRow(spot) == row);
        }

        // type: 2 for column or 1 for row
        private List<string> GetWordsFromRowOrColumn(int index, int type)
        {
            List<string> words = new List<string>();
            string word = string.Empty;

            for (int j = 0; j<25; j++)
            {
                WPFGameBoardPieceSpot pieceSpot;
                switch (type)
                {
                    case 1:
                        pieceSpot = GetPieceAt(j, index);
                        break;
                    case 2:
                        pieceSpot = GetPieceAt(index, j);
                        break;
                    default:
                        return new List<string>();
                }

                string letter = (string)pieceSpot.LetterLabel.Content;

                if (string.IsNullOrWhiteSpace(letter))
                {
                    if (word.Length > 1)
                    {
                        words.Add(word);
                        word = "";
                    }
                }
                else
                {
                    word = word + letter;

                    if (j == 24)
                    {
                        words.Add(word);
                        word = "";
                    }
                }
            }

            return words;
        }

        private (int wordsPoints, List<string> correctWords) EvaluateBoard()
        {

            List<string> builtWords = new List<string>();

            // horizontal word checking
            for (int i = 0; i<25; i++)
            {
                foreach (string word in GetWordsFromRowOrColumn(i, 1))
                {
                    builtWords.Add(word);
                }
            }

            // vertical word checking
            for (int i = 0; i<25; i++)
            {
                foreach (string word in GetWordsFromRowOrColumn(i, 2))
                {
                    builtWords.Add(word);
                }
            }

            return GetCorrectWords(builtWords);
        }

        private static int CalculateWordPoints(string word)
        {
            int points = 0;

            if (word.Length < 2)
            {
                return 0;
            }
            else if (word.Length < 7)
            {
                points = word.Length;
            }
            else if (word.Length <= 9)
            {
                points = (int)Math.Floor(word.Length * MID_WORD_MULTIPLIER);
            }
            else if (word.Length > 9)
            {
                points = (int)Math.Floor(word.Length * LARGE_WORD_MULTIPLIER);
            }

            return points;
        }

        private static (int, List<string>) GetCorrectWords(List<string> wordList)
        {
            List<string> correctWords = new List<string>();
            int points = 0;

            foreach (string word in wordList)
            {
                bool validWord = WordChecker.ValidWord(word, CurrentGame.GameConfig.WordsLanguage);

                if (validWord)
                {
                    correctWords.Add(word);
                    points += CalculateWordPoints(word);
                }
            }

            return (points, correctWords);
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

        public void AddSeedsToHand(char[] initialPieces)
        {
            foreach (var letter in initialPieces)
            {
                PiecesPanel.Children.Add(new WPFGamePiece(letter.ToString()));
            }
        }

        public void RefreshGameRoom(int piecesNumber, PlayerDC[] connectedPlayers)
        {
            CurrentGame.GameData.PilePieces = piecesNumber;
            CurrentGame.PlayersInRoom = connectedPlayers.ToList();

            ShowCurrentPlayers();
        }

        public void NotifyEndOfGame()
        {
            CalculatingResultsOverlay.Visibility = Visibility.Visible;
            CalculatingResultsOverlay.IsEnabled = true;

            (int points, List<string> words) = EvaluateBoard();
            _createdWords = words;

            try
            {
                _host.CalculateWinner(CurrentGame.RoomCode, CurrentPlayer.Player.Username, points);
            }
            catch (CommunicationObjectFaultedException)
            {
                _logger.Fatal("Couldn't connect to server to end game");
                NavigationService.Navigate(new Login.Login());
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                return;
            }

            foreach (string word in _createdWords)
            {
                CorrectWordsPanel.Children.Add(new Label() { Content = word });
            }

            ScoreLabel.Content = Properties.Resources.globalScore + points.ToString();
        }

        public void EndGame(string winnerUsername, int score)
        {
            WinnerLabel.Content = Properties.Resources.gameEndWinner + " " + winnerUsername;
            WinnerScoreLabel.Content = Properties.Resources.globalScore + score;

            CalculatingResultsOverlay.Visibility = Visibility.Hidden;
            CalculatingResultsOverlay.IsEnabled = false;

            WinnerOverlay.Visibility = Visibility.Visible;
            WinnerOverlay.IsEnabled = true;
        }

        private void ExitFinishedGame(object sender, RoutedEventArgs e)
        {
            try
            {
                _host.LeaveGame(CurrentGame.RoomCode, CurrentPlayer.Player.Username, true);
            }
            catch (CommunicationObjectFaultedException)
            {
                _logger.Fatal("Couldn't connect to server to leave game");
                NavigationService.Navigate(new Login.Login());
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                return;
            }

            CurrentGame.State = CurrentGame.GameState.Finished;
            CurrentGame.GameConfig = null;
            CurrentGame.GameData = new GameProgressData();
            CurrentGame.PlayersInRoom = null;
            CurrentGame.RoomCode = null;

            if (CurrentPlayer.IsGuest)
            {
                NavigationService.Navigate(new Login.Login());
            }
            else
            {
                NavigationService.Navigate(new MainMenu()); 
            }
        }
    }
}
