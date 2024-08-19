using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User;

public class GetProfileTutorDtos : GetProfileUserDto
{
    public string? IdCardImageUrl { get; set; }
    public string? ScheduleDescription { get; set; }
    public TutorSubjectDegreeStatus? TutorSubjectDegreeStatus { get; set; }
    public virtual TutorDegreeDto Degrees { get; set; }

}