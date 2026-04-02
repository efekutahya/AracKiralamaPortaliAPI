namespace AracKiralamaAPI.DTOs
{
    public class CategoryDto
    {
        public int     Id           { get; set; }
        public string  Name         { get; set; } = string.Empty;
        public string? Description  { get; set; }
        public string? IconClass    { get; set; }
        public int     VehicleCount { get; set; }
    }

    public class CategoryCreateDto
    {
        public string  Name        { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? IconClass   { get; set; }
    }
}
