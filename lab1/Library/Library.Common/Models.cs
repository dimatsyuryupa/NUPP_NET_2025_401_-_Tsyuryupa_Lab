using System;
using System.Collections.Generic;

namespace Library.Common
{
    // Базовий абстрактний клас Person
    public abstract class Person
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }

        public Person(string fullName, int age)
        {
            Id = Guid.NewGuid();
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

        public override void ShowInfo()
        {
            Console.WriteLine($"Автор: {FullName}, Вік: {Age}, Книг: {BooksPublished}");
        }
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

        public override void ShowInfo()
        {
            Console.WriteLine($"Бібліотекар: {FullName}, Посада: {Position}, Досвід: {Experience} років");
        }
    }

    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public Author Author { get; set; }

        public static int TotalBooks;

        static Book() { TotalBooks = 0; }

        public Book(string title, string genre, Author author)
        {
            Id = Guid.NewGuid();
            Title = title;
            Genre = genre;
            Author = author;
            TotalBooks++;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Книга: {Title}, Жанр: {Genre}, Автор: {Author.FullName}");
        }

        public static event Action<string> OnBookAdded;
        public static void RaiseBookAdded(string message) => OnBookAdded?.Invoke(message);

        public static Book CreateNew(Author author)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            return new Book(
                $"Книга-{rnd.Next(1, 10000)}",
                $"Жанр-{rnd.Next(1, 10)}",
                author
            );
        }
    }

    // Новий клас Bus
    public class Bus
    {
        public Guid Id { get; set; }
        public string Model { get; set; }
        public int Seats { get; set; }
        public int Speed { get; set; }

        public Bus()
        {
            Id = Guid.NewGuid();
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Автобус: {Model}, Місць: {Seats}, Швидкість: {Speed} км/год");
        }

        // Створення нового випадкового автобуса
        public static Bus CreateNew()
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            return new Bus
            {
                Model = $"Bus-{rnd.Next(1000, 9999)}",
                Seats = rnd.Next(20, 60),
                Speed = rnd.Next(60, 120)
            };
        }
    }

    public class LibraryCard
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public Person Owner { get; set; }
        public DateTime IssuedDate { get; set; }

        public LibraryCard(string number, Person owner)
        {
            Id = Guid.NewGuid();
            Number = number;
            Owner = owner;
            IssuedDate = DateTime.Now;
        }
    }

    public static class LibraryExtensions
    {
        public static void PrintWithStars(this string text) => Console.WriteLine($"*** {text} ***");
    }
}
