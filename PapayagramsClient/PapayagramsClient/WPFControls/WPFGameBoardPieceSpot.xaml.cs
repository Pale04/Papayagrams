using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PapayagramsClient.WPFControls
{
    public partial class WPFGameBoardPieceSpot : UserControl
    {
        public static readonly RoutedEvent PiecePickedUpEvent = EventManager.RegisterRoutedEvent(
            name: "PiecePickedUp",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(WPFGameBoardPieceSpot));

        public static readonly RoutedEvent PieceMovedEvent = EventManager.RegisterRoutedEvent(
            name: "PieceMoved",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(WPFGameBoardPieceSpot));

        public static readonly RoutedEvent PieceDumpedEvent = EventManager.RegisterRoutedEvent(
            name: "PieceDumped",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(WPFGameBoardPieceSpot));

        Brush _previousFill;
        string _previousLetter;

        public WPFGameBoardPieceSpot()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler PiecePickedUp
        {
            add { AddHandler(PiecePickedUpEvent, value); }
            remove { RemoveHandler(PiecePickedUpEvent, value); }
        }

        public void RaisePiecePickedUpEvent()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(routedEvent: PiecePickedUpEvent);
            RaiseEvent(routedEventArgs);
        }

        public event RoutedEventHandler PieceDumped
        {
            add { AddHandler(PieceDumpedEvent, value); }
            remove { RemoveHandler(PieceDumpedEvent, value); }
        }

        public void RaisePieceDumpedEvent()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(routedEvent: PieceDumpedEvent);
            RaiseEvent(routedEventArgs);
        }

        public event RoutedEventHandler PieceMoved
        {
            add { AddHandler(PieceMovedEvent, value); }
            remove { RemoveHandler(PieceMovedEvent, value); }
        }

        public void RaisePieceMovedEvent()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(routedEvent: PieceMovedEvent);
            RaiseEvent(routedEventArgs);
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (e.Data.GetDataPresent(DataFormats.StringFormat) && e.Data.GetDataPresent("Color") && e.Data.GetDataPresent("Object"))
            {
                if (!string.IsNullOrEmpty(_previousLetter))
                {
                    LetterLabel.Content = _previousLetter;
                    RaisePiecePickedUpEvent();
                }

                string letter = (string)e.Data.GetData(DataFormats.StringFormat);
                string color = (string)e.Data.GetData("Color");
               
                LetterLabel.Content = letter;
                MainGrid.Background = (Brush) new BrushConverter().ConvertFromString(color);

                e.Effects = DragDropEffects.Move;
                
                switch (e.Data.GetData("ObjectType"))
                {
                    case "Piece":
                        WPFGamePiece gamePiece = (WPFGamePiece)e.Data.GetData("Object");
                        gamePiece.RaisePiecePlacedEvent();
                        break;

                    case "PieceSpot":
                        WPFGameBoardPieceSpot pieceSpot = (WPFGameBoardPieceSpot)e.Data.GetData("Object");
                        pieceSpot.RaisePieceMovedEvent();
                        break;
                }
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
            e.Handled = true;
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);
            MainGrid.Background = _previousFill;
            _previousFill = null;
            LetterLabel.Content = _previousLetter;
            _previousLetter = "";
            e.Handled = true;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            RaisePiecePickedUpEvent();
            e.Handled = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (MainGrid.Background == null || LetterLabel.Content.Equals(""))
            {
                return;
            }

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DataObject data = new DataObject();
                data.SetData("Color", MainGrid.Background.ToString());
                data.SetData(DataFormats.StringFormat, LetterLabel.Content);
                data.SetData("Object", this);
                data.SetData("ObjectType", "PieceSpot");

                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            }
            e.Handled = true;
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
