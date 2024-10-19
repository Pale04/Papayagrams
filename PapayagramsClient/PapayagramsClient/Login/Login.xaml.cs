using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
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
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            ClearErrorLabels();

            if (string.IsNullOrEmpty(UsernameTextbox.Text))
            {
                UsernameErrorText.Content = Properties.Resources.globalEmptyUsername;
                return;
            }
            if (string.IsNullOrEmpty(PasswordTextbox.Text))
            {
                PasswordErrorText.Content = Properties.Resources.signInEmptyPassword;
                return;
            }

            string Username = UsernameTextbox.Text;
            string Password = PasswordTextbox.Text;

            PapayagramsService.LoginServiceClient host = new PapayagramsService.LoginServiceClient();
            host.Open();

            try
            {
                PapayagramsService.PlayerDC player = host.Login(Username, Password);
                CurrentPlayer.Player = player;
            }
            catch (PapayagramsService.ServerException ex)
            {
                switch (ex.ErrorCode)
                {
                    case 105:
                        PasswordErrorText.Content = Properties.Resources.signInWrongCredentials;
                        break;
                }
                return;
            }
            finally
            {
                host.Close();
            }

            this.NavigationService.Navigate(new MainMenu());
        }

        private void RegisterNewUser(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Register());
        }

        private void ClearErrorLabels()
        {
            PasswordErrorText.Content = "";
            UsernameErrorText.Content = "";
        }
    }
}
