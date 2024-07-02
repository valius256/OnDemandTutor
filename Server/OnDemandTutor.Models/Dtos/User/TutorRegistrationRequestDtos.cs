using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User;

public class TutorRegistrationRequestDtos
{
    public List<TutorRegistrationDtos> tutorRegistrationDtos { get; set; }
    public TutorSubjectDegreeStatus StatusApproved { get; set; }
}

public class TutorRegistrationDtos
{
    public int TutorDegreeId { get; set; }
    public string? RejectReason { get; set; }
}

