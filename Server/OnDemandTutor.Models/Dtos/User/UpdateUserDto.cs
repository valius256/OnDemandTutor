using System.ComponentModel.DataAnnotations;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User;

public class UpdateUserDto
{
    public int? Id { get; set; }
    [EmailAddress(ErrorMessage = "The Email format is not valid")]
    public string? Email { get; set; }
    [Phone(ErrorMessage = "The Phone format is not valid")]
    public string? Phone { get; set; }
    public string? FirstName { get; set; }
    public Sex? Sex { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? AvatarImageUrl { get; set; }
    public DateTime? Dob { get; set; }
    public decimal? TutorFeePerHour { get; set; }
    public string? IdCardImageUrl { get; set; }
    public string? ScheduleDesciption { get; set; }

}