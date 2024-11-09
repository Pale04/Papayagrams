using BussinessLogic;
using System;
using System.ServiceModel;

namespace Contracts
{
    public partial class ServiceImplementation : IGameService
    {
        public void DumpPiece(string piece)
        {
            throw new NotImplementedException();
        }

        public void LeaveGame(string username)
        {
            CallbacksPool.RemoveGameCallbackChannel(username);
        }

        public void ReachServer(string username)
        {
            CallbacksPool.PlayerArrivesToGame(username, OperationContext.Current.GetCallbackChannel<IGameServiceCallback>());
            CallbacksPool.RemovePregameCallbackChannel(username);
            //TODO: esperar a que todos reporten para que llame a StartGame()
        }

        public void ShoutPapaya(string username)
        {
            throw new NotImplementedException();
        }

        public void TakeSeed()
        {
            throw new NotImplementedException();
        }

        //manda las piezas e inicia el cronometro
        private void StartGame()
        {
            throw new NotImplementedException();
        }
        
    }
}
