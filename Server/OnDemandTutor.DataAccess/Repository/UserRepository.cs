using Mapster;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.TutorSubject;
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


    public async Task<PagedResult<User>> ViewUsersListAsync(UserFilterDto request)
    {
        var userListQuery = dbSet
            .AsQueryable()
            ;

        if (!string.IsNullOrEmpty(request.Name))
        {
            userListQuery = userListQuery.Where(ld =>
                ld.FirstName.Contains(request.Name) || ld.LastName.Contains(request.Name));
        }

        if (request.IsActive.HasValue)
        {
            userListQuery = userListQuery.Where(ld => ld.IsActive == request.IsActive);
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            userListQuery = userListQuery.Where(ld => ld.Email.Contains(request.Email));
        }

        if (!string.IsNullOrEmpty(request.Phone))
        {
            userListQuery = userListQuery.Where(ld => ld.Phone == request.Phone);
        }

        if (!string.IsNullOrEmpty(request.Address))
        {
            userListQuery = userListQuery.Where(ld => ld.Address == request.Address);
        }

        if (request.Sex.HasValue)
        {
            userListQuery = userListQuery.Where(ld => ld.Sex == request.Sex);
        }

        if (request.Role != null && request.Role.Any())
        {
            userListQuery = userListQuery.Where(ld => request.Role.Contains(ld.Role));
        }


        if (request.DobFromDate != null && request.DobToDate != null)
        {
            userListQuery = userListQuery.Where(ld => ld.Dob >= request.DobFromDate && ld.Dob <= request.DobToDate);
        }

        if (!string.IsNullOrEmpty(request.Subject))
        {
            userListQuery = userListQuery.Where(ld => ld.TutorSubjects.Any(ts => ts.Subject.Name == request.Subject));
        }

        if (request.JoinFromDate.HasValue)
        {
            userListQuery = userListQuery.Where(ld => ld.CreatedDate >= request.JoinFromDate);
        }

        if (request.JoinToDate.HasValue)
        {
            userListQuery = userListQuery.Where(ld => ld.CreatedDate <= request.JoinToDate);
        }


        int limit = request.Limit > 0 ? request.Limit : 10;
        int page = request.Page > 0 ? request.Page : 1;

        // Use the ToPagingAsync method for pagination
        var pagedResult = await userListQuery.ToNewPagingAsync(page, limit);

        return pagedResult;
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

    public async Task<PagedResult<TutorSimpleProfileDto>> ViewTutorListAsync(TutorFilterDto request)
    {
        var tutorListQuery = dbSet
            .Include(u => u.TutorSubjects)
                .ThenInclude(d => d.Subject)
            // .Where(ld => ld.Role == RoleStatus.Tutor && ld.TutorSubjects.Any(ts => ts.Status == TutorSubjectStatus.Approved));
            .Where(u => u.Role == RoleStatus.Tutor);

        if (!string.IsNullOrEmpty(request.Name))
        {
            tutorListQuery = tutorListQuery.Where(ld =>
                ld.FirstName.Contains(request.Name) || ld.LastName.Contains(request.Name));
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            tutorListQuery = tutorListQuery.Where(ld => ld.Email.Contains(request.Email));
        }

        if (request.IsActive.HasValue)
        {
            tutorListQuery = tutorListQuery.Where(ld => ld.IsActive == request.IsActive);
        }

        if (request.TutorStatus != null && request.TutorStatus.Any())
        {
            tutorListQuery = tutorListQuery.Where(ld => request.TutorStatus.Contains(ld.TutorStatus.Value));
        }

        if (!string.IsNullOrEmpty(request.Phone))
        {
            tutorListQuery = tutorListQuery.Where(ld => ld.Phone == request.Phone);
        }

        if (!string.IsNullOrEmpty(request.Address))
        {
            tutorListQuery = tutorListQuery.Where(ld => ld.Address == request.Address);
        }

        if (request.Sex != null && request.Sex.Any())
        {
            tutorListQuery = tutorListQuery.Where(ld => ld.Sex.HasValue && request.Sex.Contains(ld.Sex.Value));
        }

        if (request.JoinFromDate.HasValue)
        {
            tutorListQuery = tutorListQuery.Where(ld => ld.CreatedDate >= request.JoinFromDate);
        }

        if (request.JoinFromDate.HasValue)
        {
            tutorListQuery = tutorListQuery.Where(ld => ld.CreatedDate <= request.JoinToDate);
        }

        if (request.Subject != null)
        {
            tutorListQuery = tutorListQuery.Where(ld => ld.TutorSubjects.Any(ts => request.Subject.Contains(ts.SubjectId)));
        }

        int limit = request.Limit > 0 ? request.Limit : 10;
        int page = request.Page > 0 ? request.Page : 1;
        int skip = (page - 1) * limit;

        //tutorListQuery = tutorListQuery.Skip(skip).Take(limit);

        var filteredTutors = await tutorListQuery
            .AsNoTracking()
            .Select(u => new TutorSimpleProfileDto
            {
                Id = u.Id,
                FullName = u.FirstName + " " + u.LastName,
                Email = u.Email,
                Phone = u.Phone,
                Sex = u.Sex.Value,
                Dob = u.Dob ?? default, // Handle nullable DateTime
                AvatarImageUrl = u.AvatarImageUrl,
                JoiningDate = u.CreatedDate.Value,
                Subject = u.TutorSubjects.Select(ts => ts.Subject.Name).ToList(),
                ScheduleDesciption = u.ScheduleDesciption,
                IsActive = u.IsActive,
                TutorStatus = u.TutorStatus,
                TutorSubjects = u.TutorSubjects.Adapt<List<GetTutorSubjectDto>>()
            })
            .ToNewPagingAsync(page, limit);


        return filteredTutors;
    }

    public async Task<PagedResult<GetOutstandingTutorDto>> GetOutStandingTutors(int limit, int page)
    {
        var tutorListQuery = dbSet
            .Include(u => u.TutorSubjects)
                .ThenInclude(d => d.Subject)
            .Include(u => u.Slots)
                .ThenInclude(s => s.SlotStudents)
            .Include(u => u.Classes)
                .ThenInclude(s => s.StudentClasses)
            .Where(u => u.Role == RoleStatus.Tutor && u.TutorStatus == TutorStatus.Verified && u.IsActive);

        int skip = (page - 1) * limit;
        //tutorListQuery = tutorListQuery.Skip(skip).Take(limit);

        // Materialize the query into a list
        var tutors = await tutorListQuery.ToListAsync();

        // Perform the aggregation in memory
        var outstandingTutors = tutors
            .Select(u => new GetOutstandingTutorDto
            {
                Tutor = u.Adapt<TutorSimpleProfileDto>(),
                NumberOfBooker = u.Slots.Sum(s => s.SlotStudents.Count),
                NumberOfStudentClass = u.Classes.Sum(s => s.StudentClasses.Count),
            })
            .OrderByDescending(u => u.NumberOfBooker + u.NumberOfStudentClass)
            .ToList();

        // Implement your own paging logic here since we materialized the query already
        var pagedResult = new PagedResult<GetOutstandingTutorDto>
        {
            Items = outstandingTutors.Skip(skip).Take(limit).ToList(),
            Page = page,
            Limit = limit,
            Total = outstandingTutors.Count
        };

        return pagedResult;
    }

}