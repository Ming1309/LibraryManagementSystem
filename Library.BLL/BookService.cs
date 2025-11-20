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
            _repo.AddBook(book);
            return "Thêm thành công (Mock)";
        }
    }
}