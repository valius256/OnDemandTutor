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

    public async Task<List<TutorRegistrationResponseDtos>> GetTutorRegistration(string firebaseId)
    {
        var tutorList = await dbSet
            .Include(u => u.TutorDegrees)
            .Include(u => u.TutorSubjects)
            .Where(u => u.Role == RoleStatus.Tutor && u.FireBaseid == firebaseId && u.TutorDegrees.Any(td => td.TutorSubjectStatus == TutorSubjectDegreeStatus.Pending)) // fetch record with pending
             .Select(u => new TutorRegistrationResponseDtos
             {
                 UserName = u.FirstName + " " + u.LastName,
                 Email = u.Email,
                 DegreeImgUrl = u.TutorDegrees.FirstOrDefault().DegreeImgUrl,
                 SubjectDegreeId = u.TutorDegrees.FirstOrDefault().Id,
                 DegreeNumber = u.TutorDegrees.FirstOrDefault().DegreeNumber,
                 SubjectId = u.TutorDegrees.FirstOrDefault().SubjectId,
                 IssuranceDate = u.TutorDegrees.FirstOrDefault().IssuranceDate,
                 Status = u.TutorDegrees.FirstOrDefault().TutorSubjectStatus
             })
             .AsNoTracking()
            .ToListAsync();
        return tutorList;
    }

    public async Task<PagedResult<User>> ViewTutorListAsync(
        PagingModel<TutorSimpleProfileRequest> request)
    {
        var tutorList = await dbSet
            .Include(ld => ld.TutorSubjects)
            .Where(ld => ld.Role == RoleStatus.Tutor)
            .AsNoTracking()
            .ToPagingAsync(request.Page, request.Limit);
        return tutorList;
    }
}