using DataAccess;
using DomainClasses;
using System;

namespace Contracts
{
    public partial class ServiceImplementation : IUserService
    {
        public int RegisterUser(PlayerDC player)
        {
            int result = 0;
            Player newPlayer = new Player();

            try
            {
                newPlayer.Username = player.Username;
                newPlayer.Email = player.Email;
                newPlayer.Password = player.Password;
            }
            catch (ArgumentException error)
            {
                //TODO: Log error
                result = -1;
            }

            if (result != -1)
            {
                result = UserDB.RegisterUser(newPlayer);
            }

            return result;
        }

        public PlayerDC LogIn(string username, string password)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
    }
}
