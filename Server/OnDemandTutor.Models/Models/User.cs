using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Models
{
    public class User : IBaseEntity
    {
        public int Id { get; set; }
        public string FireBaseid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public required string Email { get; set; }
        public string? Address { get; set; }
        public string? AvatarImageId { get; set; }
        public UserStatus? Status { get; set; }
        public DateTime? Dob { get; set; }
        public RoleStatus Role { get; set; }
        public decimal? Balance { get; set; }
        public decimal? TutorFeePerHour { get; set; }
        public double? Rating { get; set; }
        public string? DegreeImageId { get; set; }
        public string? IdCardImageID { get; set; }
        public string? ScheduleDesciption { get; set; }
        public required string Password { get; set; }

        public virtual ICollection<Blog> BlogCreateBy { get; set; } = new List<Blog>();
        public virtual ICollection<Blog> BlogUpdateBy { get; set; } = new List<Blog>();


        public virtual ICollection<FAQ> FAQs { get; set; } = new List<FAQ>();
        public virtual ICollection<SlotStudent> SlotStudents { get; set; } = new List<SlotStudent>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<Transaction> TransactionCreatedBy { get; set; } = new List<Transaction>();

        public virtual ICollection<TutorDegree> TutorDegrees { get; set; } = new List<TutorDegree>();
        public virtual ICollection<Subject> SubjectCreateBy { get; set; } = new List<Subject>();
        public virtual ICollection<TutorVideo> TutorVideos { get; set; } = new List<TutorVideo>();
        public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
        public virtual ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
        public virtual ICollection<TutorSubject> TutorSubjects { get; set; } = new List<TutorSubject>();
    }
}
