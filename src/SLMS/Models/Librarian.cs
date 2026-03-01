namespace SLMS.Models
{
    public class Librarian : User
    {
        public Librarian(int id, string name) : base(id, name) { }

        // Polymorphism: override Role to identify as Librarian
        public override string Role() => "Librarian";

        // Librarians can add books, manage members, etc.
        public string ApproveReturn(string bookTitle)
            => $"Librarian {Name} approved return of '{bookTitle}'.";

        public string IssueBook(string memberName, string bookTitle)
            => $"Librarian {Name} issued '{bookTitle}' to {memberName}.";
    }
}
