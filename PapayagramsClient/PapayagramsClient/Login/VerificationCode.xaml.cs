using log4net;
using PapayagramsClient.PapayagramsService;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;

namespace PapayagramsClient.Login
{
    public partial class VerificationCode : Page
    {
        PlayerDC _player;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(VerificationCode));
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
                _logger.Fatal("Couldn't connect to server for verification");
                NavigationService.GoBack();
                return;
            }

            string code = CodeTextbox.Text.Trim().ToUpper();
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
                _logger.Fatal("Couldn't connect to server for resending verification code");
                NavigationService.GoBack();
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
                    new SelectionPopUpWindow(Properties.Resources.verificationCode, Properties.Resources.verificationCode, 1).ShowDialog();
                    break;
            }
        }
    }
}
