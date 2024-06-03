using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnDemandTutor.DataAccess.Models;

[Table("ConsultationRequest")]
public partial class ConsultationRequest
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    [Required]
    [StringLength(15)]
    [Unicode(false)]
    public string Phone { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RequestDate { get; set; }

    public int Status { get; set; }
}
