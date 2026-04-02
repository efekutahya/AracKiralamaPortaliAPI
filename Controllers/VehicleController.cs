using AracKiralamaAPI.DTOs;
using AracKiralamaAPI.Models;
using AracKiralamaAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AracKiralamaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository _repo;
        public VehicleController(IVehicleRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok((await _repo.GetAllWithCategoryAsync()).Select(Map));

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable() =>
            Ok((await _repo.GetAvailableVehiclesAsync()).Select(Map));

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId) =>
            Ok((await _repo.GetByCategoryAsync(categoryId)).Select(Map));

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q) =>
            Ok((await _repo.SearchAsync(q)).Select(Map));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var v = await _repo.GetByIdWithCategoryAsync(id);
            return v == null ? NotFound() : Ok(Map(v));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] VehicleCreateDto dto)
        {
            var entity = ToEntity(dto);
            var created = await _repo.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id=created.Id }, Map(created));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();

            entity.Brand=dto.Brand; entity.Model=dto.Model; entity.Year=dto.Year;
            entity.PlateNumber=dto.PlateNumber; entity.Color=dto.Color;
            entity.FuelType=dto.FuelType; entity.TransmissionType=dto.TransmissionType;
            entity.SeatCount=dto.SeatCount; entity.DailyPrice=dto.DailyPrice;
            entity.IsAvailable=dto.IsAvailable; entity.ImageUrl=dto.ImageUrl;
            entity.Description=dto.Description; entity.CategoryId=dto.CategoryId;
            await _repo.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) =>
            await _repo.DeleteAsync(id) ? NoContent() : NotFound();

        [HttpPatch("{id}/toggle")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Toggle(int id)
        {
            var v = await _repo.GetByIdAsync(id);
            if (v == null) return NotFound();
            v.IsAvailable = !v.IsAvailable;
            await _repo.UpdateAsync(v);
            return Ok(new { isAvailable = v.IsAvailable });
        }

        private static VehicleDto Map(Vehicle v) => new()
        {
            Id=v.Id, Brand=v.Brand, Model=v.Model, Year=v.Year, PlateNumber=v.PlateNumber,
            Color=v.Color, FuelType=v.FuelType, TransmissionType=v.TransmissionType,
            SeatCount=v.SeatCount, DailyPrice=v.DailyPrice, IsAvailable=v.IsAvailable,
            ImageUrl=v.ImageUrl, Description=v.Description,
            CategoryId=v.CategoryId, CategoryName=v.Category?.Name ?? ""
        };

        private static Vehicle ToEntity(VehicleCreateDto d) => new()
        {
            Brand=d.Brand, Model=d.Model, Year=d.Year, PlateNumber=d.PlateNumber,
            Color=d.Color, FuelType=d.FuelType, TransmissionType=d.TransmissionType,
            SeatCount=d.SeatCount, DailyPrice=d.DailyPrice, IsAvailable=d.IsAvailable,
            ImageUrl=d.ImageUrl, Description=d.Description, CategoryId=d.CategoryId
        };
    }
}
