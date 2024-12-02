using System.Windows;
using System.Windows.Controls;

namespace PapayagramsClient.Menu
{
    public partial class GameInvitation : UserControl
    {
        public static readonly RoutedEvent AcceptedInvitationEvent = EventManager.RegisterRoutedEvent(
            name: "AcceptedInvitation",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(GameInvitation));

        public static readonly RoutedEvent RejectedInvitationEvent = EventManager.RegisterRoutedEvent(
            name: "RejectedInvitation",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(GameInvitation));

        public GameInvitation()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler AcceptedInvitation
        {
            add { AddHandler(AcceptedInvitationEvent, value); }
            remove { RemoveHandler(AcceptedInvitationEvent, value); }
        }

        public void RaiseAcceptedInvitationEvent()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(routedEvent: AcceptedInvitationEvent);
            RaiseEvent(routedEventArgs);
        }

        public event RoutedEventHandler RejectedInvitation
        {
            add { AddHandler(RejectedInvitationEvent, value); }
            remove { RemoveHandler(RejectedInvitationEvent, value); }
        }

        public void RaiseRejectedInvitationEvent()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(routedEvent: RejectedInvitationEvent);
            RaiseEvent(routedEventArgs);
        }

        private void AcceptInvitation(object sender, RoutedEventArgs e)
        {
            RaiseAcceptedInvitationEvent();
        }

        private void RejectInvitation(object sender, RoutedEventArgs e)
        {
            RaiseRejectedInvitationEvent();
        }
    }
}
