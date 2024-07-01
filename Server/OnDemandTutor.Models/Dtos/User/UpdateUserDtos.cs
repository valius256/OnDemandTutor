using System.ComponentModel.DataAnnotations;

namespace OnDemandTutor.Models.Dtos.User;

public class UpdateUserDtos
{
    public int? Id { get; set; }
    [EmailAddress(ErrorMessage = "The email format is not valid")]
    public string? Email { get; set; }
    public string? Password { get; set; }
    [Compare(nameof(Password), ErrorMessage = "The passwords didn't match.")]
    public required string ConfirmPassword { get; set; }
    [Phone(ErrorMessage = "The phone format is not valid")]
    public string? Phone { get; set; }
    public string? FirstName { get; set; } 
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? AvatarImageUrl { get; set; }
    public DateTime? Dob { get; set; }
    public decimal? TutorFeePerHour { get; set; }
    public string? IdCardImageUrl { get; set; }
    public string? ScheduleDesciption { get; set; } 
    
}