using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository
{
    public interface ISlotRepository : IGenericRepository<Slot>
    {
        Task<PagedResult<GetSlotsDtos>> GetSlotsAsync(PagingModel<QuerySlotDto> request);
        Task<GetSlotsDtos> GetSlotByIdAsync(int id);
        Task<CreateSlotsDtos> CreateSlotAsync(CreateSlotsDtos slot);
        Task<UpdateSlotDtos> UpdateSlotAsync(UpdateSlotDtos slot);
        Task<bool> DeleteSlotAsync(int id);
        Task<GetSlotWithSlotStudentDto?> GetSlotWithSlotStudentStudentById(int id);
        Task<List<GetSlotWithSlotStudentDto>?> GetSlotWithSlotStudentByStudentId(int studentId);
    }
}

