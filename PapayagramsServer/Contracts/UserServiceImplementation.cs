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

        public PlayerDC LogIn(string username, string password)
        {
            PlayerDC playerLogged;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Username and password cannot be empty");
            }
            else
            {
                Option<Player> foundPlayer = UserDB.GetPlayerByUsername(username);
                if (foundPlayer.IsSome)
                {
                    Player player = (Player)foundPlayer.Case;
                    if (player.Password == password)
                    {
                        playerLogged = ConvertPlayerToDataContract(player);
                    }
                    else
                    {
                        throw new Exception("Incorrect password");
                    }
                }
                else
                {
                    throw new Exception("Player not found");
                }
            }

            return playerLogged;
        }

        private PlayerDC ConvertPlayerToDataContract(Player player)
        {
            return new PlayerDC()
            {
                Id = player.Id,
                Username = player.Username,
                Email = player.Email,
                Password = player.Password
            };
        }
    }
}
