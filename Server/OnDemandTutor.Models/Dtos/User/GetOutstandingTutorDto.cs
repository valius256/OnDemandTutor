namespace OnDemandTutor.Models.Dtos.User;

public class GetOutstandingTutorDto
{
    public TutorSimpleProfileDto Tutor { get; set; } = new();

    public int NumberOfStudentClass { get; set; }

    public int NumberOfBooker { get; set; }
}