using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository
{
    public interface ISlotRepository : IGenericRepository<Slot>
    {
        Task<PagedResult<GetSlotsDtos>> GetSlotsAsync(PagingModel<QuerySlotDto> request);
        Task<GetSlotDetailDto> GetSlotByIdAsync(int id);
        Task<bool> DeleteSlotAsync(int id);

        Task<Slot?> GetClosestFutureSlotOfTutor(int tutorId);

        Task<List<Slot>> GetFinishedSlotsToTransfer();
        //Task<List<GetSlotWithSlotStudentDto>?> GetSlotWithSlotStudentByStudentId(int studentId);
    }
}

