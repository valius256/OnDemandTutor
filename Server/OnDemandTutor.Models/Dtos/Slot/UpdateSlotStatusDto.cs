using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Slot;

public class UpdateSlotStatusDto
{
    public int Id { get; set; }
    public SlotStatus Status { get; set; }
}