using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PapayagramsClient.WPFControls
{
    public partial class WPFGameBoardPieceSpot : UserControl
    {
        Brush _previousFill;
        string _previousLetter;

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
                WPFGamePiece gamePiece = (WPFGamePiece)e.Data.GetData("Object");
                gamePiece.RaisePiecePlacedEvent();
            }
            e.Handled = true;
        }
        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            _previousFill = MainGrid.Background;
            _previousLetter = (string)LetterLabel.Content;

            if (e.Data.GetDataPresent("Color"))
            {
                string colorString = (string)e.Data.GetData("Color");

                BrushConverter converter = new BrushConverter();
                if (converter.IsValid(colorString))
                {
                    Brush newFill = (Brush)converter.ConvertFromString(colorString.ToString());
                    MainGrid.Background = newFill;
                }
            }

            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string letter = (string)e.Data.GetData(DataFormats.StringFormat);

                LetterLabel.Content = letter;
            }
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);
            MainGrid.Background = _previousFill;
            LetterLabel.Content = _previousLetter;
        }
    }
}
