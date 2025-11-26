using Library.Common;
using Library.Infrastructure.Services;
using Library.REST.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ICrudServiceAsync<Book> _service;

        public BooksController(ICrudServiceAsync<Book> service)
        {
            _service = service;
        }

        // GET api/books
        // Доступно всім, авторизація не потрібна
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _service.ReadAllAsync();
            return Ok(books);
        }

        // GET api/books/{id}
        // Доступно всім, авторизація не потрібна
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _service.ReadAsync(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        // POST api/books
        // Створення книги – лише Librarian або Admin
        [Authorize(Roles = "Librarian,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookModel model)
        {
            var author = new Author { Id = model.AuthorId };

            var book = new Book(model.Title, model.Genre, author);

            await _service.CreateAsync(book);

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        // PUT api/books/{id}
        // Оновлення книги – лише Librarian або Admin
        [Authorize(Roles = "Librarian,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookModel model)
        {
            var existing = await _service.ReadAsync(id);
            if (existing == null) return NotFound();

            existing.Title = model.Title;
            existing.Genre = model.Genre;
            existing.AuthorId = model.AuthorId;

            await _service.UpdateAsync(existing);

            return Ok(existing);
        }

        // DELETE api/books/{id}
        // Видалення книги – лише Librarian або Admin
        [Authorize(Roles = "Librarian,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _service.ReadAsync(id);
            if (book == null) return NotFound();

            await _service.RemoveAsync(book);
            return NoContent();
        }

        // POST api/books/reserve
        // Бронювання книги – лише авторизований User
        [Authorize(Roles = "User,Librarian,Admin")]
        [HttpPost("reserve/{id}")]
        public async Task<IActionResult> ReserveBook(int id)
        {
            var book = await _service.ReadAsync(id);
            if (book == null) return NotFound();

            return Ok(new { message = $"Book {book.Title} reserved successfully" });
        }
    }
}
