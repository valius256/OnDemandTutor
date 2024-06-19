namespace OnDemandTutor.Models.Models
{
    public class Subject : IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubjectType { get; set; }
        public int? CreateById { get; set; }
        public string Description { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool Status { get; set; }
        public int? ClassId { get; set; }
        

        public virtual ICollection<Class> Class { get; set; } = new List<Class>();
        public virtual User CreateBy { get; set; }
        public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
        public virtual ICollection<TutorSubject> TutorSubjects { get; set; } = new List<TutorSubject>();
    }
}
