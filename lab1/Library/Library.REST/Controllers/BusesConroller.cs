using Library.Common;
using Library.Infrastructure.Services;
using Library.REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusesController : ControllerBase
    {
        private readonly ICrudServiceAsync<Bus> _service;

        public BusesController(ICrudServiceAsync<Bus> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var buses = await _service.ReadAllAsync();
            return Ok(buses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var bus = await _service.ReadAsync(id);
            if (bus == null) return NotFound();
            return Ok(bus);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusModel model)
        {
            var bus = new Bus(model.Model, model.Seats, model.Speed);
            await _service.CreateAsync(bus);
            return CreatedAtAction(nameof(GetById), new { id = bus.Id }, bus);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusModel model)
        {
            var existing = await _service.ReadAsync(id);
            if (existing == null) return NotFound();

            existing.Model = model.Model;
            existing.Seats = model.Seats;
            existing.Speed = model.Speed;

            await _service.UpdateAsync(existing);

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bus = await _service.ReadAsync(id);
            if (bus == null) return NotFound();

            await _service.RemoveAsync(bus);
            return NoContent();
        }
    }
}
