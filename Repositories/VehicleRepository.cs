using AracKiralamaAPI.Data;
using AracKiralamaAPI.Models;
using AracKiralamaAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AracKiralamaAPI.Repositories
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Vehicle>> GetAllWithCategoryAsync() =>
            await _context.Vehicles.Include(v => v.Category).OrderBy(v => v.Brand).ToListAsync();

        public async Task<Vehicle?> GetByIdWithCategoryAsync(int id) =>
            await _context.Vehicles.Include(v => v.Category).FirstOrDefaultAsync(v => v.Id == id);

        public async Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync() =>
            await _context.Vehicles.Include(v => v.Category)
                .Where(v => v.IsAvailable).OrderBy(v => v.DailyPrice).ToListAsync();

        public async Task<IEnumerable<Vehicle>> GetByCategoryAsync(int categoryId) =>
            await _context.Vehicles.Include(v => v.Category)
                .Where(v => v.CategoryId == categoryId).ToListAsync();

        public async Task<IEnumerable<Vehicle>> SearchAsync(string keyword) =>
            await _context.Vehicles.Include(v => v.Category)
                .Where(v => v.Brand.Contains(keyword) ||
                             v.Model.Contains(keyword) ||
                             v.Category.Name.Contains(keyword))
                .ToListAsync();
    }
}
