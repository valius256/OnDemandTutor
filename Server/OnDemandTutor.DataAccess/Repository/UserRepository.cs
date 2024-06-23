using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.Repository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }


    public async Task<List<User>> GetUsersListDegreeData()
    {
        return await dbSet.Include(ld => ld.TutorDegrees)
            .ToListAsync();
    }

    public async Task<User> GetTutorRegistration(string firebaseId)
    {
        var rs = await dbSet
            .Where(u => u.Role == RoleStatus.Tutor && u.FireBaseid == firebaseId)
            .Include(u => u.TutorDegrees)
            .Include(u => u.TutorSubjects)
            .FirstOrDefaultAsync();
        return rs;
    }

    public async Task<PagedResult<TutorSimpleProfileDtos>> GetTutorListAsync(
        PagingModel<TutorSimpleProfileRequest> request)
    {
        var rs = await dbSet
            .Include(ld => ld.TutorSubjects)
            .Where(ld => ld.Role == RoleStatus.Tutor)
            .ToPagingAsync<TutorSimpleProfileDtos, User>(request.Page, request.Limit);
        return rs;
    }
}