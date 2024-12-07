using PapayagramsClient.PapayagramsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PapayagramsClient.Menu
{
    /// <summary>
    /// Lógica de interacción para ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Page
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void ReturnToConfig(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SavePassword(object sender, RoutedEventArgs e)
        {
            ApplicationSettingsServiceClient host = new ApplicationSettingsServiceClient();

            try
            {
                host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                NavigationService.GoBack();
                return;
            }

            string password = PasswordTextbox.Text;
            if (string.IsNullOrWhiteSpace(password))
            {
                new SelectionPopUpWindow(Properties.Resources.signInEmptyPassword, Properties.Resources.signInEmptyPassword, 3).ShowDialog();
                return;
            }

            string newPassword = NewPasswordTextbox.Text;
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                new SelectionPopUpWindow(Properties.Resources.signInEmptyPassword, Properties.Resources.signInEmptyPassword, 3).ShowDialog();
                return;
            }

            if (!newPassword.Equals(RepeatNewPasswordTextbox.Text))
            {
                RepeatPasswordErrorText.Content = Properties.Resources.recoverPasswordsDontMatch;
                return;
            }

            int returnCode = host.UpdatePassword(CurrentPlayer.Player.Username, password, newPassword);

            switch (returnCode)
            {
                case 0:
                    new SelectionPopUpWindow(Properties.Resources.globalPasswordChanged, Properties.Resources.globalPasswordChanged, 0).ShowDialog();
                    break;

                case 101:
                    new SelectionPopUpWindow(Properties.Resources.errorOccurredTitle, Properties.Resources.errorUnexpectedError, 3).ShowDialog();
                    return;

                case 102:
                    new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorDatabaseConnection, 3).ShowDialog();
                    return;

                case 503:
                    new SelectionPopUpWindow(Properties.Resources.errorOccurredTitle, Properties.Resources.errorUnexpectedError, 3).ShowDialog();
                    return;
            }

            host.Close();
            NavigationService.GoBack();
        }
    }
}
