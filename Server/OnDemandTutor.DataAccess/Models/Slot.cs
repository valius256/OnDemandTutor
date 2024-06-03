using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnDemandTutor.DataAccess.Models;

[Table("Slot")]
public partial class Slot
{
    [Key]
    public int Id { get; set; }

    public int ClassId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndTime { get; set; }

    public int PaymentStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ActualEndtime { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("Slots")]
    public virtual Class Class { get; set; }

    [InverseProperty("Slot")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
