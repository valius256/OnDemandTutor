using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnDemandTutor.DataAccess.Models;

[Table("TutorTeachTimeSchedule")]
public partial class TutorTeachTimeSchedule
{
    [Key]
    public int Id { get; set; }

    public DateOnly? DayOfWeek { get; set; }

    [Precision(0)]
    public TimeOnly? StartTime { get; set; }

    [Precision(0)]
    public TimeOnly? EndTime { get; set; }

    public int? TutorId { get; set; }

    public int? Status { get; set; }

    [ForeignKey("TutorId")]
    [InverseProperty("TutorTeachTimeSchedules")]
    public virtual User Tutor { get; set; }
}
