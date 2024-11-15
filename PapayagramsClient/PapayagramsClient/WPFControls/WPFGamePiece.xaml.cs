using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PapayagramsClient.WPFControls
{
    public partial class WPFGamePiece : UserControl
    {
        public static readonly RoutedEvent PiecePlacedEvent = EventManager.RegisterRoutedEvent(
            name: "PiecePlaced",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(WPFGamePiece));

        public WPFGamePiece(string letter)
        {
            InitializeComponent();
            PieceLetter.Text = letter;
        }

        public event RoutedEventHandler PiecePlaced
        {
            add { AddHandler(PiecePlacedEvent, value); }
            remove { RemoveHandler(PiecePlacedEvent, value); }
        }

        public void RaisePiecePlacedEvent()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(routedEvent: PiecePlacedEvent);
            RaiseEvent(routedEventArgs);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            DataObject data = new DataObject();
            data.SetData("Color", GamePiece.Fill.ToString());
            data.SetData(DataFormats.StringFormat, PieceLetter.Text);
            data.SetData("Object", this);

            DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
            if (e.Effects.HasFlag(DragDropEffects.Move))
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
