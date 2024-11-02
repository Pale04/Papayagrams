using BussinessLogic;
using System;
using System.ServiceModel;

namespace Contracts
{
    public partial class ServiceImplementation : IGameService
    {
        public void LeaveGame(string username)
        {
            CallbacksPool.RemoveGameCallbackChannel(username);
        }

        public void ReachServer(string username)
        {
            CallbacksPool.PlayerArrivesToGame(username, OperationContext.Current.GetCallbackChannel<IGameServiceCallback>());
            CallbacksPool.RemovePregameCallbackChannel(username);
        }
    }
}
