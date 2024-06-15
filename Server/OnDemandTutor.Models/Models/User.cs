using OnDemandTutor.Models.Enum;
using System.ComponentModel.DataAnnotations.Schema;

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

        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<Transaction> TransactionCreatedByNavigations { get; set; } = new List<Transaction>();

        [InverseProperty("ReferenceNavigation")]
        public virtual ICollection<Transaction> TransactionReferences { get; set; } = new List<Transaction>();

        public virtual ICollection<TutorDegree> TutorDegrees { get; set; } = new List<TutorDegree>();

        public virtual ICollection<TutorVideo> TutorVideos { get; set; } = new List<TutorVideo>();
        public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();

    }
}
