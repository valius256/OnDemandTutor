using OnDemandTutor.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace OnDemandTutor.Models.Models
{
    public class User : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string FireBaseid { get; set; }
        [StringLength(50)]
        [AllowNull]
        public string? FirstName { get; set; }
        [StringLength(50)]
        [AllowNull]
        public string? LastName { get; set; }
        [StringLength(10)]
        [AllowNull]
        public string? Phone { get; set; }

        [StringLength(50)]
        public required string Email { get; set; }

        [StringLength(100)]
        [AllowNull]
        public string? Address { get; set; }

        public string? AvatarImageId { get; set; }

        public UserStatus? Status { get; set; }


        public DateTime? Dob { get; set; }

        public RoleStatus Role { get; set; }

        [Column(TypeName = "money")]
        public decimal? Balance { get; set; }

        [Column(TypeName = "money")]
        public decimal? TutorFeePerHour { get; set; }
        public double? Rating { get; set; }
        public string? DegreeImageId { get; set; }
        public string? IdCardImageID { get; set; }
        public string? ScheduleDesciption { get; set; }
        public required string Password { get; set; }

        public virtual ICollection<Blog> BlogCreateBy{ get; set; } = new List<Blog>();
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
