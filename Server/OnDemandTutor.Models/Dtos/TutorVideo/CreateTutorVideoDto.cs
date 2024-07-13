namespace OnDemandTutor.Models.Dtos.TutorVideo
{
    public class CreateTutorVideoDto
    {
        public int? TutorId { get; set; }
        public string VideoUrl { get; set; }
        public string Description { get; set; }
    }
}