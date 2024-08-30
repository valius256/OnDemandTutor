namespace OnDemandTutor.Models.Dtos.Subject
{
    public class GetSubjectDtos
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SubjectType { get; set; } = string.Empty;
        public int? CreateById { get; set; }
        public string Description { get; set; } = string.Empty ;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsEnable { get; set; }
    }
}

