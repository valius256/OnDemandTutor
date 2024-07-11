using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.TutorSubject;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.Repository
{
    public class TutorSubjectRepository : GenericRepository<TutorSubject>, ITutorSubjectRepository
    {
        public TutorSubjectRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<TutorSubject?> GetTutorSubjectById(int id)
        {
            return await dbSet
                .Include(ts => ts.User)
                .Include(ts => ts.Subject)
                .FirstOrDefaultAsync(ts => ts.Id == id);
        }
        public async Task<PagedResult<TutorSubject>> GetTutorSubjects(PagingModel<QueryTutorSubjectDto> request)
        {
            var query = dbSet
                .Include(ts => ts.User)
                .Include(ts => ts.Subject)
                .AsQueryable();

            
            var queryTutorSubjectDto = request.Filter;
            if (queryTutorSubjectDto != null)
            {
                if (queryTutorSubjectDto.Status.HasValue)
                {
                    query = query.Where(ts => ts.Status == queryTutorSubjectDto.Status);
                }
                if (queryTutorSubjectDto.TutorName != null)
                {
                    query = query.Where(ts => ts.User != null && (ts.User.FirstName + " " + ts.User.LastName).Contains(queryTutorSubjectDto.TutorName));
                }
                if (queryTutorSubjectDto.CreateTo.HasValue)
                {
                    query = query.Where(ts => ts.CreatedDate <= queryTutorSubjectDto.CreateTo);
                }
                if (queryTutorSubjectDto.CreateFrom.HasValue)
                {
                    query = query.Where(ts => ts.CreatedDate >= queryTutorSubjectDto.CreateFrom);
                }
                if (queryTutorSubjectDto.SubjectIds.Count > 0)
                {
                    query = query.Where(ts => queryTutorSubjectDto.SubjectIds.Contains(ts.SubjectId));
                }
            }
           
            int limit = request.Limit > 0 ? request.Limit : 10;
            int page = request.Page > 0 ? request.Page : 1;

            var result = await query.ToNewPagingAsync(page,limit);
            return result;
        }
    }
}