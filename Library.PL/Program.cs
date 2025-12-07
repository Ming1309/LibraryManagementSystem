using System;
using Library.BLL;
using Library.Core;
using Library.Core.Common;
using Library.Core.Constants;
using Library.Core.Interfaces;
using Library.PL.Infrastructure;
using Library.DAL;
using System.Text;

namespace Library.PL
{
    class Program
    {
        // Dependency Injection: Create dependencies first
        private static readonly IPathProvider _pathProvider = new ConsolePathProvider();
        private static readonly IRepository<Book> _repository = new BookRepository(_pathProvider);
        private static readonly BookService _service = new BookService(_repository);

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            while (true)
            {
                ShowMenu();
                Console.Write("Select function: ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        ViewBooks();
                        break;
                    case "2":
                        CreateBook();
                        break;
                    case "3": 
                        UpdateBookUI(); 
                        break;
                    case "4": 
                        DeleteBookUI(); 
                        break;
                    case "5":
                        SearchBooksUI();
                        break;
                    case "0":
                        Console.WriteLine("Exiting program...");
                        return; 
                    default:
                        Console.WriteLine("Invalid function. Please select again!");
                        break;
                }

                Console.WriteLine("\nPress any key to return to Menu...");
                Console.ReadKey();
            }
        }
        static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("   LIBRARY MANAGEMENT SYSTEM (V1.0)    ");
            Console.WriteLine("========================================");
            Console.WriteLine("1. View Book List");
            Console.WriteLine("2. Add New Book");
            Console.WriteLine("3. Update Book");
            Console.WriteLine("4. Delete Book");
            Console.WriteLine("5. Search Books");
            Console.WriteLine("0. Exit");
            Console.WriteLine("========================================");
        }

        static void ViewBooks()
        {
            Console.WriteLine("\n--- ALL BOOKS LIST ---");
            List<Book> books = _service.GetAllBooks();
            DisplayTable(books); 
        }

        static void CreateBook()
        {
            Console.WriteLine("\n--- ADD NEW BOOK ---");
            
            Book newBook = new Book();

            Console.Write("Enter book title: ");
            newBook.Title = Console.ReadLine() ?? "";

            Console.Write("Enter author: ");
            newBook.Author = Console.ReadLine() ?? "";

            Console.Write("Enter publish year: ");
            string yearInput = Console.ReadLine() ?? "";
            if (int.TryParse(yearInput, out int year))
            {
                newBook.PublishYear = year;
            }
            else
            {
                newBook.PublishYear = -1;
            }

            Result result = _service.AddBook(newBook);

            if (result.IsSuccess)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(result.Message);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Message);
                Console.ResetColor();
            }
        }

        static void UpdateBookUI()
        {
            Console.WriteLine("\n--- UPDATE BOOK INFORMATION ---");
            Console.Write("Enter book ID to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID must be a number!");
                return;
            }

            Book? oldBook = _service.GetBookById(id);
            if (oldBook == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Book with this ID not found!");
                Console.ResetColor();
                return;
            }

            Console.WriteLine($"Editing book: {oldBook.Title}");
            Console.WriteLine("(Press Enter to keep old value)");

            Console.Write($"New title (Old: {oldBook.Title}): ");
            string? newTitle = Console.ReadLine();
            string finalTitle = string.IsNullOrWhiteSpace(newTitle) ? oldBook.Title : newTitle;

            Console.Write($"New author (Old: {oldBook.Author}): ");
            string? newAuthor = Console.ReadLine();
            string finalAuthor = string.IsNullOrWhiteSpace(newAuthor) ? oldBook.Author : newAuthor;

            Console.Write($"New publish year (Old: {oldBook.PublishYear}): ");
            string? newYearStr = Console.ReadLine();
            int finalYear = oldBook.PublishYear; 
            if (!string.IsNullOrWhiteSpace(newYearStr))
            {
                if (int.TryParse(newYearStr, out int y)) finalYear = y;
                else Console.WriteLine("Invalid year input, will keep old year.");
            }

            Book bookToUpdate = new Book(id, finalTitle, finalAuthor, finalYear);

            Result result = _service.UpdateBook(bookToUpdate);
            
            if (result.IsSuccess)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Red;
            
            Console.WriteLine(result.Message);
            Console.ResetColor();
        }

        static void DeleteBookUI()
        {
            Console.WriteLine("\n--- DELETE BOOK ---");
            Console.Write("Enter book ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            Book? book = _service.GetBookById(id);
            if (book == null)
            {
                Console.WriteLine("Book not found!");
                return;
            }

            Console.WriteLine($"Are you sure you want to delete: '{book.Title}'? (y/n)");
            string? confirm = Console.ReadLine();

            if (confirm?.ToLower() == "y")
            {
                Result result = _service.DeleteBook(id);
                
                if (result.IsSuccess)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Red;
                
                Console.WriteLine(result.Message);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Delete operation cancelled.");
            }
        }

        static void DisplayTable(List<Book> listData, string emptyMessage = "List is empty!")
        {
            if (listData.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(emptyMessage);
                Console.ResetColor();
                return;
            }

            Console.WriteLine("{0,-5} | {1,-30} | {2,-20} | {3,-10}", "ID", "TITLE", "AUTHOR", "YEAR");
            Console.WriteLine(new string('-', 75));

            foreach (var b in listData)
            {
                Console.WriteLine("{0,-5} | {1,-30} | {2,-20} | {3,-10}", 
                    b.Id, 
                    FormatString(b.Title, AppSettings.MaxTitleDisplayLength), 
                    FormatString(b.Author, AppSettings.MaxAuthorDisplayLength), 
                    b.PublishYear);
            }
        }

        static void SearchBooksUI()
        {
            Console.WriteLine("\n--- SEARCH BOOKS ---");
            Console.WriteLine("Tip: Use 'author:keyword' to search by author only");
            Console.WriteLine("     Use 'title:keyword' to search by title only");
            Console.WriteLine("     Or just enter keyword to search both");
            Console.Write("\nEnter search term: ");
            string keyword = Console.ReadLine() ?? "";

            List<Book> results = _service.SearchBooks(keyword);

            Console.WriteLine($"\nFound {results.Count} result(s) for '{keyword}':");
            
            DisplayTable(results, "No books found matching your search."); 
        }


        static string FormatString(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input)) return "";
            if (input.Length <= maxLength) return input;
            return input.Substring(0, maxLength - 3) + "...";
        }
    }
}