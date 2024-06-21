using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User;

public class GetProfileTutorDtos : GetProfileUserDtos
{
    public string? DegreeIdentityUrl { get; set; }
    public string? AvatarImageUrl { get; set; }
    public string? IdentityCardUrl { get; set; }
    public string? ScheduleDescription { get; set; }
    public TutorSubjectStatus? TutorSubjectStatus { get; set; }
}