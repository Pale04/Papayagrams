using DataAccess;
using DomainClasses;
using System.ServiceModel;

namespace Contracts
{
    public partial class ServiceImplementation : IUserService
    {
        public int RegisterUser(string user, string email, string password)
        {
            Player player = new Player() { Username = user, Email = email, Password = password };

            int result = 0;
            if (player.HasValidAtributes())
            {
                result = UserDB.RegisterUser(player);
            }
            return result;
        }
    }
}
