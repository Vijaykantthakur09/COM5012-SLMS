using System;
using System.Collections.Generic;
using System.Linq;
using SLMS.Models;

namespace SLMS
{
    
    public class LibrarySystem
    {
        public List<Book> Books { get; } = new();
        public List<Member> Members { get; } = new();

        public LibrarySystem()
        {
            // Sample data
            Books.Add(new Book(1, "The Maze Runner", "James Dashner"));
            Books.Add(new Book(2, "1984", "George Orwell"));
            Books.Add(new Book(3, "Dune", "Frank Herbert"));

            Members.Add(new Member(1, "Vijay"));
        }

        public void ClearExpiredReservations()
        {
            var today = DateTime.Today;
            foreach (var b in Books)
                b.ClearExpiredReservation(today);
        }

        public IEnumerable<Book> Search(string keyword)
        {
            keyword = keyword?.Trim() ?? "";
            return Books.Where(b =>
                b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        public string BorrowBook(int memberId, int bookId)
        {
            ClearExpiredReservations();

            var member = Members.FirstOrDefault(m => m.Id == memberId);
            if (member == null) return "Member not found.";

            if (!member.CanBorrow()) return "Borrow limit reached (max 5 books).";

            var book = Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null) return "Book not found.";

            if (book.Status != BookStatus.Available)
                return $"Book is not available (current status: {book.Status}).";

            book.Borrow(member.Id, DateTime.Today);
            member.AddBorrowedBook(book.Id);

            return $"Borrowed: {book.Title} (Due: {book.DueDate:yyyy-MM-dd})";
        }

        public string ReturnBook(int memberId, int bookId)
        {
            var member = Members.FirstOrDefault(m => m.Id == memberId);
            if (member == null) return "Member not found.";

            var book = Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null) return "Book not found.";

            if (!member.BorrowedBookIds.Contains(bookId))
                return "This member did not borrow that book.";

            book.Return();
            member.RemoveBorrowedBook(bookId);

            return $"Returned: {book.Title}";
        }

        public string ReserveBook(int bookId)
        {
            ClearExpiredReservations();

            var book = Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null) return "Book not found.";

            if (book.Status != BookStatus.Borrowed)
                return $"You can only reserve a book that is currently Borrowed (status: {book.Status}).";

            book.Reserve(DateTime.Today);
            return $"Reserved: {book.Title} (Until: {book.ReservedUntil:yyyy-MM-dd})";
        }

        public IEnumerable<Book> OverdueReport()
        {
            var today = DateTime.Today;
            return Books.Where(b => b.IsOverdue(today));
        }
    }
}
