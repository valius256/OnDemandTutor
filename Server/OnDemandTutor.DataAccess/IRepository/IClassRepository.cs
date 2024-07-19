using OnDemandTutor.Models.Dtos.Class;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        Task<PagedResult<Class>> GetClassesOfStudent(int studentId, int page, int limit);
        Task<PagedResult<Class>> GetClassesOfTutor(int tutorId, int page, int limit);
        Task<PagedResult<Class>> GetClasses(PagingModel<QueryClassDTO> pagingModel);
        Task<Class?> GetClassWithSlotsByIdAsync(int id);
    }
}

