using System;
using System.Linq;
using SLMS;
using SLMS.Models;

var system = new LibrarySystem();
const int memberId = 1; // demo member

Console.WriteLine("Smart Library Management System (SLMS)");
Console.WriteLine("Role test (polymorphism): " + system.Members[0].Role());

while (true)
{
    Console.WriteLine("\nMenu:");
    Console.WriteLine("1) List all books");
    Console.WriteLine("2) Search books");
    Console.WriteLine("3) Borrow book");
    Console.WriteLine("4) Return book");
    Console.WriteLine("5) Reserve book");
    Console.WriteLine("6) Overdue report");
    Console.WriteLine("0) Exit");
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
            Console.WriteLine(system.BorrowBook(memberId, borrowId));
            break;

        case "4":
            Console.Write("Enter Book ID to return: ");
            if (!int.TryParse(Console.ReadLine(), out var returnId))
            {
                Console.WriteLine("Invalid Book ID.");
                break;
            }
            Console.WriteLine(system.ReturnBook(memberId, returnId));
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

        default:
            Console.WriteLine("Invalid option. Choose 0–6.");
            break;
    }
}

Console.WriteLine("Goodbye!");

static void PrintBooks(System.Collections.Generic.IEnumerable<Book> books)
{
    foreach (var b in books)
    {
        var due = b.DueDate.HasValue ? b.DueDate.Value.ToString("yyyy-MM-dd") : "-";
        var res = b.ReservedUntil.HasValue ? b.ReservedUntil.Value.ToString("yyyy-MM-dd") : "-";
        Console.WriteLine($"[{b.Id}] {b.Title} — {b.Author} | {b.Status} | Due: {due} | ReservedUntil: {res}");
    }
}
