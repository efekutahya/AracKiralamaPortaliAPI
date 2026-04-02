namespace AracKiralamaAPI.Models
{
    public class Category
    {
        public int     Id          { get; set; }
        public string  Name        { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? IconClass   { get; set; }  // örn: fa-car

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
