using OnDemandTutor.Models.Dtos.TutorDegree;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.TutorSubject
{
    public class UpdateTutorSubjectDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<UpdateTutorDegreeDto> Degrees { get; set; } = new List<UpdateTutorDegreeDto>();
    }
}
