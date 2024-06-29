using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Models;

public class SlotStudent
{
    public int SlotId { get; set; }
    public virtual Slot Slot { get; set; }

    public int UserId { get; set; }
    public virtual User User { get; set; }
    
    public PaymentStatus PaymentStatus { get; set; }
    public string Feedback { get; set;}
}

