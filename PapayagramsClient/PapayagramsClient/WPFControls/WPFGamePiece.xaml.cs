using ExCSS;
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

namespace PapayagramsClient.WPFControls
{
    /// <summary>
    /// Lógica de interacción para GamePiece.xaml
    /// </summary>
    public partial class WPFGamePiece : UserControl
    {
        public WPFGamePiece()
        {
            InitializeComponent();
        }
        public WPFGamePiece(WPFGamePiece c)
        {
            InitializeComponent();
            this.GamePiece.Height = c.GamePiece.Height;
            this.GamePiece.Width = c.GamePiece.Height;
            this.GamePiece.Fill = c.GamePiece.Fill;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            DataObject data = new DataObject();
            data.SetData(DataFormats.StringFormat, GamePiece.Fill.ToString());
            data.SetData("Double", GamePiece.Height);
            data.SetData("Double", GamePiece.Width);
            data.SetData(DataFormats.StringFormat, PieceLetter.Text);
            data.SetData("Object", this);

            DragDrop.DoDragDrop(this, data, DragDropEffects.Copy | DragDropEffects.Move);
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
            // These Effects values are set in the drop target's
            // DragOver event handler.
            if (e.Effects.HasFlag(DragDropEffects.Copy))
            {
                Mouse.SetCursor(Cursors.Cross);
            }
            else if (e.Effects.HasFlag(DragDropEffects.Move))
            {
                Mouse.SetCursor(Cursors.Pen);
            }
            else
            {
                Mouse.SetCursor(Cursors.No);
            }
            e.Handled = true;
        }
    }
}
