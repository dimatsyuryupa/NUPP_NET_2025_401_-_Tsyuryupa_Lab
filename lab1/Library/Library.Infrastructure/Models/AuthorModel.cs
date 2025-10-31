using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Models
{
    public class Author
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public int BooksPublished { get; set; }

        // Один-до-багатьох: один автор має багато книг
        public List<Book> Books { get; set; } = new();

        // EF Core порожній конструктор
        public Author() { }

        // Зручний конструктор для створення в коді
        public Author(string fullName, int age, string nationality, int booksPublished)
        {
            FullName = fullName;
            Age = age;
            Nationality = nationality;
            BooksPublished = booksPublished;
        }

        public void ShowInfo() =>
            Console.WriteLine($"Автор: {FullName}, Вік: {Age}, Книг: {BooksPublished}");
    }
}
