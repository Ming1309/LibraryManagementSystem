using Library.Core;
using System.Collections.Generic;

namespace Library.DAL
{
    public class BookRepository
    {
        public List<Book> GetAllBooks()
        {
            return new List<Book>();
        }

        public bool AddBook(Book book)
        {
            return true;
        }

        public bool UpdateBook(Book book)
        {
            return true;
        }

        public bool DeleteBook(int bookId)
        {
            return true;
        }
    }
}
