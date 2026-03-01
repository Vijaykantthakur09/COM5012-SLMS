namespace SLMS.Models
{
    public class User
    {
        // Encapsulation: private set prevents random changes from outside
        public int Id { get; private set; }
        public string Name { get; private set; }

        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }

        // Polymorphism: subclasses can override this
        
        public virtual string Role() => "User";
    }
}
