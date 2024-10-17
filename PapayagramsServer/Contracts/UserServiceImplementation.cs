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
                Option<Player> foundPlayer = UserDB.GetPlayer(username);
                if (foundPlayer.IsSome)
                {
                    Player player = (Player)foundPlayer.Case;
                    if (player.Password == password)
                    {
                        playerLogged = ConvertToContractClass(player);
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

        private PlayerDC ConvertToContractClass(Player player)
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
