using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnDemandTutor.DataAccess.Models;

[Table("Class")]
public partial class Class
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

    [InverseProperty("Class")]
    public virtual ICollection<ClassRequest> ClassRequests { get; set; } = new List<ClassRequest>();

    [InverseProperty("Class")]
    public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();

    [InverseProperty("Class")]
    public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();

    [ForeignKey("StudentId")]
    [InverseProperty("ClassStudents")]
    public virtual User Student { get; set; }

    [ForeignKey("SubjectId")]
    [InverseProperty("Classes")]
    public virtual Subject Subject { get; set; }

    [ForeignKey("TutorId")]
    [InverseProperty("ClassTutors")]
    public virtual User Tutor { get; set; }
}
