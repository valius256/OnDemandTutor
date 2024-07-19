using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.Slot;

public interface ISlotServices
{
    Task<PagedResult<GetSlotsDtos>> GetSlotsAsync(PagingModel<QuerySlotDto> request);
    Task<GetSlotsDtos> GetSlotByIdAsync(int id);
    Task<GetSlotsDtos> CreateSlotAsync(CreateSlotsDtos slotDto);
    Task<UpdateSlotDtos> UpdateSlotAsync(UpdateSlotDtos slotDto);
    Task<bool> DeleteSlotAsync(int id);
    Task CronJobForAutoDereasedMoneyAfterSlotStart();
    Task CronJobForAutoCheckIfStudentDeptIsMoreThan20Percent();
    Task<List<GetSlotWithSlotStudentDto>> GetListOfSlotSameClassBySlotId(int slotId);
}