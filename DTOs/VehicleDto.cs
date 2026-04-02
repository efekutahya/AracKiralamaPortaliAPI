namespace AracKiralamaAPI.DTOs
{
    public class VehicleDto
    {
        public int     Id               { get; set; }
        public string  Brand            { get; set; } = string.Empty;
        public string  Model            { get; set; } = string.Empty;
        public int     Year             { get; set; }
        public string  PlateNumber      { get; set; } = string.Empty;
        public string? Color            { get; set; }
        public string  FuelType         { get; set; } = string.Empty;
        public string  TransmissionType { get; set; } = string.Empty;
        public int     SeatCount        { get; set; }
        public decimal DailyPrice       { get; set; }
        public bool    IsAvailable      { get; set; }
        public string? ImageUrl         { get; set; }
        public string? Description      { get; set; }
        public int     CategoryId       { get; set; }
        public string  CategoryName     { get; set; } = string.Empty;
    }

    public class VehicleCreateDto
    {
        public string  Brand            { get; set; } = string.Empty;
        public string  Model            { get; set; } = string.Empty;
        public int     Year             { get; set; }
        public string  PlateNumber      { get; set; } = string.Empty;
        public string? Color            { get; set; }
        public string  FuelType         { get; set; } = string.Empty;
        public string  TransmissionType { get; set; } = string.Empty;
        public int     SeatCount        { get; set; }
        public decimal DailyPrice       { get; set; }
        public bool    IsAvailable      { get; set; } = true;
        public string? ImageUrl         { get; set; }
        public string? Description      { get; set; }
        public int     CategoryId       { get; set; }
    }

    public class VehicleUpdateDto : VehicleCreateDto
    {
        public int Id { get; set; }
    }
}
