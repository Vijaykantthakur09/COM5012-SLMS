using System;
using SLMS.Models;

namespace SLMS.Models
{
    public class Book
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }

        public BookStatus Status { get; private set; } = BookStatus.Available;

        public DateTime? DueDate { get; private set; }            // for Borrowed
        public DateTime? ReservedUntil { get; private set; }      // for Reserved
        public int? BorrowedByMemberId { get; private set; }      // track who borrowed (simple)

        public Book(int id, string title, string author)
        {
            Id = id;
            Title = title;
            Author = author;
        }

        public bool IsOverdue(DateTime today) =>
            Status == BookStatus.Borrowed && DueDate.HasValue && DueDate.Value.Date < today.Date;

        public void Borrow(int memberId, DateTime today)
        {
            Status = BookStatus.Borrowed;
            BorrowedByMemberId = memberId;
            DueDate = today.Date.AddDays(14);
            ReservedUntil = null;
        }

        public void Return()
        {
            Status = BookStatus.Available;
            BorrowedByMemberId = null;
            DueDate = null;
            ReservedUntil = null;
        }

        public void Reserve(DateTime today)
        {
            Status = BookStatus.Reserved;
            ReservedUntil = today.Date.AddDays(3);
        }

        public void ClearExpiredReservation(DateTime today)
        {
            if (Status == BookStatus.Reserved && ReservedUntil.HasValue && ReservedUntil.Value.Date < today.Date)
            {
                Status = BookStatus.Available;
                ReservedUntil = null;
            }
        }
    }
}
