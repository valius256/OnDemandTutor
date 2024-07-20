
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.StudentClass;
using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Class
{
    public class GetClassWithStudentClassDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int TutorId { get; set; }

        public List<GetStudentClassWithStudentDto> StudentClasses = new List<GetStudentClassWithStudentDto>();
    }
}
