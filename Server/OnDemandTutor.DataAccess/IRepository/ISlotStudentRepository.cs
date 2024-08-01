using OnDemandTutor.BusinessLogic.Services.Slot;
using OnDemandTutor.Models.Dtos.StudentSlot;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository;

public interface ISlotStudentRepository : IGenericRepository<SlotStudent>
{
    Task<List<SlotStudent>> GetStudentSlotsAsync(QuerySlotStudentDto request, int? studentId);
    Task<SlotStudent?> GetClosestFutureSlot(int studentId);

    Task<PagedResult<SlotStudent>> GetStudentSlotByTutor(PagingModel<QueryRatingDto> queryDto);

    Task<PagedResult<SlotStudent>> GetStudentsSlotWithStudentBySlotIdPaged(int slotId, int page, int limit);
    Task<List<SlotStudent>> GetStudentsSlotWithStudentBySlotId(int slotId);

    Task<List<SlotStudent>> GetSlotOfStudent(int studentId);

    Task<List<SlotStudent>> GetAboutToStartStudentSlots();

    Task<SlotStudent?> GetSlotStudentBySlotIdAndStudentId(int slotId, int studentId);
}