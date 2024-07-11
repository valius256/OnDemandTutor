
using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Dtos.TutorDegree;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.TutorSubject
{
    public class GetTutorSubjectDetailDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public GetTutorDto User { get; set; } = default!;
        public int SubjectId { get; set; }
        public GetSubjectDtos Subject { get; set; } = default!;
        public string Description { get; set; } = string.Empty;
        public TutorSubjectStatus Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public List<GetTutorDegreeDto> Degrees { get; set; } = new List<GetTutorDegreeDto>();
    }
}
