using OnDemandTutor.Models.Dtos.Blog;
using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        Task<PagedResult<Subject>> GetSubjects(PagingModel<QuerySubjectDTO> pagingModel);
    }
}

