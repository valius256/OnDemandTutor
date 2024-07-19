
using OnDemandTutor.Models.Dtos.User;

namespace OnDemandTutor.Models.Dtos.StudentClass
{
    public class GetStudentClassWithStudentDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public int? Rating { get; set; }

        public GetProfileUserDtos Student { get; set; } = default!;
    }
}
