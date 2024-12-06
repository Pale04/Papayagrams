
namespace DomainClasses
{
    public enum ApplicationLanguage
    {
        english,
        spanish,
        auto
    }

    public class ApplicationSettings
    {
        public int PieceColor { get; set; }
        public ApplicationLanguage SelectedLanguage { get; set; }
        public int Cursor { get; set; }

        public override bool Equals(object obj)
        {
            bool isEqual = false;
            if (obj != null && GetType() == obj.GetType())
            {
                ApplicationSettings other = (ApplicationSettings)obj;
                isEqual = PieceColor == other.PieceColor && SelectedLanguage.Equals(other.SelectedLanguage) && Cursor == other.Cursor;
            }
            return isEqual;
        }

        public override int GetHashCode()
        {
            return PieceColor.GetHashCode() ^ SelectedLanguage.GetHashCode() ^ Cursor.GetHashCode();
        }
    }
}
