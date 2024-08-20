using OnDemandTutor.Models.Dtos.TutorSubject;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User;

public class TutorSimpleProfileDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime Dob { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? AvatarImageUrl { get; set; }
    public string? Address { get; set; }
    public Sex Sex { get; set; }
    public List<string> Subject { get; set; } = new List<string>();
    public string ScheduleDesciption { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public double? Rating { get; set; }
    public decimal? TutorFeePerHour { get; set; }
    public TutorStatus? TutorStatus { get; set; } = Enum.TutorStatus.Un_Verified;
    public List<GetTutorSubjectWithSubjectDto> TutorSubjects { get; set; } = new List<GetTutorSubjectWithSubjectDto>();
}