using System;
using Library.BLL;
using Library.Core;

namespace Library.PL
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== KHỞI ĐỘNG ỨNG DỤNG QUẢN LÝ THƯ VIỆN ===");
            
            BookService service = new BookService();

            Book newBook = new Book(1, "Dế Mèn Phiêu Lưu Ký", "Tô Hoài", 1941);

            string result = service.AddBook(newBook);

            Console.WriteLine("Thông tin sách vừa tạo:");
            Console.WriteLine(newBook); 

            Console.WriteLine(result);
            
            Console.ReadLine();
        }
    }
}