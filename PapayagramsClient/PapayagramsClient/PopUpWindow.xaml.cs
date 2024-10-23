using System;
using System.IO;
using System.Windows;

namespace PapayagramsClient
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
            string imagePath;

            switch (type)
            {
                case 0:
                    imagePath = (Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\PopUpIcons\\success-svgrepo-com.svg");
                    break;

                case 1:
                    imagePath = (Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\PopUpIcons\\info-circle-svgrepo-com.svg");
                    break;

                case 2:
                    imagePath = (Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\PopUpIcons\\warning-svgrepo-com.svg");
                    break;

                case 3:
                    imagePath = (Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\PopUpIcons\\error-svgrepo-com.svg");
                    break;

                default:
                    return;
            }

            IconImage.SetImage(imagePath);
        }
    }
}
