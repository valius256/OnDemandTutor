using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User;

public class UserFilterDto
{
    public string? Name { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty;
    public Sex? Sex { get; set; }
    public DateTime? DobFromDate { get; set; }
    public DateTime? DobToDate { get; set; }
    public string? Subject { get; set; } = string.Empty;
    public RoleStatus? Role { get; set; }
    public int Limit { get; set; }
    public int Page { get; set; }
}