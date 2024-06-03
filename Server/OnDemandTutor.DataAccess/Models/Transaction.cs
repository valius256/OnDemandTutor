using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnDemandTutor.DataAccess.Models;

[Table("Transaction")]
public partial class Transaction
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

    public int? SlotId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TransactionCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ReferenceId")]
    [InverseProperty("TransactionReferences")]
    public virtual User Reference { get; set; }

    [ForeignKey("SlotId")]
    [InverseProperty("Transactions")]
    public virtual Slot Slot { get; set; }
}
