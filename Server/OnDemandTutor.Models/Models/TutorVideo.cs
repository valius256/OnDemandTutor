namespace OnDemandTutor.Models.Models
{
    public partial class TutorVideo : IBaseEntity
    {
        public int Id { get; set; }
        public int? TutorId { get; set; }
        public string VideoUrl { get; set; }
        public string Description { get; set; }
        public virtual User Tutor { get; set; }
    }
}
