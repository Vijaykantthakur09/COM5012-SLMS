using System.Collections.Generic;

namespace SLMS.Models
{
    public class Member : User
    {
        private readonly List<int> _borrowedBookIds = new();

        public Member(int id, string name) : base(id, name) { }

        // Encapsulation: expose read-only list view
        public IReadOnlyList<int> BorrowedBookIds => _borrowedBookIds.AsReadOnly();

        public int BorrowedCount => _borrowedBookIds.Count;

        public override string Role() => "Member"; // Polymorphism (override)

        public bool CanBorrow() => BorrowedCount < 5;

        public void AddBorrowedBook(int bookId) => _borrowedBookIds.Add(bookId);

        public bool RemoveBorrowedBook(int bookId) => _borrowedBookIds.Remove(bookId);
    }
}
