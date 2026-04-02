using AracKiralamaAPI.Models;

namespace AracKiralamaAPI.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllWithVehicleCountAsync();
    }
}
