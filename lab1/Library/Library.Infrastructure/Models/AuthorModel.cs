using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Infrastructure.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public int BooksPublished { get; set; }

        public List<Book> Books { get; set; } = new();

        protected Author() { }

        public Author(string fullName, int age, string nationality, int booksPublished)
        {
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Age = age;
            Nationality = nationality ?? throw new ArgumentNullException(nameof(nationality));
            BooksPublished = booksPublished;
        }
    }
}
