using System;
using System.Collections.Generic;
using System.Linq;
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
                // No se como poner los recursos en cs en vez del xaml
                UsernameErrorText.Content = "El usuario no puede quedarse vacío";
                return;
            }
            if (string.IsNullOrEmpty(PasswordTextbox.Text))
            {
                PasswordErrorText.Content = "La contraseña no puede ser vacía";
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
            catch (ArgumentException ex)
            {
                return;
            }
            catch (System.ServiceModel.EndpointNotFoundException ex)
            {
                Console.WriteLine("No se pudo conectar al server");
                return;
            }
            catch (Exception ex)
            {
                PasswordErrorText.Content += ex.ToString();
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
