using System;

namespace DomainClasses
{
    public enum PlayerStatus
    {
        online,
        offline,
        in_game
    }

    public class Player
    {
        private string _username;
        private string _email;
        private string _password;

        public int Id { get; set; }

        /// <summary>
        /// Obtain or set the username of the player
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when try to set a empty username</exception>
        public string Username 
        { 
            get { return _username; } 
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Username cannot be empty");
                }
                _username = value;
            } 
        }

        /// <summary>
        /// Obtain or set the email of the player
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when try to set a empty email</exception>"
        public string Email 
        {
            get { return _email; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Email cannot be empty");
                }
                _email = value;
            }
        }

        /// <summary>
        /// Obtain or set the password of the player
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when try to set a empty password</exception>"
        public string Password 
        {
            get { return _password; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Password cannot be empty");
                }
                _password = value;
            }
        }

        public override bool Equals(object other)
        {
            bool isEqual = false;

            if (other != null && GetType() == other.GetType())
            {
                Player player = (Player)other;
                isEqual = Id == player.Id && Username == player.Username && Email == player.Email && Password == player.Password;
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Username.GetHashCode() ^ Email.GetHashCode() ^ Password.GetHashCode();
        }
    }
}
