using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Blog;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.Repository
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        public BlogRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<PagedResult<Blog>> GetBlogs(PagingModel<QueryBlogDto> pagingModel)
        {
            var blogQuery = dbSet
                .Include(b => b.CreateBy)
                .Include(b => b.UpdateBy)
                .AsQueryable();
            if (pagingModel.Filter != null)
            {
                if (!string.IsNullOrEmpty(pagingModel.Filter.Keyword))
                {
                    blogQuery = blogQuery.Where(b => (b.Title != null && b.Title.Contains(pagingModel.Filter.Keyword))
                        || (b.Content != null && b.Content.Contains(pagingModel.Filter.Keyword)));
                }

                if (pagingModel.Filter.IsHidden.HasValue)
                {
                    blogQuery = blogQuery.Where(b => b.IsHidden == pagingModel.Filter.IsHidden);
                }
                if (pagingModel.Filter.CreateFrom.HasValue)
                {
                    blogQuery = blogQuery.Where(b => b.CreatedDate >= pagingModel.Filter.CreateFrom);
                }

                if (pagingModel.Filter.CreateTo.HasValue)
                {
                    blogQuery = blogQuery.Where(b => b.CreatedDate <= pagingModel.Filter.CreateTo);
                }

                if (pagingModel.Filter.UpdateFrom.HasValue)
                {
                    blogQuery = blogQuery.Where(b => b.UpdatedDate >= pagingModel.Filter.UpdateFrom);
                }

                if (pagingModel.Filter.UpdateTo.HasValue)
                {
                    blogQuery = blogQuery.Where(b => b.UpdatedDate <= pagingModel.Filter.UpdateTo);
                }

                if (pagingModel.Filter.CreateBy.HasValue)
                {
                    blogQuery = blogQuery.Where(b => b.CreateById == pagingModel.Filter.CreateBy);
                }
            }
            if (pagingModel.Sorts != null)
            {
                blogQuery.OrderProperty(pagingModel.Sorts);
            }

            int limit = pagingModel.Limit > 0 ? pagingModel.Limit : 10;
            int page = pagingModel.Page > 0 ? pagingModel.Page : 1;
            int skip = (page - 1) * limit;

            //blogQuery = blogQuery.Skip(skip).Take(limit);

            var filterBlogs = await blogQuery
               .ToNewPagingAsync(page, limit);

            return filterBlogs;
        }

        public async Task<Blog?> GetBlogDetail(int id)
        {
            return await dbSet.Include(b => b.CreateBy).Include(b => b.UpdateBy).FirstOrDefaultAsync(b => b.Id == id);
        }

    }
}

