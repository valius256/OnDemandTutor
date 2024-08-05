using Microsoft.EntityFrameworkCore;
using OnDemandTutor.BusinessLogic.Services.Slot;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.StudentClass;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.Repository
{
    public class StudentClassRepository : GenericRepository<StudentClass>, IStudentClassRepository
    {
        public StudentClassRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async Task<List<StudentClass>> GetAllStudentClassesThatHaveAtLeastOneDebtSlot()
        {
            return await dbSet
            .Include(sc => sc.Student)
            .Include(sc => sc.Class)
                .ThenInclude(sc => sc.Tutor)
            .Include(sc => sc.Class)
                .ThenInclude(c => c.Slots)
                    .ThenInclude(s => s.SlotStudents)
            .Where(sc => sc.Class.Slots
                .Any(s => s.SlotStudents
                    .Any(ss => ss.PaymentStatus == Models.Enum.PaymentStatus.Notpaid
                        && ss.UserId == sc.StudentId) && s.SlotStatus == Models.Enum.SlotStatus.Finished))
            .ToListAsync();
        }
        public async Task<PagedResult<StudentClass>> QueryStudentClass(PagingModel<QueryStudentClassDto> request)
        {
            var query = dbSet.AsQueryable()
               .Include(sc => sc.Class).ThenInclude(c => c.Subject)
               .Include(sc => sc.Student)
               .AsQueryable();

            if (request.Filter != null)
            {
                if (request.Filter.TutorId.HasValue)
                {
                    query = query.Where(sc => sc.Class.TutorId == request.Filter.TutorId);
                }
                if (request.Filter.IsRated.HasValue && request.Filter.IsRated.Value)
                {
                    query = query.Where(sc => (sc.Rating != null) == request.Filter.IsRated);
                }
            }

            // Apply filtering if necessary

            return await query.ToNewPagingAsync(request.Page > 0 ? request.Page : 1, request.Limit > 0 ? request.Limit : 10);
        }


    }
}

