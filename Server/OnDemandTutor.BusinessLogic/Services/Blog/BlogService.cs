using HtmlAgilityPack;
using LinqKit;
using Mapster;
using Microsoft.AspNetCore.Http;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Blog;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.Blog
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IAuthServices _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogService(IUnitOfWorkRepository unitOfWork, IAuthServices authService, IHttpContextAccessor HttpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _httpContextAccessor = HttpContextAccessor;
        }
        public async Task<PagedResult<GetBlogDtos>> GetBlogsAsync(PagingModel<QueryBlogDto> request)
        {
            var pagedBlogs = await _unitOfWork.BlogRepository.GetBlogs(request);
            pagedBlogs.Items.ForEach(b => b.Content = ConvertHtmlToPlainText(b.Content ?? ""));
            return pagedBlogs.Adapt<PagedResult<GetBlogDtos>>();
        }

        public async Task<GetBlogDtos> GetBlogByIdAsync(int id)
        {
            var blogEntity = await _unitOfWork.BlogRepository.GetBlogDetail(id);
            if (blogEntity == null)
            {
                throw new NotFoundException($"Blog with ID {id} not found.");
            }
            return blogEntity.Adapt<GetBlogDtos>();
        }

        public async Task<CreateBlogDtos> CreateBlogAsync(CreateBlogDtos blogDto)
        {
            var blogEntity = blogDto.Adapt<Models.Models.Blog>();
            var createdBlogEntity = await _unitOfWork.BlogRepository.AddAsync(blogEntity);
            var user = await _authService.GetUserProfileByClaim(_httpContextAccessor.HttpContext.User);
            createdBlogEntity.Entity.CreatedDate = DateTime.Now;
            createdBlogEntity.Entity.CreateById = user.Id;
            await _unitOfWork.SaveChangesAsync();
            return createdBlogEntity.Entity.Adapt<CreateBlogDtos>();
        }

        public async Task<UpdateBlogDtos> UpdateBlogAsync(UpdateBlogDtos blogDto)
        {
            var existingBlogEntity = await _unitOfWork.BlogRepository.FirstOrDefaultAsync(b => b.Id == blogDto.Id);
            if (existingBlogEntity == null)
            {
                throw new NotFoundException($"Blog with ID {blogDto.Id} not found.");
            }

            var user = await _authService.GetUserProfileByClaim(_httpContextAccessor.HttpContext.User);
            existingBlogEntity = blogDto.Adapt(existingBlogEntity);

            existingBlogEntity.UpdateById = user.Id;
            existingBlogEntity.UpdatedDate = DateTime.Now; // Update this field if needed

            var updatedBlogEntity = _unitOfWork.BlogRepository.Update(existingBlogEntity);
            await _unitOfWork.SaveChangesAsync();

            return updatedBlogEntity.Entity.Adapt<UpdateBlogDtos>();
        }

        public async Task<bool> DeleteBlogAsync(int id)
        {
            var existingBlogEntity = await _unitOfWork.BlogRepository.FirstOrDefaultAsync(b => b.Id == id);
            if (existingBlogEntity == null)
            {
                throw new NotFoundException($"Blog with ID {id} not found.");
            }

            _unitOfWork.BlogRepository.Remove(existingBlogEntity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private static string ConvertHtmlToPlainText(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return string.Empty;
            }

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            return htmlDoc.DocumentNode.InnerText;
        }

    }
}

