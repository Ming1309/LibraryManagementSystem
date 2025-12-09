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
        // Dependency Injection: Only keep Service reference
        private static readonly BookService _service;

        /// <summary>
        /// Static constructor - Composition Root
        /// Initialize and wire up all dependencies here
        /// </summary>
        static Program()
        {
            // Create dependencies (local variables, not fields)
            IPathProvider pathProvider = new ConsolePathProvider();
            IRepository<Book> repository = new BookRepository(pathProvider);
            
            // Only store the Service layer reference
            _service = new BookService(repository);
        }

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
                        Console.WriteLine(Messages.ExitProgram);
                        return; 
                    default:
                        Console.WriteLine(Messages.InvalidFunction);
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
            if (!int.TryParse(yearInput, out int year))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Messages.ErrorYearMustBeNumber);
                Console.ResetColor();
                return;
            }
            newBook.PublishYear = year;

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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Messages.ErrorIdMustBeNumber);
                Console.ResetColor();
                return;
            }

            Book? oldBook = _service.GetBookById(id);
            if (oldBook == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Messages.ErrorBookNotFound);
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
                if (int.TryParse(newYearStr, out int y))
                {
                    finalYear = y;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(Messages.ErrorInvalidYearInput);
                    Console.ResetColor();
                    return;
                }
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
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Messages.ErrorIdMustBeNumber);
                Console.ResetColor();
                return;
            }

            Book? book = _service.GetBookById(id);
            if (book == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Messages.ErrorBookNotFound);
                Console.ResetColor();
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
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(Messages.DeleteCancelled);
                Console.ResetColor();
            }
        }

        static void DisplayTable(List<Book> listData, string emptyMessage = null!)
        {
            if (listData.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(emptyMessage ?? Messages.ListEmpty);
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
            
            DisplayTable(results, Messages.SearchNoResults); 
        }


        static string FormatString(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input)) return "";
            if (input.Length <= maxLength) return input;
            return input.Substring(0, maxLength - 3) + "...";
        }
    }
}