using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PapayagramsClient.WPFControls
{
    public partial class FriendInfoPanel : UserControl
    {
        public static readonly RoutedEvent RejectedFriendRequestEvent = EventManager.RegisterRoutedEvent(
            name: "RejectedFriendRequest",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(FriendInfoPanel));

        public static readonly RoutedEvent AcceptedFriendRequestEvent = EventManager.RegisterRoutedEvent(
            name: "AcceptedFriendRequest",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(FriendInfoPanel));

        public static readonly RoutedEvent BloquedUserEvent = EventManager.RegisterRoutedEvent(
            name: "BloquedUser",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(FriendInfoPanel));
        
        public static readonly RoutedEvent UnblockedUserEvent = EventManager.RegisterRoutedEvent(
            name: "UnblockedUser",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(FriendInfoPanel));

        public static readonly RoutedEvent RemovedFriendEvent = EventManager.RegisterRoutedEvent(
            name: "RemovedFriend",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(FriendInfoPanel));

        public static readonly RoutedEvent FriendInvitedEvent = EventManager.RegisterRoutedEvent(
            name: "FriendInvited",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(FriendInfoPanel));

        // panelType: what does the panel holds,
        // determines what action needs to occur when the button is pressed
        // 1: Friend
        // 2: Request
        // 3: Bloqued
        // 4: For invitations
        public FriendInfoPanel(int panelType, int imageId, string Username)
        {
            InitializeComponent();
            switch (panelType)
            {
                case 1:
                    ActionButton.Click += new RoutedEventHandler(RemoveFriend);
                    ActionButtonImage.Source = new BitmapImage(new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Icons\\unfriend-svgrepo-com.png"));
                    ActionButton.ToolTip = Properties.Resources.friendsRemoveFriendTooltip;

                    SecondaryActionButton.Click += new RoutedEventHandler(BlockUser);
                    SecondaryActionButtonImage.Source = new BitmapImage(new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Icons\\error-svgrepo-com.png"));
                    SecondaryActionButton.ToolTip = Properties.Resources.friendsBlockUserTooltip;
                    SecondaryActionButton.Visibility = Visibility.Visible;
                    SecondaryActionButton.IsEnabled = true;
                    break;

                case 2:
                    ActionButton.Click += new RoutedEventHandler(AcceptRequest);
                    ActionButtonImage.Source = new BitmapImage(new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Icons\\done-ring-round-svgrepo-com.png"));
                    ActionButton.ToolTip = Properties.Resources.friendsAcceptRequestTooltip;

                    SecondaryActionButton.Click += new RoutedEventHandler(RejectRequest);
                    SecondaryActionButtonImage.Source = new BitmapImage(new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Icons\\cross-circle-svgrepo-com.png"));
                    SecondaryActionButton.ToolTip = Properties.Resources.friendsRejectRequestTooltip;
                    SecondaryActionButton.Visibility = Visibility.Visible;
                    SecondaryActionButton.IsEnabled = true;
                    break;

                case 3:
                    ActionButton.Click += new RoutedEventHandler(UnblockUser);
                    ActionButtonImage.Source = new BitmapImage(new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Icons\\unfriend-svgrepo-com.png"));
                    ActionButton.ToolTip = Properties.Resources.friendsUnblockUserTooltip;

                    SecondaryActionButton.Visibility = Visibility.Hidden;
                    SecondaryActionButton.IsEnabled = false;
                    break;

                case 4:
                    ActionButton.Click += new RoutedEventHandler(InviteFriendToGame);
                    ActionButtonImage.Source = new BitmapImage(new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Icons\\invitation-svgrepo-com.png"));

                    SecondaryActionButton.Visibility = Visibility.Hidden;
                    SecondaryActionButton.IsEnabled = false;
                    break;

                default:
                    SecondaryActionButton.Visibility = Visibility.Hidden;
                    SecondaryActionButton.IsEnabled = false;
                    ActionButton.Visibility = Visibility.Hidden;
                    ActionButton.IsEnabled = false;
                    return;
            }

            UsernameLabel.Text = Username;
            UserImage.Source = ImagesService.GetImageFromId(imageId);
        }

        private void UnblockUser(object sender, RoutedEventArgs e)
        {
            RaiseUnblockedUserEvent();
        }

        private void BlockUser(object sender, RoutedEventArgs e)
        {
            RaiseBloquedUserEvent();
        }

        private void AcceptRequest(object sender, RoutedEventArgs e)
        {
            RaiseAcceptedFriendRequestEvent();
        }

        private void RejectRequest(object sender, RoutedEventArgs e)
        {
            RaiseRejectedFriendRequestEvent();
        }

        private void RemoveFriend(object sender, RoutedEventArgs e)
        {
            RaiseRemovedFriendEvent();
        }

        private void InviteFriendToGame(object sender, RoutedEventArgs e)
        {
            RaiseFriendInvitedEvent();
        }

        public event RoutedEventHandler FriendInvited
        {
            add { AddHandler(FriendInvitedEvent, value); }
            remove { RemoveHandler(FriendInvitedEvent, value); }
        }

        public void RaiseFriendInvitedEvent()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(routedEvent: FriendInvitedEvent);
            RaiseEvent(routedEventArgs);
        }

        public event RoutedEventHandler RejectedFriendRequest
        {
            add { AddHandler(RejectedFriendRequestEvent, value); }
            remove { RemoveHandler(RejectedFriendRequestEvent, value); }
        }

        public void RaiseRejectedFriendRequestEvent()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(routedEvent: RejectedFriendRequestEvent);
            RaiseEvent(routedEventArgs);
        }

        public event RoutedEventHandler AcceptedFriendRequest
        {
            add { AddHandler(AcceptedFriendRequestEvent, value); }
            remove { RemoveHandler(AcceptedFriendRequestEvent, value); }
        }

        public void RaiseAcceptedFriendRequestEvent()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(routedEvent: AcceptedFriendRequestEvent);
            RaiseEvent(routedEventArgs);
        }

        public event RoutedEventHandler UnblockedUser
        {
            add { AddHandler(UnblockedUserEvent, value); }
            remove { RemoveHandler(UnblockedUserEvent, value); }
        }

        public void RaiseUnblockedUserEvent()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(routedEvent: UnblockedUserEvent);
            RaiseEvent(routedEventArgs);
        }

        public event RoutedEventHandler BloquedUser
        {
            add { AddHandler(BloquedUserEvent, value); }
            remove { RemoveHandler(BloquedUserEvent, value); }
        }

        public void RaiseBloquedUserEvent()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(routedEvent: BloquedUserEvent);
            RaiseEvent(routedEventArgs);
        }

        public event RoutedEventHandler RemovedFriend
        {
            add { AddHandler(RemovedFriendEvent, value); }
            remove { RemoveHandler(RemovedFriendEvent, value); }
        }

        public void RaiseRemovedFriendEvent()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(routedEvent: RemovedFriendEvent);
            RaiseEvent(routedEventArgs);
        }
    }
}
