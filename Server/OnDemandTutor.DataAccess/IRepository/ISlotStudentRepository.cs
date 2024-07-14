using OnDemandTutor.BusinessLogic.Services.Slot;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.IRepository;

public interface ISlotStudentRepository : IGenericRepository<SlotStudent>
{
    Task<List<SlotStudent>> GetStudentSlotsAsync(QuerySlotStudentDto request, int studentId);
}