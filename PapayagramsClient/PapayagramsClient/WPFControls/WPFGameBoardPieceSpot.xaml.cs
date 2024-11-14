using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PapayagramsClient.WPFControls
{
    public partial class WPFGameBoardPieceSpot : UserControl
    {
        public WPFGameBoardPieceSpot()
        {
            InitializeComponent();
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (e.Data.GetDataPresent(DataFormats.StringFormat) && e.Data.GetDataPresent("Color"))
            {
                string letter = (string)e.Data.GetData(DataFormats.StringFormat);
                string color = (string)e.Data.GetData("Color");
               
                LetterLabel.Content = letter;
                MainGrid.Background = (Brush) new BrushConverter().ConvertFromString(color);

                e.Effects = DragDropEffects.Move;
            }
            e.Handled = true;
        }
    }
}
