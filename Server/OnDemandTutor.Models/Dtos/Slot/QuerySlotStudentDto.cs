using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.BusinessLogic.Services.Slot;

public class QuerySlotStudentDto
{
    public DateTime From { get; set; } = DateTime.Now.AddDays(-15);
    public DateTime To { get; set; } = DateTime.Now.AddDays(15);
    public PaymentStatus? PaymentStatus { get; set; }
    public int? ClassId { get; set; }
    public bool? IsRated { get; set; }
    public int? TutorId { get; set; }
}