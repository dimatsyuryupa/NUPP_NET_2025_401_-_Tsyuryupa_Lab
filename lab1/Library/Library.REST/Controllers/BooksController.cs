using Library.Common;
using Library.Infrastructure.Services;
using Library.REST.Models;
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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _service.ReadAllAsync();
            return Ok(books);
        }

        // GET api/books/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _service.ReadAsync(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        // POST api/books
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookModel model)
        {
            var author = new Author { Id = model.AuthorId };

            var book = new Book(model.Title, model.Genre, author);

            await _service.CreateAsync(book);

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        // PUT api/books/{id}
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _service.ReadAsync(id);
            if (book == null) return NotFound();

            await _service.RemoveAsync(book);
            return NoContent();
        }
    }
}
