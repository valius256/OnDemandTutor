using OnDemandTutor.Models.Dtos.SlotStudent;

namespace OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;

public interface ISlotStudentServices
{
    Task<SlotStudentDto> GetSlotStudentAsync(int slotId, int studentId);
    Task<bool> SlotStudentPaidAsync(int slotId, int studentId);
}