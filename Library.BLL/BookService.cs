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

        public Book? GetBookById(int id)
        {
            return _repo.GetBookById(id);
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

        public string UpdateBook(Book book)
        {
            if (_repo.GetBookById(book.Id) == null)
            {
                return "Lỗi: Sách không tồn tại trên hệ thống.";
            }

            if (string.IsNullOrWhiteSpace(book.Title)) return "Lỗi: Tên sách không được để trống!";
            if (book.Title.Length < 2) return "Lỗi: Tên sách quá ngắn!";
            if (book.PublishYear > DateTime.Now.Year) return "Lỗi: Năm xuất bản không hợp lệ!";

            bool result = _repo.UpdateBook(book);
            return result ? "Thành công: Đã cập nhật thông tin sách." : "Lỗi: Không thể lưu xuống file.";
        }

        public string DeleteBook(int id)
        {
            if (_repo.GetBookById(id) == null)
            {
                return "Lỗi: Sách không tìm thấy để xóa.";
            }
            
            bool result = _repo.DeleteBook(id);
            return result ? "Thành công: Đã xóa sách khỏi hệ thống." : "Lỗi: Không thể xóa file.";
        }

        public List<Book> SearchBooks(string keyword)
        {
            List<Book> allBooks = GetAllBooks();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<Book>();
            }

            keyword = keyword.ToLower();

            List<Book> results = new List<Book>();
            foreach (var b in allBooks)
            {
                if (b.Title.ToLower().Contains(keyword) || 
                    b.Author.ToLower().Contains(keyword))
                {
                    results.Add(b);
                }
            }

            return results;
        }
    }
}