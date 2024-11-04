using PapayagramsClient.PapayagramsService;

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
            GameConfigurationDC config = new GameConfigurationDC();

            Language wordsLanguage;
            switch (WordsLanguageComboBox.Text)
            {
                case "English":
                    wordsLanguage = PapayagramsService.Language.English;
                    break;
                case "Español":
                    wordsLanguage = PapayagramsService.Language.Spanish;
                    break;
                default:
                    return;
            }
            config.WordsLanguage = wordsLanguage;

            GameMode gameMode;
            switch (GameModeComboBox.Text)
            {
                case "Classic":
                    gameMode = GameMode.Oiginal;
                    break;
                case "Sudden death":
                    gameMode = GameMode.SuddenDeath;
                    break;
                case "Time attack":
                    gameMode = GameMode.TimeAttack;
                    break;
                default:
                    return;
            }
            config.GameMode = gameMode;

            config.MaxPlayers = int.Parse(MaxPlayersComboBox.Text);
            config.InitialPieces = int.Parse(InitialPiecesComboBox.Text);
            config.TimeLimitMinutes = int.Parse(TimeLimitComboBox.Text);
        }
    }
}
