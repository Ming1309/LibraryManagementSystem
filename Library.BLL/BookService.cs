using Library.Core;
using Library.Core.Common;
using Library.Core.Constants;
using Library.Core.Interfaces;

namespace Library.BLL
{
    public class BookService
    {
        private readonly IRepository<Book> _repo;

        /// <summary>
        /// Constructor with Dependency Injection
        /// </summary>
        /// <param name="repository">Repository for data access</param>
        public BookService(IRepository<Book> repository)
        {
            _repo = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        public List<Book> GetAllBooks()
        {
            return _repo.GetAll();
        }

        public Book? GetBookById(int id)
        {
            return _repo.GetById(id);
        }

        public Result AddBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                return Result.Failure(Messages.ErrorTitleEmpty, ErrorCodes.TitleEmpty);
            }

            if (book.Title.Length < AppSettings.MinTitleLength)
            {
                return Result.Failure(Messages.ErrorTitleTooShort, ErrorCodes.TitleTooShort);
            }

            int currentYear = DateTime.Now.Year;
            if (book.PublishYear <= AppSettings.MinYearValue)
            {
                return Result.Failure(Messages.ErrorYearInvalid, ErrorCodes.YearInvalid);
            }
            if (book.PublishYear > currentYear)
            {
                return Result.Failure(
                    string.Format(Messages.ErrorYearFuture, currentYear), 
                    ErrorCodes.YearFuture
                );
            }

            bool isSuccess = _repo.Add(book);

            if (isSuccess)
            {
                return Result.Success(Messages.SuccessAddBook);
            }
            else
            {
                return Result.Failure(Messages.ErrorSaveFile, ErrorCodes.SaveError);
            }
        }

        public Result UpdateBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                return Result.Failure(Messages.ErrorTitleEmpty, ErrorCodes.TitleEmpty);
            
            if (book.Title.Length < AppSettings.MinTitleLength)
                return Result.Failure(Messages.ErrorTitleTooShortUpdate, ErrorCodes.TitleTooShort);
            
            if (book.PublishYear > DateTime.Now.Year)
                return Result.Failure(Messages.ErrorYearInvalidUpdate, ErrorCodes.YearInvalid);

            bool result = _repo.Update(book);
            
            return result 
                ? Result.Success(Messages.SuccessUpdateBook) 
                : Result.Failure(Messages.ErrorBookNotExist, ErrorCodes.BookNotExist);
        }

        public Result DeleteBook(int id)
        {
            bool result = _repo.Delete(id);
            
            return result 
                ? Result.Success(Messages.SuccessDeleteBook) 
                : Result.Failure(Messages.ErrorDeleteFile, ErrorCodes.DeleteError);
        }

        public List<Book> SearchBooks(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<Book>();
            }

            keyword = keyword.Trim();
            string searchTerm = keyword.ToLower();

            // Search by Author only: 
            if (searchTerm.StartsWith("author:"))
            {
                string authorKeyword = searchTerm.Substring(7).Trim();
                if (string.IsNullOrWhiteSpace(authorKeyword))
                    return new List<Book>();
                    
                return _repo.Find(book => 
                    book.Author.ToLower().Contains(authorKeyword)
                );
            }

            // Search by Title only:
            if (searchTerm.StartsWith("title:"))
            {
                string titleKeyword = searchTerm.Substring(6).Trim();
                if (string.IsNullOrWhiteSpace(titleKeyword))
                    return new List<Book>();
                    
                return _repo.Find(book => 
                    book.Title.ToLower().Contains(titleKeyword)
                );
            }

            // Default: Search both Title and Author
            return _repo.Find(book => 
                book.Title.ToLower().Contains(searchTerm) || 
                book.Author.ToLower().Contains(searchTerm)
            );
        }
    }
}