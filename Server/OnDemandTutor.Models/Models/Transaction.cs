using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnDemandTutor.Models.Models
{
    public class Transaction : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string TransactionCode { get; set; }

        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        public int? Status { get; set; }

        public string Notes { get; set; }

        public int ReferenceId { get; set; }

        public int CreatedBy { get; set; }

        [InverseProperty("TransactionCreatedByNavigations")]
        public virtual User CreatedByNavigation { get; set; }

        [InverseProperty("TransactionReferences")]
        public virtual User ReferenceNavigation { get; set; }
        
        public virtual Slot Slot { get; set; } 
    }
}