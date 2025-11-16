using Library.Common;
using Library.Infrastructure.Services;
using Library.REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICrudServiceAsync<Author> _service;

        public AuthorsController(ICrudServiceAsync<Author> service)
        {
            _service = service;
        }

        // GET api/authors
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _service.ReadAllAsync();
            return Ok(authors);
        }

        // GET api/authors/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var author = await _service.ReadAsync(id);
            if (author == null) return NotFound();
            return Ok(author);
        }

        // POST api/authors
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorModel model)
        {
            var author = new Author(model.FullName, model.Age, model.Nationality, model.BooksPublished);

            await _service.CreateAsync(author);

            return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
        }

        // PUT api/authors/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorModel model)
        {
            var existing = await _service.ReadAsync(id);
            if (existing == null) return NotFound();

            existing.FullName = model.FullName;
            existing.Age = model.Age;
            existing.Nationality = model.Nationality;
            existing.BooksPublished = model.BooksPublished;

            await _service.UpdateAsync(existing);

            return Ok(existing);
        }

        // DELETE api/authors/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _service.ReadAsync(id);
            if (author == null) return NotFound();

            await _service.RemoveAsync(author);
            return NoContent();
        }
    }
}
