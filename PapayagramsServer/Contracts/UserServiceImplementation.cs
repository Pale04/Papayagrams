using DataAccess;
using DomainClasses;
using System;

namespace Contracts
{
    public partial class ServiceImplementation : IUserService
    {
        public int RegisterUser(PlayerDC player)
        {
            Player newPlayer = new Player()
            {
                Username = player.Username,
                Email = player.Email,
                Password = player.Password
            };

            if (UserDB.GetPlayerByUsername(newPlayer.Username).IsSome)
            {
                throw new Exception("An account with the same username exists");
            }
            else if (UserDB.GetPlayerByEmail(newPlayer.Email).IsSome)
            {
                throw new Exception("An account with the same email exists");
            }

            return UserDB.RegisterUser(newPlayer);
        }
    }
}
