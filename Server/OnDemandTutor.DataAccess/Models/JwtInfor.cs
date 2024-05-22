namespace OnDemandTutor.DataAccess.Models
{
    public class JwtInfor
    {
        public string JwtId { get; set; }
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? LocationId { get; set; }
        public long Expires { get; set; }
        public string? Jwt { get; set; }
        public string? NextJwt { get; set; }
        public List<string>? Roles { get; set; }
        public long TimeRemaining => (Expires - DateTimeOffset.UtcNow.ToUnixTimeSeconds());

    }
}
