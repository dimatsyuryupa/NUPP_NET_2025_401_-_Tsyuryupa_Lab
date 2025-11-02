using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Infrastructure.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;

        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        protected Book() { }

        // Конструктор для EF Core / коду
        public Book(string title, string genre, int authorId)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Genre = genre ?? throw new ArgumentNullException(nameof(genre));
            AuthorId = authorId;
        }

        public Book(string title, string genre, Author author)
            : this(title, genre, author?.Id ?? throw new ArgumentNullException(nameof(author)))
        {
            Author = author;
        }

        // Альтернативна фабрика для створення книги
        public static Book CreateNew(string title, string genre, Author author)
        {
            return new Book(title, genre, author);
        }
    }
}
