using BussinessLogic;
using DomainClasses;
using System;
using System.ServiceModel;

namespace Contracts
{
    public class LoginServiceImplementation : ILoginService
    {
        public int Login(string username, string password)
        {
            Player player = new Player() { Email = "mail@example.com", UserName = username};
            PlayerData.AddPlayer(player, OperationContext.Current);
            return 0;
        }

        public int Logout()
        {
            PlayerData.RemovePlayer(OperationContext.Current);
            return 0;
        }
    }
}
