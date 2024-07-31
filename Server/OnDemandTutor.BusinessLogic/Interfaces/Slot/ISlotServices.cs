using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.Slot;

public interface ISlotServices
{
    Task<PagedResult<GetSlotsDtos>> GetSlotsAsync(PagingModel<QuerySlotDto> request);
    Task<GetSlotsDtos> GetClosestSlotOfTutor(GetProfileUserDtos tutor);
    Task<GetSlotDetailDto> GetSlotByIdAsync(int id);
    Task<GetSlotsDtos> CreateSlotAsync(CreateSlotsDto slotDto, GetProfileUserDtos user);
    Task<GetSlotsDtos> UpdateSlotAsync(UpdateSlotDto slotDto, GetProfileUserDtos user);
    Task<bool> DeleteSlotAsync(int id);
    Task CronJobForAutoDereasedMoneyAfterSlotStart();
    Task CronJobForAutoCheckIfStudentDeptIsMoreThan20Percent();
    Task<List<GetSlotWithSlotStudentDto>> GetListOfSlotSameClassBySlotId(int slotId);
    Task UpdateSlotStatusAsync(UpdateSlotStatusDto updateSlotStatusDto);
    Task<bool> EnrollForSlot(int studentId, int slotId);
    Task ValidateSlotForStudent(int slotId, int studentId);

}