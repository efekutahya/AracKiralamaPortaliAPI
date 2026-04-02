using AracKiralamaAPI.Data;
using AracKiralamaAPI.Models;
using AracKiralamaAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AracKiralamaAPI.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Category>> GetAllWithVehicleCountAsync() =>
            await _context.Categories.Include(c => c.Vehicles).ToListAsync();
    }
}
