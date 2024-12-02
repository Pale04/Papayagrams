
namespace DomainClasses
{
    public enum RelationState
    {
        Pending,
        Blocked,
        Friend
    }

    public class Friend: Player
    {
        public RelationState RelationState { get; set; }

        public override bool Equals(object obj)
        {
            bool isEqual = false;
            if (obj != null && GetType() == obj.GetType())
            {
                Friend friend = (Friend)obj;
                isEqual = Id == friend.Id && Username.Equals(friend.Username) && RelationState.Equals(friend.RelationState);
            }
            return isEqual;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Username.GetHashCode() ^ RelationState.GetHashCode();
        }
    }
}
