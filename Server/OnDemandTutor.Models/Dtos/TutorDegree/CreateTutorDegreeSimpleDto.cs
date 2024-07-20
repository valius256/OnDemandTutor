

namespace OnDemandTutor.Models.Dtos.TutorDegree
{
    public class CreateTutorDegreeSimpleDto
    {
        public string? DegreeImgUrl { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? TutorDegreeName { get; set; }
        public string DegreeNumber { get; set; } = string.Empty;
        public DateOnly IssuranceDate { get; set; }
    }
}
