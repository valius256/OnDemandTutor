using OnDemandTutor.Models.Dtos.WithDrawDto;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository;

public interface IRequestWithDrawRepository : IGenericRepository<RequestWithDraw>
{
    Task<PagedResult<RequestWithDraw>> GetAllRequestWithDraws(RequestWithDrawFilterDto request, int userId = 0);
}