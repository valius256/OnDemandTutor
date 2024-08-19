using OnDemandTutor.Models.Dtos.User;

namespace OnDemandTutor.Models.Dtos.TutorVideo
{
    public class GetTutorVideoDto
    {
        public int Id { get; set; }
        public int? TutorId { get; set; }
        public string VideoUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public GetProfileUserDto Tutor { get; set; } = default!;
    }
}