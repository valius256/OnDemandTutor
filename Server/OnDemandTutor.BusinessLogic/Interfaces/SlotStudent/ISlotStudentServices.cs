using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.SlotStudent;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;

public interface ISlotStudentServices
{
    Task<SlotStudentDto> GetSlotStudentAsync(int slotId, int studentId);
    Task<bool> SlotStudentPaidAsync(int slotId, int studentId);
    Task CreateSlotStudentIfNotExist(int slotId, int studentId);
    Task<SlotStudentDto> GetSlotStudentById(int slotId);
    Task<List<GetStudentSlotDto>> GetListSLotStudentByStatus(PaymentStatus status);
}