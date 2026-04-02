using AracKiralamaAPI.Models;

namespace AracKiralamaAPI.Repositories.Interfaces
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        Task<IEnumerable<Vehicle>> GetAllWithCategoryAsync();
        Task<Vehicle?>             GetByIdWithCategoryAsync(int id);
        Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync();
        Task<IEnumerable<Vehicle>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Vehicle>> SearchAsync(string keyword);
    }
}
