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
                    case "3": 
                        UpdateBookUI(); 
                        break;
                    case "4": 
                        DeleteBookUI(); 
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
            Console.WriteLine("3. Cập nhật sách");
            Console.WriteLine("4. Xoá sách");
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

        static void UpdateBookUI()
        {
            Console.WriteLine("\n--- CẬP NHẬT THÔNG TIN SÁCH ---");
            Console.Write("Nhập ID sách cần sửa: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID phải là số!");
                return;
            }

            Book? oldBook = _service.GetBookById(id);
            if (oldBook == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Không tìm thấy sách có ID này!");
                Console.ResetColor();
                return;
            }

            Console.WriteLine($"Đang sửa sách: {oldBook.Title}");
            Console.WriteLine("(Bấm Enter để giữ nguyên giá trị cũ)");

            Console.Write($"Tên mới (Cũ: {oldBook.Title}): ");
            string? newTitle = Console.ReadLine();
            string finalTitle = string.IsNullOrWhiteSpace(newTitle) ? oldBook.Title : newTitle;

            Console.Write($"Tác giả mới (Cũ: {oldBook.Author}): ");
            string? newAuthor = Console.ReadLine();
            string finalAuthor = string.IsNullOrWhiteSpace(newAuthor) ? oldBook.Author : newAuthor;

            Console.Write($"Năm XB mới (Cũ: {oldBook.PublishYear}): ");
            string? newYearStr = Console.ReadLine();
            int finalYear = oldBook.PublishYear; 
            if (!string.IsNullOrWhiteSpace(newYearStr))
            {
                if (int.TryParse(newYearStr, out int y)) finalYear = y;
                else Console.WriteLine("Năm nhập sai, sẽ giữ nguyên năm cũ.");
            }

            Book bookToUpdate = new Book(id, finalTitle, finalAuthor, finalYear);

            string result = _service.UpdateBook(bookToUpdate);
            
            if (result.Contains("Thành công")) Console.ForegroundColor = ConsoleColor.Green;
            else Console.ForegroundColor = ConsoleColor.Red;
            
            Console.WriteLine(result);
            Console.ResetColor();
        }

        static void DeleteBookUI()
        {
            Console.WriteLine("\n--- XÓA SÁCH ---");
            Console.Write("Nhập ID sách cần xóa: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            Book? book = _service.GetBookById(id);
            if (book == null)
            {
                Console.WriteLine("Không tìm thấy sách!");
                return;
            }

            // Hỏi xác nhận (Confirmation)
            Console.WriteLine($"Bạn có chắc chắn muốn xóa cuốn: '{book.Title}'? (y/n)");
            string? confirm = Console.ReadLine();

            if (confirm?.ToLower() == "y")
            {
                string result = _service.DeleteBook(id);
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("Đã hủy thao tác xóa.");
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