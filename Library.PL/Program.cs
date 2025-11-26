using System;
using Library.BLL;
using Library.Core;
using System.Text;

namespace Library.PL
{
    class Program
    {
        static BookService _service = new BookService();
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            while (true)
            {
                ShowMenu();
                Console.Write("Chọn chức năng: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewBooks();
                        break;
                    case "2":
                        CreateBook();
                        break;
                    case "0":
                        Console.WriteLine("Đang thoát chương trình...");
                        return; 
                    default:
                        Console.WriteLine("Chức năng không hợp lệ. Vui lòng chọn lại!");
                        break;
                }

                Console.WriteLine("\nẤn phím bất kỳ để quay lại Menu...");
                Console.ReadKey();
            }
        }
        static void ShowMenu()
        {
            Console.Clear(); // Xóa màn hình cho sạch
            Console.WriteLine("========================================");
            Console.WriteLine("    HỆ THỐNG QUẢN LÝ THƯ VIỆN (V1.0)    ");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Xem danh sách Sách");
            Console.WriteLine("2. Thêm Sách mới");
            Console.WriteLine("0. Thoát");
            Console.WriteLine("========================================");
        }

        static void ViewBooks()
        {
            Console.WriteLine("\n--- DANH SÁCH SÁCH ---");
            List<Book> books = _service.GetAllBooks();

            if (books.Count == 0)
            {
                Console.WriteLine("Hiện chưa có cuốn sách nào.");
            }
            else
            {
                Console.WriteLine("{0,-5} | {1,-30} | {2,-20} | {3,-10}", "ID", "TÊN SÁCH", "TÁC GIẢ", "NĂM");
                Console.WriteLine(new string('-', 75));

                foreach (var b in books)
                {
                    Console.WriteLine("{0,-5} | {1,-30} | {2,-20} | {3,-10}", 
                        b.Id, 
                        FormatString(b.Title, 30), 
                        FormatString(b.Author, 20), 
                        b.PublishYear);
                }
            }
        }

        static void CreateBook()
        {
            Console.WriteLine("\n--- THÊM SÁCH MỚI ---");
            
            Book newBook = new Book();

            Console.Write("Nhập tên sách: ");
            newBook.Title = Console.ReadLine();

            Console.Write("Nhập tác giả: ");
            newBook.Author = Console.ReadLine();

            Console.Write("Nhập năm xuất bản: ");
            string yearInput = Console.ReadLine();
            if (int.TryParse(yearInput, out int year))
            {
                newBook.PublishYear = year;
            }
            else
            {
                newBook.PublishYear = -1;
            }

            string result = _service.AddBook(newBook);

            if (result.StartsWith("Thành công"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(result);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result);
                Console.ResetColor();
            }
        }

        static string FormatString(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input)) return "";
            if (input.Length <= maxLength) return input;
            return input.Substring(0, maxLength - 3) + "...";
        }
    }
}