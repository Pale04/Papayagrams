using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PapayagramsClient.WPFControls
{
    public partial class WPFDump : UserControl
    {
        string _baseImagePath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Icons\\trash-bin-svgrepo-com.png";
        string _altImagePath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Icons\\trash-bin-open-svgrepo-com.png";

        public WPFDump()
        {
            InitializeComponent();
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (e.Data.GetDataPresent(DataFormats.StringFormat) && e.Data.GetDataPresent("Color") && e.Data.GetDataPresent("Object"))
            {
                e.Effects = DragDropEffects.Move;
                WPFGamePiece gamePiece = (WPFGamePiece)e.Data.GetData("Object");
                gamePiece.RaisePieceDumpedEvent();
            }
            e.Handled = true;
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && e.Data.GetDataPresent("Color") && e.Data.GetDataPresent("Object"))
            {
                DumpIcon.Source = new BitmapImage(new Uri(_altImagePath));
            }
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);
            DumpIcon.Source = new BitmapImage(new Uri(_baseImagePath));
        }
    }
}
