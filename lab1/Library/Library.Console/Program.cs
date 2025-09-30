using System;
using Library.Common;

namespace Library.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // підписка на подію додавання книги
            Book.OnBookAdded += (msg) => msg.PrintWithStars();

            var author = new Author("Тарас Шевченко", 47, "Українець", 20);
            var librarian = new Librarian("Iван Франко", 50, "Головний бiблiотекар", 25);

            var bookService = new CrudService<Book>();

            var book1 = new Book("Кобзар", "Поезiя", author);
            Book.RaiseBookAdded($"Додано книгу: {book1.Title}");

            var book2 = new Book("Гайдамаки", "Поезiя", author);
            Book.RaiseBookAdded($"Додано книгу: {book2.Title}");

            bookService.Create(book1);
            bookService.Create(book2);

            Console.WriteLine("\nУ бiблiотецi:");
            foreach (var b in bookService.ReadAll())
            {
                b.ShowInfo();
            }

            var card = new LibraryCard("LC-001", librarian);
            Console.WriteLine($"\nКартка: {card.Number}, Власник: {card.Owner.FullName}");
        }
    }
}
