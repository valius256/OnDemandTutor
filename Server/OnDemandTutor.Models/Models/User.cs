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

        public int? AvatarImageId { get; set; }

        public UserStatus? Status { get; set; }


        public DateTime? Dob { get; set; }

        public RoleStatus Role { get; set; }

        [Column(TypeName = "money")]
        public decimal? Balance { get; set; }
        public double? Rating { get; set; }
        public int? DegreeImageId { get; set; }
        public int? IdCardImageID { get; set; }
        public string? ScheduleDesciption { get; set; }

        public required string Password { get; set; }

        public virtual ICollection<Blog> BlogCreateByNavigations { get; set; } = new List<Blog>();
        public virtual ICollection<Blog> BlogUpdateByNavigations { get; set; } = new List<Blog>();


        public virtual ICollection<Class> ClassStudents { get; set; } = new List<Class>();


        public virtual ICollection<Class> ClassTutors { get; set; } = new List<Class>();


        public virtual Medium DegreeImage { get; set; }


        public virtual ICollection<FAQ> FAQs { get; set; } = new List<FAQ>();

        public virtual Medium IdCardImage { get; set; }


        public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();


        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<Transaction> TransactionCreatedByNavigations { get; set; } = new List<Transaction>();

        [InverseProperty("ReferenceNavigation")]
        public virtual ICollection<Transaction> TransactionReferences { get; set; } = new List<Transaction>();

        public virtual ICollection<TutorDegree> TutorDegrees { get; set; } = new List<TutorDegree>();

        public virtual ICollection<TutorTeachTimeSchedule> TutorTeachTimeSchedules { get; set; } = new List<TutorTeachTimeSchedule>();


        public virtual ICollection<TutorVideo> TutorVideos { get; set; } = new List<TutorVideo>();
    }
}
