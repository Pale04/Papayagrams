using System.ServiceModel;

namespace DomainClasses
{
    public class Player
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email {  get; set; }
        public OperationContext Context { get; set; }
    }
}
