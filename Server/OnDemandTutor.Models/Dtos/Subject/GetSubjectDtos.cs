namespace OnDemandTutor.Models.Dtos.Subject
{
    public class GetSubjectDtos
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubjectType { get; set; }
        public int? CreateById { get; set; }
        public string Description { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool Status { get; set; }
        public int? ClassId { get; set; }
        public int TutorDegreeId { get; set; }
    }
}

