using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User;

public class CompareStatusDto
{
    public TutorStatus? OldStatus { get; set; }
    public TutorStatus? NewStatus { get; set; }
}