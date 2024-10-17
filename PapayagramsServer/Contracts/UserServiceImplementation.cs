using DataAccess;
using DomainClasses;

namespace Contracts
{
    public class UserServiceImplementation : IUserService
    {
        public void RegisterUser(Player player)
        {
            UserDB.RegisterUser(player);
        }
    }
}
