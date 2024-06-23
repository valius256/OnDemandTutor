using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.Dtos.User;

public class TutorSimpleProfileDtos
{
    public string FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Description { get; set; }
    public virtual TutorSubject Subject { get; set; }
}