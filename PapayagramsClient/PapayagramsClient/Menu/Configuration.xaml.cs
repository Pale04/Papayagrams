using log4net;
using PapayagramsClient.PapayagramsService;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PapayagramsClient.Menu
{
    public partial class Configuration : Page
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Leaderboards));

        public Configuration()
        {
            InitializeComponent();
            ShowCurrentConfiguration();
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            SaveChanges();
            NavigationService.Navigate(new MainMenu());
        }

        private void SaveChanges()
        {
            CurrentPlayer.Configuration = GetChanges();

            ApplicationSettingsServiceClient host = new ApplicationSettingsServiceClient();

            try
            {
                host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                _logger.Fatal("Couldn't connect to server to update user settings");
                NavigationService.GoBack();
                return;
            }

            if (CurrentPlayer.Configuration == null)
            {
                host.UpdateAplicationSettings(CurrentPlayer.Player.Username, CurrentPlayer.Configuration);
            }

            host.Close();
        }

        private ApplicationSettingsDC GetChanges()
        {
            ApplicationLanguageDC language;
            switch (LanguageCombobox.SelectedIndex)
            {
                case 0:
                    language = ApplicationLanguageDC.english;
                    break;

                case 1:
                    language = ApplicationLanguageDC.spanish;
                    break;

                case 2:
                    language = ApplicationLanguageDC.auto;
                    break;

                default:
                    return CurrentPlayer.Configuration;
            }

            int cursor = CursorCombobox.SelectedIndex;

            ApplicationSettingsDC configuration = new ApplicationSettingsDC { SelectedLanguage = language, Cursor = cursor, PieceColor = CurrentPlayer.Configuration.PieceColor };

            return configuration;
        }

        private void ShowCurrentConfiguration()
        {
            switch (CurrentPlayer.Configuration.SelectedLanguage)
            {
                case ApplicationLanguageDC.auto:
                    LanguageCombobox.SelectedIndex = 2;
                    break;

                case ApplicationLanguageDC.english:
                    LanguageCombobox.SelectedIndex = 0;
                    break;

                case ApplicationLanguageDC.spanish:
                    LanguageCombobox.SelectedIndex = 1;
                    break;
            }

            CursorCombobox.SelectedIndex = CurrentPlayer.Configuration.Cursor;
        }

        private void GoToChangePassword(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ChangePassword());
        }
    }
}
