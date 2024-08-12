using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.TutorSubject
{
    public class UpdateTutorSubjectStatusDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SubjectId { get; set; }
        public TutorSubjectStatus Status { get; set; }
        public string? ReasonReject { get; set; }
    }
}