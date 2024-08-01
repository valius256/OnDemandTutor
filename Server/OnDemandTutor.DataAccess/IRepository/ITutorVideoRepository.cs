using OnDemandTutor.Models.Dtos.TutorVideo;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository
{
    public interface ITutorVideoRepository : IGenericRepository<TutorVideo>
    {
        Task<PagedResult<TutorVideo>> QueryTutorVideoAsync(PagingModel<QueryTutorVideoDto> pagingModel);

        Task<TutorVideo?> GetTutorVideoByIdAsync(int id);
    }
}