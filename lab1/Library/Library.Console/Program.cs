using Library.Common;
using Library.Infrastructure;
using Library.Infrastructure.Repositories;
using Library.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp
{
    class Program
    {
        static async Task Main()
        {

            // Налаштування контексту SQLite
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseSqlite(@"Data Source=D:\.NET\lab1\Library\Library.Console\library.db")
                .Options;

            using var context = new LibraryContext(options);

            var authorRepo = new Repository<Author>(context);
            var bookRepo = new Repository<Book>(context);
            var busRepo = new Repository<Bus>(context);

            var authorService = new CrudServiceAsync<Author>(authorRepo);
            var bookService = new CrudServiceAsync<Book>(bookRepo);
            var busService = new CrudServiceAsync<Bus>(busRepo);

            while (true)
            {
                Console.WriteLine("\n--- Меню ---");
                Console.WriteLine("1. Додати автора");
                Console.WriteLine("2. Додати книгу");
                Console.WriteLine("3. Додати автобус");
                Console.WriteLine("4. Показати всі книги");
                Console.WriteLine("5. Видалити книгу");
                Console.WriteLine("0. Вийти");

                Console.Write("Виберіть дію: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Write("Введіть ім'я автора: ");
                        string name = Console.ReadLine();
                        Console.Write("Вік: ");
                        int age = int.Parse(Console.ReadLine());
                        Console.Write("Національність: ");
                        string nationality = Console.ReadLine();
                        Console.Write("Кількість книг: ");
                        int bookCount = int.Parse(Console.ReadLine());

                        var author = new Author(name, age, nationality, bookCount);
                        await authorService.CreateAsync(author);
                        Console.WriteLine("Автор доданий");
                        break;

                    case "2":
                        var authors = (await authorService.ReadAllAsync()).ToList();
                        if (!authors.Any())
                        {
                            Console.WriteLine("Спочатку додайте автора!");
                            break;
                        }
                        Console.WriteLine("Оберіть автора (індекс):");
                        for (int i = 0; i < authors.Count; i++)
                            Console.WriteLine($"{i}: {authors[i].FullName}");
                        int index = int.Parse(Console.ReadLine());
                        Console.Write("Введіть назву книги: ");
                        string title = Console.ReadLine() ?? "Без назви";
                        Console.Write("Введіть жанр книги: ");
                        string genre = Console.ReadLine() ?? "Невідомий";
                        var book = Book.CreateNew(title, genre, authors[index]);
                        await bookService.CreateAsync(book);
                        Console.WriteLine("Книга додана");
                        break;

                    case "3":
                        var bus = Bus.CreateNew();
                        await busService.CreateAsync(bus);
                        Console.WriteLine("Автобус доданий");
                        break;

                    case "4":
                        var books = await context.Books.Include(b => b.Author).ToListAsync();
                        if (!books.Any()) { Console.WriteLine("Книг немає."); break; }
                        foreach (var b in books)
                            b.ShowInfo();
                        break;

                    case "5":
                        var allBooks = (await bookService.ReadAllAsync()).ToList();
                        if (!allBooks.Any())
                        {
                            Console.WriteLine("Книг немає.");
                            break;
                        }

                        Console.WriteLine("Список книг:");
                        foreach (var b in allBooks)
                            Console.WriteLine($"{b.Id} - {b.Title} ({b.Genre})");

                        Console.Write("Введіть Id книги для видалення: ");
                        string inputId = Console.ReadLine();

                        if (!int.TryParse(inputId, out int bookId))
                        {
                            Console.WriteLine("Невірний формат Id");
                            break;
                        }

                        var bookToDelete = allBooks.FirstOrDefault(b => b.Id == bookId);
                        if (bookToDelete == null)
                        {
                            Console.WriteLine("Книга з таким Id не знайдена");
                            break;
                        }

                        await bookService.RemoveAsync(bookToDelete);
                        Console.WriteLine($"Книга \"{bookToDelete.Title}\" видалена ✅");
                        break;

                    case "0":
                        return; // вихід з програми

                    default:
                        Console.WriteLine("Невірний вибір, спробуйте ще раз.");
                        break;
                }
            }
        }
    }
}