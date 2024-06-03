using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnDemandTutor.DataAccess.Models;

[Table("ClassRequest")]
public partial class ClassRequest
{
    [Key]
    public int Id { get; set; }

    public int ClassId { get; set; }

    public int TutorId { get; set; }

    public int? ApproverId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RequestDate { get; set; }

    public int Status { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("ClassRequests")]
    public virtual Class Class { get; set; }

    [ForeignKey("TutorId")]
    [InverseProperty("ClassRequests")]
    public virtual User Tutor { get; set; }
}
