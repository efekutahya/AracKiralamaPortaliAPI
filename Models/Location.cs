namespace AracKiralamaAPI.Models
{
    public class Location
    {
        public int    Id      { get; set; }
        public string Name    { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City    { get; set; } = string.Empty;
        public string? Phone  { get; set; }

        public ICollection<Rental> PickupRentals  { get; set; } = new List<Rental>();
        public ICollection<Rental> DropoffRentals { get; set; } = new List<Rental>();
    }
}
