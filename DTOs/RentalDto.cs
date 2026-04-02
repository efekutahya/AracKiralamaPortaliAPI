namespace AracKiralamaAPI.DTOs
{
    public class RentalDto
    {
        public int      Id                  { get; set; }
        public string   UserId              { get; set; } = string.Empty;
        public string   UserFullName        { get; set; } = string.Empty;
        public string   UserEmail           { get; set; } = string.Empty;
        public int      VehicleId           { get; set; }
        public string   VehicleName         { get; set; } = string.Empty;
        public string?  VehicleImageUrl     { get; set; }
        public decimal  VehicleDailyPrice   { get; set; }
        public int      PickupLocationId    { get; set; }
        public string   PickupLocationName  { get; set; } = string.Empty;
        public int      DropoffLocationId   { get; set; }
        public string   DropoffLocationName { get; set; } = string.Empty;
        public DateTime StartDate    { get; set; }
        public DateTime EndDate      { get; set; }
        public int      TotalDays    { get; set; }
        public decimal  TotalPrice   { get; set; }
        public string   Status       { get; set; } = string.Empty;
        public string?  Notes        { get; set; }
        public DateTime CreatedAt    { get; set; }
    }

    public class RentalCreateDto
    {
        public int      VehicleId         { get; set; }
        public int      PickupLocationId  { get; set; }
        public int      DropoffLocationId { get; set; }
        public DateTime StartDate         { get; set; }
        public DateTime EndDate           { get; set; }
        public string?  Notes             { get; set; }
    }

    public class RentalStatusUpdateDto
    {
        public int Id     { get; set; }
        public int Status { get; set; }
    }
}
