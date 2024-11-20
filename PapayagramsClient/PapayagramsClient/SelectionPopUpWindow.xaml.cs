using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PapayagramsClient
{
    public partial class SelectionPopUpWindow : Window
    {
        public SelectionPopUpWindow(string title, string message, int type)
        {
            InitializeComponent();
            Title = title;
            MessageText.Text = message;

            string imagePath;
            switch (type)
            {
                case 0:
                    imagePath = (Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Icons\\success-svgrepo-com.png");
                    break;

                case 1:
                    imagePath = (Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Icons\\info-circle-svgrepo-com.png");
                    break;

                case 2:
                    imagePath = (Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Icons\\warning-svgrepo-com.png");
                    break;

                case 3:
                    imagePath = (Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Icons\\error-svgrepo-com.png");
                    break;

                case 4:
                    imagePath = (Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Icons\\question-circle-svgrepo-com.png");
                    PrimaryButton.IsEnabled = true;
                    PrimaryButton.Visibility = Visibility.Visible;
                    break;

                default:
                    return;
            }
            IconImage.Source = new BitmapImage(new Uri(imagePath));
        }

        private void SecondaryButton_Click(object sender, RoutedEventArgs e)
        {
            if (SecondaryButton.Content.Equals(Properties.Resources.globalClose))
            {
                Close();
            }
            else
            {
                DialogResult = false;
                Close();
            }
        }

        private void PrimaryButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
