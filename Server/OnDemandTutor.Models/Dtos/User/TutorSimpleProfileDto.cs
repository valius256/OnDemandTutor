using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.Dtos.User;

public class TutorSimpleProfileDto
{
    public string FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime Dob { get; set; }
    public DateTime JoiningDate { get; set; }
    public List<string> Subject { get; set; }
    public string Description { get; set; }
    public virtual TutorSubject SubjectTutor { get; set; }
}