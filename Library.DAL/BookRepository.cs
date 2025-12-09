using Library.Core;
using Library.Core.Interfaces;
using Library.Core.Constants;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Library.DAL
{
    public class BookRepository : IRepository<Book>
    {
        private readonly string _filePath;

        public BookRepository(IPathProvider pathProvider)
        {
            if (pathProvider == null)
                throw new ArgumentNullException(nameof(pathProvider));

            _filePath = pathProvider.GetDataPath(AppSettings.DataFileName);
        }

        private List<Book> LoadData()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    File.WriteAllText(_filePath, "[]", Encoding.UTF8);
                    return new List<Book>();
                }

                string jsonContent = File.ReadAllText(_filePath, Encoding.UTF8);

                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    return new List<Book>();
                }

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<Book>>(jsonContent, options) ?? new List<Book>();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Messages.ErrorReadFile, ex.Message));
            }
        }

        private void SaveData(List<Book> books)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                
                string jsonContent = JsonSerializer.Serialize(books, options);

                File.WriteAllText(_filePath, jsonContent, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Messages.ErrorWriteFile, ex.Message));
            }
        }

        // Implement IRepository<Book> interface
        public List<Book> GetAll()
        {
            return GetAllBooks();
        }

        public Book? GetById(int id)
        {
            return GetBookById(id);
        }

        public bool Add(Book entity)
        {
            return AddBook(entity);
        }

        public bool Update(Book entity)
        {
            return UpdateBook(entity);
        }

        public bool Delete(int id)
        {
            return DeleteBook(id);
        }

        public List<Book> Find(Func<Book, bool> predicate)
        {
            try
            {
                List<Book> books = LoadData();
                return books.Where(predicate).ToList();
            }
            catch
            {
                return new List<Book>();
            }
        }

        // Original methods (kept for backward compatibility)
        public List<Book> GetAllBooks()
        { 
            return LoadData();
        }

        public Book? GetBookById(int id)
        {
            List<Book> books = LoadData();
            return books.FirstOrDefault(b => b.Id == id);
        }

        public bool AddBook(Book book)
        {
            try
            {
                List<Book> currentBooks = LoadData();

                int newId = (currentBooks.Count == 0) ? 1 : currentBooks.Max(b => b.Id) + 1;
                book.Id = newId;

                currentBooks.Add(book);

                SaveData(currentBooks);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateBook(Book book)
        {
            try
            {
                List<Book> books = LoadData();
                int index = books.FindIndex(b => b.Id == book.Id);

                if (index == -1) return false;

                books[index] = book;
                SaveData(books);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteBook(int bookId)
        {
            try
            {
                List<Book> books = LoadData();
                Book? bookToDelete = books.FirstOrDefault(b => b.Id == bookId);

                if (bookToDelete == null) return false;

                books.Remove(bookToDelete);
                SaveData(books);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
