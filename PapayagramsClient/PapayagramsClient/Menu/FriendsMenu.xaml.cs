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
            ownerType: typeof(FriendsMenu));

        public static readonly RoutedEvent AddedFriendEvent = EventManager.RegisterRoutedEvent(
            name: "AddedFriend",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(FriendsMenu));

        public FriendsMenu()
        {
            InitializeComponent();
        }

        public void FillLists()
        {
            throw new NotImplementedException();
        }

        private void FillFriendList()
        {
            throw new NotImplementedException();
        }

        private void FillRequestsList()
        {
            throw new NotImplementedException();
        }

        private void FillBloquedList()
        {
            throw new NotImplementedException();
        }

        private void ToggleSocialLists(object sender, RoutedEventArgs e)
        {
            if (FriendListScrollViewer.Visibility == Visibility.Visible)
            {
                AddFriendPanel.Visibility = Visibility.Hidden;
                FriendListScrollViewer.Visibility = Visibility.Hidden;
                BloquedListScrollViewer.Visibility = Visibility.Visible;
                RequestsListScrollViewer.Visibility = Visibility.Visible;

                AddFriendPanel.IsEnabled = false;
                FriendListScrollViewer.IsEnabled = false;
                BloquedListScrollViewer.IsEnabled = true;
                RequestsListScrollViewer.IsEnabled = true;
            }
            else
            {
                AddFriendPanel.Visibility = Visibility.Visible;
                FriendListScrollViewer.Visibility = Visibility.Visible;
                BloquedListScrollViewer.Visibility = Visibility.Hidden;
                RequestsListScrollViewer.Visibility = Visibility.Hidden;

                AddFriendPanel.IsEnabled = true;
                FriendListScrollViewer.IsEnabled = true;
                BloquedListScrollViewer.IsEnabled = false;
                RequestsListScrollViewer.IsEnabled = false;
            }
        }

        private void CloseMenu(object sender, RoutedEventArgs e)
        {
            RaiseClosedFriendsMenuEvent();
        }

        private void AddFriend(object sender, RoutedEventArgs e)
        {
            RaiseAddedFriendEvent();
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

        public event RoutedEventHandler AddedFriend
        {
            add { AddHandler(AddedFriendEvent, value); }
            remove { RemoveHandler(AddedFriendEvent, value); }
        }

        public void RaiseAddedFriendEvent()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(routedEvent: AddedFriendEvent);
            RaiseEvent(routedEventArgs);
        }
    }
}
