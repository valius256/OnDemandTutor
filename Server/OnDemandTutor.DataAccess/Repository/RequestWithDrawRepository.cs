using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.WithDrawDto;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.Repository;

public class RequestWithDrawRepository : GenericRepository<RequestWithDraw>, IRequestWithDrawRepository
{
    public RequestWithDrawRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<RequestWithDraw>> GetAllRequestWithDraws(RequestWithDrawFilterDto request, int userId)
    {
        var queryFilter = dbSet
             .Where(ld => ld.UserId == userId)
            .AsQueryable();

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
        queryFilter = queryFilter.Skip(skip).Take(limit);

        var filteredUsers = await queryFilter
            .AsNoTracking()
            .ToListAsync();

        return filteredUsers;

    }
}