using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.Repository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }


    public async Task<List<User>> ViewUsersListAsync(UserFilterDto request)
    {
        var userListQuery = dbSet
            .AsQueryable()
            ;

        if (!string.IsNullOrEmpty(request.name))
        {
            userListQuery = userListQuery.Where(ld =>
                ld.FirstName.Contains(request.name) || ld.LastName.Contains(request.name));
        }

        if (!string.IsNullOrEmpty(request.email))
        {
            userListQuery = userListQuery.Where(ld => ld.Email == request.email);
        }

        if (!string.IsNullOrEmpty(request.phone))
        {
            userListQuery = userListQuery.Where(ld => ld.Phone == request.phone);
        }

        if (!string.IsNullOrEmpty(request.Address))
        {
            userListQuery = userListQuery.Where(ld => ld.Address == request.Address);
        }

        if (request.sex != Sex.Other)
        {
            userListQuery = userListQuery.Where(ld => ld.Sex == request.sex);
        }

        if (request.Role != null)
        {
            userListQuery = userListQuery.Where(ld => ld.Role == request.Role);
        }

        if (request.DobFromDate != null && request.DobToDate != null)
        {
            userListQuery = userListQuery.Where(ld => ld.Dob >= request.DobFromDate && ld.Dob <= request.DobToDate);
        }

        if (!string.IsNullOrEmpty(request.Subject))
        {
            userListQuery = userListQuery.Where(ld => ld.TutorSubjects.Any(ts => ts.Subject.Name == request.Subject));
        }

        int limit = request.Limit > 0 ? request.Limit : 10;
        int page = request.Page > 0 ? request.Page : 1;
        int skip = (page - 1) * limit;
        userListQuery = userListQuery.Skip(skip).Take(limit);

        var filteredUsers = await userListQuery
            .AsNoTracking()
            .ToListAsync();

        return filteredUsers;
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

    public async Task<List<User>> ViewTutorListAsync(TutorFilterDto request)
    {
        var tutorListQuery = dbSet
            .Include(ld => ld.TutorSubjects)
            .Where(ld => ld.Role == RoleStatus.Tutor);

        if (!string.IsNullOrEmpty(request.name))
        {
            tutorListQuery = tutorListQuery.Where(ld =>
                ld.FirstName.Contains(request.name) || ld.LastName.Contains(request.name));
        }

        if (!string.IsNullOrEmpty(request.email))
        {
            tutorListQuery = tutorListQuery.Where(ld => ld.Email == request.email);
        }

        if (!string.IsNullOrEmpty(request.phone))
        {
            tutorListQuery = tutorListQuery.Where(ld => ld.Phone == request.phone);
        }

        if (!string.IsNullOrEmpty(request.Address))
        {
            tutorListQuery = tutorListQuery.Where(ld => ld.Address == request.Address);
        }

        if (request.sex != Sex.Other)
        {
            tutorListQuery = tutorListQuery.Where(ld => ld.Sex == request.sex);
        }

        if (request.DobFromDate != null && request.DobToDate != null)
        {
            tutorListQuery = tutorListQuery.Where(ld => ld.Dob >= request.DobFromDate && ld.Dob <= request.DobToDate);
        }

        if (request.JoinFromDate != null && request.JoinToDate != null)
        {
            tutorListQuery = tutorListQuery.Where(ld => ld.CreatedDate >= request.JoinFromDate && ld.CreatedDate <= request.JoinToDate);
        }

        if (!string.IsNullOrEmpty(request.Subject))
        {
            tutorListQuery = tutorListQuery.Where(ld => ld.TutorSubjects.Any(ts => ts.Subject.Name == request.Subject));
        }


        int limit = request.Limit > 0 ? request.Limit : 10;
        int page = request.Page > 0 ? request.Page : 1;
        int skip = (page - 1) * limit;
        tutorListQuery = tutorListQuery.Skip(skip).Take(limit);

        var filteredUsers = await tutorListQuery
            .AsNoTracking()
            .ToListAsync();

        return filteredUsers;
    }



}