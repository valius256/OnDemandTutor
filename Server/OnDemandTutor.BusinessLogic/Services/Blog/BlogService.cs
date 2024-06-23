using System;
using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models.Dtos.Blog;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.Blog
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public BlogService(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PagedResult<GetBlogDtos>> GetBlogsAsync(PagingModel<GetBlogDtos> request)
        {
            var pagedBlogs = await _unitOfWork.BlogRepository.PagingAsync(request);
            return pagedBlogs.Adapt<PagedResult<GetBlogDtos>>();
        }

        public async Task<GetBlogDtos> GetBlogByIdAsync(int id)
        {
            var blogEntity = await _unitOfWork.BlogRepository.FirstOrDefaultAsync(b => b.Id == id);
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

            existingBlogEntity = blogDto.Adapt(existingBlogEntity);

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
    }
}

