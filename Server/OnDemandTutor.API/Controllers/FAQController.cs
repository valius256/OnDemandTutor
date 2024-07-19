using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.BusinessLogic.Interfaces.FAQ;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.FAQ;
using OnDemandTutor.Models.Paging;
using ValidationErrorModel = OnDemandTutor.API.Middlesware.ValidationErrorModel;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQController : ControllerBase
    {
        private readonly IFAQService _faqService;
        private readonly ILogger<FAQController> _logger;

        public FAQController(ILogger<FAQController> logger, IFAQService faqService)
        {
            _logger = logger;
            _faqService = faqService;
        }


        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(FAQDTO), 200)]
        [ProducesResponseType(typeof(ValidationErrorModel), 400)]
        public async Task<IActionResult> CreateFAQ([FromBody] CreateFAQDto FAQDTO)
        {
            var createdFaq = await _faqService.CreateFAQAsync(FAQDTO);
            if (createdFaq == null)
            {
                throw new BadRequestException("Failed to create FAQ.");
            }
            return Ok(createdFaq);
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(PagedResult<FAQDTO>), 200)]
        [ProducesResponseType(typeof(ValidationErrorModel), 400)]
        public async Task<IActionResult> GetFAQs([FromQuery] PagingModel<QueryFAQDTO> pagingModel)
        {
            var faqs = await _faqService.GetFAQsAsync(pagingModel);
            if (faqs == null)
            {
                throw new BadRequestException("Failed to retrieve FAQs.");
            }
            return Ok(faqs);
        }

        [HttpGet("get-by-id")]
        [ProducesResponseType(typeof(FAQDTO), 200)]
        [ProducesResponseType(typeof(ValidationErrorModel), 400)]
        public async Task<IActionResult> GetFAQById(int id)
        {
            var faq = await _faqService.GetFAQByIdAsync(id);
            if (faq == null)
            {
                throw new BadRequestException($"Failed to retrieve FAQ by ID {id}.");
            }
            return Ok(faq);
        }

        [Authorize]
        [HttpPut("update")]
        [ProducesResponseType(typeof(FAQDTO), 200)]
        [ProducesResponseType(typeof(ValidationErrorModel), 400)]
        public async Task<IActionResult> UpdateFAQ([FromBody] UpdateFAQDto FAQDTO)
        {
            var updatedFaq = await _faqService.UpdateFAQAsync(FAQDTO);
            if (updatedFaq == null)
            {
                throw new BadRequestException("Failed to update FAQ.");
            }
            return Ok(updatedFaq);
        }

        [Authorize]
        [HttpDelete("delete")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(ValidationErrorModel), 400)]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            var isDeleted = await _faqService.DeleteFAQAsync(id);
            if (!isDeleted)
            {
                throw new BadRequestException($"Failed to delete FAQ with ID {id}.");
            }
            return Ok(isDeleted);
        }
    }
}
