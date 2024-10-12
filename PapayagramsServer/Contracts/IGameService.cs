using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IGameServiceCallback))]
    public interface IGameService
    {
        [OperationContract]
        void CreateGame();
        void JoinGame(int roomCode);
    }

    [ServiceContract]
    public interface IGameServiceCallback 
    {
        [OperationContract]
        void JoinGame(int roomCode);
    }
}
