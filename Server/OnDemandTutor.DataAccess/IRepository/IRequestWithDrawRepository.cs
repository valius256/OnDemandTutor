using OnDemandTutor.Models.Dtos.WithDrawDto;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.IRepository;

public interface IRequestWithDrawRepository : IGenericRepository<RequestWithDraw>
{
    Task<List<RequestWithDraw>> GetAllRequestWithDraws(RequestWithDrawFilterDto request, int userId);
}