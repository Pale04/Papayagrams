using System.Runtime.Serialization;

namespace Contracts
{
    [DataContract]
    public class PlayerDC
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        
        public override bool Equals(object obj)
        {
            bool isEqual = false;

            if (obj != null && GetType() == obj.GetType())
            {
                PlayerDC player = (PlayerDC)obj;
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
