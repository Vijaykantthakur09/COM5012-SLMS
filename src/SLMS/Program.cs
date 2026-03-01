using System;
using System.Linq;
using SLMS;
using SLMS.Models;

var system = new LibrarySystem();

Console.WriteLine("===========================================");
Console.WriteLine("  Smart Library Management System (SLMS)  ");
Console.WriteLine("===========================================");

// Show librarian on duty
var librarian = system.Librarians.First();
Console.WriteLine($"Librarian on duty : {librarian.Name} ({librarian.Role()})");

// Show available members and let user pick
Console.WriteLine("\nRegistered Members:");
foreach (var m in system.Members)
    Console.WriteLine($"  [{m.Id}] {m.Name} ({m.Role()})");

int memberId = 0;
while (true)
{
    Console.Write("\nSelect Member ID to log in: ");
    if (int.TryParse(Console.ReadLine(), out memberId) &&
        system.Members.Any(m => m.Id == memberId))
        break;
    Console.WriteLine("Invalid selection. Please enter a valid Member ID.");
}

var activeMember = system.Members.First(m => m.Id == memberId);
Console.WriteLine($"\nWelcome, {activeMember.Name}!\n");

while (true)
{
    Console.WriteLine("Menu:");
    Console.WriteLine("  1) List all books");
    Console.WriteLine("  2) Search books");
    Console.WriteLine("  3) Borrow book");
    Console.WriteLine("  4) Return book");
    Console.WriteLine("  5) Reserve book");
    Console.WriteLine("  6) Overdue report");
    Console.WriteLine("  7) Switch member");
    Console.WriteLine("  0) Exit");
    Console.Write("Choose: ");

    var choice = Console.ReadLine()?.Trim();
    if (choice == "0") break;

    switch (choice)
    {
        case "1":
            system.ClearExpiredReservations();
            PrintBooks(system.Books);
            break;

        case "2":
            Console.Write("Keyword (title/author): ");
            var keyword = Console.ReadLine() ?? "";
            var results = system.Search(keyword).ToList();
            if (results.Count == 0) Console.WriteLine("No matches found.");
            else PrintBooks(results);
            break;

        case "3":
            Console.Write("Enter Book ID to borrow: ");
            if (!int.TryParse(Console.ReadLine(), out var borrowId))
            {
                Console.WriteLine("Invalid Book ID.");
                break;
            }
            var result = system.BorrowBook(memberId, borrowId);
            if (result.StartsWith("Borrowed"))
            {
                var borrowBook = system.Books.FirstOrDefault(b => b.Id == borrowId);
                Console.WriteLine(librarian.IssueBook(activeMember.Name, borrowBook?.Title ?? "Unknown"));
            }
            Console.WriteLine(result);
            break;

        case "4":
            Console.Write("Enter Book ID to return: ");
            if (!int.TryParse(Console.ReadLine(), out var returnId))
            {
                Console.WriteLine("Invalid Book ID.");
                break;
            }
            var returningBook = system.Books.FirstOrDefault(b => b.Id == returnId);
            var returnMsg = system.ReturnBook(memberId, returnId);
            Console.WriteLine(returnMsg);
            if (returningBook != null && returnMsg.StartsWith("Returned"))
                Console.WriteLine(librarian.ApproveReturn(returningBook.Title));
            break;

        case "5":
            Console.Write("Enter Book ID to reserve: ");
            if (!int.TryParse(Console.ReadLine(), out var reserveId))
            {
                Console.WriteLine("Invalid Book ID.");
                break;
            }
            Console.WriteLine(system.ReserveBook(reserveId));
            break;

        case "6":
            var overdue = system.OverdueReport().ToList();
            if (overdue.Count == 0) Console.WriteLine("No overdue books.");
            else
            {
                Console.WriteLine("Overdue books:");
                PrintBooks(overdue);
            }
            break;

        case "7":
            Console.WriteLine("\nRegistered Members:");
            foreach (var m in system.Members)
                Console.WriteLine($"  [{m.Id}] {m.Name}");
            while (true)
            {
                Console.Write("Select Member ID: ");
                if (int.TryParse(Console.ReadLine(), out var newId) &&
                    system.Members.Any(m => m.Id == newId))
                {
                    memberId = newId;
                    activeMember = system.Members.First(m => m.Id == memberId);
                    Console.WriteLine($"Switched to: {activeMember.Name}");
                    break;
                }
                Console.WriteLine("Invalid selection.");
            }
            break;

        default:
            Console.WriteLine("Invalid option. Choose 0–7.");
            break;
    }

    Console.WriteLine();
}

Console.WriteLine("Goodbye!");

static void PrintBooks(System.Collections.Generic.IEnumerable<Book> books)
{
    foreach (var b in books)
    {
        var due = b.DueDate.HasValue ? b.DueDate.Value.ToString("yyyy-MM-dd") : "-";
        var res = b.ReservedUntil.HasValue ? b.ReservedUntil.Value.ToString("yyyy-MM-dd") : "-";
        Console.WriteLine($"  [{b.Id}] {b.Title} — {b.Author} | {b.Status} | Due: {due} | Reserved: {res}");
    }
}
