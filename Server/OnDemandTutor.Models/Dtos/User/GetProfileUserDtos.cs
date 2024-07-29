using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User;

public class GetProfileUserDtos
{
    public int Id { get; set; }
    public string FireBaseid { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? AvatarImageUrl { get; set; }
    public string? IdCardImageUrl { get; set; }
    public RoleStatus Role { get; set; }
    public DateTime? Dob { get; set; }
    public string? Sex { get; set; }
    public string? BankAccount { get; set; }
    public string? ScheduleDesciption { get; set; }
    public string? DeaActiveReason { get; set; }
    public double? Rating { get; set; }
    public TutorStatus? TutorStatus { get; set; }
    public decimal? Balance { get; set; }
    public bool IsActive { get; set; } = true;
    public decimal TutorFeePerHour { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

}