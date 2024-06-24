using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User;

public class RegisterTutorDtos
{
    public string? AvatarImageurl { get; set; }
    public string? IdentityCardUrl { get; set; }
    public string? ScheduleDescription { get; set; }

    public List<TutorDegreeDto> Degrees { get; set; } = new();
}

public class TutorDegreeDto
{
    public string? DegreeImgUrl { get; set; }
    public string? Description { get; set; }
    public int SubjectId { get; set; }
    public string DegreeNumber { get; set; }
    public TutorSubjectDegreeStatus Status { get; set; }
    public DateOnly IssuranceDate { get; set; }
}