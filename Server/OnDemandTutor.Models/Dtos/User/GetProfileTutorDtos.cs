using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.Dtos.User;

public class GetProfileTutorDtos : GetProfileUserDtos
{
    public string? IdCardImageUrl { get; set; }
    public string? ScheduleDescription { get; set; }
    public TutorSubjectDegreeStatus? TutorSubjectDegreeStatus { get; set; }
    public virtual TutorDegreeDto Degrees { get; set; }

}