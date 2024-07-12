using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Blog;
using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.Repository
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<Subject>> GetSubjects(PagingModel<QuerySubjectDTO> pagingModel)
        {

            var subjectQuery = dbSet
                .Include(s => s.CreateBy)
                .Include(s => s.TutorDegree)
                .Include(s => s.Class)
                .Include(s => s.Slots)
                .Include(s => s.TutorSubjects)
                .AsQueryable();

            if (pagingModel.Filter != null)
            {
                if (pagingModel.Filter.Name != null)
                {
                    subjectQuery = subjectQuery.Where(s => s.Name.Contains(pagingModel.Filter.Name));
                }

                if (pagingModel.Filter.Type != null)
                {
                    subjectQuery = subjectQuery.Where(s => s.SubjectType == pagingModel.Filter.Type);
                }

                if (pagingModel.Filter.Description != null)
                {
                    subjectQuery = subjectQuery.Where(s => s.Description.Contains(pagingModel.Filter.Description));
                }

                if (pagingModel.Filter.Status != null)
                {
                    bool isEnable = pagingModel.Filter.Status.Equals("Enabled", StringComparison.OrdinalIgnoreCase);
                    subjectQuery = subjectQuery.Where(s => s.IsEnable == isEnable);
                }

                if (pagingModel.Filter.CreateFrom.HasValue)
                {
                    subjectQuery = subjectQuery.Where(s => s.CreateAt >= pagingModel.Filter.CreateFrom.Value);
                }

                if (pagingModel.Filter.CreateTo.HasValue)
                {
                    subjectQuery = subjectQuery.Where(s => s.CreateAt <= pagingModel.Filter.CreateTo.Value);
                }

                if (pagingModel.Filter.UpdateFrom.HasValue)
                {
                    subjectQuery = subjectQuery.Where(s => s.UpdatedDate >= pagingModel.Filter.UpdateFrom.Value);
                }

                if (pagingModel.Filter.UpdateTo.HasValue)
                {
                    subjectQuery = subjectQuery.Where(s => s.UpdatedDate <= pagingModel.Filter.UpdateTo.Value);
                }
            }

            if (pagingModel.Sorts != null)
            {
                subjectQuery = subjectQuery.OrderProperty(pagingModel.Sorts);
            }

            int limit = pagingModel.Limit > 0 ? pagingModel.Limit : 10;
            int page = pagingModel.Page > 0 ? pagingModel.Page : 1;

            var pagedResult = await subjectQuery.ToNewPagingAsync(page, limit);

            return pagedResult;
        }
    }
}

