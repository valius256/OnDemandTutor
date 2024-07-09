using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.FAQ;
using OnDemandTutor.Models.Dtos.FAQ;
using OnDemandTutor.Models.Paging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            try
            {
                var createdFaq = await _faqService.CreateFAQAsync(FAQDTO);
                return Ok(createdFaq);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create FAQ: {ex.Message}");
                return BadRequest(new ValidationErrorModel(ex.Message));
            }
        }

        //[Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(PagedResult<FAQDTO>), 200)]
        [ProducesResponseType(typeof(ValidationErrorModel), 400)]
        public async Task<IActionResult> GetFAQs([FromQuery] PagingModel<FAQDTO> pagingModel)
        {
            try
            {
                var faqs = await _faqService.GetFAQsAsync(pagingModel);
                return Ok(faqs);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve FAQs: {ex.Message}");
                return BadRequest(new ValidationErrorModel(ex.Message));
            }
        }

      //  [Authorize]
        [HttpGet("get-by-id")]
        [ProducesResponseType(typeof(FAQDTO), 200)]
        [ProducesResponseType(typeof(ValidationErrorModel), 400)]
        public async Task<IActionResult> GetFAQById(int id)
        {
            try
            {
                var faq = await _faqService.GetFAQByIdAsync(id);
                return Ok(faq);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve FAQ by ID {id}: {ex.Message}");
                return BadRequest(new ValidationErrorModel(ex.Message));
            }
        }

        [Authorize]
        [HttpPut("update")]
        [ProducesResponseType(typeof(FAQDTO), 200)]
        [ProducesResponseType(typeof(ValidationErrorModel)   , 400)]
        public async Task<IActionResult> UpdateFAQ([FromBody] UpdateFAQDto FAQDTO)
        {
            try
            {
                var updatedFaq = await _faqService.UpdateFAQAsync(FAQDTO);
                return Ok(updatedFaq);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update FAQ: {ex.Message}");
                return BadRequest(new ValidationErrorModel(ex.Message));
            }
        }

        [Authorize]
        [HttpDelete("delete")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(ValidationErrorModel), 400)]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            try
            {
                var isDeleted = await _faqService.DeleteFAQAsync(id);
                return Ok(isDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete FAQ with ID {id}: {ex.Message}");
                return BadRequest(new ValidationErrorModel(ex.Message));
            }
        }
    }
}
