using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IChatServiceCallback))]
    public interface IChatService
    {
        [OperationContract]
        void SendMessage(string message);
    }

    [ServiceContract]
    public interface IChatServiceCallback
    {
        void ReceiveMessage(string message);
    }
}
