namespace OnDemandTutor.Models.Dtos.TutorVideo
{
    public class UpdateTutorVideoDto
    {
        public int Id { get; set; }
        public int? TutorId { get; set; }
        public string VideoUrl { get; set; }
        public string Description { get; set; }
    }
}