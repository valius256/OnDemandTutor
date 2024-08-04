using OnDemandTutor.Models.Dtos.StudentClass;

namespace OnDemandTutor.Models.Dtos.Class;

public class GetClassWithStudentClassDto
{
    public List<GetStudentClassWithStudentDto> StudentClasses = new();
    public int Id { get; set; }
    public string? Name { get; set; }
    public int TutorId { get; set; }
}