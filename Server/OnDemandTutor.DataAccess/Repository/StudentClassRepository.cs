using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.StudentClass;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.Repository;

public class StudentClassRepository : GenericRepository<StudentClass>, IStudentClassRepository
{
    public StudentClassRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<StudentClass>> QueryStudentClass(PagingModel<QueryStudentClassDto> request)
    {
        var query = dbSet.AsQueryable()
            .Include(sc => sc.Class).ThenInclude(c => c.Subject)
            .Include(sc => sc.Student)
            .AsQueryable();

        if (request.Filter != null)
        {
            if (request.Filter.TutorId.HasValue) query = query.Where(sc => sc.Class.TutorId == request.Filter.TutorId);
            if (request.Filter.IsRated.HasValue && request.Filter.IsRated.Value)
                query = query.Where(sc => sc.Rating != null == request.Filter.IsRated);
        }

        // Apply filtering if necessary

        return await query.ToNewPagingAsync(request.Page > 0 ? request.Page : 1,
            request.Limit > 0 ? request.Limit : 10);
    }
}