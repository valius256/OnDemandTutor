using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnDemandTutor.DataAccess.Models;

[Table("TutorDegree")]
public partial class TutorDegree
{
    [Key]
    public int Id { get; set; }

    public int? TutorId { get; set; }

    public int? DegreeImgID { get; set; }

    public string Description { get; set; }

    public virtual Medium DegreeImg { get; set; }

    public virtual User Tutor { get; set; }
}
