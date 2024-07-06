using OnDemandTutor.Models.Dtos.TutorSubject;
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
<<<<<<< HEAD
    public virtual GetTutorSubjectDto SubjectTutor { get; set; }
=======
>>>>>>> a30ffdd27a876d4d871337759b2c2d2dda0cfb04
}