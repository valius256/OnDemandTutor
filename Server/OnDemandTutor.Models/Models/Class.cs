using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnDemandTutor.Models.Models
{
    public class Class : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public int StudentId { get; set; }

        public int? TutorId { get; set; }

        public int NumberOfStudent { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public int SubjectId { get; set; }

        [StringLength(100)]
        public string TeachAddress { get; set; }

        public int CreateBy { get; set; }

        public double? TutorRating { get; set; }

        public int Status { get; set; }

        [Column(TypeName = "money")]
        public decimal? Salary { get; set; }

        public int? PriceRatio { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();

        public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();

        [ForeignKey("StudentId")]
        public virtual User Student { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }

        [ForeignKey("TutorId")]
        public virtual User Tutor { get; set; }
    }
}
