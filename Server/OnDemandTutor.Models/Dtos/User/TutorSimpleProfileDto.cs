using OnDemandTutor.Models.Dtos.TutorSubject;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User;

public class TutorSimpleProfileDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime Dob { get; set; }
    public DateTime JoiningDate { get; set; }
    public string? AvatarImageUrl { get; set; }
    public Sex Sex { get; set; }
    public List<string> Subject { get; set; }
    public string Description { get; set; }
    public virtual GetTutorSubjectDto SubjectTutor { get; set; }
    public bool IsActive { get; set; } = true;
    public double? Rating { get; set; }
    public TutorStatus? TutorStatus { get; set; } = Enum.TutorStatus.Un_Verified;
    public List<GetTutorSubjectDto> TutorSubjects { get; set; } = new List<GetTutorSubjectDto>();
}