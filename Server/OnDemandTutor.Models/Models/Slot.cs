using OnDemandTutor.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnDemandTutor.Models.Models
{
    public class Slot : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime StartTime { get; set; }

        public int CreateBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime EndTime { get; set; }

        [StringLength(100)]
        public string? TeachAddress { get; set; }
        public int? SubjectId { get; set; }
        public bool IsOnline { get; set; }

        public int NumberOfStudents { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ActualEndTime { get; set; }
        public virtual Subject Subject { get; set; }
        // Navigation properties
        [ForeignKey("CreateBy")]
        public virtual User CreatedByNavigation { get; set; }
        public virtual ICollection<SlotStudent> SlotStudents { get; set; } = new List<SlotStudent>();
        public virtual ICollection<Transaction> SlotTransactionNavigation { get; set; } = new List<Transaction>();
    }
}