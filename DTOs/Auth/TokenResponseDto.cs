namespace AracKiralamaAPI.DTOs.Auth
{
    public class TokenResponseDto
    {
        public string        Token      { get; set; } = string.Empty;
        public string        Email      { get; set; } = string.Empty;
        public string        FullName   { get; set; } = string.Empty;
        public IList<string> Roles      { get; set; } = new List<string>();
        public DateTime      Expiration { get; set; }
    }
}
