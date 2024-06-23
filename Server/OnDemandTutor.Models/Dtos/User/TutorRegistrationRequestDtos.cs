using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User;

public class TutorRegistrationRequestDtos
{
   public int SubjectDegreeId { get; set; } 
   public string DegreeImgUrl { get; set; }
   public string DegreeNumber { get; set; }
   public DateOnly IssuranceDate { get; set; }
   public int SubjectId { get; set; }
   public string? Description { get; set; }
   public TutorSubjectDegreeStatus Status { get; set; } 
}
