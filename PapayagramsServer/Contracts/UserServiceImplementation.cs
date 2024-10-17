using DataAccess;
using DomainClasses;
using LanguageExt;
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

            Option<Player> userWithSameUsername = UserDB.GetPlayerByUsername(newPlayer.Username);

            if (userWithSameUsername.IsSome)
            {
                throw new Exception("An account with the same username exists");
            }
            else
            {
                Option<Player> userWithSameEmail = UserDB.GetPlayerByEmail(newPlayer.Email);
                if (userWithSameEmail.IsSome)
                {
                    throw new Exception("An account with the same email exists");
                }
            }

            return UserDB.RegisterUser(newPlayer);
        }
    }
}
