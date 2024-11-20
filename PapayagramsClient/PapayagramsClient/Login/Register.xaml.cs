using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;

namespace PapayagramsClient.Login
{
    public partial class Register : Page
    {
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

            PapayagramsService.LoginServiceClient host = new PapayagramsService.LoginServiceClient();
            try
            {
                host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                return;
            }

            int returnCode = host.RegisterUser(player);
            host.Close();

            if (returnCode != 0)
            {
                switch (returnCode)
                {
                    case 101:
                        return;

                    case 102:
                        EmailErrorText.Content = Properties.Resources.errorDatabaseConnection;
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
