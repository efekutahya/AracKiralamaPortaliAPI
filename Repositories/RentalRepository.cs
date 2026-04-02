using AracKiralamaAPI.Data;
using AracKiralamaAPI.Models;
using AracKiralamaAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AracKiralamaAPI.Repositories
{
    public class RentalRepository : GenericRepository<Rental>, IRentalRepository
    {
        public RentalRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Rental>> GetAllWithDetailsAsync() =>
            await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Vehicle).ThenInclude(v => v.Category)
                .Include(r => r.PickupLocation)
                .Include(r => r.DropoffLocation)
                .OrderByDescending(r => r.CreatedAt).ToListAsync();

        public async Task<Rental?> GetByIdWithDetailsAsync(int id) =>
            await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Vehicle).ThenInclude(v => v.Category)
                .Include(r => r.PickupLocation)
                .Include(r => r.DropoffLocation)
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<IEnumerable<Rental>> GetByUserIdAsync(string userId) =>
            await _context.Rentals
                .Include(r => r.Vehicle).ThenInclude(v => v.Category)
                .Include(r => r.PickupLocation)
                .Include(r => r.DropoffLocation)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt).ToListAsync();

        public async Task<bool> IsVehicleAvailableAsync(int vehicleId, DateTime start, DateTime end) =>
            !await _context.Rentals.AnyAsync(r =>
                r.VehicleId == vehicleId &&
                r.Status != RentalStatus.Cancelled &&
                r.Status != RentalStatus.Completed &&
                r.StartDate < end &&
                r.EndDate   > start);

        public async Task<object> GetStatsAsync()
        {
            var rentals = await _context.Rentals.ToListAsync();
            return new
            {
                Total     = rentals.Count,
                Pending   = rentals.Count(r => r.Status == RentalStatus.Pending),
                Confirmed = rentals.Count(r => r.Status == RentalStatus.Confirmed),
                Active    = rentals.Count(r => r.Status == RentalStatus.Active),
                Completed = rentals.Count(r => r.Status == RentalStatus.Completed),
                Cancelled = rentals.Count(r => r.Status == RentalStatus.Cancelled),
                Revenue   = rentals.Where(r => r.Status == RentalStatus.Completed).Sum(r => r.TotalPrice)
            };
        }
    }
}
