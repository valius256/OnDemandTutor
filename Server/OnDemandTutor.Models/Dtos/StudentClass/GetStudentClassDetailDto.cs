using OnDemandTutor.Models.Dtos.Class;
using OnDemandTutor.Models.Dtos.User;

namespace OnDemandTutor.Models.Dtos.StudentClass;

public class GetStudentClassDetailDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public int? Rating { get; set; }
    public string? Feedback { get; set; }

    public GetProfileUserDtos Student { get; set; } = default!;
    public GetClassDtos Class { get; set; } = default!;
}