namespace Library.Core
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PublishYear { get; set; }

        public Book() { }

        public Book(int id, string title, string author, int year)
        {
            Id = id;
            Title = title;
            Author = author;
            PublishYear = year;
        }

        public override string ToString()
        {
            return $"[{Id}] {Title} - {Author} ({PublishYear})";
        }
    }
}