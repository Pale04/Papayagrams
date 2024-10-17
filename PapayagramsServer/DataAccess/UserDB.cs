using DomainClasses;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DataAccess
{
    public class UserDB
    {
        public static int RegisterUser(Player player)
        {
            int result = 0;
            using (var context = new papayagramsEntities())
            {
                //The stored procedure register_user that provide the PapayagramsModel does not return the number of rows affected and the ExecuteSqlCommand method does
                SqlParameter usernameParameter = new SqlParameter("@param1", player.Username);
                SqlParameter emailParameter = new SqlParameter("@param2", player.Email);
                SqlParameter passwordParameter = new SqlParameter("@param3", player.Password);
                result = context.Database.ExecuteSqlCommand("EXEC register_user @param1, @param2, @param3", usernameParameter, emailParameter, passwordParameter);
            }
            return result;
        }

        public static Player LogIn(string username, string password)
        {
            Player player = null;

            using (var context = new papayagramsEntities())
            {
                var result = context.login(username, password);
                List<login_Result> userLogged = result.ToList();

                if (userLogged.Count > 0)
                {
                    player = convertToDomainClass(userLogged[0]);
                }
            }

            return player;
        }

        private static Player convertToDomainClass(login_Result result)
        {
            return new Player()
            {
                Id = result.id,
                Username = result.username,
                Email = result.email,
                Password = result.password
            };
        }
    }
}
