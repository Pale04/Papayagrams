using DomainClasses;

namespace DataAccess
{
    public class UserDB
    {
        public static int RegisterUser(Player player)
        {
            int result = 0;
            using (var context = new papayagramsEntities())
            {
                context.register_user(player.Username,player.Email,player.Password);
                result = context.SaveChanges();
            }
            return result;
        }

        public static void LoginUser(Player player)
        {
            using (var context = new papayagramsEntities())
            {
                //context.login_user(player.Username, player.Password);
                //context.SaveChanges();
            }
        }
    }
}
