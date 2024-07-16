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

        public async Task<Class?> GetFullDataClass(int id)
        {
            var res = await dbSet.Include(ld => ld.Slots)
                .Where(cl => cl.Id == id)
                .FirstOrDefaultAsync();
            ;
            return res;
        }
        public async Task<PagedResult<Class>> GetClasses(PagingModel<QueryClassDTO> pagingModel)
        {
            var classQuery = dbSet
                .Include(c => c.Subject)
                .Include(c => c.User)
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
                    classQuery = classQuery.Where(c => c.Slots.Any(s => s.StartTime >= pagingModel.Filter.StartTime.Value));
                }

                if (pagingModel.Filter.EndTime.HasValue)
                {
                    classQuery = classQuery.Where(c => c.Slots.Any(s => s.EndTime <= pagingModel.Filter.EndTime.Value));
                }

                if (pagingModel.Filter.MinFeePerHour.HasValue)
                {
                    classQuery = classQuery.Where(c => c.User.TutorFeePerHour >= pagingModel.Filter.MinFeePerHour.Value);
                }

                if (pagingModel.Filter.MaxFeePerHour.HasValue)
                {
                    classQuery = classQuery.Where(c => c.User.TutorFeePerHour <= pagingModel.Filter.MaxFeePerHour.Value);
                }

                if (!string.IsNullOrWhiteSpace(pagingModel.Filter.Method))
                {
                    classQuery = classQuery.Where(c => c.Method.Contains(pagingModel.Filter.Method));
                }

                if (!string.IsNullOrWhiteSpace(pagingModel.Filter.UserName))
                {
                    classQuery = classQuery.Where(c => (c.User.FirstName + " " + c.User.LastName).Contains(pagingModel.Filter.UserName));
                }
            }

            if (pagingModel.Sorts != null)
            {
                classQuery = classQuery.OrderProperty(pagingModel.Sorts);
            }

            int limit = pagingModel.Limit > 0 ? pagingModel.Limit : 10;
            int page = pagingModel.Page > 0 ? pagingModel.Page : 1;
            int skip = (page - 1) * limit;

            var pagedResult = await classQuery.ToNewPagingAsync(page, limit);

            return pagedResult;
        }

    }
}
