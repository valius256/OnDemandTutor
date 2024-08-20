namespace OnDemandTutor.Models.Dtos.User;

public class UserFireBaseDtos
{
    public string UserId { get; set; } = Guid.NewGuid().ToString();
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
}