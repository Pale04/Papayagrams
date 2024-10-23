using System;
using System.IO;
using System.Windows.Controls;

namespace PapayagramsClient.Game
{
    /// <summary>
    /// Lógica de interacción para GameCreation.xaml
    /// </summary>
    public partial class GameCreation : Page
    {
        public GameCreation()
        {
            InitializeComponent();
            BackImage.SetImage(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Icons\\back-svgrepo-com.svg");
        }
    }
}
