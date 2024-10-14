using DomainClasses;

namespace DataAccess
{
    public class UserDB
    {
        public static void RegisterUser(Player player)
        {
            using (var context = new papayagramsEntities())
            {
                context.register_user(player.Username,player.Email,player.Password);
                context.SaveChanges();
            }
        }
    }
}
