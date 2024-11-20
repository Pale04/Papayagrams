using PapayagramsClient.PapayagramsService;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;

namespace PapayagramsClient.Login
{
    public partial class VerificationCode : Page
    {
        PlayerDC _player;
        public VerificationCode(PlayerDC player)
        {
            _player = player;
            InitializeComponent();
        }

        private void VerifyCode(object sender, RoutedEventArgs e)
        {
            LoginServiceClient host = new LoginServiceClient();
            try
            {
                host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                return;
            }

            string code = CodeTextbox.Text;
            int returnCode = host.VerifyAccount(_player.Username, code);
            host.Close();

            switch (returnCode)
            {
                case 0:
                    break;
                case 101:
                    ErrorText.Content = Properties.Resources.verificationEmptyCode;
                    return;
                case 208:
                    ErrorText.Content = Properties.Resources.verificationWrongCode;
                    return;
                case 102:
                    new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorDatabaseConnection, 3).ShowDialog();
                    return;
                case 209:
                    new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                    return;
                default:
                    return;
            }

            CurrentPlayer.Player = _player;
            NavigationService.Navigate(new MainMenu());
        }

        private void ReturnToLogin(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ResendCode(object sender, RoutedEventArgs e)
        {
            LoginServiceClient host = new LoginServiceClient();
            try
            {
                host.Open();
            }
            catch (EndpointNotFoundException)
            {
                new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                return;
            }

            int returnCode = host.SendAccountVerificationCode(_player.Username);

            switch (returnCode)
            {
                case 102:
                    new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorDatabaseConnection, 3).ShowDialog();
                    return;
                case 104:
                    new SelectionPopUpWindow(Properties.Resources.errorConnectionTitle, Properties.Resources.errorServerConnection, 3).ShowDialog();
                    return;
                default:
                    break;
            }
        }
    }
}
