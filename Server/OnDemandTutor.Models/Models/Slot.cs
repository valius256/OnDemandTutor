using OnDemandTutor.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnDemandTutor.Models.Models
{
    public class Slot : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int ClassId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime StartTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime EndTime { get; set; }

        public int TransactionId { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ActualEndtime { get; set; }

        [ForeignKey("ClassId")]
        [InverseProperty("Slots")]
        public virtual Class Class { get; set; }

        [InverseProperty("TransactionNavigation")]
        public virtual ICollection<Transaction> SlotTransactionNavigation { get; set; } = new List<Transaction>();
    }
}
