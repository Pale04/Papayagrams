
namespace DomainClasses
{
    public class Achievement
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsAchieved { get; set; }

        public override bool Equals(object obj)
        {
            bool isEqual = false;

            if (obj != null && GetType() == obj.GetType())
            {
                Achievement achievement = (Achievement)obj;
                isEqual = Id == achievement.Id && Description.Equals(achievement.Description) && IsAchieved == achievement.IsAchieved;
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Description.GetHashCode() ^ IsAchieved.GetHashCode();
        }
    }
}
