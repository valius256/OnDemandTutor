using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User;

public class TutorRegistrationRequestDtos
{
    public List<TutorDegreeDto> TutorDegrees { get; set; } = new List<TutorDegreeDto>();
    public TutorSubjectDegreeStatus StatusApproved { get; set; }
}
