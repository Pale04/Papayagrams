using PapayagramsClient.PapayagramsService;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace PapayagramsClient.Login
{
    public partial class RecoverPassword : Page
    {
        private string _userEmail;

        public RecoverPassword()
        {
            InitializeComponent();
        }

        private void ReturnToLogin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }

        private void StartRecoveringPassword(object sender, RoutedEventArgs e)
        {
            LoginServiceClient loginHost = new LoginServiceClient();

            try
            {
                loginHost.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                NavigationService.GoBack();
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailTextbox.Text))
            {
                new SelectionPopUpWindow(Properties.Resources.globalEmptyEmail, Properties.Resources.globalEmptyEmail, 3).ShowDialog();
                return;
            }

            _userEmail = EmailTextbox.Text;

            loginHost.SendPasswordRecoveryPIN(_userEmail);
            loginHost.Close();

            ChangePasswordPanel.Visibility = Visibility.Visible;
            ChangePasswordPanel.IsEnabled = true;
            SendCodePanel.Visibility = Visibility.Hidden;
            SendCodePanel.IsEnabled = false;
        }

        private void SavePassword(object sender, RoutedEventArgs e)
        {
            string code = VerificationCodeTextbox.Text;
            if (string.IsNullOrWhiteSpace(code))
            {
                new SelectionPopUpWindow(Properties.Resources.verificationEmptyCode, Properties.Resources.verificationEmptyCode, 3).ShowDialog();
                return;
            }

            string password = NewPasswordTextbox.Text;
            if (string.IsNullOrWhiteSpace(password))
            {
                new SelectionPopUpWindow(Properties.Resources.verificationEmptyCode, Properties.Resources.verificationEmptyCode, 3).ShowDialog();
                return;
            }

            if (!password.Equals(RepeatPasswordTextbox.Text))
            {
                RepeatPasswordErrorText.Content = Properties.Resources.recoverPasswordsDontMatch;
                return;
            }

            LoginServiceClient loginHost = new LoginServiceClient();

            try
            {
                loginHost.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                NavigationService.GoBack();
                return;
            }

            int returnCode = loginHost.RecoverPassword(code, _userEmail, password);
            loginHost.Close();

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

                case 210:
                    CodeErrorText.Content = Properties.Resources.verificationWrongCode;
                    return;

                case 211:
                    new SelectionPopUpWindow(Properties.Resources.errorOccurredTitle, Properties.Resources.errorUnexpectedError, 3).ShowDialog();
                    return;
            }
        }

        private void ResendCode(object sender, RoutedEventArgs e)
        {
            LoginServiceClient loginHost = new LoginServiceClient();

            try
            {
                loginHost.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                NavigationService.GoBack();
                return;
            }

            loginHost.SendPasswordRecoveryPIN(_userEmail);
            loginHost.Close();

            new SelectionPopUpWindow(Properties.Resources.verificationCode, Properties.Resources.verificationCode, 1).ShowDialog();
        }
    }
}
