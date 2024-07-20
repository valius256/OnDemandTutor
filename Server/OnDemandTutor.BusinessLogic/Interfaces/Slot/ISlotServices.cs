using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.Slot;

public interface ISlotServices
{
    Task<PagedResult<GetSlotsDtos>> GetSlotsAsync(PagingModel<QuerySlotDto> request);
    Task<GetSlotDetailDto> GetSlotByIdAsync(int id);
    Task<GetSlotsDtos> CreateSlotAsync(CreateSlotsDto slotDto);
    Task<UpdateSlotDto> UpdateSlotAsync(UpdateSlotDto slotDto);
    Task<bool> DeleteSlotAsync(int id);
    Task CronJobForAutoDereasedMoneyAfterSlotStart();
    Task CronJobForAutoCheckIfStudentDeptIsMoreThan20Percent();
    Task<List<GetSlotWithSlotStudentDto>> GetListOfSlotSameClassBySlotId(int slotId);
    Task UpdateSlotStatusAsync(UpdateSlotStatusDto updateSlotStatusDto);
    Task<bool> EnrollForSlot(int studentId, int slotId);
    Task<SlotConflictDto> IsSlotConflict(int slotId, int studentId);
    Task<List<GetSlotWithSlotStudentDto>?> GetListSlotOfStudentByStudentId(int studentId);
}