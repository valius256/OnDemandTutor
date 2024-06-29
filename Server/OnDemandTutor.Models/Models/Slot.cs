using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Models;

public class Slot : BaseEntity
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public int CreateById { get; set; }
    public DateTime EndTime { get; set; }
    public string? TeachAddress { get; set; }
    public int? ClassId { get; set; }
    public int? SubjectId { get; set; }
    public bool IsOnline { get; set; }
    public int NumberOfStudents { get; set; }
    public SlotStatus SlotStatus { get; set; }
    public DateTime? ActualEndTime { get; set; }
    public bool Finished { get; set; }
    public virtual Subject Subject { get; set; }
    
    

    // Navigation properties
    public virtual User CreatedBy { get; set; }
    public virtual ICollection<SlotStudent> SlotStudents { get; set; } = new List<SlotStudent>();
    public virtual ICollection<Transaction> SlotTransaction { get; set; } = new List<Transaction>();
    public virtual Class Classes { get; set; }
}