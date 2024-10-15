
namespace DomainClasses
{
    public class Player
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool HasValidAtributes()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
        }
    }
}
