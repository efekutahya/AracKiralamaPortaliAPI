using Microsoft.AspNetCore.Identity;

namespace AracKiralamaAPI.Models
{
    public class AppUser : IdentityUser
    {
        public string  FirstName  { get; set; } = string.Empty;
        public string  LastName   { get; set; } = string.Empty;
        public string? Address    { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
