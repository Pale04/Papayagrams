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

namespace PapayagramsClient.Menu
{
    public partial class FriendsMenu : UserControl
    {
        public static readonly RoutedEvent ClosedFriendsMenuEvent = EventManager.RegisterRoutedEvent(
            name: "ClosedFriendsMenu",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(GameInvitation));

        public FriendsMenu()
        {
            InitializeComponent();
        }

        public void FillLists()
        {
            throw new NotImplementedException();
        }

        private void CloseMenu(object sender, RoutedEventArgs e)
        {
            RaiseClosedFriendsMenuEvent();
        }

        private void ToggleSocialLists(object sender, RoutedEventArgs e)
        {
            if (FriendListPanel.Visibility == Visibility.Visible)
            {
                FriendListPanel.Visibility = Visibility.Hidden;
                BloquedListPanel.Visibility = Visibility.Visible;
                RequestsListPanel.Visibility = Visibility.Visible;
            }
            else
            {
                FriendListPanel.Visibility = Visibility.Visible;
                BloquedListPanel.Visibility = Visibility.Hidden;
                RequestsListPanel.Visibility = Visibility.Hidden;
            }
        }

        public event RoutedEventHandler ClosedFriendsMenu
        {
            add { AddHandler(ClosedFriendsMenuEvent, value); }
            remove { RemoveHandler(ClosedFriendsMenuEvent, value); }
        }

        public void RaiseClosedFriendsMenuEvent()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(routedEvent: ClosedFriendsMenuEvent);
            RaiseEvent(routedEventArgs);
        }
    }
}
