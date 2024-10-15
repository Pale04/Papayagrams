using BussinessLogic;
using DomainClasses;
using System;
using System.ServiceModel;

namespace Contracts
{
    public partial class ServiceImplementation : ILoginService
    {
        public int Login(string username, string password)
        {
            Console.WriteLine("Login attempt for user: " + username);
            Player player = new Player() { Email = "mail@example.com", Username = username};
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
