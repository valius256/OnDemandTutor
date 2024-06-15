namespace OnDemandTutor.Models.Models;

public class SlotStudent
{
    public int SlotId { get; set; }
    public virtual required Slot Slot { get; set; }

    public int UserId { get; set; }
    public virtual required User User { get; set; }
}