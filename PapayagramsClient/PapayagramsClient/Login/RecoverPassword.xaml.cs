using log4net;
using log4net.Repository.Hierarchy;
using PapayagramsClient.PapayagramsService;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace PapayagramsClient.Login
{
    public partial class RecoverPassword : Page
    {
        private string _userEmail;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(RecoverPassword));

        public RecoverPassword()
        {
            InitializeComponent();
        }

        private void ReturnToLogin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }

        private bool IsValiEmail(string email)
        {
            string emailPattern = "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$";
            return Regex.IsMatch(email, emailPattern);
        }

        private bool IsSafePassword(string password)
        {
            string safePasswordPattern = "[!-#*-/=_@\\dA-z]{8,}";
            return Regex.IsMatch(password, safePasswordPattern);
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
                _logger.Fatal("Couldn't connect to server for recovering password");
                NavigationService.GoBack();
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailTextbox.Text))
            {
                new SelectionPopUpWindow(Properties.Resources.globalEmptyEmail, Properties.Resources.globalEmptyEmail, 3).ShowDialog();
                return;
            }

            _userEmail = EmailTextbox.Text;

            if (!IsValiEmail(_userEmail))
            {
                new SelectionPopUpWindow(Properties.Resources.globalNotEmail, Properties.Resources.globalNotEmail, 2).ShowDialog();
                return;
            }

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

            if (!IsSafePassword(password))
            {
                new SelectionPopUpWindow(Properties.Resources.globalWeakPassword, Properties.Resources.globalWeakPassword, 2).ShowDialog();
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
                _logger.Fatal("Couldn't connect to server for saving new password");
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
                _logger.Fatal("Couldn't connect to server for sending recovery code");
                NavigationService.GoBack();
                return;
            }

            loginHost.SendPasswordRecoveryPIN(_userEmail);
            loginHost.Close();

            new SelectionPopUpWindow(Properties.Resources.verificationCode, Properties.Resources.verificationCode, 1).ShowDialog();
        }
    }
}
