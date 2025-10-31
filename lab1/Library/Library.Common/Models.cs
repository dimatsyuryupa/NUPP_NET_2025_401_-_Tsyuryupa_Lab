using System;
using System.Threading;

namespace Library.Common
{
    // ===== Люди =====
    public abstract class Person : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; }
        public int Age { get; set; }

        protected Person(string fullName, int age)
        {
            FullName = fullName;
            Age = age;
        }

        public abstract void ShowInfo();
    }

    public class Author : Person
    {
        public string Nationality { get; set; }
        public int BooksPublished { get; set; }

        public Author(string fullName, int age, string nationality, int booksPublished)
            : base(fullName, age)
        {
            Nationality = nationality;
            BooksPublished = booksPublished;
        }

        public override void ShowInfo() =>
            Console.WriteLine($"Автор: {FullName}, Вік: {Age}, Книг: {BooksPublished}");
    }

    public class Librarian : Person
    {
        public string Position { get; set; }
        public int Experience { get; set; }

        public Librarian(string fullName, int age, string position, int experience)
            : base(fullName, age)
        {
            Position = position;
            Experience = experience;
        }

        public override void ShowInfo() =>
            Console.WriteLine($"Бібліотекар: {FullName}, Посада: {Position}, Досвід: {Experience} років");
    }

    // ===== Книги =====
    public class Book : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Genre { get; set; }
        public Author Author { get; set; }

        public static int TotalBooks;
        private static readonly Random rnd = new Random();

        public static event Action<string> OnBookAdded;

        public Book(string title, string genre, Author author)
        {
            Title = title;
            Genre = genre;
            Author = author;
            Interlocked.Increment(ref TotalBooks);
        }

        public void ShowInfo() =>
            Console.WriteLine($"Книга: {Title}, Жанр: {Genre}, Автор: {Author.FullName}");

        public static Book CreateNew(Author author)
        {
            var book = new Book(
                $"Книга-{rnd.Next(1, 10000)}",
                $"Жанр-{rnd.Next(1, 10)}",
                author
            );
            OnBookAdded?.Invoke($"Додано книгу: {book.Title}");
            return book;
        }
    }

    // ===== Автобуси =====
    public class Bus : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Model { get; set; }
        public int Seats { get; set; }
        public int Speed { get; set; }

        private static readonly Random rnd = new Random();

        public void ShowInfo() =>
            Console.WriteLine($"Автобус: {Model}, Місць: {Seats}, Швидкість: {Speed} км/год");

        public static Bus CreateNew() => new Bus
        {
            Model = $"Bus-{rnd.Next(1000, 9999)}",
            Seats = rnd.Next(20, 60),
            Speed = rnd.Next(60, 120)
        };
    }

    // ===== Картка бібліотеки =====
    public class LibraryCard : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Number { get; set; }
        public Person Owner { get; set; }
        public DateTime IssuedDate { get; set; } = DateTime.Now;

        public LibraryCard(string number, Person owner)
        {
            Number = number;
            Owner = owner;
        }
    }

    // ===== Розширення =====
    public static class LibraryExtensions
    {
        public static void PrintWithStars(this string text) =>
            Console.WriteLine($"*** {text} ***");
    }
}
