using DataAccess;
using DomainClasses;
using System.ServiceModel;

namespace Contracts
{
    public partial class ServiceImplementation : IUserService
    {
        public int RegisterUser(Player player)
        {
            int result = 0;
            if (player != null && player.HasValidAtributes())
            {
                result = UserDB.RegisterUser(player);        
            }
            else
            {
                //TODO: Send error message to client, maybe a personalized exception
            }

            return result;
        }
    }
}
