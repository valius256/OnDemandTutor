using System.ComponentModel.DataAnnotations;

namespace OnDemandTutor.Models.Models;

public class StudentClass : BaseEntity
{
    public int StudentId { get; set; }
    public virtual User Student { get; set; } = default!;

    public int ClassId { get; set; }
    public virtual Class Class { get; set; } = default!;
    [Range(1, 5)]
    public int? Rating { get; set; }
    public string? Feedback { get; set; }
    public decimal? DepositPaid { get; set; }
}