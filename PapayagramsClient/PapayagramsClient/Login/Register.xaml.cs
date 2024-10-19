using PapayagramsClient.Login.Popups;
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
            PapayagramsService.PlayerDC player = new PapayagramsService.PlayerDC();
            player.Username = UsernameTextbox.Text;
            player.Password = PasswordTextbox.Text;
            player.Email = EmailTextbox.Text;

            PapayagramsService.LoginServiceClient host = new PapayagramsService.LoginServiceClient();
            host.Open();

            try 
            { 
                int result = host.RegisterUser(player);
            }
            catch (PapayagramsClient.ServerException ex)
            {
                switch (ex.ErrorCode)
                {
                    case 101:
                        UsernameErrorText.Content = Properties.Resources.registerExistingUsername;
                        break;

                    case 102:
                        EmailErrorText.Content = Properties.Resources.registerEmailAlreadyLinked;
                        break;
                }
            }

            host.Close();

            new SuccessfullyRegistered().ShowDialog() ;
            this.NavigationService.GoBack();
        }
        
        private void GoToLogin(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
