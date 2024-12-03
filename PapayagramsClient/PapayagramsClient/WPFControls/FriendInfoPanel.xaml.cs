using System;
using System.Windows;
using System.Windows.Controls;

namespace PapayagramsClient.WPFControls
{
    public partial class FriendInfoPanel : UserControl
    {
        // panelType: what does the panel holds,
        // determines what action needs to occur when the button is pressed
        // 1: Friend
        // 2: Request
        // 3: Bloqued
        public FriendInfoPanel(int panelType, int imageId, string Username)
        {
            InitializeComponent();
            switch (panelType)
            {
                case 1:
                    ActionButton.Click += new RoutedEventHandler(RemoveFriend);
                    SecondaryActionButton.Visibility = Visibility.Hidden;
                    SecondaryActionButton.IsEnabled = false;
                    break;

                case 2:
                    ActionButton.Click += new RoutedEventHandler(AcceptRequest);
                    SecondaryActionButton.Click += new RoutedEventHandler(RejectRequest);
                    SecondaryActionButton.Visibility = Visibility.Visible;
                    SecondaryActionButton.IsEnabled = true;
                    break;

                case 3:
                    ActionButton.Click += new RoutedEventHandler(UnblockUser);
                    SecondaryActionButton.Visibility = Visibility.Hidden;
                    SecondaryActionButton.IsEnabled = false;
                    break;

                default:
                    SecondaryActionButton.Visibility = Visibility.Hidden;
                    SecondaryActionButton.IsEnabled = false;
                    ActionButton.Visibility = Visibility.Hidden;
                    ActionButton.IsEnabled = false;
                    return;
            }

            UsernameLabel.Text = Username;
            // TODO: Add image
        }

        private void UnblockUser(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AcceptRequest(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RejectRequest(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RemoveFriend(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
