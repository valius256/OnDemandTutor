namespace OnDemandTutor.Models.Dtos.StudentSlot;

public class QueryRatingDto
{
    public int TutorId { get; set; }

    public bool? IsRated { get; set; }
}