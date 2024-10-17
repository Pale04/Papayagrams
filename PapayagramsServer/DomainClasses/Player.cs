using System;

namespace DomainClasses
{
    public class Player
    {
        private string _username;
        private string _email;
        private string _password;

        public int Id { get; set; }

        /// <summary>
        /// Obtain or set the username of the player
        /// Throws <see cref="ArgumentException"/> if try to set a empty username
        /// </summary>
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
        /// Throws <see cref="ArgumentException"/> if try to set a empty email
        /// </summary>
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
        /// Throws <see cref="ArgumentException"/> if try to set a empty password
        /// </summary>
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
    }
}
