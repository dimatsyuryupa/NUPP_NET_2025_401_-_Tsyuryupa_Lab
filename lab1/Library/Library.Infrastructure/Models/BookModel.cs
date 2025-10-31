using System;

namespace Library.Infrastructure.Models
{
    public class Book
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;

        // Відображення на БД
        public Guid AuthorId { get; set; }
        public Author? Author { get; set; }

        // EF constructor
        public Book() { }

        // Для створення нового об’єкта в коді (не використовується EF для мапінгу)
        public static Book CreateNew(string title, string genre, Author author)
        {
            return new Book
            {
                Title = title,
                Genre = genre,
                Author = author,
                AuthorId = author.Id
            };
        }
    }
}
