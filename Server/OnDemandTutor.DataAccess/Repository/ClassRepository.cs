using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Class;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.Repository
{
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        public ClassRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<Class>> GetClasses(PagingModel<QueryClassDTO> pagingModel)
        {
            var classQuery = dbSet
                .Include(c => c.Subject)
                .Include(c => c.Tutor)
                .Include(c => c.Slots)
                .AsQueryable();

            if (pagingModel.Filter != null)
            {
                if (pagingModel.Filter.SubjectId.HasValue)
                {
                    classQuery = classQuery.Where(c => c.SubjectId == pagingModel.Filter.SubjectId.Value);
                }

                if (!string.IsNullOrWhiteSpace(pagingModel.Filter.Name))
                {
                    classQuery = classQuery.Where(c => c.Name.Contains(pagingModel.Filter.Name));
                }

                if (!string.IsNullOrWhiteSpace(pagingModel.Filter.Address))
                {
                    classQuery = classQuery.Where(c => c.Location.Contains(pagingModel.Filter.Address));
                }

                if (pagingModel.Filter.StartTime.HasValue)
                {
                    classQuery = classQuery.Where(c => c.Slots.OrderBy(s => s.StartTime).First().StartTime >= pagingModel.Filter.StartTime.Value);
                }

                if (pagingModel.Filter.EndTime.HasValue)
                {
                    classQuery = classQuery.Where(c => c.Slots.OrderBy(s => s.StartTime).Last().EndTime <= pagingModel.Filter.EndTime.Value);
                }

                if (pagingModel.Filter.MinFeePerHour.HasValue)
                {
                    classQuery = classQuery.Where(c => c.Tutor.TutorFeePerHour >= pagingModel.Filter.MinFeePerHour.Value);
                }

                if (pagingModel.Filter.MaxFeePerHour.HasValue)
                {
                    classQuery = classQuery.Where(c => c.Tutor.TutorFeePerHour <= pagingModel.Filter.MaxFeePerHour.Value);
                }

                if (!string.IsNullOrWhiteSpace(pagingModel.Filter.Method))
                {
                    classQuery = classQuery.Where(c => c.Method.Contains(pagingModel.Filter.Method));
                }

                if (!string.IsNullOrWhiteSpace(pagingModel.Filter.UserName))
                {
                    classQuery = classQuery.Where(c => (c.Tutor.FirstName + " " + c.Tutor.LastName).Contains(pagingModel.Filter.UserName));
                }
            }


            int limit = pagingModel.Limit > 0 ? pagingModel.Limit : 10;
            int page = pagingModel.Page > 0 ? pagingModel.Page : 1;
            int skip = (page - 1) * limit;

            var pagedResult = await classQuery.ToNewPagingAsync(page, limit);

            return pagedResult;
        }

        public async Task<PagedResult<Class>> GetClassesOfStudent(int studentId, int page, int limit)
        {
            var classQuery = dbSet
                .Include(c => c.Subject)
                .Include(c => c.Tutor)
                .Include(c => c.Slots)
                .Include(c => c.StudentClasses)
                .Where(c => c.StudentClasses.Any(sc => sc.StudentId == studentId));



            limit = limit > 0 ? limit : 10;
            page = page > 0 ? page : 1;
            int skip = (page - 1) * limit;

            var pagedResult = await classQuery.ToNewPagingAsync(page, limit);

            return pagedResult;
        }
        public async Task<PagedResult<Class>> GetClassesOfTutor(int tutorId, int page, int limit)
        {
            var classQuery = dbSet
                .Include(c => c.Subject)
                .Include(c => c.Tutor)
                .Include(c => c.Slots)
                .Where(c => c.Tutor.Id == tutorId);

            limit = limit > 0 ? limit : 10;
            page = page > 0 ? page : 1;
            int skip = (page - 1) * limit;

            var pagedResult = await classQuery.ToNewPagingAsync(page, limit);

            return pagedResult;
        }
        //public async Task<PagedResult<Class>> GetClassWithStudentClassOfTeacher(int tutorId, int page, int limit)
        //{
        //    return await dbSet.Include(c => c.StudentClasses).ThenInclude(sc => sc.Student)
        //         .AsQueryable()
        //         .Where(c => c.TutorId == tutorId)
        //        .ToNewPagingAsync(page > 0 ? page : 1, limit > 0 ? limit : 10);
        //}
        public async Task<Class?> GetClassWithSlotsByIdAsync(int id)
        {
            return await dbSet
                .Include(c => c.Subject)
                .Include(c => c.Tutor)
                .Include(c => c.Slots)
                .Include(c => c.StudentClasses)
                    .ThenInclude(sc => sc.Student)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Class>> GetClassWithSlotsByStudentIdAsync(int studentId)
        {
            return await dbSet
                .Include(c => c.Slots)
                .ThenInclude(c => c.SlotStudents)
                .Include(c => c.StudentClasses)
                .ThenInclude(sc => sc.Student)
                .Where(c => c.StudentClasses.Any(sc => sc.StudentId == studentId))
                .ToListAsync();
        }
    }
}
