using System;
using System.Collections.Generic;
using System.Threading;

namespace Library.Common
{

    // ===== Люди =====
    public abstract class Person : IEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }

        public List<LibraryCard> LibraryCards { get; set; } = new();

        protected Person(string fullName, int age)
        {
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Age = age;
        }

        protected Person() { }

        public abstract void ShowInfo();
    }

    public class Author : Person
    {
        public string Nationality { get; set; } = string.Empty;
        public int BooksPublished { get; set; }

        public List<Book> Books { get; set; } = new();

        public Author(string fullName, int age, string nationality, int booksPublished)
            : base(fullName, age)
        {
            Nationality = nationality ?? throw new ArgumentNullException(nameof(nationality));
            BooksPublished = booksPublished;
        }

        public Author() : base() { }

        public override void ShowInfo() =>
            Console.WriteLine($"Автор: {FullName}, Вік: {Age}, Книг: {BooksPublished}");
    }

    public class Librarian : Person
    {
        public string Position { get; set; } = string.Empty;
        public int Experience { get; set; }

        public Librarian(string fullName, int age, string position, int experience)
            : base(fullName, age)
        {
            Position = position ?? throw new ArgumentNullException(nameof(position));
            Experience = experience;
        }

        protected Librarian() : base() { }

        public override void ShowInfo() =>
            Console.WriteLine($"Бібліотекар: {FullName}, Посада: {Position}, Досвід: {Experience} років");
    }

    // ===== Книги =====
    public class Book : IEntity
    {
        public int Id { get; set; }                // int, автогенерується EF
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;

        public int AuthorId { get; set; }          // int, як Id автора
        public Author? Author { get; set; }        // навігаційна властивість

        protected Book() { }

        public Book(string title, string genre, Author author)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Genre = genre ?? throw new ArgumentNullException(nameof(genre));
            Author = author ?? throw new ArgumentNullException(nameof(author));
            AuthorId = author.Id;                  // встановлюємо FK відразу
        }

        public void ShowInfo() =>
            Console.WriteLine($"Книга: {Title}, Жанр: {Genre}, Автор: {Author?.FullName ?? "—"}");

        public static Book CreateNew(string title, string genre, Author author) =>
            new Book(title, genre, author);
    }

    // ===== Автобуси =====
    public class Bus : IEntity
    {
        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public int Seats { get; set; }
        public int Speed { get; set; }

        private static readonly Random rnd = new();

        protected Bus() { }

        public Bus(string model, int seats, int speed)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Seats = seats;
            Speed = speed;
        }

        public static Bus CreateNew() => new Bus(
            $"Bus-{rnd.Next(1000, 9999)}",
            rnd.Next(20, 60),
            rnd.Next(60, 120)
        );

        public void ShowInfo() =>
            Console.WriteLine($"Автобус: {Model}, Місць: {Seats}, Швидкість: {Speed} км/год");
    }

    // ===== Картка бібліотеки =====
    public class LibraryCard : IEntity
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;

        public int OwnerId { get; set; }
        public Person? Owner { get; set; }

        public DateTime IssuedDate { get; set; } = DateTime.Now;

        public LibraryCard(string number, Person owner)
        {
            Number = number ?? throw new ArgumentNullException(nameof(number));
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            OwnerId = owner.Id;
        }

        protected LibraryCard() { }
    }

    // ===== Розширення =====
    public static class LibraryExtensions
    {
        public static void PrintWithStars(this string text) =>
            Console.WriteLine($"*** {text} ***");
    }
}
