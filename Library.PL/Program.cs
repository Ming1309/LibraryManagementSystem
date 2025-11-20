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

            Book newBook = new Book() 
            { 
                Id = 1, 
                Title = "Dế Mèn Phiêu Lưu Ký", 
                Author = "Tô Hoài", 
                PublishYear = 1941 
            };

            string result = service.AddBook(newBook);

            Console.WriteLine($"Kết quả test: {result}");
            Console.WriteLine("Nếu bạn thấy dòng này, cấu trúc 3 lớp đã thông suốt!");
            
            Console.ReadLine();
        }
    }
}