using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
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

namespace PapayagramsClient.Login
{
    /// <summary>
    /// Lógica de interacción para Signin.xaml
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        private void RegisterUser(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameTextbox.Text))
            {
                UsernameErrorText.Content = Properties.Resources.globalEmptyUsername;
            }
            if (string.IsNullOrEmpty(PasswordTextbox.Text))
            {
                PasswordErrorText.Content = Properties.Resources.globalEmptyUsername;
            }
            if (string.IsNullOrEmpty(EmailTextbox.Text))
            {
                EmailErrorText.Content = Properties.Resources.globalEmptyEmail;
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
                new PopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                return;
            }

            try 
            { 
                int result = host.RegisterUser(player);
            }
            catch (FaultException<PapayagramsService.ServerException> ex)
            {
                switch (ex.Detail.ErrorCode)
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

            host.Close();

            new PopUpWindow(Properties.Resources.registerSuccessfulTitle, Properties.Resources.registerSuccessful, 0).ShowDialog();
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
