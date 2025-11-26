using Library.Core;
using Library.DAL;

namespace Library.BLL
{
    public class BookService
    {
        private BookRepository _repo;

        public BookService()
        {
            _repo = new BookRepository();
        }


        public List<Book> GetAllBooks()
        {
            return _repo.GetAllBooks();
        }

        public string AddBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                return "Lỗi: Tên sách không được để trống!";
            }

            if (book.Title.Length < 2)
            {
                return "Lỗi: Tên sách phải có ít nhất 2 ký tự!";
            }

            int currentYear = DateTime.Now.Year;
            if (book.PublishYear <= 0)
            {
                return "Lỗi: Năm xuất bản phải lớn hơn 0!";
            }
            if (book.PublishYear > currentYear)
            {
                return $"Lỗi: Năm xuất bản không được lớn hơn năm hiện tại ({currentYear})!";
            }

            bool isSuccess = _repo.AddBook(book);

            if (isSuccess)
            {
                return "Thành công: Đã thêm sách vào hệ thống.";
            }
            else
            {
                return "Lỗi hệ thống: Không thể lưu vào file.";
            }
        }
    }
}