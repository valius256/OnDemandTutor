using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User
{
    public class TutorRegistrationResponseDtos
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public int SubjectDegreeId { get; set; }
        public string DegreeImgUrl { get; set; }
        public string DegreeNumber { get; set; }
        public int SubjectId { get; set; }
        public string RejectReason { get; set;  }
        public DateOnly IssuranceDate { get; set; }
        public TutorSubjectDegreeStatus Status { get; set; }
    }
}
