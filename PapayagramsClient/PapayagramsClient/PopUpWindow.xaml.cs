using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace PapayagramsClient.Login.Popups
{
    public partial class PopUpWindow : Window
    {
        // 0: Success
        // 1: Info
        // 2: Warning
        // 3: Error
        public PopUpWindow(string title, string message, int type)
        {
            InitializeComponent();
            Title = title;
            MessageLabel.Content = message;
            BitmapImage image = new BitmapImage();
            image.BeginInit();

            switch (type)
            {
                case 0:
                    image.UriSource = new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\PopUpIcons\\success-svgrepo-com.svg");
                    break;

                case 1:
                    image.UriSource = new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\PopUpIcons\\info-circle-svgrepo-com.svg");
                    break;

                case 2:
                    image.UriSource = new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\PopUpIcons\\warning-svgrepo-com.svg");
                    break;

                case 3:
                    image.UriSource = new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\PopUpIcons\\error-svgrepo-com.svg");
                    break;
            }

            image.EndInit();
            IconImage.Source = image;
        }
    }
}
