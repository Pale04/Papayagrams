using log4net;
using PapayagramsClient.PapayagramsService;
using System.Diagnostics.Eventing.Reader;

namespace PapayagramsClient.Game
{
    public partial class GameCreation
    {
        public GameCreation()
        {
            InitializeComponent();
        }

        private void ReturnToMainMenu(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainMenu());
        }

        private void CreateGame(object sender, System.Windows.RoutedEventArgs e)
        {
            GameConfigurationDC config = new GameConfigurationDC();

            LanguageDC wordsLanguage;
            if (WordsLanguageComboBox.Text.Equals(Properties.Resources.createGameEnglishLanguage))
            {
                wordsLanguage = LanguageDC.English;
            }
            else if (WordsLanguageComboBox.Text.Equals(Properties.Resources.createGameSpanishLanguage))
            {
                wordsLanguage = LanguageDC.Spanish;
            }
            else
            {
                return;
            }
            config.WordsLanguage = wordsLanguage;

            GameModeDC gameMode;
            if (GameModeComboBox.Text.Equals(Properties.Resources.createGameClassicMode))
            {
                gameMode = GameModeDC.Original;
            }
            // else if (GameModeComboBox.Text.Equals(Properties.Resources.createGameSuddenDeathMode))
            // {
                // gameMode = GameModeDC.SuddenDeath;
            // }
            // else if (GameModeComboBox.Text.Equals(Properties.Resources.createGameTimeAttackMode))
            // {
                // gameMode = GameModeDC.TimeAttack;
            // }
            else
            {
                return;
            }
            config.GameMode = gameMode;

            int gameTime = 0;
            if (!TimeLimitComboBox.Text.Equals(Properties.Resources.createGameNoTimeLimit))
            {
                gameTime = int.Parse(TimeLimitComboBox.Text);
            }
            config.TimeLimitMinutes = gameTime;

            config.MaxPlayers = int.Parse(MaxPlayersComboBox.Text);
            config.InitialPieces = int.Parse(InitialPiecesComboBox.Text);

            NavigationService.Navigate(new Lobby(config));
        }
    }
}
