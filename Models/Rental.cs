namespace AracKiralamaAPI.Models
{
    public enum RentalStatus
    {
        Pending   = 0,   // Beklemede
        Confirmed = 1,   // Onaylandı
        Active    = 2,   // Aktif (araç teslim edildi)
        Completed = 3,   // Tamamlandı
        Cancelled = 4    // İptal edildi
    }

    public class Rental
    {
        public int     Id        { get; set; }
        public string  UserId    { get; set; } = string.Empty;
        public AppUser User      { get; set; } = null!;
        public int     VehicleId { get; set; }
        public Vehicle Vehicle   { get; set; } = null!;

        public int      PickupLocationId  { get; set; }
        public Location PickupLocation    { get; set; } = null!;
        public int      DropoffLocationId { get; set; }
        public Location DropoffLocation   { get; set; } = null!;

        public DateTime     StartDate  { get; set; }
        public DateTime     EndDate    { get; set; }
        public decimal      TotalPrice { get; set; }
        public RentalStatus Status     { get; set; } = RentalStatus.Pending;
        public string?      Notes      { get; set; }
        public DateTime     CreatedAt  { get; set; } = DateTime.Now;
    }
}
