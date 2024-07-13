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
        public bool IsEnable { get; set; }
        public string CreateByName { get; set; }
    }
}

