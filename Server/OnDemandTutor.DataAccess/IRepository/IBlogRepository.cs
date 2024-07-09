using OnDemandTutor.Models.Dtos.Blog;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
        Task<PagedResult<Blog>> GetBlogs(PagingModel<QueryBlogDto> pagingModel);

        Task<Blog?> GetBlogDetail(int id);
    }
}

