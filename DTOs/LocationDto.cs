namespace AracKiralamaAPI.DTOs
{
    public class LocationDto
    {
        public int     Id      { get; set; }
        public string  Name    { get; set; } = string.Empty;
        public string  Address { get; set; } = string.Empty;
        public string  City    { get; set; } = string.Empty;
        public string? Phone   { get; set; }
    }

    public class LocationCreateDto
    {
        public string  Name    { get; set; } = string.Empty;
        public string  Address { get; set; } = string.Empty;
        public string  City    { get; set; } = string.Empty;
        public string? Phone   { get; set; }
    }
}
