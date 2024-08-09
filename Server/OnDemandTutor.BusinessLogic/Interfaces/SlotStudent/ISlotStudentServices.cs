using OnDemandTutor.BusinessLogic.Services.Slot;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.SlotStudent;
using OnDemandTutor.Models.Dtos.StudentSlot;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;

public interface ISlotStudentServices
{
    Task<List<GetSlotStudentDetailDto>> QuerySlotStudent(QuerySlotStudentDto querySlotStudentDto, GetProfileUserDtos? user);

    Task<List<GetSlotStudentDetailDto>> GetSimpleStudentSlotOfStudent(int studentId);
    Task<PagedResult<GetSlotStudentDetailDto>> GetStudentSlotByTutor(PagingModel<QueryRatingDto> queryRatingDto);
    Task<GetSlotStudentDetailDto> GetClosestFutureSlot(GetProfileUserDtos user);
    Task<SlotStudentDto> GetSlotStudentAsync(int slotId, int studentId);
    Task<PagedResult<GetSlotStudentWithDetailStudentDto>> GetSlotStudentsOfSlotPaged(int slotId, int page, int limit);
    Task<List<GetSlotStudentWithDetailStudentDto>> GetSlotStudentsOfSlotAsync(int slotId);
    Task<bool> SlotStudentPaidAsync(int slotId, int studentId, decimal value);
    Task<SlotStudentDto> GetSlotStudentById(int slotId);
    Task<List<GetStudentSlotDto>> GetListSLotStudentByStatus(PaymentStatus status);
    Task<bool> SoftDeleteSlotStudent(int slotId, int studentId);
    Task<bool> UpdateSlotStudentAsync(int slotId, int studentId, decimal rate, string feedback);

    Task<List<SlotStudentDto>> GetListSlotStudentByStudentId(int studentId);
    Task<bool> CreateSlotStudentIfNotExists(int slotId, int studentId);

    Task CronJobForAutoDereasedMoneyAfterSlotStart();
    Task LeaveSlot(int slotId, GetProfileUserDtos user);
    Task Refund(int slotId, int userId);

    Task SetTransferred(int id);
}
