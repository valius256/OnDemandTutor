using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.WithDrawDto;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.Repository;

public class RequestWithDrawRepository : GenericRepository<RequestWithDraw>, IRequestWithDrawRepository
{
    public RequestWithDrawRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<RequestWithDraw>> GetAllRequestWithDraws(RequestWithDrawFilterDto request, int userId = 0)
    {
        var queryFilter = dbSet
            .Include(rq => rq.Operator)
            .Include(rq => rq.User)
            .AsQueryable();

        if (userId != 0)
        {
            queryFilter = queryFilter.Where(ld => ld.UserId == userId);
        }
        else
        {
            queryFilter = queryFilter.Where(ld => ld.Status == WithDrawStatus.Pending);
        }

        if (request.FromDate != null && request.ToDate != null)
        {
            queryFilter = queryFilter.Where(ld => ld.CreatedDate >= request.FromDate && ld.CreatedDate <= request.ToDate);
        }

        if (request.MinAmount > 0)
        {
            queryFilter = queryFilter.Where(t => t.Amount >= request.MinAmount);
        }

        if (request.MaxAmount > 0)
        {
            queryFilter = queryFilter.Where(t => t.Amount <= request.MaxAmount);
        }



        int limit = request.Limit > 0 ? request.Limit : 10;
        int page = request.Page > 0 ? request.Page : 1;
        int skip = (page - 1) * limit;
        //queryFilter = queryFilter.Skip(skip).Take(limit);

        var filteredUsers = await queryFilter
            .AsNoTracking()
            .ToNewPagingAsync(page, limit);

        return filteredUsers;

    }
}