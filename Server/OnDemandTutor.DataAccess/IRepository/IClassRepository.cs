using OnDemandTutor.Models.Dtos.Class;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        Task<Class?> GetFullDataClass(int id);
        Task<PagedResult<Class>> GetClasses(PagingModel<QueryClassDTO> pagingModel);
    }
}

