using OnDemandTutor.BusinessLogic.Services.Slot;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.SlotStudent;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;

public interface ISlotStudentServices
{
    Task<List<GetSlotStudentDetailDto>> QuerySlotStudent(QuerySlotStudentDto querySlotStudentDto, GetProfileUserDtos user);
    Task<GetSlotStudentDetailDto> GetClosestFutureSlot(GetProfileUserDtos user);
    Task<SlotStudentDto> GetSlotStudentAsync(int slotId, int studentId);
    Task<IEnumerable<SlotStudentDto>> GetSlotStudentsOfSlotAsync(int slotId);
    Task<bool> SlotStudentPaidAsync(int slotId, int studentId);
    Task<Models.Models.SlotStudent> CreateSlotStudentIfNotExist(int slotId, int studentId);
    Task<SlotStudentDto> GetSlotStudentById(int slotId);
    Task<List<GetStudentSlotDto>> GetListSLotStudentByStatus(PaymentStatus status);
    Task<bool> SoftDeleteSlotStudent(int slotId, int studentId);
    Task<bool> UpdateSlotStudentAsync(int slotId, int studentId, double rate, string feedback);

    Task<List<SlotStudentDto>> GetListSlotStudentByStudentId(int studentId);
    Task<bool> CreateSlotStudent(int slotId, int studentId);
}
