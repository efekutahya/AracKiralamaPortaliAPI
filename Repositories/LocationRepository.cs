using AracKiralamaAPI.Data;
using AracKiralamaAPI.Models;
using AracKiralamaAPI.Repositories.Interfaces;

namespace AracKiralamaAPI.Repositories
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(AppDbContext context) : base(context) { }
    }
}
