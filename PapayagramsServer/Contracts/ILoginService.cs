using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface ILoginService
    {
        [OperationContract]
        int Login(string username, string password);
    }
}
