using AracKiralamaAPI.Models;

namespace AracKiralamaAPI.Repositories.Interfaces
{
    public interface IRentalRepository : IGenericRepository<Rental>
    {
        Task<IEnumerable<Rental>> GetAllWithDetailsAsync();
        Task<Rental?>             GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Rental>> GetByUserIdAsync(string userId);
        Task<bool> IsVehicleAvailableAsync(int vehicleId, DateTime start, DateTime end);
        Task<object> GetStatsAsync();
    }
}
