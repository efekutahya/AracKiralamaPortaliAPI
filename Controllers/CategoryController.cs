using AracKiralamaAPI.DTOs;
using AracKiralamaAPI.Models;
using AracKiralamaAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AracKiralamaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repo;
        public CategoryController(ICategoryRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _repo.GetAllWithVehicleCountAsync();
            return Ok(list.Select(c => new CategoryDto
            { Id=c.Id, Name=c.Name, Description=c.Description, IconClass=c.IconClass, VehicleCount=c.Vehicles.Count }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            return c == null ? NotFound() :
                Ok(new CategoryDto { Id=c.Id, Name=c.Name, Description=c.Description, IconClass=c.IconClass });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            var e = new Category { Name=dto.Name, Description=dto.Description, IconClass=dto.IconClass };
            var created = await _repo.CreateAsync(e);
            return CreatedAtAction(nameof(GetById), new { id=created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryCreateDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return NotFound();
            e.Name=dto.Name; e.Description=dto.Description; e.IconClass=dto.IconClass;
            await _repo.UpdateAsync(e);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) =>
            await _repo.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
