namespace OnDemandTutor.Models.Dtos.StudentClass
{
    public class GetStudentClassDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public int RatingForTutorId { get; set; }
        public int? Rating { get; set; }
        public string? Feedback { get; set; }
    }
}

