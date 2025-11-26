using Library.Core;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;

namespace Library.DAL
{
    public class BookRepository
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");
        private List<Book> LoadData()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
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
                throw new Exception($"Lỗi đọc file dữ liệu: {ex.Message}");
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
                throw new Exception($"Lỗi ghi file dữ liệu: {ex.Message}");
            }
        }
        public List<Book> GetAllBooks()
        { 
            return LoadData();
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
            return true;
        }

        public bool DeleteBook(int bookId)
        {
            return true;
        }
    }
}
