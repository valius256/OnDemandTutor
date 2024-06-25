namespace OnDemandTutor.Models.Models;

public class SlotStudent
{
    public int SlotId { get; set; }
    public virtual Slot Slot { get; set; }

    public int UserId { get; set; }
    public virtual User User { get; set; }
}