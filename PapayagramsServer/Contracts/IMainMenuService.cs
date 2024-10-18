using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IMainMenuServiceCallback))]
    public interface IMainMenuService
    {

    }

    [ServiceContract]
    public interface IMainMenuServiceCallback
    {

    }
}
