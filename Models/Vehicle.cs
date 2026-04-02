namespace AracKiralamaAPI.Models
{
    public class Vehicle
    {
        public int     Id               { get; set; }
        public string  Brand            { get; set; } = string.Empty;
        public string  Model            { get; set; } = string.Empty;
        public int     Year             { get; set; }
        public string  PlateNumber      { get; set; } = string.Empty;
        public string? Color            { get; set; }
        public string  FuelType         { get; set; } = string.Empty; // Benzin, Dizel, Elektrik, Hibrit
        public string  TransmissionType { get; set; } = string.Empty; // Manuel, Otomatik
        public int     SeatCount        { get; set; }
        public decimal DailyPrice       { get; set; }
        public bool    IsAvailable      { get; set; } = true;
        public string? ImageUrl         { get; set; }
        public string? Description      { get; set; }

        public int      CategoryId { get; set; }
        public Category Category   { get; set; } = null!;

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
