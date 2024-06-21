using OnDemandTutor.Models.Dtos.Register;

namespace OnDemandTutor.Models.Dtos.User;

public class RegisterTutorDtos : RegisterDtos
{
    public string? DegreeIdentityId { get; set; }
    public string? AvatarImageId { get; set; }
    public string? IdentityCardId { get; set; }
    public string? ScheduleDescription { get; set; }
    public string DiplomaNumber { get; set; }
    public DateOnly IssuanceDate { get; set; }
}