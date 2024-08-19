using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Blog;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<GetBlogDtos>), 200)]
        public async Task<IActionResult> GetBlogs([FromQuery] PagingModel<QueryBlogDto> pagingModel)
        {
            var blogs = await _blogService.GetBlogsAsync(pagingModel);
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetBlogDtos), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBlogById(int id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            return Ok(blog);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CreateBlogDtos), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateBlog([FromBody] CreateBlogDtos blogDto)
        {
            var createdBlog = await _blogService.CreateBlogAsync(blogDto);
            return CreatedAtAction(nameof(GetBlogById), createdBlog);
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateBlogDtos), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] UpdateBlogDtos blogDto)
        {
            await _blogService.UpdateBlogAsync(blogDto);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            await _blogService.DeleteBlogAsync(id);
            return NoContent();
        }
    }
}
