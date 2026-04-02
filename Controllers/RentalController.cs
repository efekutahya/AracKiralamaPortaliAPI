using AracKiralamaAPI.DTOs;
using AracKiralamaAPI.Models;
using AracKiralamaAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AracKiralamaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RentalController : ControllerBase
    {
        private readonly IRentalRepository  _rentalRepo;
        private readonly IVehicleRepository _vehicleRepo;

        public RentalController(IRentalRepository rr, IVehicleRepository vr)
        { _rentalRepo = rr; _vehicleRepo = vr; }

        // GET api/rental  (Admin)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll() =>
            Ok((await _rentalRepo.GetAllWithDetailsAsync()).Select(Map));

        // GET api/rental/my
        [HttpGet("my")]
        public async Task<IActionResult> GetMyRentals()
        {
            var uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            return Ok((await _rentalRepo.GetByUserIdAsync(uid)).Select(Map));
        }

        // GET api/rental/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var r = await _rentalRepo.GetByIdWithDetailsAsync(id);
            if (r == null) return NotFound();
            var uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!User.IsInRole("Admin") && r.UserId != uid) return Forbid();
            return Ok(Map(r));
        }

        // POST api/rental
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RentalCreateDto dto)
        {
            if (dto.StartDate >= dto.EndDate)
                return BadRequest(new { message = "Bitiş tarihi başlangıçtan sonra olmalıdır." });
            if (dto.StartDate.Date < DateTime.Today)
                return BadRequest(new { message = "Başlangıç tarihi bugünden önce olamaz." });

            var vehicle = await _vehicleRepo.GetByIdAsync(dto.VehicleId);
            if (vehicle == null)      return NotFound(new { message = "Araç bulunamadı." });
            if (!vehicle.IsAvailable) return BadRequest(new { message = "Araç müsait değil." });

            if (!await _rentalRepo.IsVehicleAvailableAsync(dto.VehicleId, dto.StartDate, dto.EndDate))
                return BadRequest(new { message = "Araç seçilen tarihlerde müsait değil." });

            var uid      = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            int days     = (int)(dto.EndDate - dto.StartDate).TotalDays;
            var rental   = new Rental
            {
                UserId = uid, VehicleId = dto.VehicleId,
                PickupLocationId  = dto.PickupLocationId,
                DropoffLocationId = dto.DropoffLocationId,
                StartDate  = dto.StartDate, EndDate = dto.EndDate,
                TotalPrice = days * vehicle.DailyPrice,
                Notes      = dto.Notes, Status = RentalStatus.Pending
            };

            var created = await _rentalRepo.CreateAsync(rental);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                new { id = created.Id, totalPrice = created.TotalPrice, totalDays = days });
        }

        // PATCH api/rental/status  (Admin)
        [HttpPatch("status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus([FromBody] RentalStatusUpdateDto dto)
        {
            var r = await _rentalRepo.GetByIdAsync(dto.Id);
            if (r == null) return NotFound();
            r.Status = (RentalStatus)dto.Status;
            await _rentalRepo.UpdateAsync(r);
            return NoContent();
        }

        // DELETE api/rental/{id}  (iptal)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var r = await _rentalRepo.GetByIdAsync(id);
            if (r == null) return NotFound();
            var uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!User.IsInRole("Admin") && r.UserId != uid) return Forbid();
            if (r.Status == RentalStatus.Active || r.Status == RentalStatus.Completed)
                return BadRequest(new { message = "Aktif veya tamamlanmış kiralama iptal edilemez." });
            r.Status = RentalStatus.Cancelled;
            await _rentalRepo.UpdateAsync(r);
            return NoContent();
        }

        // GET api/rental/stats  (Admin)
        [HttpGet("stats")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStats() => Ok(await _rentalRepo.GetStatsAsync());

        // ── Mapper ──────────────────────────────────────────────
        private static RentalDto Map(Rental r) => new()
        {
            Id = r.Id, UserId = r.UserId,
            UserFullName        = r.User != null ? $"{r.User.FirstName} {r.User.LastName}" : "",
            UserEmail           = r.User?.Email ?? "",
            VehicleId           = r.VehicleId,
            VehicleName         = r.Vehicle != null ? $"{r.Vehicle.Brand} {r.Vehicle.Model}" : "",
            VehicleImageUrl     = r.Vehicle?.ImageUrl,
            VehicleDailyPrice   = r.Vehicle?.DailyPrice ?? 0,
            PickupLocationId    = r.PickupLocationId,
            PickupLocationName  = r.PickupLocation?.Name ?? "",
            DropoffLocationId   = r.DropoffLocationId,
            DropoffLocationName = r.DropoffLocation?.Name ?? "",
            StartDate           = r.StartDate, EndDate = r.EndDate,
            TotalDays           = (int)(r.EndDate - r.StartDate).TotalDays,
            TotalPrice          = r.TotalPrice, Status = r.Status.ToString(),
            Notes = r.Notes, CreatedAt = r.CreatedAt
        };
    }
}
