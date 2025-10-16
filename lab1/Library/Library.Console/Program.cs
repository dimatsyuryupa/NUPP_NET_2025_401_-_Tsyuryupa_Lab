using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;

namespace Library.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Book.OnBookAdded += (msg) => msg.PrintWithStars();

            var author = new Author("Тарас Шевченко", 47, "Українець", 20);
            var librarian = new Librarian("Іван Франко", 50, "Головний бібліотекар", 25);

            var bookService = new CrudServiceAsync<Book>("books.json");
            var busService = new CrudServiceAsync<Bus>("buses.json");

            int totalBooks = 1000;
            int totalBuses = 1000;
            int parallelTasks = 8;
            int chunkSizeBooks = totalBooks / parallelTasks;
            int chunkSizeBuses = totalBuses / parallelTasks;

            AutoResetEvent autoEvent = new AutoResetEvent(false);
            object consoleLock = new object();

            // Паралельне створення книг та автобусів
            Task[] tasks = new Task[parallelTasks * 2];
            for (int i = 0; i < parallelTasks; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    for (int j = 0; j < chunkSizeBooks; j++)
                    {
                        var book = Book.CreateNew(author);
                        await bookService.CreateAsync(book);

                        lock (consoleLock)
                            Book.RaiseBookAdded($"Додано книгу: {book.Title}");
                    }
                    autoEvent.Set();
                });

                tasks[i + parallelTasks] = Task.Run(async () =>
                {
                    for (int j = 0; j < chunkSizeBuses; j++)
                    {
                        var bus = Bus.CreateNew();
                        await busService.CreateAsync(bus);
                    }
                    autoEvent.Set();
                });
            }

            await Task.WhenAll(tasks);

            for (int i = 0; i < parallelTasks * 2; i++)
                autoEvent.WaitOne();

            // Вивід перших 20 книг
            var books = (await bookService.ReadAllAsync()).ToList();
            Console.WriteLine($"\nУ бібліотеці ({books.Count} книг):");
            foreach (var b in books.Take(20))
                b.ShowInfo();

            // Статистика по автобуcах
            var buses = (await busService.ReadAllAsync()).ToList();
            Console.WriteLine($"\nЗагальна кількість автобусів: {buses.Count}");
            Console.WriteLine($"Мін. місць: {buses.Min(b => b.Seats)}, Макс. місць: {buses.Max(b => b.Seats)}, Середнє: {buses.Average(b => b.Seats):F2}");
            Console.WriteLine($"Мін. швидкість: {buses.Min(b => b.Speed)}, Макс. швидкість: {buses.Max(b => b.Speed)}, Середнє: {buses.Average(b => b.Speed):F2}");

            // Приклад UpdateAsync для книги
            var firstBook = books.First();
            firstBook.Title = "Оновлена книга";
            bool updated = await bookService.UpdateAsync(firstBook);
            Console.WriteLine($"\nОновлення книги '{firstBook.Id}': {(updated ? "Успішно" : "Не вдалося")}");

            // Приклад RemoveAsync для автобуса
            var firstBus = buses.First();
            bool removed = await busService.RemoveAsync(firstBus);
            Console.WriteLine($"Видалення автобуса '{firstBus.Id}': {(removed ? "Успішно" : "Не вдалося")}");

            // Збереження даних у файл
            await bookService.SaveAsync();
            await busService.SaveAsync();
            Console.WriteLine("\nДані збережено у books.json та buses.json");

            var card = new LibraryCard("LC-001", librarian);
            Console.WriteLine($"\nКартка: {card.Number}, Власник: {card.Owner.FullName}");
        }
    }
}
