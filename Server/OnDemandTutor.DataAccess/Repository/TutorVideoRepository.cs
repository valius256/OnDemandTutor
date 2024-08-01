using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.TutorVideo;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.Repository
{
    public class TutorVideoRepository : GenericRepository<TutorVideo>, ITutorVideoRepository
    {
        public TutorVideoRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<PagedResult<TutorVideo>> QueryTutorVideoAsync(PagingModel<QueryTutorVideoDto> pagingModel)
        {
            var query = dbSet.Include(vd => vd.Tutor).AsQueryable();

            if (pagingModel.Filter != null)
            {
                if (pagingModel.Filter.TutorId != null)
                {
                    query = query.Where(vd => vd.TutorId == pagingModel.Filter.TutorId);
                }
            }

            return await query.OrderByDescending(vd => vd.CreatedDate).ToNewPagingAsync(pagingModel.Page, pagingModel.Limit);
        }

        public async Task<TutorVideo?> GetTutorVideoByIdAsync(int id)
        {
            return await dbSet.Include(vd => vd.Tutor).FirstOrDefaultAsync(vd => vd.Id == id);

        }
    }
}