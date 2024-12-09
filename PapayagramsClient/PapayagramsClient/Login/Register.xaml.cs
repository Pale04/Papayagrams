using log4net;
using System;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace PapayagramsClient.Login
{
    public partial class Register : Page
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Register));

        public Register()
        {
            InitializeComponent();
        }

        private void RegisterUser(object sender, RoutedEventArgs e)
        {
            ClearErrorLabels();

            if (string.IsNullOrEmpty(UsernameTextbox.Text))
            {
                UsernameErrorText.Content = Properties.Resources.globalEmptyUsername;
                return;
            }
            if (string.IsNullOrEmpty(PasswordTextbox.Text))
            {
                PasswordErrorText.Content = Properties.Resources.globalEmptyUsername;
                return;
            }
            if (string.IsNullOrEmpty(EmailTextbox.Text))
            {
                EmailErrorText.Content = Properties.Resources.globalEmptyEmail;
                return;
            }

            PapayagramsService.PlayerDC player = new PapayagramsService.PlayerDC();
            player.Username = UsernameTextbox.Text;
            player.Password = PasswordTextbox.Text;
            player.Email = EmailTextbox.Text;

            if (!IsSafePassword(player.Password))
            {
                new SelectionPopUpWindow(Properties.Resources.globalWeakPassword, Properties.Resources.globalWeakPassword, 2).ShowDialog();
                return;
            }
            else if (!IsValiEmail(player.Email))
            {
                new SelectionPopUpWindow(Properties.Resources.globalNotEmail, Properties.Resources.globalNotEmail, 2).ShowDialog();
                return;
            }

            PapayagramsService.LoginServiceClient host = new PapayagramsService.LoginServiceClient();
            try
            {
                host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                _logger.Fatal("Couldn't connect to server for registering");
                NavigationService.GoBack();
                return;
            }

            int returnCode = host.RegisterUser(player);
            host.Close();

            if (returnCode != 0)
            {
                switch (returnCode)
                {
                    case 101:
                        _logger.Error("One of the passed parameters on register user was null or empty");
                        new SelectionPopUpWindow(Properties.Resources.errorUnexpectedError, Properties.Resources.errorUnexpectedError, 3).ShowDialog();
                        return;

                    case 102:
                        EmailErrorText.Content = Properties.Resources.errorDatabaseConnection;
                        _logger.Error("Could not register user to database");
                        return;

                    case 201:
                        UsernameErrorText.Content = Properties.Resources.registerExistingUsername;
                        return;

                    case 202:
                        EmailErrorText.Content = Properties.Resources.registerEmailAlreadyLinked;
                        return;
                }
            }

            new SelectionPopUpWindow(Properties.Resources.registerSuccessfulTitle, Properties.Resources.registerSuccessful, 0).ShowDialog();
            this.NavigationService.GoBack();
        }

        private bool IsValiEmail(string email)
        {
            string emailPattern = "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$";
            return Regex.IsMatch(email, emailPattern);
        }

        private bool IsSafePassword(string password)
        {
            string containsLettersPasswordPattern = "[A-z]+";
            string containsDigitPattern = "[\\d]+";
            string containsSpecialSimbolPattern = "[!-#*-/=_@]+";
            return Regex.IsMatch(password, containsLettersPasswordPattern) && Regex.IsMatch(password, containsSpecialSimbolPattern) && Regex.IsMatch(password, containsDigitPattern) && password.Length >= 8;
        }
        
        private void GoToLogin(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void ClearErrorLabels()
        {
            UsernameErrorText.Content = "";
            EmailErrorText.Content = "";
            PasswordErrorText.Content = "";
        }
    }
}
