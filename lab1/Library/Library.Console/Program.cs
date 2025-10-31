using Library.Common;                       // для моделей та ICrudServiceAsync<T>
using Library.Infrastructure;               // для LibraryContext
using Library.Infrastructure.Repositories;  // для Repository<T>
using Library.Infrastructure.Services;      // для CrudServiceAsync<T>
using Microsoft.EntityFrameworkCore;        // для DbContextOptions

namespace LibraryApp
{
    class Program
    {
        static async Task Main()
        {
            // Налаштування DbContext для SQLite
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseSqlite("Data Source=library.db")
                .Options;

            using var context = new LibraryContext(options);

            // Ініціалізація репозиторіїв
            var authorRepo = new Repository<Author>(context);
            var bookRepo = new Repository<Book>(context);
            var busRepo = new Repository<Bus>(context);
            var librarianRepo = new Repository<Librarian>(context);

            // CRUD-сервіси через репозиторії
            var authorService = new CrudServiceAsync<Author>(authorRepo);
            var bookService = new CrudServiceAsync<Book>(bookRepo);
            var busService = new CrudServiceAsync<Bus>(busRepo);

            // Створення авторів і книг
            var author = new Author("Тарас Шевченко", 47, "Українець", 20);
            await authorService.CreateAsync(author);

            var book = Book.CreateNew(author);
            await bookService.CreateAsync(book);

            var bus = Bus.CreateNew();
            await busService.CreateAsync(bus);

            Console.WriteLine("✅ Дані додано до бази даних SQLite");

            // Вивід книг
            var books = (await bookService.ReadAllAsync()).ToList();
            foreach (var b in books)
                b.ShowInfo();

            // Приклад видалення
            if (books.Any())
            {
                await bookService.RemoveAsync(books.First());
                Console.WriteLine("✅ Перша книга видалена");
            }
        }
    }
}
