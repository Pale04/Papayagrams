using Microsoft.VisualStudio.TestTools.UnitTesting;
using PapayagramsClient.PapayagramsService;

namespace ClientTests
{
    [TestClass]
    public class MainMenuCallbackTests : IMainMenuServiceCallback
    {
        public void ReceiveFriendRequest(PlayerDC player)
        {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void ReceiveGameInvitation(GameInvitationDC invitation)
        {
            throw new System.NotImplementedException();
        }
    }
}
