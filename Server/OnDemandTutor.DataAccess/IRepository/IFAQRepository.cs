using OnDemandTutor.Models.Dtos.FAQ;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository
{
    public interface IFAQRepository : IGenericRepository<FAQ>
    {
        Task<PagedResult<FAQ>> GetFAQs(PagingModel<QueryFAQDTO> pagingModel);
    }
}

