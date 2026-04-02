using AracKiralamaAPI.DTOs;
using AracKiralamaAPI.Models;
using AracKiralamaAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AracKiralamaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _repo;
        public LocationController(ILocationRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _repo.GetAllAsync();
            return Ok(list.Select(l => new LocationDto { Id=l.Id, Name=l.Name, Address=l.Address, City=l.City, Phone=l.Phone }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var l = await _repo.GetByIdAsync(id);
            return l == null ? NotFound() :
                Ok(new LocationDto { Id=l.Id, Name=l.Name, Address=l.Address, City=l.City, Phone=l.Phone });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] LocationCreateDto dto)
        {
            var e = new Location { Name=dto.Name, Address=dto.Address, City=dto.City, Phone=dto.Phone };
            var c = await _repo.CreateAsync(e);
            return CreatedAtAction(nameof(GetById), new { id=c.Id }, c);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] LocationCreateDto dto)
        {
            var l = await _repo.GetByIdAsync(id);
            if (l == null) return NotFound();
            l.Name=dto.Name; l.Address=dto.Address; l.City=dto.City; l.Phone=dto.Phone;
            await _repo.UpdateAsync(l);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) =>
            await _repo.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
