using OnDemandTutor.Models.Dtos.Blog;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces
{
    public interface IBlogService
    {
        Task<PagedResult<GetBlogDtos>> GetBlogsAsync(PagingModel<GetBlogDtos> request);
        Task<GetBlogDtos> GetBlogByIdAsync(int id);
        Task<CreateBlogDtos> CreateBlogAsync(CreateBlogDtos blogDto);
        Task<UpdateBlogDtos> UpdateBlogAsync(UpdateBlogDtos blogDto);
        Task<bool> DeleteBlogAsync(int id);
    }
}

